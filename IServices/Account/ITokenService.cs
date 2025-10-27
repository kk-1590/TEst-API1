using AdvanceAPI.DTO.Account;

namespace AdvanceAPI.IServices.Account
{
    public interface ITokenService
    {
        Task<TokenResponse?> GenerateJSONWebToken(CreateTokenRequest? tokenRequest);
        Task<TokenResponse?> GenerateTokenFromRefreshToken(string? token, string? refreshToken);
    }
}
