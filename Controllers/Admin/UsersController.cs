using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Maya.RequestProperties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AspNet.Security.OpenIdConnect.Primitives;
using Maya.Services.ProductServices;
using Maya.Models;

namespace Maya.Controllers.Admin {

	[Route("api/admin/users")]
	[Authorize(Roles = "admin")]
	public class UsersController : AdminController {


		private readonly UserManager<User> _userManager;
		private readonly IHttpContextAccessor _httpContext;

		public UsersController(UserManager<User> userManager, IHttpContextAccessor httpContext) {
			_userManager = userManager;
			_httpContext = httpContext;
		}

		[HttpGet]
		[Route("current_user")]
		public async Task<IActionResult> CurrentUser() {
			var UserId = _httpContext.HttpContext.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;

			var user = await _userManager.FindByIdAsync(UserId);


			return Ok(new {
				id = user.Id,
				name = user.Name,
				email = user.UserName,
			});
		}

		[HttpGet]
		[Route("")]
		public async Task<IActionResult> Get() {

			var users = await _userManager.GetUsersInRoleAsync("client");

			var usersCollection = new List<object>();

			foreach(var user in users){
				usersCollection.Add(new {
					id = user.Id,
					name = user.Name,
					email = user.UserName,
				});
			}

			return Ok(usersCollection);
		}


	}
}