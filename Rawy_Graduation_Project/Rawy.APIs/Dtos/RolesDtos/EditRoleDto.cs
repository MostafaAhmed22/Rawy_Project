using System.ComponentModel.DataAnnotations;

namespace Rawy.APIs.Dtos.RolesDtos
{
	public class EditRoleDto
	{
		[Required]
		public string oldName { get; set; }
		[Required]
		public string newName { get; set; }
	}
}
