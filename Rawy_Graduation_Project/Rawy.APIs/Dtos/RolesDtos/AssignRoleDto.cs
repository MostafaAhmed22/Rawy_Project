using System.ComponentModel.DataAnnotations;

namespace Rawy.APIs.Dtos.RolesDtos
{
	public class AssignRoleDto
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string RoleName { get; set; }
	}
}
