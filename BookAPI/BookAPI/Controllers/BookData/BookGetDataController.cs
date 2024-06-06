using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookAPI.Controllers.BookData
{
    public class BookGetDataController : ApiController
    {
        private readonly ModelBook _context;
        public BookGetDataController(ModelBook context)
        {
            _context = context;
        }
        public BookGetDataController()
        {
            _context = new ModelBook();
        }
        //Các hàm get
        // api/BookGetData/Get_AllBook
        [HttpGet]
        public HttpResponseMessage Get_AllBook()
        {
            var books = (from b in _context.Books.Include("Categories").Include("Authors")
                         join p in _context.Publishers on b.PublisherID equals p.ID
                         select new
                         {
                             b.ID,
                             b.Title,
                             b.DescriptionB,
                             Publisher = p.PublisherName,
                             Authors = b.Authors.Select(a => a.AuthorName).ToList(),
                             Categories = b.Categories.Select(c => c.CategoryName).ToList(),
                             b.PublishedDate,
                             b.BookLink,
                             b.CoverImage
                         }).ToList();
            if (books != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, books);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        public HttpResponseMessage Get_BookByID(int id)
        {

            var books = (from b in _context.Books.Include("Categories").Include("Authors")
                         join p in _context.Publishers on b.PublisherID equals p.ID
                         select new
                         {
                             b.ID,
                             b.Title,
                             b.DescriptionB,
                             Publisher = p.PublisherName,
                             Authors = b.Authors.Select(a => a.AuthorName).ToList(),
                             Categories = b.Categories.Select(c => c.CategoryName).ToList()
                             // Thêm các trường thông tin khác nếu cần thiết
                         }).ToList();
            if (books != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, books);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        public HttpResponseMessage Get_BookByAuthor(int id)
        {

            var books = (from b in _context.Books.Include("Categories").Include("Authors")
                         join p in _context.Publishers on b.PublisherID equals p.ID
                         select new
                         {
                             b.ID,
                             b.Title,
                             b.DescriptionB,
                             Publisher = p.PublisherName,
                             Authors = b.Authors.Select(a => a.AuthorName).ToList(),
                             Categories = b.Categories.Select(c => c.CategoryName).ToList()
                             // Thêm các trường thông tin khác nếu cần thiết
                         }).ToList();
            if (books != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, books);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        //[HttpGet]
        //public HttpResponseMessage Get_BookByID(int id)
        //{

        //    var books = (from b in _context.Books.Include("Categories").Include("Authors")
        //                 join p in _context.Publishers on b.PublisherID equals p.ID
        //                 select new
        //                 {
        //                     b.ID,
        //                     b.Title,
        //                     b.DescriptionB,
        //                     Publisher = p.PublisherName,
        //                     Authors = b.Authors.Select(a => a.AuthorName).ToList(),
        //                     Categories = b.Categories.Select(c => c.CategoryName).ToList()
        //                     // Thêm các trường thông tin khác nếu cần thiết
        //                 }).ToList();
        //    if (books != null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, books);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound);
        //    }
        //}
    }
}
