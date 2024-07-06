using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebFont.Data;
using WebFont.Models;

namespace WebFont.Controllers
{
    public class LibraryController : Controller
    {

        public GetDataBookAPI data = new GetDataBookAPI();
        public PostDataBookAPI post = new PostDataBookAPI();
        public int pageSize = 8;

        // GET: Llibrary
        public async Task<ActionResult> Index(int? page, int? CategoryID)
        {
            int pageNumber = (page ?? 1);
            if(CategoryID == null)
            {
                List<Book> allBooks = await data.Get_AllBookAsync();
                int totalBooks = allBooks.Count();
                var books = allBooks
                    .OrderBy(b => b.ID)
                    .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
                ViewBag.TotalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);
                ViewBag.CurrentPage = pageNumber;

                List<Category> categories = await data.Get_BookCategoryAsync();
                ViewData["lcategory"] = categories;

                return View(books);
            }
            else
            {
                List<Book> allBooks = await data.Get_BookbyCategoryIDAsync(CategoryID.Value);
                int totalBooks = allBooks.Count();
                var books = allBooks
                    .OrderBy(b => b.ID)
                    .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
                ViewBag.TotalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);
                ViewBag.CurrentPage = pageNumber;

                List<Category> categories = await data.Get_BookCategoryAsync();
                ViewData["lcategory"] = categories;

                return View(books);
            }
            
        }
        public async Task<ActionResult> Product(int BookID, string category, int? BookIDRedirect)
        {
            if (!isAuthenticated())
            {
                Session["RedirectBookID"] = BookID;
                return RedirectToAction("Login", "Account");
            }
            List<Book> book = await data.Get_BookbyIDAsync(BookID);
            List<Book> bookscategory = (await data.Get_BookbyCategoryAsync(category))
                .OrderBy(x => Guid.NewGuid())
                .Take(6)
                .ToList();
            ViewData["Listds"] = bookscategory;

            List<ReviewBook> reviews = await data.Get_ReviewByBook(BookID);
            if (reviews.Count() != 0)
            {
                ViewData["listrv"] = reviews;
                ViewBag.countRV = reviews[0].Count;
            }
            else
            {
                ViewBag.countRV = 0;
            }

            ViewBag.Checklogin = isAuthenticated().ToString();
            
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
            List<ReviewUser> reviewuser = await data.Get_ReviewUser(userid);
            foreach (var i in reviewuser)
            {
                if (i.BookID == BookID)
                {
                    ViewBag.Checkreview = 1;
                }
            }

            if (BookIDRedirect.HasValue)
            {
                return RedirectToAction("Product", new { BookID = BookIDRedirect.Value, category = category });
            }

            return View(book);
            
            
        }

        public async Task<ActionResult> ReadBook(string idbook, string bookLink)
        {
            if(isAuthenticated())
            {
                User_Book userbook = new User_Book();
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    // Giải mã cookie để lấy thông tin user
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null)
                    {
                        string[] userData = authTicket.UserData.Split(';');
                        if (userData.Length < 5)
                        {
                            return RedirectToAction("Login", "Account");
                        }
                        int userId;
                        if (int.TryParse(userData[0], out userId))
                        {
                            userbook.UserID = userId;
                        }
                    }
                }
                userbook.BookID = int.Parse(idbook);
                bool task = await post.Post_HistoryReadDataAsync(userbook);
                return Redirect(bookLink);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
        }

        public async Task<ActionResult> LoveBook(string idbook)
        {
            if (isAuthenticated())
            {
                User_Book userbook = new User_Book();
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null)
                    {
                        string[] userData = authTicket.UserData.Split(';');
                        if (userData.Length < 5)
                        {
                            return RedirectToAction("Login", "Account");
                        }
                        int userId;
                        if (int.TryParse(userData[0], out userId))
                        {
                            userbook.UserID = userId;
                        }
                    }
                }
                userbook.BookID = int.Parse(idbook);
                bool task = await post.Post_FavoriteDataAsync(userbook);
                return RedirectToAction("Product", new { BookID = idbook, category = "Computer" });
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        [HttpPost]
        public ActionResult SubmitReview(FormCollection form)
        {
            int userid = 0;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    string[] userData = authTicket.UserData.Split(';');
                    if (userData.Length < 5)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    int userId;
                    if (int.TryParse(userData[0], out userId))
                    {
                        userid = userId;
                    }
                }
            }

            int idbook;
            if (!int.TryParse(form["idbook"], out idbook))
            {
                return RedirectToAction("Index");
            }

            int rating;
            if (!int.TryParse(form["rating"], out rating))
            {
                return RedirectToAction("Index");
            }

            string reviewText = form["reviewtext"];
            DateTime reviewTime = DateTime.Now;

            ReviewBookPost reviewBookPost = new ReviewBookPost
            {
                ReviewID = 0,
                UserID = userid,
                BookID = idbook,
                Rating = rating,
                Content = reviewText,
                ReviewTime = reviewTime
            };

            _ = post.Post_CreateAccUser(reviewBookPost);

            return RedirectToAction("Product", new { BookID = idbook, category = "Computer" });

        }



        public bool isAuthenticated()
        {
            if (HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                try
                {
                    HttpCookie authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    string userData = authTicket.UserData;

                    if (!string.IsNullOrEmpty(userData))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error decoding authentication ticket: " + ex.Message);
                }
            }

            return false;
        }
    }
}