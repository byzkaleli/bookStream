using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace bookStream.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Column(TypeName = "TEXT")]
        [Required]
        [StringLength(200, MinimumLength = 8)]
        [JsonIgnore]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [Column(TypeName = "TEXT")]
        public string? ProfilePhoto { get; set; }

        public string Role { get; set; } = "User";
        public bool IsEmailConfirmed { get; set; } = false;
        public string? Token { get; set; }

        [JsonIgnore]
        public ICollection<Post>? Posts { get; set; }
        [JsonIgnore]
        public ICollection<PostLike>? PostLikes { get; set; }
    }
}