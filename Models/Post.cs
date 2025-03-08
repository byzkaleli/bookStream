using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using bookStream.Models;

public class Post
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId"), JsonIgnore]
    public User? User { get; set; } // Kullanıcı ilişkisi

    [Required]
    public int BookId { get; set; }

    [ForeignKey("BookId"), JsonIgnore]
    public Book? Book { get; set; } // Kitap ilişkisi

    [Required]
    public int TypeId { get; set; }

    [ForeignKey("TypeId"), JsonIgnore]
    public PostType? Type { get; set; } // Post tipi ilişkisi

    [MaxLength(1000)]
    public string Text { get; set; }

    [JsonIgnore]
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    // Post-PostLike ilişkisi
    [JsonIgnore]
    public ICollection<PostLike>? PostLikes { get; set; }
}
