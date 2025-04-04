using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rawy.APIs.Dtos;
using Rawy.APIs.Services.Token;
using Rawy.DAL.Models;

namespace Rawy.APIs.Services.Auth
{
	public class GoogleAuthService : IGoogleAuthServices
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenService _tokenService;
		private readonly string googleClientId;

		public GoogleAuthService(UserManager<AppUser> userManager,ITokenService tokenService,IConfiguration configuration)
        {
			_userManager = userManager;
			_tokenService = tokenService;
			googleClientId = configuration["Authentication:Google:ClientId"]
				?? throw new ArgumentNullException(nameof(configuration),"Google Client Id Is Not Configured In Appsetting.json");
		}
        public async Task<TokenDto> AuthenticateWithGoogleAsync(string idToken)
		{
			try
			{
				//Validate The IdToken And Retrieve The Payload
				var payload = await ValidateGoogleTokenAsync(idToken);

				if (payload == null)
					throw new Exception("Invalid Google token payload.");

				// Check If User Already Exist By Email

				var user = await _userManager.FindByEmailAsync(payload.Email);
					if (user == null)
					{
						user = new AppUser
						{
							UserName = payload.Email,
							Email = payload.Email
						};
						var result = await _userManager.CreateAsync(user);
						await _userManager.AddToRoleAsync(user, "User"); // Assign a default role

						if (!result.Succeeded)
						{
							var Errors = String.Join(", ",result.Errors.Select(e => e.Description));
							throw new Exception($"Failed To Creste User : {Errors}");
						}
					}


				var token = _tokenService.CreateTokenAsync(user, _userManager);
				return new TokenDto
				{
					Token =await token,
					Username = user.UserName
				};


			}
			catch (Exception Ex)
			{

				throw new Exception($"Google Authentication Failed : {Ex.Message}", Ex);
			}
		}

		public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string token)
		{
			try
			{
				var settings = new GoogleJsonWebSignature.ValidationSettings
				{
					// Set the valid audience (your Google Client ID)
					Audience = new[] { "728393018149-3kqintj00utg35ne0fmv5tp1ov2iprpl.apps.googleusercontent.com" }
				};

				// Validate the token and extract the payload
				var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
				return payload;
			}
			catch (Exception ex)
			{
				// Log error and return null
				Console.WriteLine($"Google Token Validation Failed: {ex.Message}");
				return null;
			}
		}
	}
}
