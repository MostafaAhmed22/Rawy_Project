using Rawy.APIs.Dtos;
using Rawy.APIs.Dtos.AcoountDtos;

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
