using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rawy.BLL.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}



		public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager)
		{


			//Header

			//Payload
			// Private Claims
			var AuthClaims = new List<Claim>(){
				//new Claim(ClaimTypes.GivenName ,user.FName),
				new Claim(ClaimTypes.Email ,user.Email)

			};

			var UserRoles = await _userManager.GetRolesAsync(user);
			foreach (var Role in UserRoles)
			{
				AuthClaims.Add(new Claim(ClaimTypes.Role, Role));
			}



			//Signature
			// Key
			var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

			var token = new JwtSecurityToken(
				issuer: _configuration["JWT:ValidIssuer"],
				audience: _configuration["JWT:MySecureKey"],
				expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
				claims: AuthClaims,
				signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
