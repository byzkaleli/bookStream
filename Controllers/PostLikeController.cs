using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using bookStream.Models;

[Route("api/[controller]")]
[ApiController]
public class PostLikeController : ControllerBase
{
    private readonly IPostLikeRepository _postLikeRepository;

    public PostLikeController(IPostLikeRepository postLikeRepository)
    {
        _postLikeRepository = postLikeRepository;
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<PostLike>>>> GetAll()
    {
        var postLikes = await _postLikeRepository.GetPostLike();
        return Ok(Response<IEnumerable<PostLike>>.SuccessResponse(postLikes));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<PostLike>>> GetById(int id)
    {
        var postLike = await _postLikeRepository.GetPostLikeById(id);
        if (postLike == null) return NotFound(Response<PostLike>.ErrorResponse("PostLike bulunamadı!"));
        return Ok(Response<PostLike>.SuccessResponse(postLike));
    }

    [HttpPost]
    public async Task<ActionResult<Response<PostLike>>> Add(PostLike postLike)
    {
        var exists = await _postLikeRepository.ExistsPostLike(postLike.UserId, postLike.PostId);
        if (exists) return BadRequest(Response<PostLike>.ErrorResponse("User bu gönderiyi zaten beğendi!"));

        await _postLikeRepository.AddPostLike(postLike);
        return CreatedAtAction(nameof(GetById), new { id = postLike.Id }, Response<PostLike>.SuccessResponse(postLike, "Beğeni başarıyla eklendi."));
    }

    [HttpDelete]
    public async Task<ActionResult<Response<string>>> Remove([FromBody] PostLike postLike)
    {
        var existingLike = await _postLikeRepository.GetPostLikeByUserAndPost(postLike.UserId, postLike.PostId);
        if (existingLike == null) return NotFound(Response<string>.ErrorResponse("Beğeni bulunamadı!"));

        await _postLikeRepository.RemovePostLike(existingLike);
        return Ok(Response<string>.SuccessResponse("Beğeni başarıyla silindi."));
    }
}
