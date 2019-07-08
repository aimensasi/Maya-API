using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Maya.RequestProperties;
using Maya.Services.UserServices;
using OpenIddict.Abstractions;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using Maya.Models;


namespace Maya.Controllers.Client {

	[Route("api")]
	public class AuthController : Controller{


		private readonly IUserServices _userServices;

		public AuthController(IUserServices userServices) {
			_userServices = userServices;
		}


		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> register(RegisterRequest request) {
			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			var (state, response) = await _userServices.create(request, "client");

			if (state == true) {
				return Ok(response);
			}

			return BadRequest(response);
		}
	}
}