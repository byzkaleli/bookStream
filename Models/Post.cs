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

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [Required]
    public int BookId { get; set; }

    [ForeignKey("BookId")]
    public Book? Book { get; set; }

    [Required]
    public int TypeId { get; set; }

    [ForeignKey("TypeId")]
    public PostType? Type { get; set; }

    [MaxLength(1000)]
    public string Text { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    // Post-PostLike ilişkisi
    [JsonIgnore]
    public ICollection<PostLike>? PostLikes { get; set; }
}
