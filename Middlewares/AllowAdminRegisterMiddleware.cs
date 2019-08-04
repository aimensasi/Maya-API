using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Maya.Models;


namespace Maya.Middlewares {

	public class AllowAdminRegisterMiddleware {
		private readonly RequestDelegate _next;
		private readonly UserManager<User> _userManager;

		public AllowAdminRegisterMiddleware(RequestDelegate next, UserManager<User> userManger){
			_next = next;
			_userManager = userManger;
		}

		public async Task InvokeAsync(HttpContext context) {
			// Call the next delegate/middleware in the pipeline
			var currentRequest = context.Request.Path;

			if(currentRequest.Equals("/api/admin/register")){
				var adminCount = _userManager.GetUsersInRoleAsync("admin").Result.Count;

				context.Response.StatusCode = 403;
				await context.Response.WriteAsync($"Requested resource is no longer avaliable {adminCount}");
				return;
			}

			
			await _next(context);
		}
	}
}