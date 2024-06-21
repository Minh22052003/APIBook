using BookAPI.Controllers.BookData;
using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BookAPI.Controllers
{
    public class QLAuthorsController : Controller
    {
        private readonly ModelBook _context;
        public QLAuthorsController(ModelBook context)
        {
            _context = context;
        }
        public QLAuthorsController()
        {
            _context = new ModelBook();
        }
        // GET: QLAuthors
        public ActionResult Index()
        {
            var lauthor = _context.Authors.ToList();
            ViewBag.Checklogin = isAuthenticated().ToString();
            return View(lauthor);
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