using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookAPI.Controllers.UserData
{
    public class UserGetDataController : ApiController
    {
        private readonly ModelBook _context;
        public UserGetDataController(ModelBook context)
        {
            _context = context;
        }
        public UserGetDataController()
        {
            _context = new ModelBook();
        }
        // api/UserGetData/Get_AllUser
        //Các hàm get
        [HttpGet]
        public HttpResponseMessage Get_AllUser()
        {
            var user = from u in _context.Users
                       select new
                       {
                           u.UserID,
                           u.Username,
                           u.Password,
                           u.Email,
                           u.BirthDate,
                           u.Gender,
                       };
            return Request.CreateResponse(HttpStatusCode.NotFound, user);
        }
    }
}
