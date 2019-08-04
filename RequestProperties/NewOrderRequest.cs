using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Maya.RequestProperties {
	public class NewOrderRequest {

		[Required]
		public string shippingAddress { get; set; }

		[Required]
		public decimal totalPrice { get; set; }

		[Required]
		public ICollection<int> items { get; set; }
	}
}