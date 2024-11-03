using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Data.Models;

namespace Ecommerse_shoes_backend.Services.CartService
{
    public interface ICartservice
    {
        Task<IEnumerable<CartDto>> GetCartItems(int id);
        Task<bool> AddToCart(int id, int productid);
        Task<bool> RemoveFromCart(int id,int productid);
        Task<bool> IncrimentQuantity(int id, int productid);
        Task<bool> DecrimentQuantity(int id, int productid);
    }
}
