using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;
using AdvanceAPI.IServices.Budget;
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
        private readonly IBudget _Ibudget;
        public BudgetController(ILogger<BudgetController> logger,IBudget budget) 
        {
            _logger = logger;
            _Ibudget = budget;
        }

        [HttpPatch]
        [Route("add-new-budget-maad")]
        public async Task<IActionResult> AddnewMaadForBudget([FromBody] MapNewMaad mapNewMaad)
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
                ApiResponse apiResponse = await _Ibudget.AddItemWithSession(employeeId,mapNewMaad);

                return  apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During add-new-budget-maad");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpPut]
        [Route("update-budget-maad")]
        public async Task<IActionResult> UpdateMaadForBudget([FromBody] UpdateMaadBudegtRequest mapNewMaad)
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
                ApiResponse apiResponse = await _Ibudget.UpdateBudgetMaad (employeeId,mapNewMaad);

                return  apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During add-new-budget-maad");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        
        [HttpGet]
        [Route("get-budget-maad-details")]
        public async Task<IActionResult> getdetails([FromBody] LimitRequest limitRequest)
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
                ApiResponse apiResponse = await _Ibudget.GetRecord  (limitRequest.NoOfItems,limitRequest.ItemsFrom);

                return  apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During add-new-budget-maad");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
    }
}
