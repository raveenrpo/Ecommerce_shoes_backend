namespace Ecommerse_shoes_backend.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Phone_no { get; set; }
        public string Email  { get; set; }
        public string Password  { get; set; }
        public string Role { get; set; }
        public bool Isblocked { get; set; }
        public virtual Cart Cart { get; set; }
        public ICollection<Wishlist> Wishlist { get; set; }
    }
}
