
namespace bookStream.Repositories
{
    public interface IPostRepository
    {
        Task<Post> GetPostById(int id);
        Task<IEnumerable<Post>> GetPost();
        Task<Post> AddPost(Post post);
        Task<Post> UpdatePost(Post post);
        Task DeletePost(int id);
    }
}
