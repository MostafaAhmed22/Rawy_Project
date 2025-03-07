using System.ComponentModel.DataAnnotations;

namespace Rawy.APIs.Dtos
{
	public class RegisterDto
	{
		[Required]
		public string FName { get; set; }
		[Required]
		public string LName { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
			ErrorMessage = "Password Must Contain uppercase/lowercase letters, numbers, and special characters.")]
		public string Password { get; set; }
		[Required]
		[Phone]
		public string PhoneNumber { get; set; }
	}
}
