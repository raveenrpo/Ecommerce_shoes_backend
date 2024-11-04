using System.Security.Claims;
using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Services.WishListService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerse_shoes_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistservice _wishlistservice;

        public WishlistController(IWishlistservice wishlistservice)
        {
            _wishlistservice = wishlistservice;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistDto>>> GetWishlist()
        {
            try
            {
                var useridres = GetUserId();
                var userid = useridres.Value;
                var result = await _wishlistservice.Get(userid);
                if (result == null)
                {
                    return NotFound("User not Found");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Addwishlist/{productId}")]
        public async Task<IActionResult> Addtowishlist(int productId)
        {
            try
            {
                var useridres = GetUserId();
                var userid = useridres.Value;
                var res = await _wishlistservice.Add(userid, productId);
                if (res)
                {
                    return Ok("product added to wishlist");
                }
                return BadRequest("product already in wishlist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("removewishlist/{productId}")]
        public async Task<IActionResult> Removewishlist(int productId)
        {
            try
            {
                var useridres = GetUserId();
                var userid = useridres.Value;
                var res = await _wishlistservice.Remove(userid, productId);
                if (res)
                {
                    return Ok("Product removed from wishlist");
                }
                return NotFound("product not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private ActionResult<int> GetUserId()
        {
            var stringid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(stringid, out var id))
            {
                return id;
            }
            return Unauthorized();
        }
    }
}
