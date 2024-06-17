using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BookAPI.Controllers.UserData
{
    public class UserPutDataController : ApiController
    {
        private readonly ModelBook _context;
        public UserPutDataController(ModelBook context)
        {
            _context = context;
        }
        public UserPutDataController()
        {
            _context = new ModelBook();
        }

        //Gọi API api/UserPutData/UpdateUser
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateUser(UserNotPass request)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(p => p.UserID == request.UserID);
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "User not found.");
                }
                user.FullName = request.FullName;
                user.Email = request.Email;
                user.BirthDate = request.BirthDate;
                user.Gender = request.Gender;
                _context.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
