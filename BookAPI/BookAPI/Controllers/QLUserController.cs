using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var luser = _context.Users.ToList();
            return View(luser);
        }
    }
}