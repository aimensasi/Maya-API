using Maya.RequestProperties;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Maya.Models;

namespace Maya.Services.OrderServices {
	public interface IOrderServices {

		(bool state, ICollection<object>) orders();

		Task<(bool state, object response)> createOrder(NewOrderRequest request);

		ICollection<object> transformCollection(ICollection<Order> orders);
		object transform(Order order);

	}
}