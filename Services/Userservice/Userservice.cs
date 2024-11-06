using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Data.Models;
using Ecommerse_shoes_backend.Dbcontext;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerse_shoes_backend.Services.Userservice
{
    public class Userservice : IUserservice
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public Userservice(ApplicationContext context,IMapper mapper,IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<string> Register(UserDto userdto)
        {
            var exist=await  _context.Users.FirstOrDefaultAsync(u => u.Email == userdto.Email);
            if (exist != null)
            {
                return ("User Already Exist");

            }
            var user = _mapper.Map<User>(userdto);
            user.Role = "User";
            user.Password = BCrypt.Net.BCrypt.HashPassword(userdto.Password); 
            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return $"Registration failed: {ex.InnerException?.Message ?? ex.Message}";
            }

            return "Registered Successfully";
        }
        public async Task<LoginDto> Login(Login login)
        {

            var exist = await _context.Users.FirstOrDefaultAsync(u => u.Email ==login.Email);
            if (exist != null)
            {
                var pass = BCrypt.Net.BCrypt.Verify(login.Password, exist.Password);
                    if (pass)
                    {
                        if (!exist.Isblocked) 
                        {
                            var token = GenerateJwtToken(exist);
                        return new LoginDto { Token = token, Error = "No error", Email = exist.Email };
                        }
                        return new LoginDto { Error = "user is blocked" };
                    }
                    return new LoginDto { Error = "wrong password" };
            }
                return new LoginDto { Error = "not found" };
        }

        public async Task<IEnumerable<AdminDto>> GetUser()
        {
            try
            {
                //var user =await _context.Users.ToListAsync();
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Role == "User");

                var admindto = _mapper.Map<IEnumerable<AdminDto>>(user);
                return admindto;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<AdminDto> GetUserById(int id)
        {
            try
            {
                //var exist = await _context.Users.FindAsync(id);
                var exist = await _context.Users.FirstOrDefaultAsync(u=>u.Id == id && u.Role=="User");

                if (exist == null)
                {
                    return null;
                }
                var user = _mapper.Map<AdminDto>(exist);
                return user;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);    
            }

        }

        public async Task<bool> Block(int id)
        {
            try
            {
                //var user = await _context.Users.FindAsync(id);
                var user = await _context.Users.FirstOrDefaultAsync(u=>u.Equals(id) && u.Role=="User");

                if (user == null)
                {
                    return false;
                }
                user.Isblocked = true;
                await _context.SaveChangesAsync();
                return true;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Unblock(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if(user == null)
                {
                    return false;
                }
                user.Isblocked = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


}

