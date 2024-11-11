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
        private readonly IJwtTokengetId _jwtTokengetId;

        public Orderservice(ApplicationContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



        public async Task<string> OrderCreate(long price)
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
                var isorder=await _context.Orders.FirstOrDefaultAsync(u=>u.UserId == userId);
                //var existorder=await _context.Users.Include(o=>o.Order).ThenInclude(oi=>oi.OrderItems).FirstOrDefaultAsync(u=>u.Id==userId);
                if(isorder==null)
                {
                    await _context.Orders.AddAsync(new Data.Models.Orders { UserId = userId });
                    await _context.SaveChangesAsync();
                    isorder = await _context.Orders.FirstOrDefaultAsync(o=>o.UserId == userId);

                }
                var orderitem = user.Cart.CartItems.Select(c => new OrderItems
                {
                    OrderId = isorder.Id,
                    ProductId = c.ProductId,
                    Price = c.Products.Price,
                    Quantity = c.Quantity,
                    TotalPrice = c.Products.Price * c.Quantity,
                    UserName = inputorderDto.UserName,
                    Address = inputorderDto.Address,
                    Phone = inputorderDto.Phone,
                    Date = DateTime.Now.Date,

                });
                await _context.OrderItems.AddRangeAsync(orderitem);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



        public async Task<IEnumerable<OutorderDto>> GetAllUserOrder()
        {
            try
            {
                var order = await _context.Orders.Include(oi => oi.OrderItems).ThenInclude(p => p.Products).ToListAsync();
                if (order == null || order.Count == 0)
                {
                    return new List<OutorderDto>();
                }
                var orderitems = order.Select(or => new OutorderDto
                {
                    Id = or.OrderItems.FirstOrDefault().Id,
                    UserId = or.UserId,
                    ProductId = or.OrderItems.FirstOrDefault().ProductId,
                    Quantity = or.OrderItems.FirstOrDefault().Quantity,
                    Image = $"{_configuration["Hosturl:images"]}/Products/{or.OrderItems.FirstOrDefault().Products.ImageUrl}",
                    Price = or.OrderItems.FirstOrDefault().Price,
                    Total = or.OrderItems.FirstOrDefault().TotalPrice,

                }).ToList();
                return orderitems;
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
                var orderlist = user.Orders.OrderItems.Select(k => new OutorderDto
                {
                    Id = k.Id,
                    UserId = id,
                    ProductId = k.ProductId,
                    Quantity = k.Quantity,
                    Image = $"{_configuration["Hosturl:images"]}/Products/{k.Products.ImageUrl}",
                    Price = k.Price,
                    Total = k.TotalPrice,
                }).ToList();
                return orderlist;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
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

        public Task<int> TotalPurchaseByProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> TotalRevenue()
        {
            throw new NotImplementedException();
        }
    }
}
