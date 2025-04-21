using Rawy.APIs.Dtos;

namespace Rawy.APIs.Services.AccountService
{
    public interface IAccountService
    {
        Task<ApiResponse> RegisterWriterAsync(WriterDto model);
        Task<UserDto> LoginAsync(LoginDto model);
        Task<TokenDto> GoogleLoginAsync(string token);
        Task<ApiResponse> ForgotPasswordAsync(ForgetPasswordDto model);
        Task<ApiResponse> ResetPasswordAsync(ResetPasswordDto model);
    }
}
