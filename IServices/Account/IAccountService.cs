using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Account;

namespace AdvanceAPI.IServices.Account
{
    public interface IAccountService
    {
        Task<ApiResponse> Login(UserLoginRequest? loginRequest);
        Task<ApiResponse> RefreshToken(RefreshTokenRequest? refreshTokenRequest);
        Task<ApiResponse> Logout(string? token);

    }
}
