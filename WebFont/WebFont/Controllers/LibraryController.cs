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
    public class LibraryController : Controller
    {

        public GetDataBookAPI data = new GetDataBookAPI();
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
        public async Task<ActionResult> Product(int BookID, string category)
        {
            List<Book> book = await data.Get_BookbyIDAsync(BookID);
            List<Book> bookscategory = (await data.Get_BookbyCategoryAsync(category))
                .OrderBy(x => Guid.NewGuid())
                .Take(6)
                .ToList();
            ViewData["Listds"] = bookscategory;
            return View(book);
        }
    }
}