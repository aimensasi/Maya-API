using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Maya.Models {
	public class BundleContext : IdentityDbContext<User, UserRole, Guid> {

		// Models Access Points
		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Category { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }

		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }

		public BundleContext(DbContextOptions<BundleContext> options) : base(options){
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		=> optionsBuilder
				.UseLazyLoadingProxies();

		// protected override void OnModelCreating(ModelBuilder modelBuilder) {
		// 	modelBuilder.Entity<User>()
		// 			.HasOne(u => u.Cart)
		// 			.WithOne(c => c.User)
		// 			.HasForeignKey<Cart>(c => c.UserID);
		// }
	}
}