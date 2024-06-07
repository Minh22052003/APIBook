using BookAPI.Controllers.BookData;
using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookAPI.Controllers
{
    public class QLBookController : Controller
    {
        private readonly ModelBook _context;
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
        public ActionResult Index()
        {
            var books = from b in _context.Books
                        .Include("Publisher")
                        .Include("Authors")
                        .Include("Categories")
                        select b;
            return View(books.ToList());
        }
        [HttpPost]
        public async Task<ActionResult> SubmitInputAsync(string inputString)
        {
            int q = 1;
            //1: gọi theo tên sách
            //2: Gọi theo tên chủ đề
            BookdataFrAPI bookdataFrAPI = await datafromAPIController.SearchBooksAsync(q, inputString, 1);
            await datafromAPIController.ProcessBookDataAsync(bookdataFrAPI);
            // Xử lý chuỗi nhập vào nếu cần
            return RedirectToAction("Index");
        }
    }
    
}