using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Maya.RequestProperties {
	public class UpdateOrderRequest {

		[Required]
		public string status { get; set; }

	}
}