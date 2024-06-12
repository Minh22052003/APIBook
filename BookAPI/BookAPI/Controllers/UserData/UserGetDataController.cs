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
        // api/UserGetData/Get_User?iduser=1
        //Các hàm get
        [HttpGet]
        public HttpResponseMessage Get_User(int iduser)
        {
            var user = from u in _context.Users
                       where u.UserID == iduser
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
        //Gọi API api/UserPostData/Get_UserName
        [HttpGet]
        public HttpResponseMessage Get_UserName()
        {
            var username = from n in _context.Users
                            select new
                            {
                                n.Username,
                            };
            return Request.CreateResponse(HttpStatusCode.OK, username);
        }
    }
}
