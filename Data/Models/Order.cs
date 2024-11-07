namespace Ecommerse_shoes_backend.Data.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public long UserPhone { get; set; }
        public string City { get; set; }
        public string HomeAddress { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string TransactionId {  get; set; }
        public virtual User User { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
        



    }
}
