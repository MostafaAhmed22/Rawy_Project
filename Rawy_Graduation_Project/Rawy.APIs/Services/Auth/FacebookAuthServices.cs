using Microsoft.AspNetCore.Identity;
using Rawy.APIs.Dtos;
using Rawy.APIs.Services.Token;
using Rawy.DAL.Models;
using System.Text.Json;

namespace Rawy.APIs.Services.Auth
{
	public class FacebookAuthServices : IFacebookAuthServices
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenService _tokenService;
		private readonly HttpClient _httpClient;

		public FacebookAuthServices(UserManager<AppUser> userManager, ITokenService tokenService)
        {
			_userManager = userManager;
			_tokenService = tokenService;
			_httpClient = new HttpClient();
		}
        public async Task<TokenDto> AuthenticateWithFacebookAsync(string accessToken)
		{
			try
			{
				// Get user info from Facebook Graph API
				var fbUserInfoUrl = $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}";
				var response = await _httpClient.GetAsync(fbUserInfoUrl);

				if (!response.IsSuccessStatusCode)
					throw new Exception("Invalid Facebook token.");

				var json = await response.Content.ReadAsStringAsync();
				var fbData = JsonSerializer.Deserialize<FacebookUserDto>(json);

				if (string.IsNullOrEmpty(fbData.Email))
					throw new Exception("Email is required from Facebook login.");

				// Check if user exists in DB
				var user = await _userManager.FindByEmailAsync(fbData.Email);
				if (user == null)
				{
					user = new AppUser
					{
						UserName = fbData.Email,
						Email = fbData.Email
					};

					var result = await _userManager.CreateAsync(user);
					if (!result.Succeeded)
					{
						var errors = string.Join(", ", result.Errors.Select(e => e.Description));
						throw new Exception($"Failed to create user: {errors}");
					}

					// await _userManager.AddToRoleAsync(user, "User");
				}

				var token = await _tokenService.CreateTokenAsync(user, _userManager);
				return new TokenDto
				{
					Token = token,
					Username = user.UserName
				};
			}
			catch (Exception ex)
			{
				throw new Exception($"Facebook Authentication Failed: {ex.Message}", ex);
			}
		}
	}
}
