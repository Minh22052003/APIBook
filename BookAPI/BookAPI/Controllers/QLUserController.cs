using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public ActionResult EditUser(int iduser)
        {
            var existingUser = _context.Users.Find(iduser);
            return View(existingUser);
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                var tmpuser = _context.Users.FirstOrDefault(t => t.UserID == user.UserID);
                tmpuser.Status = user.Status;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
    }
}