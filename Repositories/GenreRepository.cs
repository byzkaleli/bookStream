using bookStream.Data;
using bookStream.Models;
using Microsoft.EntityFrameworkCore;

namespace bookStream.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Tüm türleri getir
        public async Task<List<Genre>> GetAllGenres()
        {
            return await _context.Genres.ToListAsync();
        }

        // Yeni tür ekle
        public async Task<Genre> AddGenre(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        // Türü sil
        public async Task<bool> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return false;
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return true;
        }

         // ID'ye göre türü getir
        public async Task<Genre> GetGenreById(int id)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}
