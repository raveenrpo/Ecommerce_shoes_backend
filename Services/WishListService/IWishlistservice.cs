using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Data.Models;

namespace Ecommerse_shoes_backend.Services.WishListService
{
    public interface IWishlistservice
    {
        Task<IEnumerable<WishlistDto>> Get(int id);
        Task<bool> Add(int id,int productId);
        Task<bool> Remove(int id, int productId);


    }
}
