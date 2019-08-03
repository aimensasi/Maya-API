using Maya.RequestProperties;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Maya.Models;

namespace Maya.Services.OrderServices {
	public interface IOrderServices {

		(bool state, ICollection<object>) orders();
		(bool state, ICollection<object>) ordersFor(Guid UserId);
		Task<(bool state, object response)> createOrder(NewOrderRequest request);

		Task<(bool state, object response)> update(UpdateOrderRequest request, int id);
		ICollection<object> transformCollection(ICollection<Order> orders);
		object transform(Order order);

	}
}