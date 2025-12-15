using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Media;
using AdvanceAPI.IServices.Media;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdvanceAPI.Controllers
{
    [ApiVersion("1.0", Deprecated = false)]
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [Authorize]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private readonly ILogger<MediaController> _logger;
        public MediaController(IMediaService mediaService, ILogger<MediaController> logger)
        {
            _mediaService = mediaService;
            _logger = logger;
        }

        [HttpGet]
        [Route("get-schedule-types")]
        public async Task<IActionResult> GetMediaScheduleTypes()
        {
            try
            {
                ApiResponse response = await _mediaService.GetMediaScheduleTypes();
                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media schedule types (get-schedule-types). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-media-types/{scheduleType}")]
        public async Task<IActionResult> GetMediaTypes([FromRoute] string? scheduleType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(scheduleType))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Schedule type is required"));
                }

                ApiResponse response = await _mediaService.GetMediaTypes(scheduleType);

                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media types (get-media-types/scheduleType). Try after some time... (Parameters: ScheduleType: {@ScheduleType})", scheduleType);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-media-type-heads")]
        public async Task<IActionResult> GetMediaTypeHead([FromBody] GetMediaTypeHeadsRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ApiResponse response = await _mediaService.GetMediaTypeHeads(request);

                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media type heads (get-media-type-heads). Try after some time... (Parameters: {@Request})", request);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("add-media-type-head")]
        public async Task<IActionResult> AddMediaTypeHead([FromBody] AddNewMediaTypeHeadRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ApiResponse response = await _mediaService.AddMediaTypeHeads(request);
                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while adding media type head (add-media-type-head). Try after some time... (Parameters: {@Request})", request);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpDelete]
        [Route("delete-media-type-head/{id}")]
        public async Task<IActionResult> DeleteMediaTypeHead([FromRoute] int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }

                ApiResponse response = await _mediaService.DeleteMediaTypeHead(id);

                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while deleting media type head (delete-media-type-head/id). Try after some time... (Parameters: {@Id})", id);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }



        [HttpGet]
        [Route("get-edition-advertisement-types/{scheduleType}")]
        public async Task<IActionResult> GetEditionAdvertisementTypes([FromRoute] string? scheduleType)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(scheduleType))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Schedule type is required"));
                }

                ApiResponse response = await _mediaService.GetEditionAdvertisementTypes(scheduleType);

                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media types (get-edition-advertisement-types/scheduleType). Try after some time... (Parameters: ScheduleType: {@ScheduleType})", scheduleType);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-edition-advertisement-heads")]
        public async Task<IActionResult> GetEditionAdvertisementHeads([FromBody] GetEditionAdvertisementHeadsRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ApiResponse response = await _mediaService.GetEditionAdvertisementHeads(request);

                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting edition advertisement heads (get-edition-advertisement-heads). Try after some time... (Parameters: {@Request})", request);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("add-edition-advertisement-head")]
        public async Task<IActionResult> AddEditionAdvertisementHead([FromBody] AddEditionAdvertisementHeadsRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ApiResponse response = await _mediaService.AddEditionAdvertisementHead(request);

                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sorry!! an error occurred while adding edition advertisement head (add-edition-advertisement-head). Try after some time... (Parameters: {@Request})", request);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpDelete]
        [Route("delete-edition-advertisement-head/{id}")]
        public async Task<IActionResult> DeleteEditionAdvertisementHead([FromRoute] int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }

                ApiResponse response = await _mediaService.DeleteEditionAdvertisementHead(id);

                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while deleting edition advertisement head (delete-edition-advertisement-head/id). Try after some time... (Parameters: {@Id})", id);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }



        [HttpGet]
        [Route("get-media-budget-sessions")]
        public async Task<IActionResult> GetMediaBudgetSessions()
        {
            try
            {
                ApiResponse response = await _mediaService.GetMediaBudgetSessions();
                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media budget sessions (get-media-budget-sessions). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-media-budget-details")]
        public async Task<IActionResult> GetMediaBudgetDetails([FromBody] GetMediaBudgetDetailsRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ApiResponse response = await _mediaService.GetMediaBudgetDetails(request);
                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media budget details (get-media-budget-details). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("add-media-budget-details")]
        public async Task<IActionResult> AddMediaBudgetDetails(AddMediaBudgetDetailsRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                ApiResponse response = await _mediaService.AddMediaBudgetDetails(request, employeeId);
                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while adding media budget details (add-media-budget-details). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpDelete]
        [Route("delete-media-budget-details/{id}")]
        public async Task<IActionResult> DeleteMediaBudgetDetails([FromRoute] long? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }

                ApiResponse response = await _mediaService.DeleteMediaBudgetDetails(id);

                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while deleting media budget details (delete-media-budget-details/id). Try after some time... (Parameters: {@Id})", id);
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }




        [HttpGet]
        [Route("get-media-schedule-manager-sessions")]
        public async Task<IActionResult> GetMediaScheduleManagerSessions()
        {
            try
            {
                ApiResponse response = await _mediaService.GetMediaScheduleManagerSessions();
                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media schedule manager sessions (get-media-schedule-manager-sessions). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-media-schedule-manager-media-types")]
        public async Task<IActionResult> GetMediaScheduleManagerMediaTypes()
        {
            try
            {
                ApiResponse response = await _mediaService.GetMediaScheduleManagerMediaTypes();
                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media schedule manager media types (get-media-schedule-manager-media-types). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-media-schedule-manager-media-titles")]
        public async Task<IActionResult> GetMediaScheduleManagerMediaTitles(GetMediaScheduleManagerMediaTitlesRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                ApiResponse response = await _mediaService.GetMediaScheduleManagerMediaTitles(request);
                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media schedule manager media titles (get-media-schedule-manager-media-titles). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-media-schedule-manager-create-release-order-authorities")]
        public async Task<IActionResult> GetMediaScheduleManagerCreateReleaseOrderAuthorities(GetMediaScheduleManagerCreateReleaseOrderAuthoritiesRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                ApiResponse response = await _mediaService.GetMediaScheduleManagerCreateReleaseOrderAuthorities(request);
                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media schedule manager media release order authorities (get-media-schedule-manager-create-release-order-authorities). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-advertisement-vendors")]
        public async Task<IActionResult> GetAdvertisementVendors(GetAdvertisementVendorsRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                ApiResponse response = await _mediaService.GetAdvertisementVendors(request);
                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting advertisement vendors (get-advertisement-vendors). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-media-schedule-create-media-titles")]
        public async Task<IActionResult> GetMediaScheduleCreateMediaTitles(GetScheduleCreateMediaTitles? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                ApiResponse response = await _mediaService.GetNewScheduleCreateMediaTitles(request, employeeId);

                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media schedule create media titles (get-media-schedule-create-media-titles). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-can-create-media-release-order-details")]
        public async Task<IActionResult> GetCanCreateMediaRODetails(GetCanCreateMediaReleaseOrderDetailsRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ApiResponse response = await _mediaService.GetCanCreateMediaRODetails(request);
                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting can create release order details (get-can-create-media-release-order-details). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("get-media-schedules")]
        public async Task<IActionResult> GetMediaSchedules(GetMediaSchedulesRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                ApiResponse response = await _mediaService.GetMediaSchedules(request, employeeId);

                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media schedules (get-media-schedules). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpGet]
        [Route("get-media-schedule-to-edit/{id}")]
        public async Task<IActionResult> GetMediaSchedules([FromRoute] string? id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ApiResponse response = await _mediaService.GetMediaScheduleToEdit(id);

                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while getting media schedule to edit (get-media-schedule-to-edit). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }

        [HttpPost]
        [Route("add-media-schedules")]
        public async Task<IActionResult> AddMediaSchedule(AddMediaScheduleRequest? request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid request found.."));
                }
                string? employeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(employeeId))
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! Invalid Request Found..."));
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                ApiResponse response = await _mediaService.AddMediaSchedule(request, employeeId);

                return response.Status == 200 ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "sorry!! an error occurred while adding media schedules (add-media-schedules). Try after some time...");
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Sorry!! There is an error.. Please try after some time..."));
            }
        }
    }
}