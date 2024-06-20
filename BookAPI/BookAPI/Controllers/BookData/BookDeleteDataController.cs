using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookAPI.Controllers.BookData
{
    public class BookDeleteDataController : ApiController
    {
        private readonly ModelBook _context;
        public BookDeleteDataController(ModelBook context)
        {
            _context = context;
        }
        public BookDeleteDataController()
        {
            _context = new ModelBook();
        }

        // api/BookDeleteData/DeleteBookInHistory
        [HttpDelete]
        public HttpResponseMessage DeleteBookInHistory(User_Book user_Book)
        {
            var historyRead = _context.ReadingHistories.FirstOrDefault(h => h.BookID == user_Book.BookID && h.UserID == user_Book.UserID);
            if (historyRead == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found.");
            }
            _context.ReadingHistories.Remove(historyRead);
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // api/BookDeleteData/DeleteBookInFavorite
        [HttpDelete]
        public HttpResponseMessage DeleteBookInFavorite(User_Book user_Book)
        {
            var favoriteRead = _context.FavoriteBooks.FirstOrDefault(f => f.BookID == user_Book.BookID && f.UserID == user_Book.UserID);
            if (favoriteRead == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found.");
            }
            _context.FavoriteBooks.Remove(favoriteRead);
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // api/BookDeleteData/DeleteBookInReview
        [HttpDelete]
        public HttpResponseMessage DeleteBookInReview(User_Book user_Book)
        {
            var bookReview = _context.Reviews.FirstOrDefault(f => f.BookID == user_Book.BookID && f.UserID == user_Book.UserID);
            if (bookReview == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found.");
            }
            _context.Reviews.Remove(bookReview);
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
