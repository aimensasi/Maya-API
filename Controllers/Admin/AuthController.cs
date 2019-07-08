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

namespace Maya.Controllers.Admin {

	public class AuthController : AdminController {

		private readonly IUserServices _userServices;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IOptions<IdentityOptions> _identityOptions;

		public AuthController(IOptions<IdentityOptions> identityOptions, IUserServices userServices, UserManager<User> userManager, SignInManager<User> signInManager) {
			_identityOptions = identityOptions;
			_userServices = userServices;
			_userManager = userManager;
			_signInManager = signInManager;
		}


		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> register(RegisterRequest request) {
			if(!_userServices.IsAdminRegisterOpen()){
				return BadRequest( new { message = "Requested resource is no longer avaliable"});
			}

			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			var (state, response) = await _userServices.create(request, "admin");

			if (state == true) {
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpPost("~/api/oauth/token")]
		public async Task<IActionResult> TokenExchange(OpenIdConnectRequest request) {

			// Check if request is of type password grant
			if (!request.IsPasswordGrantType()) {
				return BadRequest(new OpenIdConnectResponse {
					Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
					ErrorDescription = "The specified grant type is not supported."
				});	
			}

			var user = await _userManager.FindByNameAsync(request.Username);

			if (user == null) {
				return BadRequest(new OpenIdConnectResponse {
					Error = OpenIdConnectConstants.Errors.InvalidGrant,
					ErrorDescription = "The username or password is invalid."
				});
			}

			// Ensure the user is allowed to sign in
			if (!await _signInManager.CanSignInAsync(user)) {
				return BadRequest(new OpenIdConnectResponse {
					Error = OpenIdConnectConstants.Errors.InvalidGrant,
					ErrorDescription = "The specified user is not allowed to sign in."
				});
			}

			// Ensure the user is not already locked out
			if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user)) {
				return BadRequest(new OpenIdConnectResponse {
					Error = OpenIdConnectConstants.Errors.InvalidGrant,
					ErrorDescription = "The username or password is invalid."
				});
			}

			// Ensure the password is valid
			if (!await _userManager.CheckPasswordAsync(user, request.Password)) {
				if (_userManager.SupportsUserLockout) {
					await _userManager.AccessFailedAsync(user);
				}

				return BadRequest(new OpenIdConnectResponse {
					Error = OpenIdConnectConstants.Errors.InvalidGrant,
					ErrorDescription = "The username or password is invalid."
				});
			}

			// Reset the lockout count
			if (_userManager.SupportsUserLockout) {
				await _userManager.ResetAccessFailedCountAsync(user);
			}

			// Look up the user's roles (if any)
			var roles = new string[0];
			if (_userManager.SupportsUserRole) {
				roles = (await _userManager.GetRolesAsync(user)).ToArray();
			}

			// Create a new authentication ticket w/ the user identity
			var ticket = await CreateTicketAsync(request, user, roles);

			return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);

		}


		private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, User user, string[] roles) {

			var principal = await _signInManager.CreateUserPrincipalAsync(user);

			AddRolesToPrincipal(principal, roles);

			var ticket = new AuthenticationTicket(principal,
					new AuthenticationProperties(),
					OpenIdConnectServerDefaults.AuthenticationScheme);

			ticket.SetScopes(OpenIddictConstants.Scopes.Roles);

			// Explicitly specify which claims should be included in the access token
			foreach (var claim in ticket.Principal.Claims) {
				// Never include the security stamp (it's a secret value)
				if (claim.Type == _identityOptions.Value.ClaimsIdentity.SecurityStampClaimType) continue;

				// TODO: If there are any other private/secret claims on the user that should
				// not be exposed publicly, handle them here!
				// The token is encoded but not encrypted, so it is effectively plaintext.

				claim.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken);
			}

			return ticket;
		}

		private static void AddRolesToPrincipal(ClaimsPrincipal principal, string[] roles) {
			var identity = principal.Identity as ClaimsIdentity;

			var alreadyHasRolesClaim = identity.Claims.Any(c => c.Type == "role");
			if (!alreadyHasRolesClaim && roles.Any()) {
				identity.AddClaims(roles.Select(r => new Claim("role", r)));
			}

			var newPrincipal = new System.Security.Claims.ClaimsPrincipal(identity);
		}
	}
}