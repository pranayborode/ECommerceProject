using ECommerceProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProject.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<MainCategory> MainCategories { get; set; }
		public DbSet<SubCategory> SubCategories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Cart> Carts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
		public DbSet<PromoCode> PromoCodes { get; set; }

		

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Specify cascade delete behavior
			modelBuilder.Entity<Product>()
				.HasOne(p => p.SubCategory)
				.WithMany(sc => sc.Products)
				.HasForeignKey(p => p.SubCategoryId)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<Product>()
			   .HasOne(p => p.MainCategory)
			   .WithMany(m => m.Products)
			   .HasForeignKey(p => p.MainCategoryId);

			modelBuilder.Entity<Product>()
				.HasOne(p => p.Brand)
				.WithMany(b => b.Products)
				.HasForeignKey(p => p.BrandId);


			// Define relationships
			modelBuilder.Entity<ProductImage>()
				.HasOne(pi => pi.Product)
				.WithMany(p => p.Images)
				.HasForeignKey(pi => pi.ProductId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
