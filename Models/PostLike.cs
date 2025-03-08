using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using bookStream.Models;

public class PostLike
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PostId { get; set; }
    
    [ForeignKey("PostId"), JsonIgnore]
    public Post? Post { get; set; } // İlişki

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId"), JsonIgnore]
    public User? User { get; set; } // İlişki
}
