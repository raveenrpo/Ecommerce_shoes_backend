namespace Ecommerse_shoes_backend.Data.Dto
{
    public class InputorderDto
    {
        public string UserName { get; set; }
        public long Phone { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
        public string Orderstring { get; set; }
        public string TransactionId { get; set; }

    }
}
