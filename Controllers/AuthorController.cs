using Microsoft.AspNetCore.Mvc;
using bookStream.DTOs;
using bookStream.Models;
using bookStream.Repositories;

namespace bookStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<Author>>>> GetAuthors()
        {
            var authors = await _authorRepository.GetAllAsync();

            if (authors == null || !authors.Any())
            {
                return NotFound(new Response<IEnumerable<Author>>(false, null, "No authors found."));
            }

            authors = authors.Select(a => new Author
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Photo = a.Photo
            }).ToList();

            return Ok(new Response<IEnumerable<Author>>(true, authors, "Authors retrieved successfully."));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<AuthorDto>>> GetAuthor(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound(new Response<AuthorDto>(false, null, "Author not found."));
            }

            var authorDto = new AuthorDto
            {
                FirstName = author.FirstName,
                LastName = author.LastName,
                Photo = author.Photo
            };

            return Ok(new Response<AuthorDto>(true, authorDto, "Author retrieved successfully."));
        }

        [HttpPost]
        public async Task<ActionResult<Response<AuthorDto>>> CreateAuthor(AuthorDto authorDto)
        {
            var author = new Author
            {
                FirstName = authorDto.FirstName,
                LastName = authorDto.LastName,
                Photo = authorDto.Photo,
                CreatedAt = DateTime.UtcNow
            };

            await _authorRepository.AddAsync(author);

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, new Response<AuthorDto>(true, authorDto, "Author created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorDto authorDto)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound(new Response<string>(false, null, "Author not found."));
            }

            author.FirstName = authorDto.FirstName;
            author.LastName = authorDto.LastName;
            author.Photo = authorDto.Photo;
            author.UpdatedAt = DateTime.UtcNow;

            await _authorRepository.UpdateAsync(author);

            return NoContent();  // We don't return data in this case, just a success status
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound(new Response<string>(false, null, "Author not found."));
            }

            await _authorRepository.DeleteAsync(id);

            return NoContent();  // We don't return data in this case, just a success status
        }
    }
}
