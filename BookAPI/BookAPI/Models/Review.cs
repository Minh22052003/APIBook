namespace BookAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Review
    {
        public int ReviewID { get; set; }

        public int UserID { get; set; }

        public int BookID { get; set; }

        public double Rating { get; set; }

        public string Content { get; set; }

        public DateTime ReviewTime { get; set; }

        public virtual Book Book { get; set; }

        public virtual User User { get; set; }
    }
}
