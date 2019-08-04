using System.ComponentModel.DataAnnotations;
namespace Maya.RequestProperties {
	public class CartItemRequest {

		[Required]
		public int ProductId { get; set; }

		[Required]
		public int Quantity { get; set; }
	}
}