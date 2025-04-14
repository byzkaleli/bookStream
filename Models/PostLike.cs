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
    
    [ForeignKey("PostId")]
    public Post? Post { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
