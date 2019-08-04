using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Maya.Models {
	public class Order {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public Guid UserId { get; set; }

		[Required]
		public string Status { get; set; }

		[Required]
		public decimal totalPrice { get; set; }

		[Required]
		public string shippingAddress { get; set; }

		public DateTimeOffset shippedAt { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public virtual User User { get; set; }

		public virtual ICollection<OrderItem> OrderItems { get; set; }
	}
}