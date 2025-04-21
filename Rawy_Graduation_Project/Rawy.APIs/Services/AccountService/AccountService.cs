using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Rawy.APIs.Dtos;
using Rawy.APIs.Helper;
using Rawy.APIs.Services.Auth;
using Rawy.APIs.Services.Token;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;

namespace Rawy.APIs.Services.AccountService
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGoogleAuthServices _googleAuthService;
		private readonly IConfiguration _configuration;

		public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
							  ITokenService tokenService, IMapper mapper, IUnitOfWork unitOfWork,
							  IGoogleAuthServices googleAuthService,
							  IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_googleAuthService = googleAuthService;
			_configuration = configuration;
		}

		public async Task<ApiResponse> RegisterWriterAsync(WriterDto model)
		{
			var existingUser = await _userManager.FindByEmailAsync(model.Email);
			if (existingUser != null)
				return new ApiResponse(400, "Email already in use.");

			var user = _mapper.Map<AppUser>(model);
			var result = await _userManager.CreateAsync(user, model.Password);

			var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
			if (!result.Succeeded)
				return new ApiResponse(400, errors);

			var writer = new Writer
			{
				WriterId = user.Id,
				FName = model.FirstName,
				LName = model.LastName,
				PreferedLanguage = model.PreferredLanguage,
				WritingStyle = model.WritingStyle
			};

			await _unitOfWork.WriterRepository.AddAsync(writer);

			return new ApiResponse(200, "Success", new UserDto
			{
				Email = user.Email,
				UserName = user.UserName,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			});

		}

		public async Task<UserDto> LoginAsync(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null) throw new UnauthorizedAccessException();

			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
			if (!result.Succeeded) throw new UnauthorizedAccessException();

			return new UserDto
			{
				Email = user.Email,
				UserName = user.UserName,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			};
		}

		public async Task<TokenDto> GoogleLoginAsync(string token)
		{
			return await _googleAuthService.AuthenticateWithGoogleAsync(token);
		}


		public async Task<ApiResponse> ForgotPasswordAsync(ForgetPasswordDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null) return new ApiResponse(200, "Email doesn't exist");

			var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
			var jwtToken = await _tokenService.CreateTokenAsync(user, _userManager, resetToken);
			var resetLink = $"{_configuration["BaseUrl"]}/reset-password?token={jwtToken}&email={user.Email}";

			var email = new Email
			{
				Subject = "Reset Your Password",
				Body = $"Please reset your password by clicking here: <a href='{resetLink}'>Reset Password</a>",
				Recipents = user.Email
			};

			await EmailService.SendEmailAsync(email);
			return new ApiResponse(200, "Reset link sent if email exists.");
		}

		public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null) return new ApiResponse(400, "Invalid request");

			try
			{
				var principal = _tokenService.GetPrincipalFromToken(model.Token);
				var resetToken = principal.Claims.FirstOrDefault(c => c.Type == "resetToken")?.Value;
				if (string.IsNullOrEmpty(resetToken)) return new ApiResponse(400, "Invalid token");

				var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

				var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
				if (!result.Succeeded)
					return new ApiResponse(400, errors);

				return new ApiResponse(200, "Password reset successful.");
			}
			catch
			{
				return new ApiResponse(400, "Invalid or expired token");
			}
		}
	}
}
