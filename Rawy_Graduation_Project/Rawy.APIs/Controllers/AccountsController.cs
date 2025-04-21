using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rawy.APIs.Dtos;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;
using Rawy.APIs.Services.Token;
using Rawy.APIs.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Rawy.APIs.Helper;
using Rawy.APIs.Services.AccountService;

namespace Rawy.APIs.Controllers
{
    public class AccountsController : BaseApiController
	{
		private readonly IAccountService _accountService;

		public AccountsController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		// Register
		#region OldRegister
		//[HttpPost("Register")] // POST // api/accounts/Register
		//public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		//{
		//	// _mapper.Map<AppUser>(model);
		//	var user = new AppUser()
		//	{
		//			//FName = model.FName,
		//			//LName = model.LName,
		//			Email = model.Email,
		//			UserName = model.Email.Split("@")[0],
		//			PhoneNumber = model.PhoneNumber
		//		};

		//	var Result = await _userManager.CreateAsync(user, model.Password);
		//	if (!Result.Succeeded) return BadRequest(new ApiResponse(400));


		//	//_mapper.Map<UserDto>(user);
		//	var ReturnedUser = new UserDto()
		//	{
		//		//	FName = user.FName,
		//		//	LName = user.LName,
		//			Email = user.Email,
		//			Token = await _tokenService.CreateTokenAsync(user, _userManager)
		//		};
		//	return Ok(ReturnedUser);

		//} 
		#endregion


		[HttpPost("Register")]
		public async Task<IActionResult> RegisterWriter([FromBody] WriterDto model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var response = await _accountService.RegisterWriterAsync(model);
			return StatusCode(response.StatusCode, response.Data);
		}

		// Login
		[HttpPost("Login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var userDto = await _accountService.LoginAsync(model);
				return Ok(userDto);
			}
			catch
			{
				return Unauthorized(new ApiResponse(401, "Invalid credentials"));
			}

		}

		[HttpPost("GoogleLogin")]
		public async Task<IActionResult> GoogleLogin([FromBody] ExternalAuthDto model)
		{
			var token = await _accountService.GoogleLoginAsync(model.Token);
			return Ok(token);
			#region LogicWithoutGoogleService
			//var payload = await _googleAuthService.VerifyGoogleTokenAsync(model.IdToken);
			//if (payload == null)
			//{
			//	return BadRequest(new { message = "Invalid Google token." });
			//}

			//var user = await _userManager.FindByEmailAsync(payload.Email);
			//if (user == null)
			//{
			//	user = new AppUser
			//	{
			//		UserName = payload.Email,
			//		Email = payload.Email
			//	};
			//	var result = await _userManager.CreateAsync(user);
			//	if (!result.Succeeded)
			//	{
			//		return BadRequest(result.Errors);
			//	}

			//	await _userManager.AddToRoleAsync(user, "User"); // Assign a default role
			//}

			//var token = _tokenService.CreateTokenAsync(user,_userManager);
			//return Ok(new { token }); 
			#endregion
		}


		//[HttpPost("FacebookLogin")]
		//public async Task<ActionResult<TokenDto>> FacebookLogin([FromBody] ExternalAuthDto model)
		//{
		//	if (!ModelState.IsValid) return BadRequest(ModelState);

		//	var token = await _facebookAuthServices.AuthenticateWithFacebookAsync(model.Token);
		//	return Ok(token);
		//}



		/// Initiates the Facebook login process.
		
		//[HttpGet("Facebook-Login")]
		//public IActionResult FacebookLogin()

		//{
		//	var redirectUri = Url.Action("FacebookCallback", "Auth", null, Request.Scheme);
		//	var properties = new AuthenticationProperties { RedirectUri = redirectUri };
		//	return Challenge(properties, "Facebook");
		//}

		
		/// Handles the callback from Facebook after authentication.
		
		//[HttpGet("Facebook-Callback")]
		//public async Task<IActionResult> FacebookCallback()
		//{

		//	var authenticateResult = await HttpContext.AuthenticateAsync("Facebook");
		//	if (!authenticateResult.Succeeded)
		//	{
		//		return BadRequest("Facebook authentication failed.");
		//	}

		//	// Extract user information from Facebook
		//	var externalUser = authenticateResult.Principal;
		//	var email = externalUser.FindFirst(ClaimTypes.Email)?.Value;
		//	var name = externalUser.FindFirst(ClaimTypes.Name)?.Value;

		//	if (string.IsNullOrEmpty(email))
		//	{
		//		return BadRequest("Email claim not found in Facebook response.");
		//	}

		//	// Create new Appuser

		//	var user = await _userManager.FindByEmailAsync(email);
		//	if (user == null)
		//	{
		//		user = new AppUser
		//		{
		//			Email = email,
		//			UserName = name
		//		};
		//		var result = await _userManager.CreateAsync(user);
		//		if (!result.Succeeded)
		//		{
		//			return BadRequest("Failed to create user.");
		//		}
		//	}
		//		// Generate a JWT token for the authenticated user
		//		var token = await _tokenService.CreateTokenAsync(user, _userManager);
			
		//	return Ok(new { Token = token });
		//}

		[HttpPost("forgot-password")]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordDto model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var response = await _accountService.ForgotPasswordAsync(model);
			return StatusCode(response.StatusCode, response.Message);
		}


		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var response = await _accountService.ResetPasswordAsync(model);
			return StatusCode(response.StatusCode, response.Message);
		}
	}
}
