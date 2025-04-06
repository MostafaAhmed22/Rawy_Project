using Google.Apis.Auth;
using Rawy.APIs.Dtos;

namespace Rawy.APIs.Services.Auth
{
	public interface IFacebookAuthServices
	{
		Task<TokenDto> AuthenticateWithFacebookAsync(string accessToken);
	//	Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string idToken);
	}
}
