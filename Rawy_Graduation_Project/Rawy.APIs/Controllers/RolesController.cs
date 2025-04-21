using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rawy.APIs.Dtos.RolesDtos;
using Rawy.DAL.Models;

namespace Rawy.APIs.Controllers
{
	
	public class RolesController : BaseApiController
	{
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<AppUser> _userManager;

		public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

		// create role
		[HttpPost("create")]
		public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRole)
		{
			if (string.IsNullOrWhiteSpace(createRole.RoleName))
				return BadRequest("Role name is required.");

			if (await _roleManager.RoleExistsAsync(createRole.RoleName))
				return BadRequest("Role already exists.");

			var result = await _roleManager.CreateAsync(new IdentityRole(createRole.RoleName));
			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Ok($"Role '{createRole.RoleName}' created successfully.");
		}

	
		// edit roles
		[HttpPut("edit")]
		public async Task<IActionResult> EditRole([FromQuery] EditRoleDto editRole)
		{
			if (string.IsNullOrWhiteSpace(editRole.oldName) || string.IsNullOrWhiteSpace(editRole.newName))
				return BadRequest("Old and new role names are required.");

			var role = await _roleManager.FindByNameAsync(editRole.oldName);
			if (role == null)
				return NotFound("Role not found.");

			role.Name = editRole.newName;
			var result = await _roleManager.UpdateAsync(role);
			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Ok($"Role renamed from '{editRole.oldName}' to '{editRole.newName}' successfully.");
		}

		// Delete role
		[HttpDelete("delete")]
		public async Task<IActionResult> DeleteRole([FromQuery] string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				return BadRequest("Role name is required.");

			var role = await _roleManager.FindByNameAsync(name);
			if (role == null)
				return NotFound("Role not found.");

			var result = await _roleManager.DeleteAsync(role);
			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Ok($"Role '{name}' deleted successfully.");
		}

		//assign role to user
		[HttpPost("assign")]
		public async Task<IActionResult> AssignRole([FromQuery] AssignRoleDto assignRole)
		{
			if (string.IsNullOrWhiteSpace(assignRole.Email) || string.IsNullOrWhiteSpace(assignRole.RoleName))
				return BadRequest("Email and role name are required.");

			var user = await _userManager.FindByEmailAsync(assignRole.Email);
			if (user == null)
				return NotFound("User not found.");

			if (!await _roleManager.RoleExistsAsync(assignRole.RoleName))
				return BadRequest("Role does not exist.");

			var result = await _userManager.AddToRoleAsync(user, assignRole.RoleName);
			if (!result.Succeeded)
				return BadRequest(result.Errors);

			return Ok($"User '{assignRole.Email}' assigned to role '{assignRole.RoleName}' successfully.");
		}

		// get user roles
		[HttpGet("user-roles")]
		public async Task<IActionResult> GetUserRoles([FromQuery] string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				return BadRequest("Email is required.");

			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
				return NotFound("User not found.");

			var roles = await _userManager.GetRolesAsync(user);
			return Ok(roles);
		}
	}
}
