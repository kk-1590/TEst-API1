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
        public BudgetController(ILogger<BudgetController> logger, IBudget budget)
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
                ApiResponse apiResponse = await _Ibudget.AddItemWithSession(employeeId, mapNewMaad);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
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
                ApiResponse apiResponse = await _Ibudget.UpdateBudgetMaad(employeeId, mapNewMaad);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
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
                ApiResponse apiResponse = await _Ibudget.GetRecord(limitRequest.NoOfItems, limitRequest.ItemsFrom, limitRequest.CampusCode, limitRequest.Session, Convert.ToInt32(limitRequest.BudgetRequired));

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During add-new-budget-maad");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpPost]
        [Route("get-budget-required-maad")]
        public async Task<IActionResult> GetRequiredBudgerMaad([FromBody] LimitRequest limitRequest)
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
                ApiResponse apiResponse = await _Ibudget.GetMaadBudgetRequired(limitRequest);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-budget-required-maad");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpPost]
        [Route("get-budget-non-required-maad")]
        public async Task<IActionResult> GetRequiredNonBudgerMaad([FromBody] LimitRequest limitRequest)
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
                ApiResponse apiResponse = await _Ibudget.GetMaadNonBudgetRequired(limitRequest);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-budget-non-required-maad");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpGet]
        [Route("get-added-budget-maad-details/{RefNo}")]
        public async Task<IActionResult> GetSummaryDetails(string RefNo)
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
                ApiResponse apiResponse = await _Ibudget.GetAddedMaad(RefNo);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-added-budget-maad-details/refno");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpPut]
        [Route("update-budget-maad-details")]
        public async Task<IActionResult> UpdateDetails(UpdateBudgetDetails updateBudgetDetails)
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
                ApiResponse apiResponse = await _Ibudget.UpdateaadDetails(employeeId, updateBudgetDetails);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During update-budget-maad-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpDelete]
        [Route("Delete-budget-maad-details")]
        public async Task<IActionResult> DeleteDetails(UpdateBudgetDetails updateBudgetDetails)
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
                //Task<ApiResponse> DeleteaadDetails( UpdateBudgetDetails updateBudgetDetails)
                ApiResponse apiResponse = await _Ibudget.DeleteaadDetails(updateBudgetDetails);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During update-budget-maad-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }


        [HttpGet]
        [Route("get-budget-create-departments/{campusCode}")]
        public async Task<IActionResult> GetBudgetDepartments([FromRoute] string? campusCode)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                if (string.IsNullOrEmpty(campusCode))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _Ibudget.GetCreateBudgetSummaryDepartments(employeeId, campusCode);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-budget-create-departments");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("create-department-budget-summary")]
        public async Task<IActionResult> CreateDepartmentBudgetSummary([FromBody] CreateDepartmentBudgetSummaryRequest? budgetRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _Ibudget.CreateBudgetSummary(employeeId, budgetRequest);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During create-department-budget-summary");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }


    }
}
