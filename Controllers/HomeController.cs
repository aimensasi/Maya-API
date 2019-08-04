using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace viewpage.Controllers {

	[Route("")]
	public class HomeController : Controller {
		public IActionResult Index() {
			return View();
		}
	}
}
