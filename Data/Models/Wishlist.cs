namespace Ecommerse_shoes_backend.Data.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public virtual User User { get; set; }
        public virtual Products Products { get; set; }

    }
}
