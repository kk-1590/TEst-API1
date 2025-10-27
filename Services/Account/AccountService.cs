using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Account;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace AdvanceAPI.Services.Account
{
    public class AccountService : IAccountService
    {
        public readonly ITokenService _token;
        private readonly ILogger<AccountService> _logger;
        private readonly IGeneral _general;
        private readonly IAccountRepository _procedures;

        public AccountService(ITokenService token, ILogger<AccountService> logger, IGeneral general, IAccountRepository procedures)
        {
            _token = token;
            _logger = logger;
            _general = general;
            _procedures = procedures;
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

                DataTable result = await _procedures.GetUserStatus(loginRequest?.UserId!);

                if (result == null || result.Rows.Count == 0)
                {
                    return new ApiResponse(StatusCodes.Status400BadRequest, "Sorry! Specified User Account Does Not Exist.....");
                }

                if (result.Rows[0]["WebActive"].ToString() != "1")
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Sorry! Your Login Has Been Disabled...");
                }

                string masterPassword = string.Empty;
                DataTable mainPassword = await _procedures.GetMainPassword();
                if (mainPassword != null && mainPassword.Rows.Count > 0)
                {
                    masterPassword = mainPassword.Rows[0][0]?.ToString() ?? string.Empty;
                }

                if (_general.EncryptOrDecrypt(result.Rows[0][1]?.ToString() ?? string.Empty) == password || masterPassword == _general.EncryptOrDecrypt(password))
                {
                    if ((result.Rows[0]["currentstatus"]?.ToString()?.ToUpper() ?? string.Empty) == "ACTIVE")
                    {
                        DataTable advanceAccess = await _procedures.GetAdvanceAccess(loginRequest?.UserId);

                        if (advanceAccess != null && advanceAccess.Rows.Count > 0)
                        {
                            CreateTokenRequest createTokenRequest = new CreateTokenRequest();
                            createTokenRequest.EmployeeCode = loginRequest?.UserId;
                            createTokenRequest.AdditionalEmployeeCode = string.Empty;
                            DataTable additionalEmployeeCode = await _procedures.GetAdditionalEmployeeCode(loginRequest?.UserId);
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

                            TokenResponse? tokenResponse = await _token.GenerateJSONWebToken(createTokenRequest);

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
                return new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Sorry!! Invalid token details found...");
            }
        }

    }
}
