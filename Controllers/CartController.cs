using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Services.CartService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerse_shoes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartservice _cartservice;
        public CartController(ICartservice cartservice)
        {
            _cartservice = cartservice;
        }
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CartDto>> Get()
        //{
        //    try
        //    {
        //        var id = Convert.ToInt32(HttpContext.Items[Use])
        //        var res = await _cartservice.GetCartItems(id);
        //    }
        //}
    }
}
