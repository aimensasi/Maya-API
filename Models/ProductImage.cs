using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Maya.Models {
	public class ProductImage {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public int ProductId { get; set; }

		[Required]
		public string Image { get; set; }

		public virtual Product Product { get; set; }
	}
}