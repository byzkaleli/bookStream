using Microsoft.AspNetCore.Mvc;
using bookStream.Models;
using bookStream.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace bookStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<Post>>>> GetPosts()
        {
            var posts = await _postRepository.GetPost();
            return Ok(Response<IEnumerable<Post>>.SuccessResponse(posts));
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Post>>> GetPost(int id)
        {
            var post = await _postRepository.GetPostById(id);
            if (post == null)
            {
                return NotFound(Response<Post>.ErrorResponse("Gönderi bulunamadı!"));
            }
            return Ok(Response<Post>.SuccessResponse(post));
        }

        // POST: api/Post
        [HttpPost]
        public async Task<ActionResult<Response<Post>>> PostPost(Post post)
        {
            var createdPost = await _postRepository.AddPost(post);
            return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, Response<Post>.SuccessResponse(createdPost, "Post created successfully"));
        }

        // PUT: api/Post/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<string>>> PutPost(int id, Post post)
        {
            if (id != post.Id)
            {
                return BadRequest(Response<string>.ErrorResponse("GönderiId eşleşmedi!"));
            }

            await _postRepository.UpdatePost(post);
            return Ok(Response<string>.SuccessResponse("Gönderi başarıyla güncellendi."));
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeletePost(int id)
        {
            var post = await _postRepository.GetPostById(id);
            if (post == null)
            {
                return NotFound(Response<string>.ErrorResponse("Gönderi bulunamadı!"));
            }

            await _postRepository.DeletePost(id);
            return Ok(Response<string>.SuccessResponse("Gönderi başarıyla silindi."));
        }
    }
}
