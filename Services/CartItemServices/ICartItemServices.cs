using Maya.RequestProperties;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Maya.Models;

namespace Maya.Services.CartItemServices {
	public interface ICartItemServices {


		(bool state, object response) FindItemsByCartId(int CartId);
		Task<(bool state, object response)> AddItemAsync(CartItemRequest request, int CartId);
		ICollection<object> transformCollection(ICollection<CartItem> cartItem);
		Task<(bool state, object response)> DeleteItemAsync(int CartId, int ItemId);
		
		object transform(CartItem cartItem);
	}
}