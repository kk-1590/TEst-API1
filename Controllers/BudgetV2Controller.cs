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

        [HttpPost]
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

        [HttpPatch]
        [Route("update-budget-session-amount-summary")]
        public async Task<IActionResult> UpdateBudgetSessionAmountSummary(UpdateBudgetSessionAmountRequest? updateRequest)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (updateRequest == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                string? employeeId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _iBudget.UpdateBudgetSessionAmountSummary(updateRequest, employeeId);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During update-budget-session-amount-summary");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("add-budget-session-amount-summary")]
        public async Task<IActionResult> AddBudgetSessionAmountSummary(CreateNewBudgetSessionAmountSummaryRequest? createRequest)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (createRequest == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                string? employeeId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _iBudget.AddBudgetSessionAmountSummary(createRequest, employeeId);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During add-budget-session-amount-summary");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpDelete]
        [Route("delete-budget-session-amount-summary")]
        public async Task<IActionResult> DeleteBudgetSessionSummaryAmount(DeleteBudgetSessionSummaryAmountRequest? deleteRequest)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (deleteRequest == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                string? employeeId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse apiResponse = await _iBudget.DeleteBudgetSessionAmountSummary(deleteRequest, employeeId);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During delete-budget-session-amount-summary");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPatch]
        [Route("lock-budget-session-amount-summary/{budgetId}")]
        public async Task<IActionResult> LockBudgetSessionAmountSummary([FromRoute] string? budgetId)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(budgetId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                string? employeeId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _iBudget.LockBudgetSessionAmountSummary(budgetId, employeeId);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During lock-budget-session-amount-summary");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-budget-maad/{Maad}")]
        [AllowAnonymous]
        public async Task<IActionResult> getBudgetMaad(string Maad)
        {
            try
            {

                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

                //Task<ApiResponse> GetMaadForfilter(string Maad)

                ApiResponse apiResponse = await _iBudget.GetMaadForfilter(Maad);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-budget-session-amount-summary");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-budget-sessions-summary-amount-add")]
        public async Task<IActionResult> GetBudgetSummaryNewSessions()
        {
            try
            {
                ApiResponse apiResponse = await _iBudget.GetBudgetSummaryNewSessions();

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-budget-sessions-summary-amount-add");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPut]
        [Route("add-budget-department-details")]
        public async Task<IActionResult> AddBudgetDepartment(AddDepartmentSummaryRequest request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string? employeeId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse apiResponse = await _iBudget.AddDepartmentDetails(employeeId, request);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During add-budget-department-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-budget-department-details/{RefNo}")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetBudgetDepartment(string RefNo)
        {
            try
            {

                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                //string? employeeId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //if (string.IsNullOrEmpty(employeeId))
                //{
                //    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                //}
                ApiResponse apiResponse = await _iBudget.GetDepartmentBudgetDetails(RefNo);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error During get-budget-department-details/{RefNo}");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPut]
        [Route("update-budget-department-details")]
        public async Task<IActionResult> PutBudgetDepartment(AddDepartmentSummaryUpdateRequest request)
        {
            try
            {
                //Task<ApiResponse> UpdateDepartmentBudgetDetails(string EmpCode, AddDepartmentSummaryUpdateRequest request)
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string? employeeId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse apiResponse = await _iBudget.UpdateDepartmentBudgetDetails(employeeId, request);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error During update-budget-department-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-budget-head-mapping")]
        public async Task<IActionResult> GetBudgetHeadMapping(BudgetHeadMappingRequest? mappingRequest)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (mappingRequest == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _iBudget.GetBudgetHeadMapping(mappingRequest);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-budget-head-mapping");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("add-budget-head-mapping")]
        public async Task<IActionResult> AddBudgetHeadMapping(AddBudgetTypeHeadRequest? addRequest)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (addRequest == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                string? employeeId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _iBudget.AddBudgetHeadMapping(addRequest, employeeId);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During add-budget-head-mapping");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpDelete]
        [Route("delete-budget-head-mapping")]
        public async Task<IActionResult> DeleteBudgetHeadMapping(DeleteBudgetHeadMappingRequest? deleteRequest)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (deleteRequest == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                string? employeeId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (employeeId == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _iBudget.DeleteBudgetHeadMapping(deleteRequest, employeeId);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During delete-budget-head-mapping");
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

                ApiResponse apiResponse = await _iBudget.GetCreateBudgetSummaryDepartments(employeeId, campusCode);

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
        public async Task<IActionResult> CreateDepartmentBudgetSummary([FromBody] CreateDepartmentBudgetSummaryV2Request? budgetRequest)
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

                ApiResponse apiResponse = await _iBudget.CreateBudgetSummary(employeeId, budgetRequest);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During create-department-budget-summary");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-department-budget-summary")]
        public async Task<IActionResult> GetDepartmentBudgetSummary([FromBody] GetDepartmentBudgetSummaryV2Request? budgetRequest)
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

                ApiResponse apiResponse = await _iBudget.GetBudgetSummary(employeeId, budgetRequest);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-department-budget-summary");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpDelete]
        [Route("Delete-budget-department-details/{refNo}/{Id}")]
        public async Task<IActionResult> DeleteBudgetDepartment(string refNo, int Id)
        {
            try
            {
                // Task<ApiResponse> DeleteDepartmentBudgetDetails(string EmpCode,int Id)
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string? employeeId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse apiResponse = await _iBudget.DeleteDepartmentBudgetDetails(employeeId, Id, refNo);
                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error During Delete-budget-department-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpGet]
        [Route("Lock-budget-department-details/{refNo}")]
        public async Task<IActionResult> LockBudgetDepartment(string refNo)
        {
            try
            {
               // Task<ApiResponse> LockDepartmentDetails(string RefNo, string EmpCode)
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string? employeeId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _iBudget.LockDepartmentDetails( refNo,employeeId);
                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error During Lock-budget-department-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpGet]
        [Route("get-budget-head/{Type}")]
        public async Task<IActionResult> getbudgethead(string Type)
        {
            try
            {
               // Task<ApiResponse> LockDepartmentDetails(string RefNo, string EmpCode)
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string? employeeId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _iBudget.GetHeadFilter(Type);
                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error During Lock-budget-department-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        

    }
}
