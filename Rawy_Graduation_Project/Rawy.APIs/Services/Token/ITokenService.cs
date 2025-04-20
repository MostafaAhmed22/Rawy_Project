using Microsoft.AspNetCore.Identity;
using Rawy.DAL.Models;
using System.Security.Claims;

namespace Rawy.APIs.Services.Token
{
	public interface ITokenService
	{
		Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager,string resetToken = null);
		ClaimsPrincipal GetPrincipalFromToken(string token);
	}
}
