using Microsoft.AspNetCore.Builder;

namespace Maya.Middlewares {
	public static class MiddlewareExtention {
		public static IApplicationBuilder UseAdminRegisterBlocker(this IApplicationBuilder builder) {
			return builder.UseMiddleware<AllowAdminRegisterMiddleware>();
		}
	}
}