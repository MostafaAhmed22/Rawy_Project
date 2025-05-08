using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.AcoountDtos;
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

		public async Task<ApiResponse> RegisterWriterAsync(RegisterDto model)
		{
			var existingUser = await _userManager.FindByEmailAsync(model.Email);
			if (existingUser != null)
				return new ApiResponse(400, "Email already in use.");

			var user = _mapper.Map<AppUser>(model);
			var result = await _userManager.CreateAsync(user, model.Password);

			var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
			if (!result.Succeeded)
				return new ApiResponse(400, errors);

			//var writer = new Writer
			//{
			//	WriterId = user.Id,
			//	FName = model.FirstName,
			//	LName = model.LastName,
				
			//};

			//await _unitOfWork.WriterRepository.AddAsync(writer);

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
			if (user == null) return new ApiResponse(404, "Email doesn't exist");
			var code = new Random().Next(100000, 999999).ToString();
			var resetCode = new ResetPassword
			{
				Email = model.Email,
				Code = code,
				CreatedAt = DateTime.UtcNow
			};

			await _unitOfWork.ResetPasswordRepository.AddAsync(resetCode);
			_unitOfWork.Complete();

			var email = new Email
			{
				Subject = "Reset Your Password",
				Body = code,
				Recipents = user.Email
			};

			await EmailService.SendEmailAsync(email);
			return new ApiResponse(200, "Reset code sent to email");
		}

		public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordDto model)
		{

			var resetEntry = await _unitOfWork.ResetPasswordRepository
			.GetFirstOrDefaultAsync(r => r.Email == model.Email);

			//if (resetEntry == null || resetEntry.Expiration < DateTime.UtcNow)
			//	return new ApiResponse(400, "Invalid or expired code");

			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null) return new ApiResponse(404, "User not found");

			var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
			var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

			var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
			if (!result.Succeeded)
				return new ApiResponse(400, errors);

			await _unitOfWork.ResetPasswordRepository.Delete(resetEntry);
			 _unitOfWork.Complete();

			return new ApiResponse(200, "Password reset successful");

			#region LinkReset
			//try
			//{
			//	var principal = _tokenService.GetPrincipalFromToken(model.Token);
			//	var resetToken = principal.Claims.FirstOrDefault(c => c.Type == "resetToken")?.Value;
			//	if (string.IsNullOrEmpty(resetToken)) return new ApiResponse(400, "Invalid token");

			//	var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

			//	var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
			//	if (!result.Succeeded)
			//		return new ApiResponse(400, errors);

			//	return new ApiResponse(200, "Password reset successful.");
			//}
			//catch
			//{
			//	return new ApiResponse(400, "Invalid or expired token");
			////} 
			#endregion
		}

		public async Task<bool> VerifyResetCodeAsync(string code)
		{
			var resetEntry = await _unitOfWork.ResetPasswordRepository
			.GetFirstOrDefaultAsync(r => r.Code == code);

			if (resetEntry == null || resetEntry.IsExpired())
				return false;

			return true;
		}
	}
}
