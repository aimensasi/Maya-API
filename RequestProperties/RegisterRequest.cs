using System.ComponentModel.DataAnnotations;

namespace Maya.RequestProperties {
	public class RegisterRequest {


		[Required]
		[Display(Name = "Full Name")]
		public string Name { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(6)]
		public string Password { get; set; }
	}
}