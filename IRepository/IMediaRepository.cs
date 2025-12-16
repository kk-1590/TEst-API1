using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Media;
using System.Data;

namespace AdvanceAPI.IRepository
{
    public interface IMediaRepository
    {
        Task<DataTable> GetAllMediaScheduleTypes();
        Task<DataTable> GetAllMediaTypes(string? scheduleType);
        Task<DataTable> GetMediaTypeHeads(GetMediaTypeHeadsRequest? request);
        Task<DataTable> CheckMediaTypeHeadExists(AddNewMediaTypeHeadRequest? request);
        Task<int> AddMediaTypeHead(AddNewMediaTypeHeadRequest? request);
        Task<DataTable> CheckMediaTypeHeadExistsByid(int? id);
        Task<int> DeleteMediaTypeHeadByid(int? id);



        Task<DataTable> GetAllEditionAdvertisementEdition(string? scheduleType);
        Task<DataTable> GetEditionAdvertisementHeads(GetEditionAdvertisementHeadsRequest? request);
        Task<DataTable> CheckEditionAdvertisementHeadExists(AddEditionAdvertisementHeadsRequest? request);
        Task<int> AddEditionAdvertisementHead(AddEditionAdvertisementHeadsRequest? request);
        Task<DataTable> CheckEditionAdvertisementHeadExistsByid(int? id);
        Task<int> DeleteEditionAdvertisementHeadByid(int? id);


        Task<DataTable> GetMediaBudgetDetails(GetMediaBudgetDetailsRequest? request);
        Task<DataTable> CheckMediaBudgetDetailsExists(AddMediaBudgetDetailsRequest? request);
        Task<DataTable> CheckMediaBudgetDetailsExistsById(long? id);
        Task<DataTable> CheckMediaHeadTypeDetailsIsValid(AddMediaBudgetDetailsRequest? request);
        Task<int> AddMediaBudgetDetails(AddMediaBudgetDetailsRequest? request, string? employeeId);
        Task<int> UpdateMediaBudgetDocumentDetails(string? id);
        Task<int> DeleteMediaBudgetDetailsById(long? id);



        Task<DataTable> GetMediaScheduleManagerSessions();
        Task<DataTable> GetMediaScheduleMediaTypes();
        Task<DataTable> GetMediaScheduleManagerMediaTitles(GetMediaScheduleManagerMediaTitlesRequest? request);
        Task<DataTable> GetMediaScheduleManagerCreateReleaseOrderAuthorities(GetMediaScheduleManagerCreateReleaseOrderAuthoritiesRequest? request);



        Task<DataTable> GetEmployeeOnlyMediaPermission(string? employeeId);
        Task<DataTable> GetAdvertisementVendors(GetAdvertisementVendorsRequest? request);
        Task<DataTable> GetNewScheduleCreateMediaTitles(GetScheduleCreateMediaTitles? request, string? employeeId);
        Task<DataTable> CheckMediaBudgetDetailsExistsWithAmount(GetCanCreateMediaReleaseOrderDetailsRequest? request);
        Task<DataTable> GetPendingReleaseOrdersScheduleIds();
        Task<DataTable> GetPendingMediaSchedules(GetCanCreateMediaReleaseOrderDetailsRequest? request, string? scheduleIds);
        Task<DataTable> GetMediaBudgetAmount(GetCanCreateMediaReleaseOrderDetailsRequest? request);
        Task<DataTable> GetScheduledMediaBudgetAmount(GetCanCreateMediaReleaseOrderDetailsRequest? request);
        Task<DataTable> GetApprovedReleaseOrders(string? session);
        Task<DataTable> GetusedScheduledAmount(GetCanCreateMediaReleaseOrderDetailsRequest? request, string? scheduleIds);
        Task<DataTable> GetMediaSchedules(GetMediaSchedulesRequest? request, string? onlyMediaPermission);
        Task<DataTable> GetExternalReleaseOrderDetailsByScheduleId(string? scheduleId);
        Task<DataTable> GetInternalReleaseOrderDetailsByScheduleId(string? scheduleId);
        Task<DataTable> GetMediaScheduleById(string? id);
        Task<DataTable> GetExternalPendingOrAPprovedReleaseOrderDetailsByScheduleId(string? scheduleId);
        Task<string> AddMediaSchedule(AddMediaScheduleRequest? request, string? currentSession, string? employeeId);
        Task<int> UpdateScheduleDocumentId(string? id);
        Task<DataTable> CheckIsMediaScheduleExistsById(string? id);
        Task<int> DeleteMediaScheduleById(string? id);
        Task<int> EditMediaSchedule(EditMediaScheduleRequest? request, EditMediaScheduleOldDetails? oldDetails, string? employeeId);
    }
}
