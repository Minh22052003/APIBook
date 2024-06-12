using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebFont.Data;
using WebFont.Models;

namespace WebFont.Controllers
{
    public class HomeController : Controller
    {
        public GetDataBookAPI data = new GetDataBookAPI();
        public int pageSize = 8;


        public async Task<ActionResult> Index(int? page)
        {
            int pageNumber = (page ?? 1);
            List<Book> allBooks = await data.Get_AllBookAsync();
            int totalBooks = allBooks.Count();
            var books = allBooks
                .OrderBy(b => b.ID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(totalBooks / (double)pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(books);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Product()
        {
            return View();
        }
    }
}