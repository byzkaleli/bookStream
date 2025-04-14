using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace bookStream.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public Author? Author { get; set; }

        [MaxLength(13)]
        public string ISBN { get; set; }

        public int PublishedYear { get; set; }

        public int PageCount { get; set; }

        [MaxLength(255)]
        public string? Publisher { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "TEXT")]
        public string? CoverImage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }

        [Required]
        public int GenreId { get; set; }

        public Genre? Genre { get; set; }

        [JsonIgnore]
        public ICollection<Post>? Posts { get; set; }
    }
}
