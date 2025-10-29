using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Inclusive;
using AdvanceAPI.IServices.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace AdvanceAPI.Controllers
{

    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [Authorize]
    public class ApprovalController : ControllerBase
    {
        private readonly ILogger<ApprovalController> _logger;
        public ApprovalController(ILogger<ApprovalController> logger)
        {
            _logger = logger;
        }

        
    }


}
