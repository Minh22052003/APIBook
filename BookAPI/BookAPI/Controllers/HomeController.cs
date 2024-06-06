using BookAPI.Controllers.BookData;
using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookAPI.Controllers
{
    public class HomeController : Controller
    {
        public readonly DatafromAPIController datafromAPIController;
        public HomeController()
        {
            datafromAPIController = new DatafromAPIController();
        }
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Home Page";

            BookdataFrAPI bookdataFrAPI = await datafromAPIController.SearchBooksAsync("Machine Learning", 1);
            await datafromAPIController.ProcessBookDataAsync(bookdataFrAPI);

            return View();
        }
    }
}
