using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFont.Models
{
    public class Category
    {
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string CategoryName { get; set; }
    }
}