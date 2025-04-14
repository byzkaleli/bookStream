using bookStream.DTOs;
using bookStream.Models;
using bookStream.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bookStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;

        public GenreController(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        // Tür ekleme (POST)
        [HttpPost]
        public async Task<IActionResult> AddGenre([FromBody] GenreDto genreDto)
        {
            if (genreDto == null || string.IsNullOrEmpty(genreDto.Name))
            {
                var errorResponse = Response<Genre>.ErrorResponse("Tür adı gereklidir.");
                return BadRequest(errorResponse);
            }

            var genre = new Genre
            {
                Name = genreDto.Name
            };

            var addedGenre = await _genreRepository.AddGenre(genre);

            var successResponse = Response<Genre>.SuccessResponse(addedGenre, "Tür başarıyla eklendi.");
            return CreatedAtAction(nameof(GetGenreById), new { id = addedGenre.Id }, successResponse);
        }

        // Tür silme (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var isDeleted = await _genreRepository.DeleteGenre(id);
            if (!isDeleted)
            {
                // Hata durumu için Response kullanımı
                var errorResponse = Response<Genre>.ErrorResponse("Tür bulunamadı.");
                return NotFound(errorResponse); // 404 Not Found
            }

            // Başarılı yanıt için Response kullanımı
            var successResponse = Response<Genre>.SuccessResponse(null, "Tür başarıyla silindi.");
            return Ok(successResponse); // 204 No Content
        }

        // Türleri listeleme (GET)
        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _genreRepository.GetAllGenres();

            // Başarılı yanıt için Response kullanımı
            var successResponse = Response<List<Genre>>.SuccessResponse(genres, "Türler başarıyla listelendi.");
            return Ok(successResponse); // 200 OK
        }

        // ID'ye göre türü alma (GET)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenreById(int id)
        {
            var genre = await _genreRepository.GetGenreById(id);
            if (genre == null)
            {
                // Hata durumu için Response kullanımı
                var errorResponse = Response<Genre>.ErrorResponse("Tür bulunamadı.");
                return NotFound(errorResponse); // 404 Not Found
            }

            // Başarılı yanıt için Response kullanımı
            var successResponse = Response<Genre>.SuccessResponse(genre, "Tür başarıyla bulundu.");
            return Ok(successResponse); // 200 OK
        }
    }
}
