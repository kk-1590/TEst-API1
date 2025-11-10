using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;
using AdvanceAPI.IServices.Budget;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdvanceAPI.Controllers
{
    [ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [Authorize]
    [Tags($"Budget Management V2.0")]
    public class BudgetV2Controller : ControllerBase
    {
        private readonly ILogger<BudgetV2Controller> _logger;
        private readonly IBudgetV2 _iBudget;
        public BudgetV2Controller(ILogger<BudgetV2Controller> logger, IBudgetV2 budget)
        {
            _logger = logger;
            _iBudget = budget;
        }

        [HttpGet]
        [Route("get-budget-session-filters/{campusCode}")]
        public async Task<IActionResult> GetBudgetFilterSessions(string? campusCode)
        {
            try
            {
                ApiResponse apiResponse = await _iBudget.GetBudgetFilterSessions(campusCode);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-budget-session-filters");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-budget-session-amount-summary")]
        public async Task<IActionResult> GetBudgetSessionAmountSummary(BudgetSessionAmountSummaryRequest? summaryRequest)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (summaryRequest == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _iBudget.GetBudgetSessionAmountSummary(summaryRequest);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-budget-session-amount-summary");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }


    }
}
