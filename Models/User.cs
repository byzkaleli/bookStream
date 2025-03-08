using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace bookStream.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; } // Soyisim

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Oluşturma tarihi
        public DateTime? UpdatedAt { get; set; } // Güncelleme tarihi

        [Column(TypeName = "TEXT"), JsonIgnore]
        public string? ProfilePhoto { get; set; } // Profil fotoğrafı

        [JsonIgnore]
        public string Role { get; set; } = "User";

        [JsonIgnore]
        public bool IsEmailConfirmed { get; set; } = false; // E-posta doğrulama durumu
        

        // E-posta doğrulama için kullanılan token
        [JsonIgnore]
        public string? Token { get; set; }
        
        [JsonIgnore]
        public ICollection<Post>? Posts { get; set; }

        [JsonIgnore]
        public ICollection<PostLike>? PostLikes { get; set; }
    }
}
