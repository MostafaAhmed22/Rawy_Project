using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Rawy.DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rawy.APIs.Services.Token
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
		{
			_configuration = configuration;
		}



		public async Task<string?> CreateTokenAsync(AppUser user, UserManager<AppUser> _userManager, string resetToken = null)
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

			if (!string.IsNullOrEmpty(resetToken))
			{
				AuthClaims.Add(new Claim("resetToken", resetToken));
			}




			//Signature
			// Key
			var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

			var token = new JwtSecurityToken(
				issuer: _configuration["JWT:Issuer"],
				audience: _configuration["JWT:Audience"],
				expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
				claims: AuthClaims,
				signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}


		public ClaimsPrincipal GetPrincipalFromToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]) ?? throw new InvalidOperationException("JWT Key is not configured"); 

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = _configuration["JWT:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured"),
				ValidAudience = _configuration["JWT:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured"),
				IssuerSigningKey = new SymmetricSecurityKey(key)
			};

			return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
		}
	}


}
