using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Services.Productservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerse_shoes_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductservice _productservice;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductservice productservice, IWebHostEnvironment webHostEnvironment)
        {
            _productservice = productservice;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await _productservice.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetpaginatedProduct(int pageno, int pagesize)
        {
            try
            {

                var res = await _productservice.GetProductByPagination(pageno, pagesize);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("byid/{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            try
            {
                var pr = await _productservice.GetProductsById(id);
                if (pr == null)
                {
                    return BadRequest("Product with id is not found");
                }
                return Ok(pr);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProductByCategory")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductByCategory(string name)
        {
            try
            {
                var products = await _productservice.GetProductsByCategory(name);
                if (products == null)
                {
                    return BadRequest("Product by this category not found");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> AddProduct([FromForm]ProductAddDto productAddDto,IFormFile image)
        {
            try
            {
                var res = await _productservice.AddProduct(productAddDto,image);
                if (res == "Category Id is not valid")
                {
                    return BadRequest(res);
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<string>> UpdateProduct(int id,[FromForm] ProductAddDto productAddDto,IFormFile image)
        {
            try
            {
                var product = await _productservice.UpadateProduct(id, productAddDto,image);
                if (product == null)
                {
                    return NotFound("product with id not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<string>>DeleteProduct(int id)
        {
            try
            {
                var pr = await _productservice.DeleteProduct(id);
                if (pr == "Product deleted successfully")
                {
                    return Ok(pr);
                }

                return NotFound(pr);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SearchProduct")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProduct(string name)
        {
            try
            {
                var products = await _productservice.SearchProduct(name);
                if (products == null)
                {
                    return NotFound();
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
