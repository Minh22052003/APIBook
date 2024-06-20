using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using WebFont.Data;
using WebFont.Models;

namespace WebFont.Controllers
{
    public class AccountController : Controller
    {
        public GetDataBookAPI dataGet;
        public PostDataBookAPI dataPost;
        public PutDataBookAPI dataPut;
        public DeleteDataBookAPI dataDel;
        private readonly HttpClient _httpClient;

        public AccountController()
        {
            dataGet = new GetDataBookAPI();
            dataPost = new PostDataBookAPI();
            dataPut = new PutDataBookAPI();
            dataDel = new DeleteDataBookAPI();
            _httpClient = new HttpClient();
        }
        // GET: Account
        public ActionResult Index()
        {
            UserNotPass user = new UserNotPass();
            // Lấy cookie xác thực
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                // Giải mã cookie để lấy thông tin user
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    user.Username = authTicket.Name;
                    string[] userData = authTicket.UserData.Split(';');
                    if (userData.Length < 5)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    int userId;
                    if (int.TryParse(userData[0], out userId))
                    {
                        user.UserID = userId;
                    }
                    user.FullName = userData[1];
                    user.Email = userData[2];
                    user.BirthDate = DateTime.Parse(userData[3]);
                    user.Gender = userData[4];
                }
            }
            return View(user);
        }

        [HttpPost]
        public async Task<JsonResult> SaveChangesAsync(UserNotPass user)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await dataPut.UpdateUserAsync(user);

                if (response.IsSuccessStatusCode)
                {

                    UpdateAuthCookie(user);
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false});
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, error = "Model state is invalid", details = errors });
            }
        }

        private void UpdateAuthCookie(UserNotPass user)
        {
            var userData = $"{user.UserID};{user.FullName};{user.Email};{user.BirthDate};{user.Gender}";
            var authTicket = new FormsAuthenticationTicket(
                1, user.Username, DateTime.Now, DateTime.Now.AddHours(2), false, userData);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true
            };

            HttpContext.Response.Cookies.Add(authCookie);
        }



        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
            FormsAuthentication.SignOut();

            HttpCookie authCookie1 = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie1 != null)
            {
                authCookie1.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(authCookie1);
            }
            try
            {
                LoginAcc loginuser = new LoginAcc { Username = username, Password = HashPassword(username, password) };
                HttpResponseMessage response = await dataPost.Post_LoginUserAsync(loginuser);
                if (response.IsSuccessStatusCode)
                {
                    //Nhận
                    string responseData = await response.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(responseData);
                    if (user.FullName == null)
                    {
                        user.FullName = "null";
                    }
                    if (user.Email == null)
                    {
                        user.Email = "null";
                    }
                    if (user.BirthDate == null)
                    {
                        user.BirthDate = new DateTime(0001, 01, 01);
                    }
                    if (user.Gender == null)
                    {
                        user.Gender = "null";
                    }

                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    var userData = $"{user.UserID};{user.FullName};{user.Email};{user.BirthDate};{user.Gender}";
                    var authTicket = new FormsAuthenticationTicket(
                        1, user.Username, DateTime.Now, DateTime.Now.AddHours(2), false, userData);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    ViewBag.ErrorMessage = errorMessage;
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine(ex.Message);
                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                authCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(authCookie);
            }

            return RedirectToAction("Login", "Account");
        }



        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(string username, string password, string confirmPassword)
        {
            if (username == null || password == null)
            {
                ViewBag.Error = "Tên đăng nhập và mật khẩu không được để trống.";
                return View();
            }
            if (password!= confirmPassword)
            {
                ViewBag.Error = "Xác nhận mật khẩu bị sai.";
                return View();
            }

            List<UsernameResponse> lusername = await dataGet.Get_AllUsernameAsync();


            foreach (var userName in lusername)
            {
                if (username == userName.Username1)
                {
                    ViewBag.Error = "Tên đăng nhập đã tồn tại.";
                    return View();
                }
            }

            LoginAcc newUser = new LoginAcc();
            newUser.Username = username;
            newUser.Password = HashPassword(username, password);
            bool check = await dataPost.Post_CreateAccUser(newUser);


            if (check)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Error = "Không thể tạo tài khoản: ";
                return View();
            }
        }

        //public ActionResult UpdateUser()
        //{
        //    User user = new User();
        //    // Lấy cookie xác thực
        //    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        //    if (authCookie != null)
        //    {
        //        // Giải mã cookie để lấy thông tin user
        //        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        //        if (authTicket != null)
        //        {
        //            user.Username = authTicket.Name;
        //            string[] userData = authTicket.UserData.Split(';');
        //            int userId;
        //            if (int.TryParse(userData[0], out userId))
        //            {
        //                user.UserID = userId;
        //            }
        //            user.FullName = userData[1];
        //            user.Email = userData[2];
        //            user.BirthDate = DateTime.Parse(userData[3]);
        //            user.Gender = userData[4];
        //        }
        //    }
        //    return View(user);
        //}

        //[HttpPost]
        //public async Task<ActionResult> UpdateUser(string fullname, string email, string birthdate, string gender)
        //{
        //    User user = new User();
        //    HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
        //    if (authCookie != null)
        //    {
        //        FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        //        if (authTicket != null)
        //        {
        //            user.Username = authTicket.Name;
        //            string[] userData = authTicket.UserData.Split(';');
        //            user.UserID = int.Parse(userData[0]);
        //            user.FullName = userData[1];
        //            user.Email = userData[2];
        //            user.BirthDate = DateTime.Parse(userData[3]);
        //            user.Gender = userData[4];
        //        }
        //        user.FullName = fullname;
        //        user.Email = email;
        //        user.Gender = gender;
        //        user.BirthDate = DateTime.Parse(birthdate);
        //        return View(user);
        //    }
        //    return View();

        //}



        public async Task<ActionResult> UserRead()
        {
            int userid =0;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    string[] userData = authTicket.UserData.Split(';');
                    userid = int.Parse(userData[0]);
                }
            }
            List<BookHistory> books = await dataGet.Get_HistoryUser(userid);
            return View(books);
        }

        public ActionResult DeleteHistory(int idbook)
        {
            int userid = 0;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    string[] userData = authTicket.UserData.Split(';');
                    userid = int.Parse(userData[0]);
                }
            }
            User_Book user_Book = new User_Book();
            user_Book.BookID = idbook;
            user_Book.UserID = userid;
            _ = dataDel.DeleteHistory(user_Book);
            return RedirectToAction("UserRead");
        } 

        public async Task<ActionResult> Userfavorite()
        {
            int userid = 0;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    string[] userData = authTicket.UserData.Split(';');
                    userid = int.Parse(userData[0]);
                }
            }
            List<Book> books = await dataGet.Get_FavoriteUser(userid);
            return View(books);
        }

        public ActionResult DeleteLove(int idbook)
        {
            int userid = 0;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    string[] userData = authTicket.UserData.Split(';');
                    userid = int.Parse(userData[0]);
                }
            }
            User_Book user_Book = new User_Book();
            user_Book.BookID = idbook;
            user_Book.UserID = userid;
            _ = dataDel.DeleteFavorite(user_Book);
            return RedirectToAction("Userfavorite");
        }


        public async Task<ActionResult> UserReview()
        {
            int userid = 0;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    string[] userData = authTicket.UserData.Split(';');
                    userid = int.Parse(userData[0]);
                }
            }
            List<ReviewUser> reviews = await dataGet.Get_ReviewUser(userid);
            return View(reviews);
        }
        public async Task<ActionResult> UpdateReviewAsync(int id)
        {
            int userid = 0;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    string[] userData = authTicket.UserData.Split(';');
                    userid = int.Parse(userData[0]);
                }
            }
            List<ReviewUser> reviews = await dataGet.Get_ReviewUser(userid);
            foreach( var i in reviews)
            {
                if(i.ReviewId == id)
                {
                    return View(i);
                }
            }
            return View("UserReview");
        }

        [HttpPost]
        public ActionResult UpdateReviewAsync(ReviewUser review)
        {
            int userid = 0;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    string[] userData = authTicket.UserData.Split(';');
                    userid = int.Parse(userData[0]);
                }
            }
            ReviewBookPost reviewBookPost = new ReviewBookPost();
            reviewBookPost.ReviewID = review.ReviewId;
            reviewBookPost.UserID = userid;
            reviewBookPost.BookID = review.BookID;
            reviewBookPost.Rating = review.Rating;
            reviewBookPost.Content = review.Content;
            reviewBookPost.ReviewTime = DateTime.Now;

            _ = dataPut.UpdateUserReview(reviewBookPost);
            return RedirectToAction("UserReview");
        }

        public ActionResult DeleteReview(int idbook)
        {
            int userid = 0;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    string[] userData = authTicket.UserData.Split(';');
                    userid = int.Parse(userData[0]);
                }
            }
            User_Book user_Book = new User_Book();
            user_Book.BookID = idbook;
            user_Book.UserID = userid;
            _ = dataDel.DeleteReview(user_Book);
            return RedirectToAction("UserReview");
        }





        public static string HashPassword(string username, string password)
        {
            string input = username + password;

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }


        [HttpGet]
        public JsonResult IsAuthenticated()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            return Json(isAuthenticated, JsonRequestBehavior.AllowGet);
        }

    }
    
}