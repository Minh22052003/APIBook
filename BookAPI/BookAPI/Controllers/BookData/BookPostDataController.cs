using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BookAPI.Controllers.BookData
{
    public class BookPostDataController : ApiController
    {
        private readonly ModelBook _context;
        public BookPostDataController(ModelBook context)
        {
            _context = context;
        }
        public BookPostDataController()
        {
            _context = new ModelBook();
        }

        //api/BookPostData/Create_HistoryRead
        //Thêm vào lịch sử đọc của client
        [HttpPost]
        public HttpResponseMessage Create_HistoryRead(User_Book userbookPost)
        {
            ReadingHistory readingHistory = new ReadingHistory();
            readingHistory.BookID = userbookPost.BookID;
            readingHistory.UserID = userbookPost.UserID;
            readingHistory.ReadTime = DateTime.Now;
            _context.ReadingHistories.Add(readingHistory);
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK,readingHistory);
        }

        //api/BookPostData/Create_FavoriteRead
        //Thêm vào tệp yêu thích của client
        [HttpPost]
        public HttpResponseMessage Create_FavoriteRead(User_Book userbookPost)
        {
            FavoriteBook favoriteBook = new FavoriteBook();
            favoriteBook.BookID = userbookPost.BookID;
            favoriteBook.UserID = userbookPost.UserID;
            _context.FavoriteBooks.Add(favoriteBook);
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, favoriteBook);
        }
    }
}
