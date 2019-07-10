using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Maya.Models {
	public class Cart {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public Guid UserId { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public virtual User User { get; set; }

		public virtual ICollection<CartItem> CartItems { get; set; }
	}
}