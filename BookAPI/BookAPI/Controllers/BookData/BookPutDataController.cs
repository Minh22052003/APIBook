using BookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BookAPI.Controllers.BookData
{
    public class BookPutDataController : ApiController
    {
        private readonly ModelBook _context;
        public BookPutDataController()
        {
            _context = new ModelBook();
        }
        // api/BookPutData/UpdateReview
        [HttpPut]
        public HttpResponseMessage UpdateReview(Review review)
        {
            try
            {
                var reviewtmp = _context.Reviews.FirstOrDefault(r=>r.ReviewID == review.ReviewID);
                if (reviewtmp != null)
                {
                    reviewtmp.Rating = review.Rating;
                    reviewtmp.Content = review.Content;
                    reviewtmp.ReviewTime = review.ReviewTime;
                    _context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, reviewtmp);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

        }
    }
}
