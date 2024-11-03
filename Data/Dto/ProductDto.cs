using System.ComponentModel.DataAnnotations;

namespace Ecommerse_shoes_backend.Data.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Stock { get; set; }
        public string Category_Name { get; set; }
    }
}
