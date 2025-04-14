using bookStream.Models;

namespace bookStream.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(int id);
        Task AddAsync(Author entity);
        Task UpdateAsync(Author entity);
        Task DeleteAsync(int id);
    }
}
