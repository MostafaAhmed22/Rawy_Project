using System.ComponentModel.DataAnnotations;

namespace Rawy.APIs.Dtos.RolesDtos
{
	public class CreateRoleDto
	{
		[Required]
		public string RoleName { get; set; }
	}
}
