using System.ComponentModel.DataAnnotations;

namespace Ecommerse_shoes_backend.Data.Dto
{
    public class ProductAddDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }
}
