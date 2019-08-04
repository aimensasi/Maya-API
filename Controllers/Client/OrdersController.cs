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

namespace Maya.Controllers.Client {

	[Route("api/orders")]
	[Authorize(Roles = "client")]
	public class OrdersController : Controller {

		private readonly IOrderServices _orderServices;
		private readonly IOrderItemServices _orderItemServices;

		public OrdersController(IOrderServices orderServices, IOrderItemServices orderItemServices) {
			_orderServices = orderServices;
			_orderItemServices = orderItemServices;
		}

		[HttpGet]
		[Route("")]
		public IActionResult Get() {
			var (state, response) = _orderServices.orders();

			if (state == true) {
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpPost]
		[Route("")]
		public async Task<IActionResult> Store(NewOrderRequest request) {
			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			var (state, response) = await _orderServices.createOrder(request);

			if (state == true) {
				return Ok(response);
			}

			return BadRequest(response);
		}
	}
}