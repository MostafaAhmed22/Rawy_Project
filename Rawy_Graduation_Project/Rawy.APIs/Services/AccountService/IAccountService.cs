using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.AcoountDtos;

namespace Rawy.APIs.Services.AccountService
{
    public interface IAccountService
    {
        Task<ApiResponse> RegisterWriterAsync(RegisterDto model);
        Task<UserDto> LoginAsync(LoginDto model);
        Task<TokenDto> GoogleLoginAsync(string token);
        Task<ApiResponse> ForgotPasswordAsync(ForgetPasswordDto model);
        Task<bool> VerifyResetCodeAsync(string code);

		Task<ApiResponse> ResetPasswordAsync(ResetPasswordDto model);
    }
}
