using System.Security.Claims;
using AdvanceAPI.DTO;
using AdvanceAPI.DTO.VenderPriceComp;
using AdvanceAPI.IServices.VenderPriceComp;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace AdvanceAPI.Controllers
{
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [Authorize]
    public class VenderPriceComparisionController : ControllerBase
    {
        private readonly ILogger<InclusiveController> _logger;
        private readonly IVenderPriceComparisionServices _venderPriceComparisionServices;

        public VenderPriceComparisionController(ILogger<InclusiveController> logger, IVenderPriceComparisionServices venderPriceComparisionServices)
        {
            _logger = logger;
           _venderPriceComparisionServices = venderPriceComparisionServices;
        }
        [HttpGet]
        [Route("get-basic-purchase-details/{RefNo}")]
        public async Task<IActionResult> GetBasicPurcheseDetails(string RefNo)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse response=await _venderPriceComparisionServices.GetApprovalBasicDetails(RefNo);
                return Ok(response.Status==StatusCodes.Status200OK ?response:BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message,"Error in GetBasicPurcheseDetails");
                throw;
            }
        }
        [HttpGet]
        [Route("get-basic-purchase-item-details/{RefNo}")]
        public async Task<IActionResult> GetBasicPurcheseItemDetails(string RefNo)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse response=await _venderPriceComparisionServices.GetItemDetails(RefNo);
                return Ok(response.Status==StatusCodes.Status200OK ?response:BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message,"Error in GetBasicPurcheseDetails");
                throw;
            }
        }
        [HttpGet]
        [Route("get-Vender-status/{RefNo}")]
        public async Task<IActionResult> GetVenderDetails(string RefNo)
        {
            try
            {
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse response=await _venderPriceComparisionServices.GetVendorDetails(RefNo);
                return Ok(response.Status==StatusCodes.Status200OK ?response:BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message,"Error in get-Vender-details");
                throw;
            }
        }
        //Task<ApiResponse> SubmitVendorDetails(string RefNo,string empCode,InsertDetails Details)
        [HttpPost]
        [Route("submit-vendor/{RefNo}")]
        public async Task<IActionResult> SubmitVenderDetails([FromBody]InsertDetails Details,[FromRoute]string RefNo)
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
                ApiResponse response=await _venderPriceComparisionServices.SubmitVendorDetails(RefNo,employeeId,Details);
                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message,"Error in get-Vender-details");
                throw;
            }
        }
        [HttpPut]
        [Route("lock-details/{RefNo}")]
        public async Task<IActionResult> SubmitVenderDetails(string RefNo)
        {
            try
            {
                
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse response=await _venderPriceComparisionServices.LockDetails(RefNo,employeeId);
                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message,"Error in lock-details");
                throw;
            }
        }

    }
}
