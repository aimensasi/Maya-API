using Maya.RequestProperties;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Maya.Models;

namespace Maya.Services.OrderItemServices {
	public interface IOrderItemServices {


		Task<(bool state, object response)> moveCartItemsToOrder(int OrderId, NewOrderRequest request);

		ICollection<object> transformCollection(ICollection<OrderItem> orderItems);
		object transform(OrderItem orderItem);
	}
}