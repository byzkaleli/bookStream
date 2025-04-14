using Microsoft.AspNetCore.Mvc;
using bookStream.Models;
using bookStream.Repositories;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<Response<IEnumerable<User>>>> GetAll()
    {
        var users = await _userRepository.GetUsers();
        return Ok(Response<IEnumerable<User>>.SuccessResponse(users));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Response<User>>> GetById(int id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null) return NotFound(Response<User>.ErrorResponse("User not found"));

        return Ok(Response<User>.SuccessResponse(user));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Response<User>>> Update(int id, User user)
    {
        if (id != user.Id) return BadRequest(Response<User>.ErrorResponse("Invalid user ID"));

        var updated = await _userRepository.UpdateUser(user);
        if (!updated) return BadRequest(Response<User>.ErrorResponse("Update failed"));

        return Ok(Response<User>.SuccessResponse(user, "User updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Response<string>>> Delete(int id)
    {
        var deleted = await _userRepository.DeleteUser(id);
        if (!deleted) return NotFound(Response<string>.ErrorResponse("User not found"));

        return Ok(Response<string>.SuccessResponse("User deleted successfully"));
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<Response<UserProfileDto>>> GetProfile()
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return Unauthorized(Response<UserProfileDto>.ErrorResponse("User not authenticated."));

        var user = await _userRepository.GetUserByUsername(username);
        if (user == null)
            return NotFound(Response<UserProfileDto>.ErrorResponse("User not found."));

        var userProfile = new UserProfileDto
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Name,
            Surname = user.Surname,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            ProfilePhoto = user.ProfilePhoto,
            Role = user.Role,
            IsEmailConfirmed = user.IsEmailConfirmed
        };

        return Ok(Response<UserProfileDto>.SuccessResponse(userProfile));
    }

}
