using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdvanceAPI.Controllers
{
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly ILogger<BudgetController> _logger;
        public BudgetController(ILogger<BudgetController> logger) 
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("add-new-budget-maad")]
        public async Task<IActionResult> AddStockItem([FromBody] MapNewMaad mapNewMaad)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
              
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //ApiResponse apiResponse = await _approvalService.AddItemDraft(Addstock, employeeId);

                return Ok();// apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-stock-item-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
    }
}
