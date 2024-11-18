using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Dbcontext;
using Ecommerse_shoes_backend.Middleware;
using Razorpay.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Ecommerse_shoes_backend.Data.Models;

namespace Ecommerse_shoes_backend.Services.OrderService
{
    public class Orderservice:IOrderservice
    {
        private readonly ApplicationContext _context;
        private readonly IConfiguration _configuration;

        public Orderservice(ApplicationContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        public async Task<string> RazorOrderCreate(long price)
        {
            try
            {
                if (price<=0)
                {
                    throw new Exception("price must be a positive value");
                }
                Dictionary<string, object> razorinpt = new Dictionary<string, object>();
                string transactionId = Guid.NewGuid().ToString();
                razorinpt.Add("amount", Convert.ToDecimal(price) * 100);
                razorinpt.Add("currency", "INR");
                razorinpt.Add("receipt", transactionId);

                string key = _configuration["Razorpay:KeyId"];
                string secret = _configuration["Razorpay:KeySecret"];

                RazorpayClient client = new RazorpayClient(key, secret);
                Razorpay.Api.Order order = client.Order.Create(razorinpt);
                var orderId = order["id"].ToString();
                return orderId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Razorpay order", ex);
            }
        }


        public bool PaymentVerify(RazorDto razorDto)
        {
            try
            {
                if (razorDto == null || razorDto.razorpay_order_id == null || razorDto.razorpay_payment_id == null || razorDto.razorpay_signature == null)
                {
                    return false;
                }
                RazorpayClient client = new RazorpayClient(_configuration["Razorpay:KeyId"], _configuration["Razorpay:KeySecret"]);
                Dictionary<string, string> input = new Dictionary<string, string>();
                input.Add("razorpay_payment_id", razorDto.razorpay_payment_id);
                input.Add("razorpay_order_id", razorDto.razorpay_order_id);
                input.Add("razorpay_signature", razorDto.razorpay_signature);
                Utils.verifyPaymentSignature(input);
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Payment verification error");
            }
        }



        public async Task<bool> CreateOrder(int userId, InputorderDto inputorderDto)
        {

            try
            {
                //var userId = Convert.ToInt32(HttpContext.Items["Id"]);
                if (userId ==null)
                {
                    throw new Exception("User not found");
                }
                var user=await _context.Users.Include(c=>c.Cart).ThenInclude(ci=>ci.CartItems).ThenInclude(p=>p.Products).FirstOrDefaultAsync(u=>u.Id == userId);
                if (user.Cart==null)
                {
                    throw new Exception("User dosent have a cart");
                }
                
                var cart = user.Cart;
                var order = new Orders
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    UserName = inputorderDto.UserName,
                    Phone = inputorderDto.Phone,
                    Address = inputorderDto.Address,
                    Total = inputorderDto.Total,
                    Orderstring = inputorderDto.Orderstring,
                    TransactionId = inputorderDto.TransactionId,
                    OrderItems = user.Cart.CartItems.Select(c => new OrderItems
                    {
                        ProductId = c.ProductId,
                        Quantity = c.Quantity,
                        TotalPrice = c.Quantity * c.Products.Price,
                    }).ToList()

                };
                await _context.Orders.AddAsync(order);
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<IEnumerable<OutorderDto>> GetUserOrder(int id)
        {
            try
            {
                var user = await _context.Users.Include(oi => oi.Orders).ThenInclude(p => p.OrderItems).ThenInclude(pp => pp.Products).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return new List<OutorderDto>();
                }
                if(user.Orders==null)
                {
                    throw new Exception("user dont have any order");
                }
                var ite=await _context.Orders.Include(oi=>oi.OrderItems).FirstOrDefaultAsync(o=>o.UserId == id);
                if(ite.OrderItems==null||ite.OrderItems.Count==0)
                {
                    throw new Exception("user dont have any orderitem");

                }
                var orderlist = ite.OrderItems.Select(k => new OutorderDto
                {
                    Id = k.ProductId,
                    OrderDate=k.Orders.OrderDate,
                    Title = k.Products.Title,
                    Quantity = k.Quantity,
                    Image = $"{_configuration["Hosturl:images"]}/Products/{k.Products.ImageUrl}",
                    TotalPrice = k.TotalPrice,
                    OrderId=k.Orders.Orderstring
                }).ToList();
                return orderlist;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }


        public async Task<IEnumerable<OutorderDto>> GetUserOrderById(int id)
        {
            try
            {
                var user = await _context.Users.Include(oi => oi.Orders).ThenInclude(p => p.OrderItems).ThenInclude(pp => pp.Products).FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return new List<OutorderDto>();
                }
                if (user.Orders == null)
                {
                    throw new Exception("user dont have any order");
                }
                var ite = await _context.Orders.Include(oi => oi.OrderItems).FirstOrDefaultAsync(o => o.UserId == id);
                if (ite.OrderItems == null || ite.OrderItems.Count == 0)
                {
                    throw new Exception("user dont have any orderitem");

                }
                var orderlist = ite.OrderItems.Select(k => new OutorderDto
                {
                    Id = k.ProductId,
                    OrderDate = k.Orders.OrderDate,
                    Title = k.Products.Title,
                    Quantity = k.Quantity,
                    Image = $"{_configuration["Hosturl:images"]}/Products/{k.Products.ImageUrl}",
                    TotalPrice = k.TotalPrice,
                    OrderId = k.Orders.Orderstring
                }).ToList();
                return orderlist;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }



        public async Task<IEnumerable<OrderAdminviewDto>> GetAllUserOrder()
        {
            try
            {
                var order = await _context.Orders.Include(oi => oi.OrderItems).ThenInclude(p => p.Products).ToListAsync();
                if (order == null || order.Count == 0)
                {
                    return new List<OrderAdminviewDto>();
                }
                var orderitems = order.Select(or => new OrderAdminviewDto
                {
                    Id = or.UserId,
                    UserName = or.UserName,
                    Phone = or.Phone,
                    OrderId = or.Orderstring,
                    TransactionId = or.TransactionId,
                    OrderDate = or.OrderDate,

                }).ToList();
                return orderitems;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

       

        public async Task<int> TotalPurchasedProduct()
        {
            try
            {
                var count = await _context.OrderItems.SumAsync(q => q.Quantity);
                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<decimal> TotalRevenue()
        {
            try
            {
                var total = await _context.OrderItems.SumAsync(v => v.TotalPrice);
                return total;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
