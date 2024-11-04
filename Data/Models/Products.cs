using System.ComponentModel.DataAnnotations;

namespace Ecommerse_shoes_backend.Data.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public int Stock { get; set; }

        public bool Status { get; set; }=true;
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Wishlist> Wishlist { get; set; }


    }
}
