
using Azure;
using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Data.Models;
using Ecommerse_shoes_backend.Services.Userservice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerse_shoes_backend.Controllers
{
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
        public async Task<ActionResult> Register(UserDto userDto)
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
        public async Task<ActionResult> Login(Login login)
        {
            try
            {
                var exist = await _userservice.Login(login);
                if(exist.Error=="not found")
                {
                    return NotFound();
                }
                if(exist.Error=="user is blocked")
                {
                    return BadRequest("user is blocked by admin");
                }
                if(exist.Error=="wrong password")
                {
                    return BadRequest("password are incorrect");
                }
                return Ok(new LoginDto { Token = exist.Token,Error="Login successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
