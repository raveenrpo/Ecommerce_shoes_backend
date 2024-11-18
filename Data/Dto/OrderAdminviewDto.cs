namespace Ecommerse_shoes_backend.Data.Dto
{
    public class OrderAdminviewDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string OrderId {  get; set; }
        public long Phone {  get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public string TransactionId {  get; set; }
    }
}
