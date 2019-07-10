using System.ComponentModel.DataAnnotations;
namespace Maya.RequestProperties {
	public class NewProductRequest {

		[Required]
		[Display(Name = "Product Name")]
		public string Name { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public decimal Price { get; set; }

		[Required]
		public int CategoryId { get; set; }
	}
}