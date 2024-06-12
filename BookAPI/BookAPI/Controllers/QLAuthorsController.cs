using BookAPI.Controllers.BookData;
using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View(lauthor);
        }
    }
}