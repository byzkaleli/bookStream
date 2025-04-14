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
                                 .Include(p => p.User)
                                 .Include(p => p.Book)
                                 .Include(p => p.Type)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetPost()
        {
            return await _context.Posts
                                 .Include(p => p.User)
                                 .Include(p => p.Book)
                                 .Include(p => p.Type)
                                 .ToListAsync();
        }

        public async Task<Post> AddPost(PostDto postDto)
        {
            var user = await _context.Users.FindAsync(postDto.UserId);
            if (user == null)
            {
                throw new Exception("Geçersiz UserId.");
            }

            var book = await _context.Books.FindAsync(postDto.BookId);
            if (book == null)
            {
                throw new Exception("Geçersiz BookId.");
            }

            var type = await _context.PostTypes.FindAsync(postDto.TypeId);
            if (type == null)
            {
                throw new Exception("Geçersiz TypeId.");
            }

            var post = new Post
            {
                UserId = postDto.UserId,
                BookId = postDto.BookId,
                TypeId = postDto.TypeId,
                Text = postDto.Text,
                CreateDate = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // İlişkili verileri geri döndürmek istersen:
            post.User = user;
            post.Book = book;
            post.Type = type;

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

        public async Task<List<Post>> GetPostsByUserIdAndTypeId(int userId, int typeId)
        {
            return await _context.Posts
                                 .Where(p => p.UserId == userId && p.TypeId == typeId)
                                 .Include(p => p.User)
                                 .Include(p => p.Book)
                                 .Include(p => p.Type)
                                 .ToListAsync();
        }

        public async Task<List<Post>> GetPostsByBookIdAndTypeId(int bookId, int typeId)
        {
            return await _context.Posts
                                 .Where(p => p.BookId == bookId && p.TypeId == typeId)
                                 .Include(p => p.User)
                                 .Include(p => p.Book)
                                 .Include(p => p.Type)
                                 .ToListAsync();
        }
    }
}
