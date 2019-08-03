using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Maya.RequestProperties;
using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Identity;
using Maya.Models;
using Maya.Services.CartServices;
using Maya.Services.CartItemServices;

namespace Maya.Controllers.Client {

	[Route("api/settings")]
	[Authorize(Roles = "client")]
	public class AccountSettingsController : Controller {


		private readonly UserManager<User> _userManager;
		private readonly IHttpContextAccessor _httpContext;

		public AccountSettingsController(UserManager<User> userManager, IHttpContextAccessor httpContext) {
			_userManager = userManager;
			_httpContext = httpContext;
		}

		[HttpGet]
		[Route("current_user")]
		public async Task<IActionResult> Get() {
			var UserId = _httpContext.HttpContext.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;

			var user = await _userManager.FindByIdAsync(UserId);


			return Ok(new { 
				id = user.Id,
				name = user.Name,
				email = user.UserName,
			});
		}
	}
}