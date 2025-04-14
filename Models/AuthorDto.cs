using System;
using System.Text.Json.Serialization;

namespace bookStream.DTOs
{
    public class AuthorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Photo { get; set; }
    }
}
