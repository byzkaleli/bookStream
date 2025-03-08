using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using bookStream.Models;
using bookStream.Repositories;

[Route("api/[controller]")]
[ApiController]
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
}
