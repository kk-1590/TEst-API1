using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Approval;
using AdvanceAPI.IServices.Approval;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace AdvanceAPI.Controllers
{

    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [Authorize]
    public class ApprovalController : ControllerBase
    {
        private readonly ILogger<ApprovalController> _logger;
        private readonly IApprovalService _approvalService;
        public ApprovalController(ILogger<ApprovalController> logger, IApprovalService approvalService)
        {
            _logger = logger;
            _approvalService = approvalService;
        }
        [HttpPost]
        [Route("Add-stock-item-details")]
        [AllowAnonymous]
        public async Task<IActionResult> AddStockItem([FromBody] AddStockItemRequest Addstock)
        {
            try
            {
                // string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string employeeId = "GLA108236";
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse apiResponse = await _approvalService.AddItemDraft(Addstock, employeeId);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-stock-item-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

    }


}
