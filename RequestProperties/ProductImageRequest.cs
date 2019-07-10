using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Maya.RequestProperties {
	public class ProductImageRequest {

		[Required]
		[MinLength(6)]
		public IFormFile File { get; set; }
	}
}