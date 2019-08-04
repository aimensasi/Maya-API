using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Maya.Models;
using Microsoft.AspNetCore.Http;
using System;
using AspNet.Security.OpenIdConnect.Primitives;
using System.Collections.Generic;
using Maya.Services.ProductServices;
using Maya.RequestProperties;

namespace Maya.Services.OrderItemServices {
	public class OrderItemServices : IOrderItemServices {

		private readonly BundleContext _context;
		private readonly IProductServices _productServices;

		public OrderItemServices(BundleContext context, IProductServices productServices) {
			_context = context;
			_productServices = productServices;
		}


		public async Task<(bool state, object response)> moveCartItemsToOrder(int OrderId, NewOrderRequest request){
			var items = request.items;
			int itemsAdded = 0;

			foreach (var item in items){
				var cartItem = await _context.CartItems.FindAsync(item);

				if(cartItem != null){
					
					var orderItem = new OrderItem {
						OrderId = OrderId,
						ProductId = cartItem.ProductId,
						Quantity = cartItem.Quantity,
						CreatedAt = DateTimeOffset.Now,
					};

					await _context.OrderItems.AddAsync(orderItem);
					_context.CartItems.Remove(cartItem);
					itemsAdded += 1;
				}
			}

			if(itemsAdded == 0){
				return (false, new { message = "No Items Found" });
			}

			await _context.SaveChangesAsync();

			return (true, new { message = "Items Added" });
		}

		public ICollection<object> transformCollection(ICollection<OrderItem> orderItems) {
			var orderItemsCollection = new List<object>();

			foreach (OrderItem orderItem in orderItems) {
				orderItemsCollection.Add(transform(orderItem));
			}

			return orderItemsCollection;
		}
		

		public object transform(OrderItem orderItem) {
			return new {
				id = orderItem.Id,
				cartId = orderItem.OrderId,
				productId = orderItem.ProductId,
				quantity = orderItem.Quantity,
				product = _productServices.transform(orderItem.Product),
				createdAt = orderItem.CreatedAt,
			};
		}
	}
}