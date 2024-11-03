using System.ComponentModel.DataAnnotations;

namespace Ecommerse_shoes_backend.Data.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CartId {  get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual Products Products { get; set; }
    }
}
