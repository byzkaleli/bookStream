using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using bookStream.Models;
using bookStream.Data;

namespace bookStream.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Post> GetPostById(int id)
        {
            return await _context.Posts
                                 .Include(p => p.PostLikes)
                                 .Include(p => p.User)
                                 .Include(p => p.Book)
                                 .Include(p => p.Type)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Post>> GetPost()
        {
            return await _context.Posts
                                 .Include(p => p.PostLikes)
                                 .Include(p => p.User)
                                 .Include(p => p.Book)
                                 .Include(p => p.Type)
                                 .ToListAsync();
        }

        public async Task<Post> AddPost(Post post)
        {
            var user = await _context.Users.FindAsync(post.UserId);
            if (user == null)
            {
                throw new Exception("Geçersiz UserId.");
            }
            post.User = user;

            var book = await _context.Books.FindAsync(post.BookId);
            if (book == null)
            {
                throw new Exception("Geçersiz BookId.");
            }
            post.Book = book;

            var type = await _context.PostTypes.FindAsync(post.TypeId);
            if (type == null)
            {
                throw new Exception("Geçersiz TypeId.");
            }
            post.Type = type;

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<Post> UpdatePost(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task DeletePost(int id)
        {
            var post = await GetPostById(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
