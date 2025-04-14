using bookStream.Models;

namespace bookStream.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetPostById(int id);
        Task<List<Post>> GetPost();
        Task<Post> AddPost(PostDto post);
        Task<Post> UpdatePost(Post post);
        Task DeletePost(int id);
        Task<List<Post>> GetPostsByUserIdAndTypeId(int userId, int typeId);
        Task<List<Post>> GetPostsByBookIdAndTypeId(int bookId, int typeId);
    }
}
