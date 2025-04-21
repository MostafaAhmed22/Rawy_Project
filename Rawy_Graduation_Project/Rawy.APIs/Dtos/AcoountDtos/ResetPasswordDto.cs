using System.ComponentModel.DataAnnotations;

namespace Rawy.APIs.Dtos.AcoountDtos
{
    public class ResetPasswordDto
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
            ErrorMessage = "Password Must Contain uppercase/lowercase letters, numbers, and special characters.")]
        [MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
