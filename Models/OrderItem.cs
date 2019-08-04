using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Maya.Models {
	public class OrderItem {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int OrderId { get; set; }

		public int ProductId { get; set; }

		public int Quantity { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public  virtual Product Product { get; set; }
	}
}