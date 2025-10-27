using Microsoft.IdentityModel.Tokens;
using AdvanceAPI.DTO.Account;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices.Account;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AdvanceAPI.Services.Account
{
    public class TokenService(IConfiguration configuration, ILogger<TokenService> logger, IAccountRepository procedures) : ITokenService
    {
        private readonly IConfiguration _config = configuration;
        private readonly ILogger<TokenService> _logger = logger;
        private readonly IAccountRepository _procedures = procedures;

        public async Task<TokenResponse?> GenerateJSONWebToken(CreateTokenRequest? tokenRequest)
        {
            var key = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expirationMinutes = _config["Jwt:EXPIRATION_MINUTES"];
            var refreshExpirationMinutes = _config["Jwt:REFRESH_EXPIRATION_MINUTES"];

            if (key == null || issuer == null || audience == null || expirationMinutes == null)
            {
                _logger.LogError("JWT configuration is missing.");
                throw new InvalidOperationException("JWT configuration is missing.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, tokenRequest?.EmployeeCode!),
                new Claim(ClaimTypes.Name, tokenRequest?.Name!),
                new Claim(ClaimTypes.Role, tokenRequest?.MyRoles!),
                new Claim(ClaimTypes.Authentication, tokenRequest?.Type!)
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(expirationMinutes)),
                signingCredentials: credentials);

            string refreshToken = GenerateRefreshToken();
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            await _procedures.SaveToken(tokenRequest?.EmployeeCode, tokenRequest?.Name, tokenRequest?.Type, tokenString, DateTime.Now.AddMinutes(Convert.ToInt32(expirationMinutes)).ToString("yyyy-MM-dd HH:mm:ss"), refreshToken, DateTime.Now.AddMinutes(Convert.ToInt32(refreshExpirationMinutes)).ToString("yyyy-MM-dd HH:mm:ss"), tokenRequest?.RefreshTokenId);

            return new TokenResponse
            {
                EmployeeCode = tokenRequest?.EmployeeCode!,
                AdditionalEmployeeCode = tokenRequest?.AdditionalEmployeeCode!,
                Name = tokenRequest?.Name!,
                Type = tokenRequest?.Type!,
                TypeAlways = tokenRequest?.TypeAlways!,
                MyRoles = tokenRequest?.MyRoles!,
                AgainstRoles = tokenRequest?.AgainstRoles!,
                Application = tokenRequest?.Application!,
                Token = tokenString,
                Expires = token.ValidTo.Ticks,
                RefreshToken = refreshToken,
                RefreshExpires = DateTime.Now.AddMinutes(Convert.ToInt32(refreshExpirationMinutes)).Ticks
            };
        }

        public async Task<TokenResponse?> GenerateTokenFromRefreshToken(string? token, string? refreshToken)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            // Validate the refresh token
            var principal = GetPrincipalFromExpiredToken(token);
            if (principal == null)
            {
                return null;
            }

            // Extract claims from the expired token
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var name = principal.FindFirst(ClaimTypes.Name)?.Value;
            var role = principal.FindFirst(ClaimTypes.Role)?.Value;
            var type=principal.FindFirst(ClaimTypes.Authentication)?.Value;

            if (userId == null || name == null || role == null)
            {
                return null;
            }

            DataTable dt = await _procedures.ValidateToken(userId, token, refreshToken);

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            await _procedures.UseToken(dt.Rows[0]["TokenId"]?.ToString()!);

            var newTokenRequest = new CreateTokenRequest
            {
                EmployeeCode = userId,
                Name = name,
                Type = role,
                MyRoles=role,
                RefreshTokenId = dt.Rows[0]["TokenId"]?.ToString()
            };

            return await GenerateJSONWebToken(newTokenRequest);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)),
                ValidateLifetime = true,
                ValidAudience = _config["Jwt:Audience"],
                ValidIssuer = _config["Jwt:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
