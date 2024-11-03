

using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Data.Models;
using Ecommerse_shoes_backend.Services.Userservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserservice _userservice;

    public UserController(IUserservice userservice)
    {
        _userservice = userservice;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<string>> Register(UserDto userDto)
    {
        try
        {
            var result = await _userservice.Register(userDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<ActionResult<LoginDto>> Login(Login login)
    {
        try
        {
            var exist = await _userservice.Login(login);
            if (exist.Error == "not found")
            {
                return NotFound();
            }
            if (exist.Error == "user is blocked")
            {
                return BadRequest("User is blocked by admin");
            }
            if (exist.Error == "wrong password")
            {
                return BadRequest("Password is incorrect");
            }
            return Ok(new LoginDto { Token = exist.Token, Error = "Login successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("GetAllUser")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        try
        {
            var users = await _userservice.GetUser();
            if (!users.Any())
            {
                return NotFound("Users Not Found");
            }
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("GetUserByID")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> GetUserById([FromQuery] int id)
    {
        try
        {
            var user = await _userservice.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("Block")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Block(int id)
    {
        try
        {
            var block = await _userservice.Block(id);
            if (block)
            {
                return Ok(new { message = "User has been blocked by admin." });
            }
            return BadRequest("User not found");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("UnBlock")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UnBlock(int id)
    {
        try
        {
            var block = await _userservice.Unblock(id);
            if (block)
            {
                return Ok(new { message = "User has been Unblocked by admin." });
            }
            return BadRequest("User not found");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [NonAction]
    public IActionResult ShowHttpContextItems()
    {
        var items = HttpContext.Items;

        // Iterate through each key-value pair in HttpContext.Items
        foreach (var key in items.Keys)
        {
            var value = items[key];
            Console.WriteLine($"{key}: {value}");
        }

        return Ok("Check the console for HttpContext.Items output.");
    }

}
