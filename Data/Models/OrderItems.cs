namespace Ecommerse_shoes_backend.Data.Models
{
    public class OrderItems
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public virtual Order Order { get; set; }
        public virtual Products Products { get; set; }

    }
}
