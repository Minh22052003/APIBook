using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BookAPI.Models
{
    public partial class ModelBook : DbContext
    {
        public ModelBook()
            : base("name=ModelBook")
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<FavoriteBook> FavoriteBooks { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<ReadingHistory> ReadingHistories { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasMany(e => e.Books)
                .WithMany(e => e.Authors)
                .Map(m => m.ToTable("Books_Authors").MapLeftKey("author_id").MapRightKey("book_id"));

            modelBuilder.Entity<Book>()
                .HasMany(e => e.FavoriteBooks)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.ReadingHistories)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Reviews)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Categories)
                .WithMany(e => e.Books)
                .Map(m => m.ToTable("Books_Categories").MapLeftKey("book_id").MapRightKey("category_id"));

            modelBuilder.Entity<Publisher>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.Publisher)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.FavoriteBooks)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ReadingHistories)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Reviews)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
