using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookAPI.Models
{
    public class BookdataFrAPI
    {
        public string Kind { get; set; }
        public int TotalItems { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public string Id { get; set; }
        public VolumeInfo VolumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        public string Title { get; set; }
        public List<Author> Authors { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public string PublishedDate { get; set; }
        public ImageLinks ImageLinks { get; set; }
        public string InfoLink { get; set; }
    }

    public class ImageLinks
    {
        public string SmallThumbnail { get; set; }
        public string Thumbnail { get; set; }
        public string Large { get; set; }
    }

}