using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Maya.Models {
	public class BundleContext : IdentityDbContext<User, UserRole, Guid> {

		// Models Access Points


		public BundleContext(DbContextOptions<BundleContext> options) : base(options){
		
		}
	}
}