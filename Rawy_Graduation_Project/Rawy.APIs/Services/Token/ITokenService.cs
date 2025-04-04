using Microsoft.AspNetCore.Identity;
using Rawy.DAL.Models;

namespace Rawy.APIs.Services.Token
{
	public interface ITokenService
	{
		Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager);
	}
}
