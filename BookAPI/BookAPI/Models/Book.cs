namespace BookAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Book")]
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            FavoriteBooks = new HashSet<FavoriteBook>();
            ReadingHistories = new HashSet<ReadingHistory>();
            Reviews = new HashSet<Review>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public int AuthorID { get; set; }

        public int CategoryID { get; set; }

        [Required]
        public string DescriptionB { get; set; }

        public int PublisherID { get; set; }

        [StringLength(50)]
        public string PublishedDate { get; set; }

        [StringLength(255)]
        public string CoverImage { get; set; }

        [StringLength(255)]
        public string BookLink { get; set; }

        public virtual Author Author { get; set; }

        public virtual Category Category { get; set; }

        public virtual Publisher Publisher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FavoriteBook> FavoriteBooks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReadingHistory> ReadingHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
