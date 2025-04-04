using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;

namespace Rawy.BLL.Services
{
	public class GoogleAuthService
	{
		public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string token)
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
