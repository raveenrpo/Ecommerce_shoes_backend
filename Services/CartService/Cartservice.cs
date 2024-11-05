using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Data.Models;
using Ecommerse_shoes_backend.Dbcontext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
namespace Ecommerse_shoes_backend.Services.CartService
{
    public class Cartservice : ICartservice
    {
        private readonly ApplicationContext _context;
        public Cartservice(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<CartDto>> GetCartItems(int id)
        {
            try
            {
                var user = await _context.Users.Include(c => c.Cart).ThenInclude(ci => ci.CartItems).ThenInclude(p => p.Products).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                if (user.Cart == null || !user.Cart.CartItems.Any())
                {
                    return new List<CartDto>();
                }

                var items = user.Cart.CartItems.Select(p => new CartDto
                {
                    Id = p.Id,
                    Title = p.Products.Title,
                    Description = p.Products.Description,
                    ImageUrl = p.Products.ImageUrl,
                    Price = p.Products.Price,
                    Quantity = p.Quantity,
                    Total = p.Quantity * p.Products.Price
                });
                return  items.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> AddToCart(int id, int productid)
        {
            try
            {
                var user = await _context.Users.Include(c => c.Cart).ThenInclude(ci => ci.CartItems).ThenInclude(p => p.Products).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                if (user.Cart == null)
                {
                    user.Cart = new Cart { UserId = id, CartItems = new List<CartItem>() };
                    await _context.Carts.AddAsync(user.Cart);
                    await _context.SaveChangesAsync();
                }
                var item = user.Cart.CartItems.FirstOrDefault(c => c.ProductId == productid);
                if (item != null)
                {
                    return false;
                }
                var cartitem = new CartItem { CartId = user.Cart.Id, ProductId = productid, Quantity = 1 };
                user.Cart.CartItems.Add(cartitem);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveFromCart(int id, int productid)
        {
            try
            {
                var user = await _context.Users.Include(c => c.Cart).ThenInclude(ci => ci.CartItems).ThenInclude(p => p.Products).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var item = user.Cart.CartItems.FirstOrDefault(p => p.ProductId == productid);
                if (item == null)
                {
                    return false;
                }
                user.Cart.CartItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public async Task<bool> DecrimentQuantity(int id, int productid)
        {
            try
            {
                var user = await _context.Users.Include(c => c.Cart).ThenInclude(ci => ci.CartItems).ThenInclude(p => p.Products).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    throw new Exception("user not found");
                }
                var item = user.Cart.CartItems.FirstOrDefault(p => p.ProductId == productid);
                if (item == null)
                {
                    return false;
                }
                if (item.Quantity == 1)
                {
                    return false;
                }
                item.Quantity = item.Quantity - 1;
                //item.Products.Stock = item.Products.Stock + 1;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IncrimentQuantity(int id, int productid)
        {
            try
            {
                var user = await _context.Users.Include(c => c.Cart).ThenInclude(ci => ci.CartItems).ThenInclude(p => p.Products).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    throw new Exception("user not found");
                }
                var item = user.Cart.CartItems.FirstOrDefault(u => u.ProductId == productid);
                if (item == null)
                {
                    return false;
                }
                item.Quantity += 1;
                //item.Products.Stock = item.Products.Stock - 1;
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
