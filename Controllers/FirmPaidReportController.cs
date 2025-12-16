
using AdvanceAPI.DTO;
using AdvanceAPI.DTO.FirmPaidDetails;
using AdvanceAPI.DTO.FirmPaidDetails.ApplicationReport;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices.FirmPaideport;
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
    public class FirmPaidReportController : ControllerBase
    {
        private readonly ILogger<FirmPaidReportController> _logger;
        private readonly IFirmPaidServices _firmServices;
        public FirmPaidReportController(ILogger<FirmPaidReportController> logger, IFirmPaidServices firmPaidRepository)
        {
            _logger = logger;
            _firmServices = firmPaidRepository;
            //_approvalService = approvalService;
        }
        [HttpPost]
        [Route("get-firm-report")]
        [AllowAnonymous]
        public async Task<IActionResult> AddStockItem([FromBody] FirmPaidrequest req )
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? EmpName=User.FindFirstValue(ClaimTypes.Name);
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                // string employeeId = "GLA108236";
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //IFirmPaidServices GetFirmPaidReportById
                ApiResponse apiResponse = await _firmServices.GetFirmPaidReportById(EmpName??"", req);

                return Ok(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During get-stock-item-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpPost]
        [Route("application-details")]
        public async Task<IActionResult> ApplicationDetails([FromBody] ApplicationReportRequest req)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                string? EmpType = User.FindFirstValue(ClaimTypes.Authentication);
                string? AddEmpCode = User.FindFirstValue(ClaimTypes.AuthorizationDecision);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> ApplicationReport(string EmpCode, ApplicationReportRequest req)
                ApiResponse response = await _firmServices.ApplicationReport(employeeId, req);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"application-details");
                throw;
            }
        }
    }
}
