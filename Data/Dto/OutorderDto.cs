namespace Ecommerse_shoes_backend.Data.Dto
{
    public class OutorderDto
    {
        public int Id { get; set; }
        public string Title {  get; set; }
        public int Quantity {  get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate {  get; set; }
        public string Image {  get; set; }
        public string OrderId { get; set; }
        //public string OrderStatus {  get; set; }


    }
}
