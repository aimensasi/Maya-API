using Microsoft.AspNetCore.Identity;
using System;


namespace Maya.Models {
	public class UserRole : IdentityRole<Guid>{

		public UserRole(): base(){

		}

		public UserRole(string name) : base(name){
			
		}
	}
}