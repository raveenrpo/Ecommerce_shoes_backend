namespace Ecommerse_shoes_backend.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Category_Name { get; set; }
        public virtual ICollection<Products> Products { get; set; }

    }
}
