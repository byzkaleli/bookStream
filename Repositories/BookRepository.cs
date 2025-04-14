using bookStream.Data;
using bookStream.Models;
using Microsoft.EntityFrameworkCore;

namespace bookStream.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Tüm türleri getir
        public async Task<List<Book>> GetAllBooks()
        {
            return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .ToListAsync();
        }

        // Yeni tür ekle
        public async Task<Book> AddBook(Book book)
        {
            var genre = await _context.Genres.FindAsync(book.GenreId);
            if (genre == null)
            {
                throw new Exception("Geçersiz GenreId.");
            }
            book.Genre = genre;
            
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        // Türü sil
        public async Task<bool> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

        // ID'ye göre türü getir
        public async Task<Book> GetBookById(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> UpdateBook(int bookId, Book updatedBook)
        {
            var existingBook = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);

            // Update the properties of the existing book
            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.ISBN = updatedBook.ISBN;
            existingBook.PublishedYear = updatedBook.PublishedYear;
            existingBook.PageCount = updatedBook.PageCount;
            existingBook.Publisher = updatedBook.Publisher;
            existingBook.Description = updatedBook.Description;
            existingBook.CoverImage = updatedBook.CoverImage;
            existingBook.UpdatedAt = DateTime.UtcNow;  // Set the update timestamp

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return existingBook;
        }

    }
}
