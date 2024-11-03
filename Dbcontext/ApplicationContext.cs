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
        }
    }
}
