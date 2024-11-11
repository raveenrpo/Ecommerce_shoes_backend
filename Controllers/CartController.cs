using System.Security.Claims;
using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Services.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerse_shoes_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartservice _cartservice;
        public CartController(ICartservice cartservice)
        {
            _cartservice = cartservice;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartDto>>> Get()
        {
            try
            {

                var userId = Convert.ToInt32(HttpContext.Items["Id"]);
                Console.WriteLine("userid="+userId);
                var res = await _cartservice.GetCartItems(userId);
                if (res == null || !res.Any())
                {
                    return NotFound(); 
                }
                if (res.Count() == 0)
                {
                    return BadRequest("Cart is empty");
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Add_to_cart/{productId}")]
        public async Task <IActionResult>AddToCart(int productId)
        {
            try
            {

                var userId = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _cartservice.AddToCart(userId, productId);
                if (res == true)
                {
                    return Ok("Item Added to Cart Successfully");
                }
                return BadRequest("Item already in cart");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Remove_from_cart/{productId}")]
        public async Task<IActionResult>RemoveCart( int productId)
        {
            try
            {
                //var userIdResult = GetUserIdFromClaims();
                //var userId = userIdResult.Value;
                var userId = Convert.ToInt32(HttpContext.Items["Id"]);

                var res = await _cartservice.RemoveFromCart(userId, productId);
                if (res)
                {
                    return Ok("item removed from the cart");
                }
                return NotFound("product not found in the cart");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("Incriment/{productId}")]
        public async Task<IActionResult> QuantityUp(int productId)
        {
            try
            {

                var userId = Convert.ToInt32(HttpContext.Items["Id"]);

                var res = await _cartservice.IncrimentQuantity(userId, productId);
                if (res)
                {
                    return Ok("Quantity increased");
                }
                return NotFound("product not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Decriment/{productId}")]
        public async Task<IActionResult> QuantityDwn(int productId)
        {
            try
            {
                var userId = Convert.ToInt32(HttpContext.Items["Id"]);

                var res = await _cartservice.DecrimentQuantity(userId, productId);
                if (res)
                {
                    return Ok("quantity decreased");
                }
                return NotFound("product not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private ActionResult<int> GetUserIdFromClaims()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            return Unauthorized(); 
        }

    }
}
