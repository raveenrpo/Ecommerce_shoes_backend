using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Data.Models;

namespace Ecommerse_shoes_backend.Services.Userservice
{
    public interface IUserservice
    {
        Task<string> Register(UserDto user);
        Task<LoginDto> Login(Login login);
        Task<IEnumerable<AdminDto>> GetUser();
        Task<AdminDto> GetUserById(int id);
        Task<bool> Block(int id);
        Task<bool>Unblock(int id);
    }
}
