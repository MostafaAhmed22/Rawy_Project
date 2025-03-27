﻿using System.ComponentModel.DataAnnotations;

namespace Rawy.APIs.Dtos
{
    public class WriterDto
    {
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }

        public string UserName { get; set; }
        [Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
			ErrorMessage = "Password Must Contain uppercase/lowercase letters, numbers, and special characters.")]
		[MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
			ErrorMessage = "Password Must Contain uppercase/lowercase letters, numbers, and special characters.")]
		[MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
		[Compare(nameof(Password), ErrorMessage = "Confirm Password Does Not Match Password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
		[Required]
		[Phone]
		public string PhoneNumber { get; set; }

		public string? PreferredLanguage { get; set; }
		public string? WritingStyle { get; set; }
	}
}
