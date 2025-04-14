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
        public async Task<ActionResult<Response<List<Post>>>> GetPosts()
        {
            try
            {
                var posts = await _postRepository.GetPost();

                if (posts == null || !posts.Any())
                {
                    return NotFound(Response<List<Post>>.ErrorResponse("Gönderi bulunamadı!"));
                }

                var successResponse = Response<List<Post>>.SuccessResponse(posts, "Gönderiler başarıyla listelendi.");
                return Ok(successResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Response<List<Post>>.ErrorResponse("Sunucu hatası"));
            }
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
        public async Task<ActionResult<Response<Post>>> PostPost(PostDto postDto)
        {
            try
            {
                var createdPost = await _postRepository.AddPost(postDto);
                return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id },
                    Response<Post>.SuccessResponse(createdPost, "Post created successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(Response<Post>.ErrorResponse($"Post oluşturulurken hata oluştu: {ex.Message}"));
            }
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

        [HttpGet("byUserId/alinti")]
        public async Task<ActionResult<Response<List<Post>>>> GetPostsByUserIdAlinti(int userId)
        {
            var posts = await _postRepository.GetPostsByUserIdAndTypeId(userId, 1);

            if (posts == null || posts.Count == 0)
            {
                return NotFound(Response<string>.ErrorResponse("No Alinti found for the given userId and typeId."));
            }

            return Ok(Response<List<Post>>.SuccessResponse(posts, "Posts fetched successfully."));
        }
        
        [HttpGet("byUserId/ozet")]
        public async Task<ActionResult<Response<List<Post>>>> GetPostsByUserIdOzet(int userId)
        {
            var posts = await _postRepository.GetPostsByUserIdAndTypeId(userId, 2);

            if (posts == null || posts.Count == 0)
            {
                return NotFound(Response<string>.ErrorResponse("No Ozet found for the given userId and typeId."));
            }

            return Ok(Response<List<Post>>.SuccessResponse(posts, "Posts fetched successfully."));
        }

        [HttpGet("byBookId/alinti")]
        public async Task<ActionResult<Response<List<Post>>>> GetPostsByBookIdAlinti(int bookId)
        {
            var posts = await _postRepository.GetPostsByBookIdAndTypeId(bookId, 1);

            if (posts == null || posts.Count == 0)
            {
                return NotFound(Response<string>.ErrorResponse("No Alinti found for the given bookId and typeId."));
            }

            return Ok(Response<List<Post>>.SuccessResponse(posts, "Posts fetched successfully."));
        }

        [HttpGet("byBookId/ozet")]
        public async Task<ActionResult<Response<List<Post>>>> GetPostsByBookIdOzet(int bookId)
        {
            var posts = await _postRepository.GetPostsByBookIdAndTypeId(bookId, 2);

            if (posts == null || posts.Count == 0)
            {
                return NotFound(Response<string>.ErrorResponse("No Ozet found for the given bookId and typeId."));
            }

            return Ok(Response<List<Post>>.SuccessResponse(posts, "Posts fetched successfully."));
        }
    }
}
