using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Media;

namespace AdvanceAPI.IServices.Media
{
    public interface IMediaService
    {
        Task<ApiResponse> GetMediaScheduleTypes();
        Task<ApiResponse> GetMediaTypes(string? scheduleType);
        Task<ApiResponse> GetMediaTypeHeads(GetMediaTypeHeadsRequest? request);
        Task<ApiResponse> AddMediaTypeHeads(AddNewMediaTypeHeadRequest? request);
        Task<ApiResponse> DeleteMediaTypeHead(int? id);


        Task<ApiResponse> GetEditionAdvertisementTypes(string? scheduleType);
        Task<ApiResponse> GetEditionAdvertisementHeads(GetEditionAdvertisementHeadsRequest? request);
        Task<ApiResponse> AddEditionAdvertisementHead(AddEditionAdvertisementHeadsRequest? request);
        Task<ApiResponse> DeleteEditionAdvertisementHead(int? id);


        Task<ApiResponse> GetMediaBudgetSessions();
        Task<ApiResponse> GetMediaBudgetDetails(GetMediaBudgetDetailsRequest? request);
        Task<ApiResponse> AddMediaBudgetDetails(AddMediaBudgetDetailsRequest? request, string? employeeId);
        Task<ApiResponse> DeleteMediaBudgetDetails(long? id);



        Task<ApiResponse> GetMediaScheduleManagerSessions();
        Task<ApiResponse> GetMediaScheduleManagerMediaTypes();
        Task<ApiResponse> GetMediaScheduleManagerMediaTitles(GetMediaScheduleManagerMediaTitlesRequest? request);
        Task<ApiResponse> GetMediaScheduleManagerCreateReleaseOrderAuthorities(GetMediaScheduleManagerCreateReleaseOrderAuthoritiesRequest? request);
        Task<ApiResponse> GetAdvertisementVendors(GetAdvertisementVendorsRequest? request);
        Task<ApiResponse> GetNewScheduleCreateMediaTitles(GetScheduleCreateMediaTitles? request, string? employeeId);
        Task<ApiResponse> GetCanCreateMediaRODetails(GetCanCreateMediaReleaseOrderDetailsRequest? request);
        Task<ApiResponse> GetMediaSchedules(GetMediaSchedulesRequest? request, string? employeeId);
        Task<ApiResponse> GetMediaScheduleToEdit(string? id);
        Task<ApiResponse> AddMediaSchedule(AddMediaScheduleRequest? request, string? employeeId);
        Task<ApiResponse> DeleteMediaSchedule(string? id);
        Task<ApiResponse> EditMediaSchedule(EditMediaScheduleRequest? request, string? employeeId);
        Task<ApiResponse> EditMediaScheduleFile(EditMediaScheduleFileRequest? request);

    }
}
