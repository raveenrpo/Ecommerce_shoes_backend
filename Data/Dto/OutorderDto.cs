namespace Ecommerse_shoes_backend.Data.Dto
{
    public class OutorderDto
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public int ProductId {  get; set; }
        public int Quantity {  get; set; }
        public decimal Price { get; set; }
        public decimal Total {  get; set; }
        public string Image {  get; set; }

    }
}
