using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Data.Models;
using Ecommerse_shoes_backend.Dbcontext;
using Microsoft.EntityFrameworkCore;

namespace Ecommerse_shoes_backend.Services.WishListService
{
    public class Wishlistservice:IWishlistservice
    {
        private readonly ApplicationContext _context;

        public Wishlistservice(ApplicationContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<WishlistDto>> Get(int id)
        {
            try
            {
                var user = await _context.Users.Include(w => w.Wishlist).ThenInclude(p => p.Products).ThenInclude(c => c.Category).FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (user.Wishlist == null || !user.Wishlist.Any())
                {
                    return new List<WishlistDto>();
                }

                var item = user.Wishlist.Select(p => new WishlistDto
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    Title = p.Products?.Title,
                    Description = p.Products?.Description,
                    ImageUrl = p.Products?.ImageUrl,
                    Price = ((decimal)p.Products?.Price),
                    Category_Name = p.Products.Category?.Category_Name
                });
                
                return item.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<bool> Add(int id, int productId)
        {
            try
            {
                var user = await _context.Users.Include(w => w.Wishlist).ThenInclude(p => p.Products).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var item = user.Wishlist.FirstOrDefault(p => p.ProductId == productId);
                if (item == null)
                {
                    var wishlist = new Wishlist { UserId = id, ProductId = productId };
                    user.Wishlist.Add(wishlist);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

       

        public async Task<bool> Remove(int id, int productId)
        {
            try
            {
                var user = await _context.Users.Include(w => w.Wishlist).ThenInclude(p => p.Products).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var product = user.Wishlist.FirstOrDefault(p => p.ProductId == productId);
                if (product == null)
                {
                    return false;
                }
                user.Wishlist.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
