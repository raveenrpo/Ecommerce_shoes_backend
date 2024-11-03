using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Data.Models;

namespace Ecommerse_shoes_backend.Services.Productservice
{
    public interface IProductservice
    {
        Task<IEnumerable<ProductDto>> GetProducts();
        Task<ProductDto> GetProductsById(int id);
        Task<IEnumerable<ProductDto>> GetProductsByCategory(string name);
        Task<string>AddProduct(ProductAddDto addDto);
        Task<ProductAddDto> UpadateProduct(int id,ProductAddDto addDto);
        Task<string> DeleteProduct(int id);
        Task<IEnumerable<ProductDto>> SearchProduct(string name);
    }
}
