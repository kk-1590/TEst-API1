using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Approval;
using AdvanceAPI.IServices.Approval;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


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
        public async Task<IActionResult> AddStockItem([FromBody] AddStockItemRequest Addstock)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
               // string employeeId = "GLA108236";
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
        [HttpPost]
        [Route("get-drafted-item")]
       
        public async Task<IActionResult> DraftedItem([FromBody] DraftedItemRequest itms)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!ModelState.IsValid)
                {
                    
                    return BadRequest(ModelState);
                }
               // string employeeId = "GLA108236";
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse apiResponse = await _approvalService.GetDraftedItem(employeeId, itms.AppType, itms.CampusCode.ToString(),itms.RefNo);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-stock-item-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpPost]
        [Route("get-drafted-item-summary")]
        public async Task<IActionResult> DraftedItemSummary([FromBody] DraftedItemRequest itms)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
               // string employeeId = "GLA108236";
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse apiResponse = await _approvalService.GetDraftItemSummary (employeeId, itms.AppType, itms.CampusCode.ToString(),itms.RefNo);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-stock-item-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-approval-sessions")]
        public async Task<IActionResult> GetApprovalSessions()
        {
            try
            {
                ApiResponse apiResponse = await _approvalService.GetApprovalSessions();

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-stock-item-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }


        [HttpPost]
        [Route("get-approval-number-3-authorities")]
        public async Task<IActionResult> GetAllEmployees([FromBody] GetNumber3AuthorityRequest? search)
        {
            try
            {

                ApiResponse apiResponse = await _approvalService.GetApprovalNumber3Authorities(search);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-vendors....");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }


        [HttpPost]
        [Route("get-approval_final-authorities")]
        public async Task<IActionResult> GetApprovalFinalAuthorities([FromBody] GetApprovalFinalAuthoritiesRequest? requestDetails)
        {
            try
            {
                if (string.IsNullOrEmpty(requestDetails?.CampusCode))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _approvalService.GetApprovalFinalAuthorities(requestDetails);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-approval_final-authorities");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpDelete]
        [Route("delete-approval-draft")]
        public async Task<IActionResult> GetApprovalFinalAuthorities(DeleteApprovalDraftRequest? deleteRequest)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId) || deleteRequest == null || string.IsNullOrWhiteSpace(deleteRequest?.CampusCode) || string.IsNullOrWhiteSpace(deleteRequest?.ApprovalType))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _approvalService.DeleteApprovalDraft(employeeId, deleteRequest);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-stock-item-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpPost]
        [Route("generate-approval")]
        public async Task<IActionResult> generateApproval([FromBody]GeneratePurchaseApprovalRequest generatePurchaseApprovalRequest)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId) )
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //Task<ApiResponse> GenerateApproval(string EmpCode,GeneratePurchaseApprovalRequest GeneratePurchaseApproval)
                ApiResponse apiResponse = await _approvalService.GenerateApproval(employeeId, generatePurchaseApprovalRequest);
                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-stock-item-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpDelete]
        [Route("delete-item/{itemId}")]
        public async Task<IActionResult> deleteitem(int itemId)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId) )
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                
                //Task<ApiResponse> GenerateApproval(string EmpCode,GeneratePurchaseApprovalRequest GeneratePurchaseApproval)
                ApiResponse apiResponse = await _approvalService.DeleteDraftedItem(itemId.ToString());
                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-stock-item-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-my-approvals")]
        public async Task<IActionResult> GetMyApprovals([FromBody] AprrovalsListRequest? search)
        {
            try
            {

                if (search == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                if (string.IsNullOrWhiteSpace(search?.Session))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Session is required..."));
                }

                if (string.IsNullOrWhiteSpace(search?.ReferenceNo) && string.IsNullOrWhiteSpace(search?.Status))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Status is required when Reference No is not provided..."));
                }

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? type = User.FindFirstValue(ClaimTypes.Authentication);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _approvalService.GetMyApprovals(employeeId, type, search);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-my-approvals....");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpDelete]
        [Route("delete-approval/{referenceNo}")]
        public async Task<IActionResult> DeleteApproval(string? referenceNo)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(employeeId) || string.IsNullOrWhiteSpace(referenceNo))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _approvalService.DeleteApproval(employeeId, referenceNo);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During delete-approval/{ReferenceNo} ", referenceNo);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpGet]
        [Route("get-draft")]
        public async Task<IActionResult> GetDraft()
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(employeeId) )
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse apiResponse = await _approvalService.GETDRAFt(employeeId);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-draft", "");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-print-approval-details/{referenceNo}")]
        public async Task<IActionResult> GetPrintApprovalDetails([FromRoute] string? referenceNo)
        {
            try
            {
                string? type = User.FindFirstValue(ClaimTypes.Authentication);
                if (string.IsNullOrEmpty(type))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                if (string.IsNullOrWhiteSpace(referenceNo))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Approval No Found..."));
                }

                ApiResponse apiResponse = await _approvalService.GetPOApprovalDetails(type, referenceNo);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-stock-item-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPatch]
        [Route("update-approval-note")]
        public async Task<IActionResult> UpdateApprovalNote([FromBody] UpdateApprovalNoteRequest? updateRequest)
        {
            try
            {
                string? employee = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employee))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                if (string.IsNullOrWhiteSpace(updateRequest?.ReferenceNo))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Approval number cannot be empty..."));
                }

                if (string.IsNullOrWhiteSpace(updateRequest?.Note))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Note cannot be empty..."));
                }

                ApiResponse apiResponse = await _approvalService.UpdateApprovalNote(employee, updateRequest.ReferenceNo, updateRequest.Note);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During update-approval-note");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-edit-approval-details/{referenceNo}")]
        public async Task<IActionResult> GetEditApprovalDetails([FromRoute] string? referenceNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(referenceNo))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Approval No Found..."));
                }

                ApiResponse apiResponse = await _approvalService.GetEditApprovalDetails(referenceNo);

                return apiResponse.Status == StatusCodes.Status200OK ? Ok(apiResponse) : BadRequest(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-edit-approval-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
    }

}