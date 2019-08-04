using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Maya.RequestProperties;
using Microsoft.AspNetCore.Authorization;

using Maya.Services.ProductServices;

namespace Maya.Controllers.Client {

	[Route("api/products")]
	[Authorize(Roles = "client")]
	public class ProductsController : Controller{

		private readonly IProductServices _productServices;

		public ProductsController(IProductServices productServices) {
			_productServices = productServices;
		}


		[HttpGet]
		[Route("")]
		public IActionResult Index() {
			return Ok(_productServices.products());
		}
	}
}