using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Maya.RequestProperties;
using Microsoft.AspNetCore.Authorization;

using Maya.Services.ProductServices;

namespace Maya.Controllers.Admin {

	[Route("api/admin/products")]
	[Authorize(Roles = "admin")]
	public class ProductsController : AdminController {

		private readonly IProductServices _productServices;

		public ProductsController(IProductServices productServices){
			_productServices = productServices;
		}

		[HttpGet]
		[Route("")]
		public IActionResult Index(){
			return Ok(_productServices.products());
		}

		[HttpPost]
		[Route("")]
		public async Task<IActionResult> store(NewProductRequest request){
			if(!ModelState.IsValid){
				return BadRequest(ModelState);
			}

			var (state, response) = await _productServices.store(request);

			if (state == true) {
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpPut]
		[Route("{id:int}")]
		public async Task<IActionResult> update(NewProductRequest request, int id) {
			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			var (state, response) = await _productServices.update(request, id);

			if (state == true) {
				return Ok(response);
			}

			return BadRequest(response);
		}

		[HttpPost]
		[Route("{id:int}/images")]
		public async Task<IActionResult> uploadImages(int id){
			IFormFile image = Request.Form.Files[0];
			
			var (state, response) = await _productServices.uploadImage(image, id);

			if (state == true) {
				return Ok(response);
			}

			return BadRequest(response);
		}


		[HttpDelete]
		[Route("{id:int}")]
		public async Task<IActionResult> destroy(int id){
			var ( state, response ) = await _productServices.delete(id);

			if (state == true){
				return Ok(response);
			}

			return BadRequest(response);
		}

	}
}