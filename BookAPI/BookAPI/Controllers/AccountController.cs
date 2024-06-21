using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Web.Mvc;
using BookAPI.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Web.Security;
using System.Web;
using System;
using System.Security.Cryptography;
using System.Text;

namespace BookAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelBook _dbContext;

        public AccountController(ModelBook dbContext)
        {
            _dbContext = dbContext;
        }
        public AccountController()
        {
            _dbContext = new ModelBook();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string tenDangNhap, string matKhau)
        {
            string pass1 = HashPassword(tenDangNhap, matKhau);
            User user = _dbContext.Users.FirstOrDefault( u=>u.Username == tenDangNhap && u.Password == pass1);
            if(user != null && user.Role == "Admin")
            {
                FormsAuthentication.SetAuthCookie(tenDangNhap, false);
                var roles = new[] { user.Role };
                var authTicket = new FormsAuthenticationTicket(
                    1, tenDangNhap, DateTime.Now, DateTime.Now.AddMinutes(30), false, string.Join(",", roles));
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                return RedirectToAction("Profile");
            }
            ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không chính xác.";
            return View();
        }

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

        [Authorize]
        public ActionResult Profile()
        {
            return View();
        }

        //public async Task<IActionResult> Logout()
        //{
        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return RedirectToAction("Index", "Home");
        //}
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
