using System.ComponentModel.DataAnnotations;

public class PostType
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; }

    public ICollection<Post> Posts { get; set; } // Post'lar ile ilişki
}
