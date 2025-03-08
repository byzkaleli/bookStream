using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using bookStream.Models;
using bookStream.Data;

public class PostLikeRepository : IPostLikeRepository
{
    private readonly ApplicationDbContext _context;

    public PostLikeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PostLike>> GetPostLike()
    {
        return await _context.PostLikes.ToListAsync();
    }

    public async Task<PostLike> GetPostLikeById(int id)
    {
        return await _context.PostLikes.FindAsync(id);
    }

    public async Task<PostLike> GetPostLikeByUserAndPost(int userId, int postId)
    {
        return await _context.PostLikes.FirstOrDefaultAsync(pl => pl.UserId == userId && pl.PostId == postId);
    }

    public async Task AddPostLike(PostLike postLike)
    {
        var user = await _context.Users.FindAsync(postLike.UserId);
        if (user == null)
        {
            throw new Exception("Geçersiz UserId.");
        }
        postLike.User = user;


        var post = await _context.Posts.FindAsync(postLike.PostId);
        if (post == null)
        {
            throw new Exception("Geçersiz PostId.");
        }
        postLike.Post = post;


        await _context.PostLikes.AddAsync(postLike);
        await _context.SaveChangesAsync();
    }

    public async Task RemovePostLike(PostLike postLike)
    {
        _context.PostLikes.Remove(postLike);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsPostLike(int userId, int postId)
    {
        return await _context.PostLikes.AnyAsync(pl => pl.UserId == userId && pl.PostId == postId);
    }
}
