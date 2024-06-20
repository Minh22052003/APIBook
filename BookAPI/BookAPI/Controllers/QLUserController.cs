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

        public ActionResult Edit(int useid, int status, string role)
        {
            var tmp = _context.Users.FirstOrDefault(u => u.UserID == useid);
            tmp.Status = status;
            tmp.Role = role;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}