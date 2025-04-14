using bookStream.Models;

namespace bookStream.Repositories
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAllGenres();
        Task<Genre> GetGenreById(int id);
        Task<Genre> AddGenre(Genre genre);
        Task<bool> DeleteGenre(int id);
    }
}
