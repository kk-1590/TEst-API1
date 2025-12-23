using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Account;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace AdvanceAPI.Services.Account
{
    public class AccountService : IAccountService
    {
        public readonly ITokenService _token;
        private readonly ILogger<AccountService> _logger;
        private readonly IGeneral _general;
        private readonly IAccountRepository _account;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;

        public AccountService(ITokenService token, ILogger<AccountService> logger, IGeneral general, IAccountRepository account, IDistributedCache cache, IConfiguration configuration)
        {
            _token = token;
            _logger = logger;
            _general = general;
            _account = account;
            _cache = cache;
            _configuration = configuration;
        }

        public async Task<ApiResponse> Login(UserLoginRequest? loginRequest)
        {
            try
            {
                string? password = _general.EncryptOrDecrypt(loginRequest?.Password!);
                if (string.IsNullOrEmpty(password))
                {
                    return new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Please provide valid credentials");
                }
                loginRequest!.UserId = loginRequest.UserId!.ToUpper();
                DataTable result = await _account.GetUserStatus(loginRequest?.UserId!);

                if (result == null || result.Rows.Count == 0)
                {
                    return new ApiResponse(StatusCodes.Status400BadRequest, "Sorry! Specified User Account Does Not Exist.....");
                }

                if (result.Rows[0]["WebActive"].ToString() != "1")
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Sorry! Your Login Has Been Disabled...");
                }

                string masterPassword = string.Empty;
                DataTable mainPassword = await _account.GetMainPassword();
                if (mainPassword != null && mainPassword.Rows.Count > 0)
                {
                    masterPassword =mainPassword.Rows[0][0]?.ToString() ?? string.Empty;
                }

                if (_general.EncryptOrDecrypt(result.Rows[0][1]?.ToString() ?? string.Empty) == loginRequest?.Password || _general.EncryptOrDecrypt( masterPassword) == loginRequest?.Password)
                {
                    if ((result.Rows[0]["currentstatus"]?.ToString()?.ToUpper() ?? string.Empty) == "ACTIVE")
                    {
                        DataTable advanceAccess = await _account.GetAdvanceAccess(loginRequest?.UserId);

                        if (advanceAccess != null && advanceAccess.Rows.Count > 0)
                        {
                            CreateTokenRequest createTokenRequest = new CreateTokenRequest();
                            createTokenRequest.EmployeeCode = loginRequest?.UserId;
                            createTokenRequest.AdditionalEmployeeCode = string.Empty;
                            DataTable additionalEmployeeCode = await _account.GetAdditionalEmployeeCode(loginRequest?.UserId);
                            if (additionalEmployeeCode != null && additionalEmployeeCode.Rows.Count > 0)
                            {
                                createTokenRequest.AdditionalEmployeeCode = additionalEmployeeCode.Rows[0][0]?.ToString();
                            }
                            createTokenRequest.Name = result.Rows[0]["first_name"]?.ToString();
                            createTokenRequest.Type = advanceAccess.Rows[0][0]?.ToString();
                            createTokenRequest.TypeAlways = advanceAccess.Rows[0][0]?.ToString();
                            createTokenRequest.MyRoles = advanceAccess.Rows[0][1]?.ToString();
                            createTokenRequest.AgainstRoles = advanceAccess.Rows[0][2]?.ToString();
                            createTokenRequest.Application = advanceAccess.Rows[0]["applicationupload"]?.ToString();
                            createTokenRequest.CampusCode = await GetEmployeeCampusCode(loginRequest?.UserId!);
                            createTokenRequest.Designation = result.Rows[0]["deisgnation"]?.ToString();
                            TokenResponse? tokenResponse = await _token.GenerateJSONWebToken(createTokenRequest);
                            tokenResponse!.Designation = createTokenRequest.Designation;
                            tokenResponse!.Department = result.Rows[0]["santioneddeptt"]?.ToString();
                            if (tokenResponse == null)
                            {
                                return new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an issue. Wait for some time to login.....");
                            }
                            return new ApiResponse(StatusCodes.Status200OK, "Success", tokenResponse);

                        }
                        else
                        {
                            return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Sorry! Your Account Does Not Have This Facility...");
                        }

                    }
                    else
                    {
                        return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Sorry! Your Account Is Not Activated...");
                    }
                }
                else
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Sorry! Account Credentials Mismatched...");
                }




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During Login. Parameters: Parameters: {Request}", loginRequest);
                return new ApiResponse(StatusCodes.Status500InternalServerError, "Sorry!! There is an error.. Please try after some time...");
            }

        }
        public async Task<ApiResponse> RefreshToken(RefreshTokenRequest? refreshTokenRequest)
        {
            try
            {
                TokenResponse? tokenResponse = await _token.GenerateTokenFromRefreshToken(refreshTokenRequest?.Token!, refreshTokenRequest?.RefreshToken!);
                if (tokenResponse == null)
                {
                    return new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid token details found....");
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", tokenResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid token details found. Parameters: Parameters: {Request}", refreshTokenRequest);
                return new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time...");
            }
        }
        public async Task<ApiResponse> Logout(string? token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid token details found....");
                }

                DataTable isValidToken = await _account.IsTokenNotLoggedout(token!);

                if (isValidToken == null || isValidToken.Rows.Count == 0)
                {
                    return new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid token details found....");
                }

                await _account.LogoutToken(token!);

                var tokenExpirationMinutes = _configuration.GetValue<int?>("Jwt:EXPIRATION_MINUTES") ?? 180;
                await _cache.SetStringAsync(token, bool.TrueString, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(tokenExpirationMinutes)
                });


                return new ApiResponse(StatusCodes.Status200OK, "Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid logout token details found. Parameters: Parameters: {Request}", token);
                return new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time...");
            }
        }


        public async Task<string> GetEmployeeCampusCode(string? employeeId)
        {
            try
            {
                if (string.IsNullOrEmpty(employeeId))
                {
                    return string.Empty;
                }

                using (DataTable dtEmployeeCampus = await _account.GetEmployeeCampusCode(employeeId))
                {
                    if (dtEmployeeCampus != null && dtEmployeeCampus.Rows.Count > 0)
                    {
                        return dtEmployeeCampus.Rows[0]["CampusCode"]?.ToString() ?? string.Empty;
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetEmployeeCampusCode. Parameters: Parameters: {Request}", employeeId);
                return string.Empty;
            }
        }


    }
}
