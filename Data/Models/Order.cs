namespace Ecommerse_shoes_backend.Data.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public long Phone { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
        public string Orderstring {  get; set; }
        public DateTime OrderDate { get; set; }
        public string TransactionId {  get; set; }

        public virtual User User { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
      

    }
}
