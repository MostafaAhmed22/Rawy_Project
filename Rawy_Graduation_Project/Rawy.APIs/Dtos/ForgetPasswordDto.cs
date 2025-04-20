using System.ComponentModel.DataAnnotations;

namespace Rawy.APIs.Dtos
{
	public class ForgetPasswordDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
