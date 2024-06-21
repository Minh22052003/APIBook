using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BookAPI.Controllers
{
    public class QLUserController : Controller
    {
        private readonly ModelBook _context;
        public QLUserController(ModelBook context)
        {
            _context = context;
        }
        public QLUserController()
        {
            _context = new ModelBook();
        }
        // GET: QLUser
        public ActionResult Index()
        {
            ViewBag.Checklogin = isAuthenticated().ToString();
            var luser = _context.Users.ToList();
            return View(luser);
        }
        public ActionResult EditUser(int iduser)
        {
            ViewBag.Checklogin = isAuthenticated().ToString();
            var existingUser = _context.Users.Find(iduser);
            return View(existingUser);
        }

        public ActionResult Edit(int useid, int status, string role)
        {
            var tmp = _context.Users.FirstOrDefault(u => u.UserID == useid);
            tmp.Status = status;
            tmp.Role = role;
            _context.SaveChanges();
            return RedirectToAction("Index");
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