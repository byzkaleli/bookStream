using bookStream.Models;
using Microsoft.EntityFrameworkCore;

namespace bookStream.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Burada veritabanı tablolarını temsil eden DbSet'leri tanımlayacağız.
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostType> PostTypes { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<Author> Authors { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
            .HasOne(b => b.Genre)
            .WithMany(g => g.Books)
            .HasForeignKey(b => b.GenreId);

            // Book-Post ilişkisi (eğer postlar ile bağlantılı olacaksa)
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Posts)
                .WithOne(p => p.Book)
                .HasForeignKey(p => p.BookId);

            // Post-PostType ilişkisi
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Type)
                .WithMany(t => t.Posts)
                .HasForeignKey(p => p.TypeId);

            // Post-User ilişkisi
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);

            // Post-Book ilişkisi
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Book)
                .WithMany(b => b.Posts)
                .HasForeignKey(p => p.BookId);

            // PostLike-Post ilişkisi
            modelBuilder.Entity<PostLike>()
                .HasOne(pl => pl.Post)
                .WithMany(p => p.PostLikes)
                .HasForeignKey(pl => pl.PostId);

            // PostLike-User ilişkisi
            modelBuilder.Entity<PostLike>()
                .HasOne(pl => pl.User)
                .WithMany(u => u.PostLikes)
                .HasForeignKey(pl => pl.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            // User-PostLike ilişkisi
            modelBuilder.Entity<User>()
                .HasMany(u => u.PostLikes)
                .WithOne(pl => pl.User)
                .HasForeignKey(pl => pl.UserId);
                
        }
    }
}
