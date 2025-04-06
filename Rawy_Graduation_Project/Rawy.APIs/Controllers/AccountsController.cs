using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rawy.APIs.Dtos;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;
using Rawy.APIs.Services.Token;
using Rawy.APIs.Services.Auth;

namespace Rawy.APIs.Controllers
{
    public class AccountsController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGoogleAuthServices _googleAuthService;
		private readonly IFacebookAuthServices _facebookAuthServices;

		public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService,IMapper mapper,
								IUnitOfWork unitOfWork, IGoogleAuthServices googleAuthService,IFacebookAuthServices facebookAuthServices)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_googleAuthService = googleAuthService;
			_facebookAuthServices = facebookAuthServices;
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


		[HttpPost("register-writer")]
		public async Task<IActionResult> RegisterWriter([FromBody] WriterDto model)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var existinguser = await _userManager.FindByEmailAsync(model.Email);
			if(existinguser != null )
				return BadRequest(new { message = "Email already in use." });


			var user = _mapper.Map<AppUser>(model);
			//	new AppUser
			//{
			//	Email = model.Email,
			//	UserName = model.username,
			//	PhoneNumber = model.PhoneNumber
			//};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				 return BadRequest(result.Errors);


			// Assign the WRITER role
		//	await _userManager.AddToRoleAsync(user, "WRITER");

			// Create a Writer linked to the user
			var writer = new Writer
			{
				WriterId = user.Id,
				FName = model.FirstName,
				LName = model.LastName,
				PreferedLanguage = model.PreferredLanguage,
				WritingStyle = model.WritingStyle
			};

			await _unitOfWork.WriterRepository.AddAsync(writer);
			var ReturnedUser = new UserDto()
			{
				UserName = user.UserName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			};
			return Ok(ReturnedUser);
		}

		// Login
		[HttpPost("Login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user is null) return Unauthorized(new ApiResponse(401));

			var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

			if (!result.Succeeded) return Unauthorized(new ApiResponse(401));


			return Ok(new UserDto()
			{
				UserName = user.UserName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			});


		}

		[HttpPost("GoogleLogin")]
		public async Task<IActionResult> GoogleLogin([FromBody] ExternalAuthDto model)
		{
			var token = await _googleAuthService.AuthenticateWithGoogleAsync(model.Token);
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


		[HttpPost("FacebookLogin")]
		public async Task<ActionResult<TokenDto>> FacebookLogin([FromBody] ExternalAuthDto model)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);

			var token = await _facebookAuthServices.AuthenticateWithFacebookAsync(model.Token);
			return Ok(token);
		}



	}
}
