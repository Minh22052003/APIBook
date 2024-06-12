using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookAPI.Controllers.UserData
{
    public class UserDeleteDataController : ApiController
    {
        private readonly ModelBook _context;
        public UserDeleteDataController(ModelBook context)
        {
            _context = context;
        }
        public UserDeleteDataController()
        {
            _context = new ModelBook();
        }

        // api/UserDeleteData/DeleteAccUser?id=1
        [HttpDelete]
        public HttpResponseMessage DeleteAccUser(int id)
        {
            var accUser = _context.Users.FirstOrDefault(a=> a.UserID == id);
            var favoriteRead = _context.FavoriteBooks.FirstOrDefault(f => f.UserID == id);
            var historyRead = _context.ReadingHistories.FirstOrDefault(h => h.UserID == id);
            if (accUser == null || favoriteRead == null || historyRead == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "User not found.");
            }
            _context.Users.Remove(accUser);
            _context.FavoriteBooks.Remove(favoriteRead);
            _context.ReadingHistories.Remove(historyRead);
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
