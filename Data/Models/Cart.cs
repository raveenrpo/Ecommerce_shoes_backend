using System.ComponentModel.DataAnnotations;

namespace Ecommerse_shoes_backend.Data.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
