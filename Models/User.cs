using Microsoft.AspNetCore.Identity;
using System;

namespace Maya.Models {
	public class User :  IdentityUser<Guid>{

		public string Name { get; set; }

		public DateTimeOffset createdAt { get; set; }

		public virtual Cart Cart { get; set; }
	}
}