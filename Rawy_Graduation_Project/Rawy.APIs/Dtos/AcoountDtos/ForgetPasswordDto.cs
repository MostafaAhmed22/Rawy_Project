using System.ComponentModel.DataAnnotations;

namespace Rawy.APIs.Dtos.AcoountDtos
{
    public class ForgetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
