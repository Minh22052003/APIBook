using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFont.Models
{
    public class BookHistory
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string DescriptionB { get; set; }
        public string Publisher { get; set; }
        public List<string> Authors { get; set; }
        public List<string> Categories { get; set; }
        public string PublishedDate { get; set; }
        public double Rating { get; set; }
        public string BookLink { get; set; }
        public string CoverImage { get; set; }
        public DateTime ReadTime { get; set; }

        public BookHistory()
        {
            Authors = new List<string>();
            Categories = new List<string>();
        }
    }
}