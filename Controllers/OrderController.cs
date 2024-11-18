using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerse_shoes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderservice _orderservice;

        public OrderController(IOrderservice orderservice)
        {
            _orderservice = orderservice;
        }

        [HttpPost("ordercreation")]
        [Authorize]
        public async Task<ActionResult> CreateOrder(long price)
        {
            try
            {
                if (price <= 0)
                {
                    return BadRequest("Price should be greater than zero");
                }
                var order = await _orderservice.RazorOrderCreate(price);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("paymentvalidation")]
        [Authorize]
        public  ActionResult PaymentValidate(RazorDto razorDto)
        {
            try
            {
                if (razorDto == null)
                {
                    return BadRequest("Razorepay detailes are undefined");
                }
                var valid = _orderservice.PaymentVerify(razorDto);
                return Ok(valid);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpPost("placeorder")]
        [Authorize]
        public async Task<ActionResult> Placeorder(InputorderDto inputorderDto)
        {
            try
            {
                int userid = Convert.ToInt32(HttpContext.Items["Id"]);
                var createorder = await _orderservice.CreateOrder(userid, inputorderDto);
                return Ok(createorder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("get_all_order")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderAdminviewDto>> Getallorders()
        {
            try
            {
                var res = await _orderservice.GetAllUserOrder();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get_user_order")]
        [Authorize]
        public async Task<ActionResult> GetOrder()
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _orderservice.GetUserOrder(userId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("orderbyid/{id}")]
        public async Task<ActionResult> Getorderbyid(int id)
        {
            try
            {
                var user = await _orderservice.GetUserOrderById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("total")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> TotalIncome()
        {
            try
            {
                var total = await _orderservice.TotalRevenue();
                return Ok(total);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("purchased_product")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Purchasedproducts()
        {
            try
            {
                return Ok(await _orderservice.TotalPurchasedProduct());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
