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
        //[AllowAnonymous]
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

        // api/BookGetData/Get_BookByID?id=1
        [HttpGet]
        public HttpResponseMessage Get_BookByID(int id)
        {

            var books = (from b in _context.Books.Include("Categories").Include("Authors")
                         join p in _context.Publishers on b.PublisherID equals p.ID
                         where b.ID == id
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

        //   api/BookGetData/Get_BookByAuthor?authorName=David Ping
        [HttpGet]
        public HttpResponseMessage Get_BookByAuthor(string authorName)
        {

            var books = (from b in _context.Books.Include("Categories").Include("Authors")
                         join p in _context.Publishers on b.PublisherID equals p.ID
                         where b.Authors.Any(a => a.AuthorName.Contains(authorName))
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

        //   api/BookGetData/Get_BookByCategory?categoryName=Computers
        [HttpGet]
        public HttpResponseMessage Get_BookByCategory(string categoryName)
        {

            var books = (from b in _context.Books.Include("Categories").Include("Authors")
                         join p in _context.Publishers on b.PublisherID equals p.ID
                         where b.Categories.Any(a => a.CategoryName.Contains(categoryName))
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
        //   api/BookGetData/Get_BookByPublisher?publisherName=John Wiley & Sons
        [HttpGet]
        public HttpResponseMessage Get_BookByPublisher(string publisherName)
        {

            var books = (from b in _context.Books.Include("Categories").Include("Authors")
                         join p in _context.Publishers on b.PublisherID equals p.ID
                         where p.PublisherName.Contains(publisherName)
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

        //   api/BookGetData/Get_BookByHistoryUser?iduser=1
        [HttpGet]
        public HttpResponseMessage Get_BookByHistoryUser(int iduser)
        {

            var books = (from b in _context.Books.Include("Categories").Include("Authors")
                         join p in _context.Publishers on b.PublisherID equals p.ID
                         join h in _context.ReadingHistories on b.ID equals h.BookID
                         where h.UserID == iduser
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

        //   api/BookGetData/Get_BookByFavoriteUser?iduser=1
        [HttpGet]
        public HttpResponseMessage Get_BookByFavoriteUser(int iduser)
        {

            var books = (from b in _context.Books.Include("Categories").Include("Authors")
                         join p in _context.Publishers on b.PublisherID equals p.ID
                         join f in _context.FavoriteBooks on b.ID equals f.BookID
                         where f.UserID == iduser
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
    }
}
