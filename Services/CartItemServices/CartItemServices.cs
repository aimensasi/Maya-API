using Maya.RequestProperties;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IO;
using System.Net.Http.Headers;
using Maya.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Text;
using Maya.Services.ProductServices;


namespace Maya.Services.CartItemServices {
	public class CartItemServices : ICartItemServices {

		private readonly BundleContext _context;
		private readonly IProductServices _productServices;

		public CartItemServices(BundleContext context, IProductServices productServices) {
			_context = context;
			_productServices = productServices;
		}


		public (bool state, object response) FindItemsByCartId(int CartId){
			var cartItems = _context.CartItems.Where( i => i.CartId == CartId).ToList();

			return (true, transformCollection(cartItems));
		}


		public async Task<(bool state, object response)> AddItemAsync(CartItemRequest request, int CartId){
			CartItem cartItem = new CartItem{
				CartId = CartId,
				ProductId = request.ProductId,
				Quantity = request.Quantity,
				CreatedAt = DateTimeOffset.Now
			};

			await _context.CartItems.AddAsync(cartItem);
			await _context.SaveChangesAsync();

			cartItem = await _context.CartItems
															 .Where(item => item.Id == cartItem.Id)
															 .Include( i => i.Product)
															 .FirstAsync();

			if(cartItem == null || cartItem.Product == null){
				return (false, new { message = "Something Went wrong, Product does not exists" });
			}

			return (true, transform(cartItem));
		}

		public async Task<(bool state, object response)> DeleteItemAsync(int CartId, int ItemId) {
			var CartItem = await _context.CartItems.FindAsync(ItemId);

			if (CartItem == null) {
				return (false, new { message = "Resource Not Found" });
			}

			_context.CartItems.Remove(CartItem);
			await _context.SaveChangesAsync();

			return (true, new { message = "Success" });
		}




		/* Transform Cart Items */

		public ICollection<object> transformCollection(ICollection<CartItem> cartItems) {
			var cartItemsCollection = new List<object>();

			foreach (CartItem cartItem in cartItems) {
				cartItemsCollection.Add(transform(cartItem));
			}

			return cartItemsCollection;
		}

		public object transform(CartItem CartItem) {
			return new {
				id = CartItem.Id,
				cartId = CartItem.CartId,
				productId = CartItem.ProductId,
				quantity = CartItem.Quantity,
				product = _productServices.transform(CartItem.Product),
				createdAt = CartItem.CreatedAt,
			};
		}
	}
}
