using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Maya.RequestProperties;
using Microsoft.AspNetCore.Authorization;
using AspNet.Security.OpenIdConnect.Primitives;

using Maya.Services.CartServices;

namespace Maya.Controllers.Client {

	[Route("api/cart")]
	[Authorize(Roles = "client")]
	public class CartsController : Controller {
		private readonly ICartServices _cartServices;

		public CartsController(ICartServices cartServices){
			_cartServices = cartServices;
		}


		[HttpGet]
		[Route("")]
		public async Task<IActionResult> Get(){
			var (state, response) = await _cartServices.cart();

			if (state == true) {
				return Ok(response);
			}

			return BadRequest(response);
		}




	}
}