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
using Maya.Services.CartItemServices;

namespace Maya.Controllers.Client {

	[Route("api/cart")]
	[Authorize(Roles = "client")]
	public class CartsController : Controller {
		private readonly ICartServices _cartServices;
		private readonly ICartItemServices _cartItemServices;

		public CartsController(ICartServices cartServices, ICartItemServices cartItemServices) {
			_cartServices = cartServices;
			_cartItemServices = cartItemServices;
		}


		[HttpGet]
		[Route("")]
		public async Task<IActionResult> Get() {
			var (state, response) = await _cartServices.cart();

			if (state == true) {
				return Ok(response);
			}

			return BadRequest(response);
		}



		[HttpGet]
		[Route("{id:int}/items")]
		public async Task<IActionResult> GetItems(int id) {
			var (state, resault) = await _cartServices.cart(id);

			if (state == false) {
				return BadRequest(resault);
			}

			var (s, response) = _cartItemServices.FindItemsByCartId(id);

			return Ok(response);
		}

		[HttpPost]
		[Route("{id:int}/items")]
		public async Task<IActionResult> AddItemToCart(CartItemRequest request, int id){
			if(!ModelState.IsValid){
				return BadRequest(ModelState);
			}

			var (success, resault) = await _cartServices.cart(id);

			if (success == false) {
				return BadRequest(resault);
			}

			var (state, response) = await _cartItemServices.AddItemAsync(request, id);

			
			if(state){
				return Ok(response);	
			}

			return BadRequest(response);
		}


		[HttpDelete]
		[Route("{id:int}/items/{itemId:int}")]
		public async Task<IActionResult> DeleteItemToCart(int id, int itemId) {


			var (state, response) = await _cartItemServices.DeleteItemAsync(id, itemId);


			if (state) {
				return Ok(response);
			}

			return BadRequest(response);
		}

	}
}