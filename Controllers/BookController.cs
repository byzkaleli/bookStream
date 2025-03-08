using bookStream.Models;
using bookStream.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bookStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // Get all books
        [HttpGet]
        public async Task<ActionResult<Response<List<Book>>>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBooks();
            var successResponse = Response<List<Book>>.SuccessResponse(books, "Kitaplar başarıyla listelendi.");
            return Ok(successResponse);
        }

        // Get a book by Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Book>>> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
            {
                var errorResponse = Response<Book>.ErrorResponse("Tür bulunamadı.");
                return NotFound(errorResponse);
            }

            var successResponse = Response<Book>.SuccessResponse(book, "Kitap başarıyla bulundu.");
            return Ok(successResponse);
        }

        // Add a new book
        [HttpPost]
        public async Task<ActionResult<Response<Book>>> AddBook([FromBody] Book book)
        {
            var addedBook = await _bookRepository.AddBook(book);
            var successResponse = Response<Book>.SuccessResponse(addedBook, "Kitap başarıyla eklendi.");
            return CreatedAtAction(nameof(GetBookById), new { id = addedBook.Id }, successResponse);
        }

        // Update an existing book
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<Book>>> UpdateBook(int id, [FromBody] Book updatedBook)
        {
            var book = await _bookRepository.UpdateBook(id, updatedBook);
            if (book == null)
            {
                var errorResponse = Response<Book>.ErrorResponse("Kitap bulunamadı.");
                return NotFound(errorResponse);
            }
            var successResponse = Response<Book>.SuccessResponse(book, "Tür başarıyla güncellendi.");
            return Ok(successResponse);
        }

        // Delete a book
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<bool>>> DeleteBook(int id)
        {
            var success = await _bookRepository.DeleteBook(id);
            if (!success)
            {
                var errorResponse = Response<Book>.ErrorResponse("Kitap bulunamadı.");
                return NotFound(errorResponse);
            }
            var successResponse = Response<Book>.SuccessResponse(null, "Tür başarıyla güncellendi.");
            return Ok(successResponse);
        }
    }
}
