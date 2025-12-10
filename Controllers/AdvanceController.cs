using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Advance;
using AdvanceAPI.DTO.Advance.Bill;
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
        public AdvanceController(IAdvanceService advanceService, ILogger<AdvanceController> logger)
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
        public async Task<IActionResult> SaveRefNo([FromRoute] string RefNo, [FromBody] GenerateAdvancerequest req)
        {
            try
            {

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> GenerateAdvance(GenerateAdvancerequest req,string EmpCode,string EmpName,string RefNo)
                string EmpName = User.FindFirstValue(ClaimTypes.Name);
                ApiResponse response = await _advanceService.GenerateAdvance(req, employeeId, EmpName, RefNo.Replace("PLC", "").Replace("CVV", "").Replace("ADM", ""));
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

        public async Task<IActionResult> GetFinalAuth([FromRoute] string CampusCode)
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
        public async Task<IActionResult> GetMyAdvace([FromBody] GetMyAdvanceRequest req)
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
                ApiResponse response = await _advanceService.GetMyAdvanceReq(req.Status!, req.Session!, req.CampusCode!, req.ReferenceNo!, type ?? string.Empty, employeeId, req.Department, req.ItemsFrom.ToString(), req.NoOfItems.ToString());
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
        public async Task<IActionResult> Deleteadvance([FromRoute] string RefNo)
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
                ApiResponse response = await _advanceService.DeleteAdvance(RefNo, employeeId);
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
                ApiResponse response = await _advanceService.GetApprovalDetails(employeeId, AddEmpCode!, type ?? "", req);
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
                ApiResponse response = await _advanceService.PassAdvanceApproval(employeeId, EmpName ?? string.Empty, req);
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
                ApiResponse response = await _advanceService.RejectApproval(employeeId, EmpName ?? string.Empty, req);
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
        [Route("get-basic-details-for-bill-generate-against-advance/{RefNo}")]

        public async Task<IActionResult> GetBasicDetailsagainstadvance(string RefNo)
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
                ApiResponse response = await _advanceService.GetBasicDetailsForGenerateBillAgainstAdvance(RefNo);
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
                ApiResponse response = await _advanceService.GeneratereNoForUploadBill(type, employeeId, IsThousand);
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
                ApiResponse response = await _advanceService.GetPurchase(type, employeeId);
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
                if (VisitType == "Corporate Visit")
                {
                    ApiResponse response = await _advanceService.GetBasicDetailsVisit("", type, employeeId);
                    return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
                }
                else
                {
                    if (VisitType == "Partner Visit")
                    {
                        ApiResponse response = await _advanceService.GetartnerVisit("", type, employeeId);
                        return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
                    }
                    if (VisitType == "School Visit")
                    {
                        ApiResponse response = await _advanceService.GetSchoolVisit("", type, employeeId);
                        return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
                    }
                    List<TextValues> textValues = new List<TextValues>();
                    textValues.Add(new TextValues
                    {
                        Text = "No Purchase Approval",
                        Value = "No Purchase Approval"
                    });

                    ApiResponse response1 = new ApiResponse(StatusCodes.Status200OK, "Success", textValues);
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
        public class LoadOfficesCls
        {
            public string VendorId { get; set; }
            public string TypeId { get; set; }

        }
        [HttpPost]
        [Route("load-offices")]

        public async Task<IActionResult> LoadOffices([FromBody] LoadOfficesCls cls)
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
        [HttpPost]
        [Route("get-basic-purchase-details-generate-advance")]
        public async Task<IActionResult> generateAdvanceBasicDetails([FromBody] TextValues val)
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
                ApiResponse response = await _advanceService.GetBasicDetailsForGenerateAdvance(val.Value, val.Text);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-basic-purchase-details-generate-advance");
                throw;
            }
        }

        //Task<ApiResponse> SaveBill(string EmpCode,string EmpName, AddBillGenerateRequest req)
        [HttpPost]
        [Route("UploadBill")]
        public async Task<IActionResult> SaveBillUpload([FromForm] AddBillGenerateRequest req)
        {
            try
            {

                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                string? EmpType = User.FindFirstValue(ClaimTypes.Authentication);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse response = await _advanceService.SaveBill(employeeId, EmpName!, req, EmpType!);
                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"UploadBill");
                throw;
            }
        }
        [HttpGet]
        [Route("get-bill-refno/{CanThousand}")]
        public async Task<IActionResult> advanceREfNo(bool CanThousand)
        {
            try
            {

                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                string? type = User.FindFirstValue(ClaimTypes.Authentication);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse response = await _advanceService.GeneratereNoForUploadBill(type, employeeId, CanThousand);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"UploadBill");
                throw;
            }
        }
        [HttpGet]
        [Route("LoadFirm/{Cond}")]
        public async Task<IActionResult> advanceREfNo(string Cond)
        {
            try
            {

                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                string? type = User.FindFirstValue(ClaimTypes.Authentication);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse response = await _advanceService.GetDetails(Cond);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"LoadFirm");
                throw;
            }
        }


        [HttpGet]
        [Route("get-advance-approval-print/{ReferenceNo}")]

        public async Task<IActionResult> GetAdvanceApprovalPrint([FromRoute] string ReferenceNo)
        {
            try
            {
                string? employeeName = User.FindFirstValue(ClaimTypes.Name);
                string? employeeType = User.FindFirstValue(ClaimTypes.Authentication);

                if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(employeeType))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse response = await _advanceService.GetAdvanceApprovalPrint(ReferenceNo, employeeName, employeeType);

                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"get-advance-approval-print");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpGet]
        [Route("GetLockDetails/{Type}")]

        public async Task<IActionResult> GetDateLock([FromRoute] string Type)
        {
            try
            {
                //string? employeeName = User.FindFirstValue(ClaimTypes.Name);
                //string? employeeType = User.FindFirstValue(ClaimTypes.Authentication);

                //if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(employeeType))
                //{
                //    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                //}

                ApiResponse response = await _advanceService.GetLockDetails(Type);

                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"GetLockDetails/{Type}");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpGet]
        [Route("get-ref-no-for-bill-generate")]

        public async Task<IActionResult> getrefnoagainstadvance()
        {
            try
            {
                string? employeeName = User.FindFirstValue(ClaimTypes.Name);
                string? employeeType = User.FindFirstValue(ClaimTypes.Authentication);
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(employeeType))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> GeneratereNoForUploadBillAgainstAdvance(string Type, string EmpCode, string RefNo = "")

                ApiResponse response = await _advanceService.GeneratereNoForUploadBillAgainstAdvance(employeeType, employeeId);

                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"get-ref-no-for-bill-generate");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpGet]
        [Route("get-auth-advance-bill/{RefNo}")]

        public async Task<IActionResult> getrefnoagainstadvance(string RefNo)
        {
            try
            {
                string? employeeName = User.FindFirstValue(ClaimTypes.Name);
                string? employeeType = User.FindFirstValue(ClaimTypes.Authentication);
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(employeeType))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> GetAdvancebillAuth(string refNo)

                ApiResponse response = await _advanceService.GetAdvancebillAuth(RefNo);

                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"get-auth-advance-bill/{RefNo}");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpPost]
        [Route("get-purchase-approval-bill")]

        public async Task<IActionResult> getPurchaseApprovalBill([FromBody] GetApprovalBillRequest req)
        {
            try
            {
                string? employeeName = User.FindFirstValue(ClaimTypes.Name);
                string? employeeType = User.FindFirstValue(ClaimTypes.Authentication);
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(employeeType))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> GetPurchaseApprovalBill(string EmpCode, string Type, GetApprovalBillRequest req)

                ApiResponse response = await _advanceService.GetPurchaseApprovalBill(employeeId, employeeType, req);

                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"get-purchase-approval-bill " + req);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpPut]
        [Route("update-approval-bill")]

        public async Task<IActionResult> UpdateBill([FromBody] UpdatePurchaseBillDateRequest req)
        {
            try
            {
                string? employeeName = User.FindFirstValue(ClaimTypes.Name);
                string? employeeType = User.FindFirstValue(ClaimTypes.Authentication);
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (ModelState.IsValid == false)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(employeeType))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> GetPurchaseApprovalBill(string EmpCode, string Type, GetApprovalBillRequest req)

                ApiResponse response = await _advanceService.UpdatePurchaseBillDate(employeeId!, req);

                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"update-approval-bill " + req);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-bill-edit-details/{RefNo}")]

        public async Task<IActionResult> UpdateBillDetails([FromRoute] string RefNo)
        {
            try
            {
                string? employeeName = User.FindFirstValue(ClaimTypes.Name);
                string? employeeType = User.FindFirstValue(ClaimTypes.Authentication);
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(employeeType))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> UpdateBillDetails(string RefNo)
                ApiResponse response = await _advanceService.UpdateBillDetails(RefNo);
                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"update-bill-details/{RefNo}");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }

        }
        [HttpDelete]
        [Route("delete-bill/{TransId}")]
        public async Task<IActionResult> DeleteBillBase([FromRoute] string TransId)
        {
            try
            {
                string? employeeName = User.FindFirstValue(ClaimTypes.Name);
                string? employeeType = User.FindFirstValue(ClaimTypes.Authentication);
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(employeeType))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> DeleteBill(string EmpCode, string TransId)
                ApiResponse response = await _advanceService.DeleteBill(employeeId!, TransId);
                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"delete-bill/{TransId}");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPut]
        [Route("UpdateBill")]
        public async Task<IActionResult> UpdateBillUpload([FromForm] AddBillGenerateRequest req)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (string.IsNullOrEmpty(req.BillId))
                {
                    return BadRequest("Provide BillId");
                }

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                string? EmpType = User.FindFirstValue(ClaimTypes.Authentication);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse response = await _advanceService.UpdateBill(employeeId, EmpName!, req, EmpType!);
                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"UpdateBill");
                throw;
            }
        }
        [HttpGet]
        [Route("PrintBillPrint/{TransId}")]
        //[AllowAnonymous]
        public async Task<IActionResult> UpdateBillUpload([FromRoute] string TransId)
        {
            try
            {


                if (string.IsNullOrEmpty(TransId))
                {
                    return BadRequest("Provide TransId");
                }

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                string? EmpType = User.FindFirstValue(ClaimTypes.Authentication);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }//Task<ApiResponse> GetBillDetails(string TransId,string EmpName)
                ApiResponse response = await _advanceService.GetBillDetails(TransId, EmpName!);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"PrintBillPrint");
                throw;
            }
        }
        [HttpGet]
        [Route("get-firm-details/{VendorId}")]
        //[AllowAnonymous]
        public async Task<IActionResult> UpdateBillUpload([FromRoute] int VendorId)
        {
            try
            {


                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                string? EmpType = User.FindFirstValue(ClaimTypes.Authentication);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }//Task<ApiResponse> GetBillDetails(string TransId,string EmpName)
                ApiResponse response = await _advanceService.GetVendorDetails(VendorId.ToString());
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"get-firm-details/{VendorId}");
                throw;
            }
        }
        [HttpGet]
        [Route("load-transaction-details/{TransId}")]
        
        public async Task<IActionResult> LoadTransactionDetails([FromRoute] string TransId)
        {
            try
            {

                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                string? EmpType = User.FindFirstValue(ClaimTypes.Authentication);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }//Task<ApiResponse> GetBillDetails(string TransId,string EmpName)
                ApiResponse response = await _advanceService.LoadTransactionDetails(TransId);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"load-transaction-details/{TransId}");
                throw;
            }
        }
        [HttpGet]
        [Route("load-bill-auth/{CampusCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> billauth([FromRoute] string CampusCode)
        {
            try
            {

                //string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //string? EmpName = User.FindFirstValue(ClaimTypes.Name);
                //string? EmpType = User.FindFirstValue(ClaimTypes.Authentication);
                //if (string.IsNullOrEmpty(employeeId))
                //{
                //    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                //}//Task<ApiResponse> GetBillDetails(string TransId,string EmpName)
                ApiResponse response = await _advanceService.getAuthForDirectBill(CampusCode);
                return Ok(response.Status == StatusCodes.Status200OK ? response : BadRequest(response));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, $"load-bill-auth/{CampusCode}");
                throw;
            }
        }


        [HttpGet]
        [Route("get-bill-approval-filter-sessions")]
        public async Task<IActionResult> GetBillApprovalSessions()
        {
            try
            {
                ApiResponse response = await _advanceService.GetBillApprovalFilterSessions();
                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"get-bill-approval-filter-sessions");

                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-bill-approval-filter-initiated-by")]
        public async Task<IActionResult> GetBillApprovalFilterInitiatedBy()
        {
            try
            {
                ApiResponse response = await _advanceService.GetBillApprovalFilterInitiatedBy();
                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"t-bill-approval-filter-initiated-by");

                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-bill-approval-filter-cheque-by")]
        public async Task<IActionResult> GetBillApprovalFilterChequeBy()
        {
            try
            {
                ApiResponse response = await _advanceService.GetBillApprovalFilterChequeBy();
                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"get-bill-approval-filter-cheque-by");

                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-bill-approval-details")]
        public async Task<IActionResult> GetBillApprovalDetails(GetBillApprovalRequest? getBillApprovalRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

                string role = User.FindFirstValue(ClaimTypes.Authentication)!;

                string name = User.FindFirstValue(ClaimTypes.Name)!;

                if (string.IsNullOrWhiteSpace(employeeId) || string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse response = await _advanceService.GetBillApprovalDetails(getBillApprovalRequest, employeeId, role, name);

                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"get-bill-approval-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpGet]
        [Route("get-cheque-auth")]
        public async Task<IActionResult> chequeauth()
        {
            try
            {

                string employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

                string role = User.FindFirstValue(ClaimTypes.Authentication)!;

                string name = User.FindFirstValue(ClaimTypes.Name)!;

                if (string.IsNullOrWhiteSpace(employeeId) || string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                ApiResponse response = await _advanceService.GetChequeAuthority();

                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"get-cheque-auth");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpGet]
        [Route("get-payment-details")]
        public async Task<IActionResult> payment()
        {
            try
            {
                string employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                string role = User.FindFirstValue(ClaimTypes.Authentication)!;
                string name = User.FindFirstValue(ClaimTypes.Name)!;
                if (string.IsNullOrWhiteSpace(employeeId) || string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                ApiResponse response = await _advanceService.GetPayentDetails();

                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"get-payment-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
        [HttpPost]
        [Route("save-transaction-details")]
        
        public async Task<IActionResult> SaveDetails([FromBody] SaveCheDetailsRequest req)
        {
            try
            {
                string employeeId = "GLA308508";
                string role = "Account";
                string name = "Devendra Saras";
                //string employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
                //string role = User.FindFirstValue(ClaimTypes.Authentication)!;
                //string name = User.FindFirstValue(ClaimTypes.Name)!;
                if (string.IsNullOrWhiteSpace(employeeId) || string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }
                //Task<ApiResponse> SaveChequeDetails(string EmpCode, string Type,string EmpName, SaveCheDetailsRequest req)
                ApiResponse response = await _advanceService.SaveChequeDetails(employeeId,role,name,req);

                return response.Status == StatusCodes.Status200OK ? Ok(response) : BadRequest(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"get-payment-details");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }



    }
}
