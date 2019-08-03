using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Maya.Models;
using Microsoft.AspNetCore.Http;
using System;
using AspNet.Security.OpenIdConnect.Primitives;

namespace Maya.Services.CartServices {
	public class CartServices : ICartServices {

		private readonly BundleContext _context;
		private readonly IHttpContextAccessor _httpContext;

		public CartServices(BundleContext context, IHttpContextAccessor httpContext) {
			_context = context;
			_httpContext = httpContext;
		}

		public async Task<(bool state, object response)> cart(int id) {
			Cart cart = await _context.Carts.FindAsync(id);

			if(cart == null){
				return (false, new { message = "Resource Not Found" });
			}

			return (true, cart);
		}


		public async Task<(bool state, object response)> cart(){
			var UserId = _httpContext.HttpContext.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;

			if( UserId == null){
				return (false, new { message = "Unauthorized Request" } );
			}

			Cart cart = await _context.Carts
																.Where( c => c.UserId.ToString() == UserId)
																.FirstOrDefaultAsync();

			if(cart == null){
				cart = await createCart(UserId);
			}

			return (true, transform(cart));
		}

		private async Task<Cart> createCart(string UserId){
			Cart cart = new Cart {
				UserId = Guid.Parse(UserId),
				CreatedAt = DateTimeOffset.Now
			};

			await _context.Carts.AddAsync(cart);
			await _context.SaveChangesAsync();

			return cart;
		}

		private object transform(Cart cart) {
			return new {
				id = cart.Id,
				userId = cart.UserId,
				createdAt = cart.CreatedAt,
			};
		}

	}
}