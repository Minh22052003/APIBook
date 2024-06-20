using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFont.Models
{
    public class ReviewBookPost
    {
        public int ReviewID { get; set; }

        public int UserID { get; set; }

        public int BookID { get; set; }

        public double Rating { get; set; }

        public string Content { get; set; }

        public DateTime ReviewTime { get; set; }
    }
}