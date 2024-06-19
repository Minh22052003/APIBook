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
        public double ScoreBook(int bookID)
        {
            var reviews = _context.Reviews.Where(r => r.BookID == bookID).ToList();

            if (reviews.Count == 0)
            {
                return 0;
            }

            double totalScore = reviews.Sum(r => r.Rating);
            double averageScore = totalScore / reviews.Count;

            int roundedScore = (int)Math.Round(averageScore, MidpointRounding.AwayFromZero);

            return roundedScore;
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
                             Publisher = b.Publisher.PublisherName,
                             Authors = b.Authors.Select(a => a.AuthorName).ToList(),
                             Categories = b.Categories.Select(c => c.CategoryName).ToList(),
                             b.PublishedDate,
                             b.BookLink,
                             b.CoverImage
                         })
                        .ToList();

            var booksWithRating = books.Select(b => new
            {
                b.ID,
                b.Title,
                b.DescriptionB,
                b.Publisher,
                b.Authors,
                b.Categories,
                b.PublishedDate,
                Rating = ScoreBook(b.ID),
                b.BookLink,
                b.CoverImage
            }).ToList();

            if (booksWithRating.Any())
            {
                return Request.CreateResponse(HttpStatusCode.OK, booksWithRating);
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
            double Rating =  ScoreBook(id);
            var books = from b in _context.Books.Include("Categories").Include("Authors")
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
                            Rating,
                            b.BookLink,
                            b.CoverImage
                        };
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
                             Publisher = b.Publisher.PublisherName,
                             Authors = b.Authors.Select(a => a.AuthorName).ToList(),
                             Categories = b.Categories.Select(c => c.CategoryName).ToList(),
                             b.PublishedDate,
                             b.BookLink,
                             b.CoverImage
                         })
                        .ToList();

            var booksWithRating = books.Select(b => new
            {
                b.ID,
                b.Title,
                b.DescriptionB,
                b.Publisher,
                b.Authors,
                b.Categories,
                b.PublishedDate,
                Rating = ScoreBook(b.ID),
                b.BookLink,
                b.CoverImage
            }).ToList();

            if (booksWithRating.Any())
            {
                return Request.CreateResponse(HttpStatusCode.OK, booksWithRating);
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
                             Publisher = b.Publisher.PublisherName,
                             Authors = b.Authors.Select(a => a.AuthorName).ToList(),
                             Categories = b.Categories.Select(c => c.CategoryName).ToList(),
                             b.PublishedDate,
                             b.BookLink,
                             b.CoverImage
                         })
                        .ToList();

            var booksWithRating = books.Select(b => new
            {
                b.ID,
                b.Title,
                b.DescriptionB,
                b.Publisher,
                b.Authors,
                b.Categories,
                b.PublishedDate,
                Rating = ScoreBook(b.ID),
                b.BookLink,
                b.CoverImage
            }).ToList();

            if (booksWithRating.Any())
            {
                return Request.CreateResponse(HttpStatusCode.OK, booksWithRating);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
        //   api/BookGetData/Get_BookByCategoryID?categoryID=20
        [HttpGet]
        public HttpResponseMessage Get_BookByCategoryID(int categoryID)
        {

            var books = (from b in _context.Books.Include("Categories").Include("Authors")
                         join p in _context.Publishers on b.PublisherID equals p.ID
                         where b.Categories.Any(a => a.ID == categoryID)
                         select new
                         {
                             b.ID,
                             b.Title,
                             b.DescriptionB,
                             Publisher = b.Publisher.PublisherName,
                             Authors = b.Authors.Select(a => a.AuthorName).ToList(),
                             Categories = b.Categories.Select(c => c.CategoryName).ToList(),
                             b.PublishedDate,
                             b.BookLink,
                             b.CoverImage
                         })
                        .ToList();

            var booksWithRating = books.Select(b => new
            {
                b.ID,
                b.Title,
                b.DescriptionB,
                b.Publisher,
                b.Authors,
                b.Categories,
                b.PublishedDate,
                Rating = ScoreBook(b.ID),
                b.BookLink,
                b.CoverImage
            }).ToList();

            if (booksWithRating.Any())
            {
                return Request.CreateResponse(HttpStatusCode.OK, booksWithRating);
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
                             Publisher = b.Publisher.PublisherName,
                             Authors = b.Authors.Select(a => a.AuthorName).ToList(),
                             Categories = b.Categories.Select(c => c.CategoryName).ToList(),
                             b.PublishedDate,
                             b.BookLink,
                             b.CoverImage
                         })
                        .ToList();

            var booksWithRating = books.Select(b => new
            {
                b.ID,
                b.Title,
                b.DescriptionB,
                b.Publisher,
                b.Authors,
                b.Categories,
                b.PublishedDate,
                Rating = ScoreBook(b.ID),
                b.BookLink,
                b.CoverImage
            }).ToList();

            if (booksWithRating.Any())
            {
                return Request.CreateResponse(HttpStatusCode.OK, booksWithRating);
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
                             Publisher = b.Publisher.PublisherName,
                             Authors = b.Authors.Select(a => a.AuthorName).ToList(),
                             Categories = b.Categories.Select(c => c.CategoryName).ToList(),
                             b.PublishedDate,
                             b.BookLink,
                             b.CoverImage,
                             h.ReadTime,
                         })
                        .ToList();

            var booksWithRating = books.Select(b => new
            {
                b.ID,
                b.Title,
                b.DescriptionB,
                b.Publisher,
                b.Authors,
                b.Categories,
                b.PublishedDate,
                Rating = ScoreBook(b.ID),
                b.BookLink,
                b.CoverImage,
                b.ReadTime,
            }).ToList();

            if (booksWithRating.Any())
            {
                return Request.CreateResponse(HttpStatusCode.OK, booksWithRating);
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
                             Publisher = b.Publisher.PublisherName,
                             Authors = b.Authors.Select(a => a.AuthorName).ToList(),
                             Categories = b.Categories.Select(c => c.CategoryName).ToList(),
                             b.PublishedDate,
                             b.BookLink,
                             b.CoverImage
                         })
                        .ToList();

            var booksWithRating = books.Select(b => new
            {
                b.ID,
                b.Title,
                b.DescriptionB,
                b.Publisher,
                b.Authors,
                b.Categories,
                b.PublishedDate,
                Rating = ScoreBook(b.ID),
                b.BookLink,
                b.CoverImage
            }).ToList();

            if (booksWithRating.Any())
            {
                return Request.CreateResponse(HttpStatusCode.OK, booksWithRating);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        //   api/BookGetData/Get_ReviewByBook?idbook=71
        [HttpGet]
        public HttpResponseMessage Get_ReviewByBook(int idbook)
        {
            var tmpreview = _context.Reviews.Include("User").Where(r=>r.BookID == idbook).ToList();
            int n = tmpreview.Count();
            var review = (from r in _context.Reviews.Include("User")
                          where r.BookID == idbook
                          select new
                          {
                              r.UserID,
                              r.User.Username,
                              r.User.FullName,
                              r.Content,
                              r.Rating,
                              n,
                              r.ReviewTime,
                          }).ToList();
                                        
            if(review != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, review);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        //   api/BookGetData/Get_ReviewByUser?iduser=6
        [HttpGet]
        public HttpResponseMessage Get_ReviewByUser(int iduser)
        {
            var tmpreview = _context.Reviews.Include("Book").Where(r => r.UserID == iduser).ToList();
            int n = tmpreview.Count();
            var review = (from r in _context.Reviews.Include("Book")
                          where r.UserID == iduser
                          select new
                          {
                              r.ReviewID,
                              r.BookID,
                              r.Book.Title,
                              r.Rating,
                              n,
                              r.Content,
                              r.ReviewTime,
                          }).ToList();

            if (review != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, review);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        //   api/BookGetData/Get_BookCategory
        [HttpGet]
        public HttpResponseMessage Get_BookCategory()
        {

            var categories = from c in _context.Categories
                             select new
                             {
                                 c.ID,
                                 c.CategoryName
                             };
            if (categories != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, categories);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
