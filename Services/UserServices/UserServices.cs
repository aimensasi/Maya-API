using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Maya.RequestProperties;
using Maya.Models;
using System;
using System.Linq;

namespace Maya.Services.UserServices {
	public class UserServices : IUserServices {

		private readonly UserManager<User> _userManager;
		private readonly RoleManager<UserRole> _roleManager;

		public UserServices(UserManager<User> userManager, RoleManager<UserRole> roleManager) {
			_userManager = userManager;
			_roleManager = roleManager;
		}


		public bool IsAdminRegisterOpen(){
			var size = _userManager.GetUsersInRoleAsync("admin").Result.Count;

			return size > 0 ? false : true;
		}

		public async Task<(bool state, object response)> create(RegisterRequest request, string role = null) {
			var user = new User {
				Name = request.Name,
				UserName = request.Email,
				createdAt = DateTimeOffset.UtcNow
			};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded) {
				var message = result.Errors.FirstOrDefault()?.Description;
				return (false, new { message });
			}

			if (role != null) {
				await _roleManager.CreateAsync(new UserRole(role));
				await _userManager.AddToRoleAsync(user, role);
				await _userManager.UpdateAsync(user);
			}

			return (true, transform(user));
		}

		public object transform(User user) {
			return new {
				id = user.Id,
				name = user.Name,
				email = user.UserName,
				createdAt = user.createdAt,
				role =  _userManager.GetRolesAsync(user).Result[0],
			};
		}
	}
}