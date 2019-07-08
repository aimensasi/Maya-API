using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Maya.Models {
	public class Product {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set;}

		[Required]
		public string Name { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string Description { get; set; }

		[Required]
		public decimal Price { get; set; }

		public int? CategoryId { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public virtual Category Category { get; set; }
	}
}