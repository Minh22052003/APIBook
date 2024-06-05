namespace BookAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReadingHistory")]
    public partial class ReadingHistory
    {
        [Key]
        public int HistoryID { get; set; }

        public int UserID { get; set; }

        public int BookID { get; set; }

        public DateTime ReadTime { get; set; }

        public virtual Book Book { get; set; }

        public virtual User User { get; set; }
    }
}
