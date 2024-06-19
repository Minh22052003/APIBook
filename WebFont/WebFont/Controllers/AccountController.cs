using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        string URLLogin = "https://localhost:44380/api/UserPostData/Login";
        string URLGetUserName = "https://localhost:44380/api/UserGetData/Get_UserName";
        string URLPutUser = "https://localhost:44380/api/UserPutData/UpdateUser";
        string CreateAccUser = "https://localhost:44380/api/UserPostData/POST_CreateAccUser";
        public GetDataBookAPI data;
        private readonly HttpClient _httpClient;

        public AccountController()
        {
            data = new GetDataBookAPI();
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
                string apiUrl = URLPutUser; // URL API thực tế
                string json = JsonConvert.SerializeObject(user);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync(apiUrl, data); // Sử dụng PutAsync để cập nhật

                if (response.IsSuccessStatusCode)
                {

                    UpdateAuthCookie(user);
                    return Json(new { success = true });
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, error = errorContent });
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
            try
            {
                //Gửi
                string apiUrl = URLLogin;
                LoginAcc loginuser = new LoginAcc { Username = username, Password = password };
                string json = JsonConvert.SerializeObject(loginuser);
                StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, data);
                if (response.IsSuccessStatusCode)
                {
                    //Nhận
                    string responseData = await response.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(responseData);
                    if(user.FullName == null)
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
                    ViewBag.ErrorMessage = "Sai tài khoản hoặc mật khẩu. Vui lòng kiểm tra lại.";
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

            List<UsernameResponse> lusername;
            HttpResponseMessage response = await _httpClient.GetAsync(URLGetUserName);
            string responsData = await response.Content.ReadAsStringAsync();
            lusername = JsonConvert.DeserializeObject<List<UsernameResponse>>(responsData);
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
            newUser.Password = password;

            string json = JsonConvert.SerializeObject(newUser);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response1 = await _httpClient.PostAsync(CreateAccUser, data);

            if (response1.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            else
            {
                string errorMessage = await response1.Content.ReadAsStringAsync();
                ViewBag.Error = "Không thể tạo tài khoản: " + errorMessage;
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
            List<BookHistory> books = await data.Get_HistoryUser(userid);
            return View(books);
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
            List<Book> books = await data.Get_FavoriteUser(userid);
            return View(books);
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

        public class UsernameResponse
        {
            public string Username1 { get; set; }
        }

        [HttpGet]
        public JsonResult IsAuthenticated()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;
            return Json(isAuthenticated, JsonRequestBehavior.AllowGet);
        }

    }
    
}