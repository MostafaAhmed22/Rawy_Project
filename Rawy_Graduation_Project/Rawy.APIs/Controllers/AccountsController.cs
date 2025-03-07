using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rawy.APIs.Dtos;
using Rawy.BLL.Interfaces;
using Rawy.DAL.Models;

namespace Rawy.APIs.Controllers
{
	public class AccountsController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;

		public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService,IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_tokenService = tokenService;
			_mapper = mapper;
		}

		// Register
		[HttpPost("Register")] // POST // api/accounts/Register
		public async Task<ActionResult<UserDto>> Register(RegisterDto model)
		{
			// _mapper.Map<AppUser>(model);
			var user = new AppUser()
			{
					FName = model.FName,
					LName = model.LName,
					Email = model.Email,
					UserName = model.Email.Split("@")[0],
					PhoneNumber = model.PhoneNumber
				};

			var Result = await _userManager.CreateAsync(user, model.Password);
			if (!Result.Succeeded) return BadRequest(new ApiResponse(400));


			//_mapper.Map<UserDto>(user);
			var ReturnedUser = new UserDto()
			{
					FName = user.FName,
					LName = user.LName,
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
				FName = user.FName,
				LName = user.LName,
				Email = user.Email,
				Token = await _tokenService.CreateTokenAsync(user, _userManager)
			});


		}
	}
}
