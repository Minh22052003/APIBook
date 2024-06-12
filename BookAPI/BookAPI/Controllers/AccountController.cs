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
            if(tenDangNhap == "admin@gmail.com" && matKhau == "2205")
            {
                FormsAuthentication.SetAuthCookie(tenDangNhap, false);
                var roles = new[] { "Admin" };
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
    }
}
