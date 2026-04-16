using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OnlineMusic.Models;

namespace OnlineMusic.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<OrderHeader> OrderHeaders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "String", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Wind", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Percusssion", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Keyboard", DisplayOrder = 4 },
                new Category { Id = 5, Name = "Electronic", DisplayOrder = 5 }
                );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    InstrumentName = "Guitar",
                    Description = "Acoustic guitar with a rich sound",
                    price = 199.99,
                    stock_quantity = 20,
                    CategoryId=1,
                    ImageUrl=""
                },
                new Product
                {
                    Id = 2,
                    InstrumentName = "Piano",
                    Description = "Grand piano with smooth keys",
                    price = 4999.99,
                    stock_quantity = 5,
                    CategoryId=1,
                    ImageUrl=""
                },
                new Product
                {
                    Id = 3,
                    InstrumentName = "Drum Kit",
                    Description = "Complete drum set with cymbals",
                    price = 799.99,
                    stock_quantity = 15,
                    CategoryId=2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 4,
                    InstrumentName = "Violin",
                    Description = "4/4 Violin with case",
                    price = 299.99,
                    stock_quantity = 10,
                    CategoryId=2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 5,
                    InstrumentName = "Flute",
                    Description = "Silver flute with clear tone",
                    price = 150.00,
                    stock_quantity = 25,
                    CategoryId=3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 6,
                    InstrumentName = "Saxophone",
                    Description = "Alto saxophone with case",
                    price = 1200.00,
                    stock_quantity = 7,
                    CategoryId=3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 7,
                    InstrumentName = "Trumpet",
                    Description = "Brass trumpet with vibrant sound",
                    price = 800.00,
                    stock_quantity = 12,
                    CategoryId=4,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 8,
                    InstrumentName = "Keyboard",
                    Description = "Electric keyboard with 61 keys",
                    price = 300.00,
                    stock_quantity = 18,
                    CategoryId = 4,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 9,
                    InstrumentName = "Harmonica",
                    Description = "Chromatic harmonica with carrying case",
                    price = 50.00,
                    stock_quantity = 40,
                    CategoryId=5,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 10,
                    InstrumentName = "Bass Guitar",
                    Description = "Electric bass guitar with solid body",
                    price = 350.00,
                    stock_quantity = 8,
                    CategoryId=5,
                    ImageUrl = ""
                }
            );
        }
    }
}
