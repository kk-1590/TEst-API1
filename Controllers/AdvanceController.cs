using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Advance;
using AdvanceAPI.IServices.Advance;
using AdvanceAPI.IServices.VenderPriceComp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdvanceAPI.Controllers
{
    [ApiVersion("2.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [Authorize]
    public class AdvanceController : ControllerBase
    {
        private readonly IAdvanceService _advanceService;
        private readonly ILogger<AdvanceController> _logger;
        public AdvanceController(IAdvanceService advanceService,ILogger<AdvanceController> logger) 
        {
            _advanceService = advanceService;
            _logger = logger;
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
                ApiResponse response = await _advanceService.GetApprovals(RefNo);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-basic-purchase-details/{RefNo}");
                throw;
            }
        }



        [HttpGet]
        [Route("get-department-hod/{Dept}")]
        public async Task<IActionResult> GetHodDept(string Dept)
        {
            try
            {

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse response = await _advanceService.GetDepartmentHod(Dept);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-department-hod/{Dept}");
                throw;
            }
        }
        [HttpPost]
        [Route("SaveAdvanceDetails/{RefNo}")]
        public async Task<IActionResult> SaveRefNo([FromRoute]string RefNo, [FromBody] GenerateAdvancerequest req )
        {
            try
            {

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> GenerateAdvance(GenerateAdvancerequest req,string EmpCode,string EmpName,string RefNo)
                ApiResponse response = await _advanceService.GenerateAdvance(req,employeeId,"EmpName",RefNo);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"SaveAdvanceDetails/{RefNo}");
                throw;
            }
        }
        [HttpGet]
        [Route("get-final-auth/{CampusCode}")]
        
        public async Task<IActionResult> GetFinalAuth([FromRoute]string CampusCode )
        {
            try
            {

                //string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //if (string.IsNullOrEmpty(employeeId))
                //{
                //    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                //}
                //Task<ApiResponse> GetFinalAuth(string CampusCode)
                ApiResponse response = await _advanceService.GetFinalAuth(CampusCode);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-final-auth/{CampusCode}");
                throw;
            }
        }
        
        [HttpPost]
        [Route("get-my-advance")]
        public async Task<IActionResult> GetMyAdvace([FromBody] GetMyAdvanceRequest req )
        {
            try
            {

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> GetMyAdvanceReq(string Status, string Session, string CampusCode, string RefNo)
                string? type = User.FindFirstValue(ClaimTypes.Authentication);
                ApiResponse response = await _advanceService.GetMyAdvanceReq(req.Status!,req.Session!,req.CampusCode!,req.ReferenceNo!, type??string.Empty, employeeId,  req.Department,req.ItemsFrom.ToString(), req.NoOfItems.ToString());
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-my-advance");
                throw;
            }
        }
        [HttpDelete]
        [Route("delete-my-advance/{RefNo}")]
        public async Task<IActionResult> Deleteadvance([FromRoute]string RefNo )
        {
            try
            {

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> GetMyAdvanceReq(string Status, string Session, string CampusCode, string RefNo)
                string? type = User.FindFirstValue(ClaimTypes.Authentication);
                ApiResponse response = await _advanceService.DeleteAdvance(RefNo,employeeId);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"delete-my-advance/{RefNo}");
                throw;
            }
        }

        //Task<ApiResponse> GetApprovalDetails(string EmpCode, string EmpCodeAdd, string Type, GetMyAdvanceRequest req)
        [HttpPost]
        [Route("get-advance-approval")]
        public async Task<IActionResult> GetAdvanceApproval([FromBody] GetMyAdvanceRequest req)
        {
            try
            {

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> GetMyAdvanceReq(string Status, string Session, string CampusCode, string RefNo)
                string? type = User.FindFirstValue(ClaimTypes.Authentication);
                string? AddEmpCode = User.FindFirstValue(ClaimTypes.AuthorizationDecision);
                ApiResponse response = await _advanceService.GetApprovalDetails( employeeId,AddEmpCode!, type??"",req);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-advance-approval");
                throw;
            }
        }
        [HttpGet]
        [Route("get-advance-type")]
        public async Task<IActionResult> GetType()
        {
            try
            {

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> GetMyAdvanceReq(string Status, string Session, string CampusCode, string RefNo)
                string? type = User.FindFirstValue(ClaimTypes.Authentication);
                string? AddEmpCode = User.FindFirstValue(ClaimTypes.AuthorizationDecision);
                ApiResponse response = await _advanceService.GetAdvanceType();
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-advance-type");
                throw;
            }
        }

        [HttpPut]
        [Route("approve-advance")]
        public async Task<IActionResult> GetBasicDetails([FromBody] PassApprovalRequest req)
        {
            try
            {
                
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                ApiResponse response = await _advanceService.PassAdvanceApproval(employeeId,EmpName??string.Empty,req);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"approve-advance");
                throw;
            }
        }
        [HttpPut]
        [Route("reject-advance")]
        public async Task<IActionResult> reject([FromBody] PassApprovalRequest req)
        {
            try
            {


                
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                ApiResponse response = await _advanceService.RejectApproval(employeeId,EmpName??string.Empty,req);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"reject-advance");
                throw;
            }
        }
        [HttpGet]
        [Route("get-basic-details-for-bill-generate/{RefNo}")]
        
        public async Task<IActionResult> GetBasicDetails(string RefNo)
        {
            try
            {


                
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                //Task<ApiResponse> GetBasicDetailsForGenerateBill(string RefNo)
                ApiResponse response = await _advanceService.GetBasicDetailsForGenerateBill(RefNo);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-basic-details-for-bill-generate/{RefNo}");
                throw;
            }
        }
        [HttpGet]
        [Route("get-auth/{RefNo}")]
        
        public async Task<IActionResult> getauth(string RefNo)
        {
            try
            {


                
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                //Task<ApiResponse> GetBasicDetailsForGenerateBill(string RefNo)
                ApiResponse response = await _advanceService.GetAuthority(RefNo);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-basic-details-for-bill-generate/{RefNo}");
                throw;
            }
        }
        [HttpGet]
        [Route("get-approval-details-for-generate-bill")]
        
        public async Task<IActionResult> getvalueForbillgenerate([FromQuery] bool IsThousand)
        {
            try
            {


                
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                string? type = User.FindFirstValue(ClaimTypes.Authentication);
                //Task<ApiResponse> GeneratereNoForUploadBill(string Type, string EmpCode, bool IsThousand, string RefNo = "")
                ApiResponse response = await _advanceService.GeneratereNoForUploadBill(type,employeeId, IsThousand);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-approval-details-for-generate-bill");
                throw;
            }
        }
        [HttpGet]
        [Route("get-purchase-refno")]
        public async Task<IActionResult> getrefno()
        {
            try
            {

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                string? type = User.FindFirstValue(ClaimTypes.Authentication);
                //Task<ApiResponse> GetPurchase(string Type,string Empcode)
                ApiResponse response = await _advanceService.GetPurchase(type,employeeId);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-approval-details-for-generate-bill");
                throw;
            }
        }
        [HttpGet]
        [Route("get-visit-details/{VisitType}")]
        public async Task<IActionResult> LoadVisit(string VisitType)
        {
            try
            {
                //string? employeeId = "GLA222002";
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                string? type = User.FindFirstValue(ClaimTypes.Authentication);
                //Task<ApiResponse> GetBasicDetailsVisit(string AppType,string Type,string EmpCode)
                if(VisitType== "Corporate Visit")
                {
                    ApiResponse response = await _advanceService.GetBasicDetailsVisit("", type, employeeId);
                    return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
                }
                else
                {
                    if(VisitType== "Partner Visit")
                    {
                        ApiResponse response = await _advanceService.GetartnerVisit("", type, employeeId);
                        return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
                    }
                    if(VisitType== "School Visit")
                    {
                        ApiResponse response = await _advanceService.GetSchoolVisit("", type, employeeId);
                        return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
                    }
                    List<TextValues> textValues = new List<TextValues>();
                    textValues.Add(new TextValues
                    {
                        Text= "No Purchase Approval",
                        Value= "No Purchase Approval"
                    });

                    ApiResponse response1 = new ApiResponse(StatusCodes.Status200OK,"Success",textValues);
                    return Ok(response1.Status == StatusCodes.Status200OK ? response1 : BadRequest(response1));
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-approval-details-for-generate-bill");
                throw;
            }
        }
        

        [HttpGet]
        [Route("load-sub-firm/{vendorId}")]
        [AllowAnonymous]
        public async Task<IActionResult> LoadSubFirm(int vendorId)
        {
            try
            {

                //string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //if (string.IsNullOrEmpty(employeeId))
                //{
                //    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                //}
                //string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                //string? type = User.FindFirstValue(ClaimTypes.Authentication);
                //Task<ApiResponse> GetPurchase(string Type,string Empcode)
                ApiResponse response = await _advanceService.GetSubfirm(vendorId);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-approval-details-for-generate-bill");
                throw;
            }
        }
        public class LoadOfficesCls {
            public string  VendorId { get;set; }
            public string TypeId { get;set; }
        
        }
        [HttpGet]
        [Route("load-offices")]
        [AllowAnonymous]
        public async Task<IActionResult> LoadOffices([FromBody] LoadOfficesCls cls)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //if (string.IsNullOrEmpty(employeeId))
                //{
                //    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                //}
                //string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                //string? type = User.FindFirstValue(ClaimTypes.Authentication);
                //Task<ApiResponse> GetPurchase(string Type,string Empcode)
                ApiResponse response = await _advanceService.LoadOffices(cls.TypeId!, cls.VendorId!);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-approval-details-for-generate-bill");
                throw;
            }
        }



    }
}
