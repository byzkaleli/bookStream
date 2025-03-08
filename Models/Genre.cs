using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bookStream.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [JsonIgnore]
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
