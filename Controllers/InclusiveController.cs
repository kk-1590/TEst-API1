using AdvanceAPI.DTO;
using AdvanceAPI.IServices.Inclusive;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdvanceAPI.Controllers
{
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [Authorize]
    public class InclusiveController : ControllerBase
    {
        private readonly ILogger<InclusiveController> _logger;
        private readonly IInclusiveService _inclusiveService;

        public InclusiveController(ILogger<InclusiveController> logger, IInclusiveService inclusiveService)
        {
            _logger = logger;
            _inclusiveService = inclusiveService;
        }

        [HttpGet]
        [Route("get-campus")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _inclusiveService.GetCampusList(employeeId);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-campus-list....");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }


    }
}
