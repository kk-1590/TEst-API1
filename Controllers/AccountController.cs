using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Account;
using AdvanceAPI.IServices.Account;

namespace AdvanceAPI.Controllers
{
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _account;

        public AccountController(ILogger<AccountController> logger, IAccountService account)
        {
            _logger = logger;
            _account = account;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginRequest? loginRequest)
        {
            try
            {
                if (loginRequest == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Please provide valid credentials"));
                }
                if (!ModelState.IsValid)
                {
                    string? error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, error));
                }
                if (string.IsNullOrEmpty(loginRequest.UserId) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Please provide valid credentials"));
                }
                ApiResponse apiResponse = await _account.Login(loginRequest);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During Login. Parameters: Parameters: {Request}", loginRequest);
                return BadRequest(new ApiResponse(StatusCodes.Status500InternalServerError, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest? refreshTokenRequest)
        {
            try
            {
                if (refreshTokenRequest == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Please provide valid token details..."));
                }
                if (!ModelState.IsValid)
                {
                    string? error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, error));
                }
                if (string.IsNullOrEmpty(refreshTokenRequest.Token) || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Please provide valid token details..."));
                }

                ApiResponse apiResponse = await _account.RefreshToken(refreshTokenRequest);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During Refresh Token. Parameters: Parameters: {Request}", refreshTokenRequest);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

    }
}
