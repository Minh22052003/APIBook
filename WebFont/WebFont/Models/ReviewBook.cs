using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFont.Models
{
    public class ReviewBook
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Content { get; set; }
        public string Rating { get; set; }

        [JsonProperty("n")]
        public string Count { get; set; }
        public string ReviewTime { get; set; }
    }
}