using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BookAPI.Models;

namespace BookAPI.Controllers.UserData
{
    public class UserPostDataController : ApiController
    {
        private readonly ModelBook _context;
        public UserPostDataController(ModelBook context)
        {
            _context = context;
        }
        public UserPostDataController()
        {
            _context = new ModelBook();
        }


        //Gọi API api/UserPostData/POST_CreateAccUser
        [HttpPost]
        public HttpResponseMessage POST_CreateAccUser(UserLogin userLogin)
        {
            if (userLogin == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid data.");
            }

            try
            {
                User user = new User();
                user.UserID = 0;
                user.Username = userLogin.Username;
                user.Password = userLogin.Password;
                user.Role = "User";
                user.Status = 1;
                _context.Users.Add(user);
                _context.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //Gọi API api/UserPostData/Login
        [HttpPost]
        public HttpResponseMessage Login(UserLogin request)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.Username == request.Username && u.Password == request.Password);

                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User not found.");
                }

                return Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



    }
}
