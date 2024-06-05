namespace BookAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FavoriteBook
    {
        [Key]
        public int FavoriteID { get; set; }

        public int UserID { get; set; }

        public int BookID { get; set; }

        public virtual Book Book { get; set; }

        public virtual User User { get; set; }
    }
}
