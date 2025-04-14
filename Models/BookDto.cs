using System;

namespace bookStream.Models
{
    public class BookDto
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public string ISBN { get; set; }
        public int PublishedYear { get; set; }
        public string? Publisher { get; set; }
        public int PageCount { get; set; }
        public string? Description { get; set; }
        public string? CoverImage { get; set; }
        public int GenreId { get; set; }
    }
}