using System.Threading.Tasks;
using Maya.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using Maya.Services.OrderItemServices;
using Maya.RequestProperties;
using AspNet.Security.OpenIdConnect.Primitives;

namespace Maya.Services.OrderServices {
	public class OrderServices : IOrderServices {

		private readonly BundleContext _context;
		private readonly IHttpContextAccessor _httpContext;

		private readonly IOrderItemServices _orderItemServices;

		public OrderServices(BundleContext context, IHttpContextAccessor httpContext, IOrderItemServices orderItemServices) {
			_context = context;
			_httpContext = httpContext;
			_orderItemServices = orderItemServices;
		}

		public (bool state, ICollection<object>) orders() {
			var orders = _context.Orders.ToList();

			return (true, transformCollection(orders));
		}

		public async Task<(bool state, object response)> createOrder(NewOrderRequest request) {
			var UserId = _httpContext.HttpContext.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value;

			if (UserId == null) {
				return (false, new { message = "Unauthorized Request" });
			}

			Order order = new Order {
				UserId = Guid.Parse(UserId),
				shippingAddress = request.shippingAddress,
				totalPrice = request.totalPrice,
				Status = "Pending",
				CreatedAt = DateTimeOffset.Now
			};

			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();

			var (state, response) = await _orderItemServices.moveCartItemsToOrder(order.Id, request);

			if(state == false){
				_context.Orders.Remove(order);
				await _context.SaveChangesAsync();
			}


			return (state, new {message = "Order Created"});
		}


		public ICollection<object> transformCollection(ICollection<Order> orders) {
			var orderCollection = new List<object>();

			foreach (Order order in orders) {
				orderCollection.Add(transform(order));
			}

			return orderCollection;
		}

		public object transform(Order order) {
			return new {
				id = order.Id,
				userId = order.UserId,
				shippingAddress = order.shippingAddress,
				totalPrice = order.totalPrice,
				status = order.Status,
				shippedAt = order.shippedAt,
				createdAt = order.CreatedAt,
				items = _orderItemServices.transformCollection(order.OrderItems),
			};
		}
	}
}