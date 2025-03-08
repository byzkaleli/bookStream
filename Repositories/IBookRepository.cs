using bookStream.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookStream.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllBooks();
        Task<Book> GetBookById(int id);
        Task<Book> AddBook(Book book);
        Task<Book> UpdateBook(int bookId, Book updatedBook);
        Task<bool> DeleteBook(int id);
    }
}