using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Maya.Models {
	public class BundleContext : IdentityDbContext<User, UserRole, Guid> {

		// Models Access Points
		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Category { get; set; }

		public DbSet<ProductImage> ProductImages { get; set; }

		public BundleContext(DbContextOptions<BundleContext> options) : base(options){
		
		}
	}
}