using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Maya.RequestProperties;
using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OpenIdConnect.Primitives;

using Maya.Services.OrderServices;
using Maya.Services.OrderItemServices;

namespace Maya.Controllers.Admin {

	[Route("api/admin/orders")]
	[Authorize(Roles = "admin")]
	public class OrdersController : Controller {

		private readonly IOrderServices _orderServices;
		private readonly IOrderItemServices _orderItemServices;

		public OrdersController(IOrderServices orderServices, IOrderItemServices orderItemServices) {
			_orderServices = orderServices;
			_orderItemServices = orderItemServices;
		}

		[HttpGet]
		[Route("")]
		public IActionResult Get(Guid userId) {
			var (state, response) = _orderServices.ordersFor(userId);

			if (state == true) {
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpPatch]
		[Route("{id:int}")]
		public async Task<IActionResult> Patch(UpdateOrderRequest request, int id) {
			var (state, response) = await _orderServices.update(request, id);

			if (state == true) {
				return Ok(response);
			}

			return BadRequest(response);
		}
	}
}