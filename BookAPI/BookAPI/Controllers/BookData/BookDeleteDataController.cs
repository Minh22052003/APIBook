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

        // api/BookDeleteData/DeleteBookInHistory?id=1
        [HttpDelete]
        public HttpResponseMessage DeleteBookInHistory(int id)
        {
            var historyRead = _context.ReadingHistories.FirstOrDefault(h =>h.HistoryID == id);
            if (historyRead == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found.");
            }
            _context.ReadingHistories.Remove(historyRead);
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // api/BookDeleteData/DeleteBookInFavorite?id=1
        [HttpDelete]
        public HttpResponseMessage DeleteBookInFavorite(int id)
        {
            var favoriteRead = _context.FavoriteBooks.FirstOrDefault(f => f.FavoriteID == id);
            if (favoriteRead == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found.");
            }
            _context.FavoriteBooks.Remove(favoriteRead);
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
