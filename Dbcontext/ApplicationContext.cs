using Ecommerse_shoes_backend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerse_shoes_backend.Dbcontext
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext>options):base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();            
            modelBuilder.Entity<User>().HasData(new User { Id=10,Name="admin",Phone_no=9087675434,Email="admin@gmail.com",Role="Admin", Password=BCrypt.Net.BCrypt.HashPassword("password",salt)});

            modelBuilder.Entity<Products>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Products>()
                .HasOne(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId);



            modelBuilder.Entity<User>()
                .HasOne(c=>c.Cart)
                .WithOne(u=>u.User)
                .HasForeignKey<Cart>(u=>u.UserId);

            modelBuilder.Entity<Cart>()
                .HasMany(p => p.CartItems)
                .WithOne(c=>c.Cart)
                .HasForeignKey(c=>c.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(p=>p.Products)
                .WithMany(c=>c.CartItems)
                .HasForeignKey(p=>p.ProductId);





            modelBuilder.Entity<Wishlist>()
                .HasOne(u => u.User)
                .WithMany(w => w.Wishlist)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<Wishlist>()
                .HasOne(p=>p.Products)
                .WithMany(w=>w.Wishlist)
                .HasForeignKey(p=>p.ProductId);

           

        }
    }
}
