using System;

namespace bookStream.Models
{
    public class PostDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int TypeId { get; set; }
        public string Text { get; set; }
    }
}