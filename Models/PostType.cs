using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class PostType
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; }

    [JsonIgnore]
    public ICollection<Post> Posts { get; set; } // Post'lar ile ilişki
}
