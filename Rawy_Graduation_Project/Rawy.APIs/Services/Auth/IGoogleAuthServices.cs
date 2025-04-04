using Google.Apis.Auth;
using Rawy.APIs.Dtos;

namespace Rawy.APIs.Services.Auth
{
	public interface IGoogleAuthServices
	{
		Task<TokenDto> AuthenticateWithGoogleAsync(string idToken);
		Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken);
	}
}
