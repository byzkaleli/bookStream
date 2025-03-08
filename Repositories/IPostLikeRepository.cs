using System.Collections.Generic;
using System.Threading.Tasks;
using bookStream.Models;

public interface IPostLikeRepository
{
    Task<IEnumerable<PostLike>> GetPostLike();
    Task<PostLike> GetPostLikeById(int id);
    Task<PostLike> GetPostLikeByUserAndPost(int userId, int postId);
    Task AddPostLike(PostLike postLike);
    Task RemovePostLike(PostLike postLike);
    Task<bool> ExistsPostLike(int userId, int postId);
}
