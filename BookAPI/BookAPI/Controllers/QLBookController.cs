using BookAPI.Controllers.BookData;
using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookAPI.Controllers
{
    public class QLBookController : Controller
    {
        private readonly ModelBook _context;
        public int pageSize = 10;
        public readonly DatafromAPIController datafromAPIController;
        public QLBookController(ModelBook context)
        {
            _context = context;
        }
        public QLBookController()
        {
            _context = new ModelBook();
            datafromAPIController = new DatafromAPIController();
        }
        // GET: QLBook
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            var books = from b in _context.Books
                    .Include("Publisher")
                    .Include("Authors")
                    .Include("Categories")
                    .OrderBy(b=>b.ID)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    select b;
            ViewBag.TotalPages = (int)Math.Ceiling(_context.Books.Count() / (double)pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(books.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> SubmitInputAsync(string inputString)
        {
            int q = 1;
            BookdataFrAPI bookdataFrAPI = await datafromAPIController.SearchBooksAsync(q, inputString, 40);
            await datafromAPIController.ProcessBookDataAsync(bookdataFrAPI);
            return RedirectToAction("Index");
        }
        public ActionResult EditBook(int idbook)
        {
            var books = _context.Books
                    .Include("Publisher")
                    .Include("Authors")
                    .Include("Categories")
                    .FirstOrDefault(b => b.ID == idbook);
            // Prepare SelectList for Publisher
            var publishers = _context.Publishers.ToList();
            ViewBag.PublisherID = new SelectList(publishers, "ID", "PublisherName", books.PublisherID);
            return View(books);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook(Book book)
        {
            if (ModelState.IsValid)
            {
                var bookInDb = _context.Books.FirstOrDefault(b => b.ID == book.ID);
                if (bookInDb == null)
                {
                    return HttpNotFound();
                }

                bookInDb.Title = book.Title;
                bookInDb.DescriptionB = book.DescriptionB;
                bookInDb.PublisherID = book.PublisherID;
                bookInDb.PublishedDate = book.PublishedDate;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Prepare SelectList again if model state is invalid
            var publishers = _context.Publishers.ToList();
            ViewBag.PublisherID = new SelectList(publishers, "ID", "PublisherName", book.PublisherID);

            return View(book);
        }

        public ActionResult DeleteBook(int id)
        {
            var book = _context.Books
                       .Include("FavoriteBooks")
                       .Include("ReadingHistories")
                       .Include("Reviews")
                       .Include("Categories")
                       .Include("Authors")
                       .FirstOrDefault(b => b.ID == id);

            if (book != null)
            {
                // Xóa các thực thể liên quan
                _context.FavoriteBooks.RemoveRange(book.FavoriteBooks);
                _context.ReadingHistories.RemoveRange(book.ReadingHistories);
                _context.Reviews.RemoveRange(book.Reviews);

                // Xóa các liên kết nhiều-nhiều
                var categoriesToRemove = book.Categories.ToList();
                foreach (var category in categoriesToRemove)
                {
                    book.Categories.Remove(category);
                }

                var authorsToRemove = book.Authors.ToList();
                foreach (var author in authorsToRemove)
                {
                    book.Authors.Remove(author);
                }

                // Xóa book
                _context.Books.Remove(book);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


    }

}