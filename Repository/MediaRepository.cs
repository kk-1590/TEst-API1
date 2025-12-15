using AdvanceAPI.DTO.DB;
using AdvanceAPI.DTO.Media;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.ENUMS.Media;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.SQLConstants.Media;
using Google.Protobuf.Reflection;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlX.XDevAPI;
using System.Data;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AdvanceAPI.Repository
{
    public class MediaRepository : IMediaRepository
    {
        private ILogger<MediaRepository> _logger;
        private readonly IDBOperations _dbOperations;
        public readonly IGeneral _general;
        public MediaRepository(ILogger<MediaRepository> logger, IDBOperations dBOperations, IGeneral general)
        {
            _logger = logger;
            _dbOperations = dBOperations;
            _general = general;
        }

        public async Task<DataTable> GetAllMediaScheduleTypes()
        {
            try
            {
                return await _dbOperations.SelectAsync(MediaSql.GET_MEDIA_SCHEDULE_TYPES, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAllMediaScheduleTypes..");
                throw;
            }
        }
        public async Task<DataTable> GetAllMediaTypes(string? scheduleType)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>()
                {
                    new SQLParameters( "@ScheduleType", scheduleType ?? string.Empty)
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_MEDIA_TYPES, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAllMediaTypes (Parameters: scheduleType: {@ScheduleType})..", scheduleType);
                throw;
            }
        }
        public async Task<DataTable> GetMediaTypeHeads(GetMediaTypeHeadsRequest? request)
        {
            StringBuilder additionalQuery = new StringBuilder();

            List<SQLParameters> parameters = new List<SQLParameters>
            {
                new SQLParameters( "@ScheduleType", request?.ScheduleType ?? string.Empty),
                new SQLParameters( "@LimitItems", request?.NoOfRecords ?? 0),
                new SQLParameters( "@OffSetItems", request?.RecordFrom ?? 0),
            };

            if (!string.IsNullOrEmpty(request?.MediaType))
            {
                additionalQuery.Append(" AND Media = @MediaType ");
                parameters.Add(new SQLParameters("@MediaType", request.MediaType));
            }

            if (!string.IsNullOrEmpty(request?.Head))
            {
                additionalQuery.Append(" AND Title LIKE CONCAT('%', @Head, '%') ");
                parameters.Add(new SQLParameters("@Head", request.Head));
            }

            string finalQuery = MediaSql.GET_MEDIA_TYPE_HEADS.Replace("@AdditionalCondition", additionalQuery.ToString());

            try
            {
                return await _dbOperations.SelectAsync(finalQuery, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetMediaTypeHeads (Parameters: {@Request})..", request);
                throw;
            }

        }
        public async Task<DataTable> CheckMediaTypeHeadExists(AddNewMediaTypeHeadRequest? request)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@ScheduleType", request?.ScheduleType ?? string.Empty),
                    new SQLParameters( "@MediaType", request?.MediaType ?? string.Empty),
                    new SQLParameters( "@Head", request?.Head?.Trim()?.ToUpper() ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.CHECK_MEDIA_TYPE_HEAD_EXISTS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckMediaTypeHeadExists (Parameters: {@Request})..", request);
                throw;
            }
        }
        public async Task<int> AddMediaTypeHead(AddNewMediaTypeHeadRequest? request)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@ScheduleType", request?.ScheduleType ?? string.Empty),
                    new SQLParameters( "@MediaType", request?.MediaType ?? string.Empty),
                    new SQLParameters( "@Head", request?.Head?.Trim()?.ToUpper() ?? string.Empty),
                };

                return await _dbOperations.DeleteInsertUpdateAsync(MediaSql.ADD_MEDIA_TYPE_HEAD, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During AddMediaTypeHeadExists (Parameters: {@Request})..", request);
                throw;
            }
        }
        public async Task<DataTable> CheckMediaTypeHeadExistsByid(int? id)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@Id", id ?? 0)
                };

                return await _dbOperations.SelectAsync(MediaSql.CHECK_MEDIA_TYPE_HEAD_EXISTS_BY_ID, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckMediaTypeHeadExistsByid (Parameters: {@Request})..", id);
                throw;
            }
        }
        public async Task<int> DeleteMediaTypeHeadByid(int? id)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@Id", id ?? 0)
                };

                return await _dbOperations.DeleteInsertUpdateAsync(MediaSql.DELETE_MEDIA_TYPE_HEAD, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During DeleteMediaTypeHeadExistsByid (Parameters: {@Request})..", id);
                throw;
            }
        }



        public async Task<DataTable> GetAllEditionAdvertisementEdition(string? scheduleType)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>()
                {
                    new SQLParameters( "@ScheduleType", scheduleType ?? string.Empty)
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_TYPE_EDITION_ADVERTISEMENT, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAllEditionAdvertisementEdition (Parameters: scheduleType: {@ScheduleType})..", scheduleType);
                throw;
            }
        }
        public async Task<DataTable> GetEditionAdvertisementHeads(GetEditionAdvertisementHeadsRequest? request)
        {
            StringBuilder additionalQuery = new StringBuilder();

            List<SQLParameters> parameters = new List<SQLParameters>
            {
                new SQLParameters( "@ScheduleType", request?.ScheduleType ?? string.Empty),
                new SQLParameters( "@LimitItems", request?.NoOfRecords ?? 0),
                new SQLParameters( "@OffSetItems", request?.RecordFrom ?? 0),
            };

            if (!string.IsNullOrEmpty(request?.Type))
            {
                additionalQuery.Append(" AND Type = @Type ");
                parameters.Add(new SQLParameters("@Type", request.Type));
            }

            if (!string.IsNullOrEmpty(request?.Head))
            {
                additionalQuery.Append(" AND Value LIKE CONCAT('%', @Head, '%') ");
                parameters.Add(new SQLParameters("@Head", request.Head));
            }

            string finalQuery = MediaSql.GET_EDITION_ADVERTISEMENT_HEADS.Replace("@AdditionalCondition", additionalQuery.ToString());
            try
            {
                return await _dbOperations.SelectAsync(finalQuery, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetEditionAdvertisementHeads (Parameters: {@Request})..", request);
                throw;
            }
        }
        public async Task<DataTable> CheckEditionAdvertisementHeadExists(AddEditionAdvertisementHeadsRequest? request)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@ScheduleType", request?.ScheduleType ?? string.Empty),
                    new SQLParameters( "@Type", request?.Type ?? string.Empty),
                    new SQLParameters( "@Head", request?.Head?.Trim()?.ToUpper() ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.CHECK_EDITION_ADVERTISEMENT_HEAD_EXISTS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckEditionAdvertisementHeadExists (Parameters: {@Request})..", request);
                throw;
            }
        }
        public async Task<int> AddEditionAdvertisementHead(AddEditionAdvertisementHeadsRequest? request)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@ScheduleType", request?.ScheduleType ?? string.Empty),
                    new SQLParameters( "@Type", request?.Type ?? string.Empty),
                    new SQLParameters( "@Head", request?.Head?.Trim()?.ToUpper() ?? string.Empty),
                };

                return await _dbOperations.DeleteInsertUpdateAsync(MediaSql.ADD_EDITION_ADVERTISEMENT_HEAD, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During AddEditionAdvertisementHead (Parameters: {@Request})..", request);
                throw;
            }
        }
        public async Task<DataTable> CheckEditionAdvertisementHeadExistsByid(int? id)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@Id", id ?? 0)
                };

                return await _dbOperations.SelectAsync(MediaSql.CHECK_EDITION_ADVERTISEMENT_HEAD_EXISTS_BY_ID, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckEditionAdvertisementHeadExistsByid (Parameters: {@Request})..", id);
                throw;
            }
        }
        public async Task<int> DeleteEditionAdvertisementHeadByid(int? id)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@Id", id ?? 0)
                };

                return await _dbOperations.DeleteInsertUpdateAsync(MediaSql.DELETE_EDITION_ADVERTISEMENT_HEAD_BY_ID, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During DeleteEditionAdvertisementHeadByid (Parameters: {@Request})..", id);
                throw;
            }
        }



        public async Task<DataTable> GetMediaBudgetDetails(GetMediaBudgetDetailsRequest? request)
        {
            try
            {
                StringBuilder additionalCondition = new StringBuilder();

                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@CampusCode", request?.CampusCode ?? string.Empty),
                    new SQLParameters( "@Session", request?.Session ?? string.Empty),
                    new SQLParameters( "@ScheduleType", request?.ScheduleType ?? string.Empty),
                    new SQLParameters( "@LimitItems", request?.NoOfRecords ?? 0),
                    new SQLParameters( "@OffSetItems", request?.RecordFrom ?? 0),
                };

                if (!string.IsNullOrWhiteSpace(request?.MediaType))
                {
                    additionalCondition.Append(" AND Main=@MediaType");
                    parameters.Add(new SQLParameters("@MediaType", request.MediaType));
                }

                if (!string.IsNullOrWhiteSpace(request?.Head))
                {
                    additionalCondition.Append(" AND Title LIKE CONCAT('%', @Head, '%') ");
                    parameters.Add(new SQLParameters("@Head", request.Head));
                }

                string finalQuery = MediaSql.GET_MEDIA_BUDGET_DETAILS.Replace("@AdditionalCondition", additionalCondition.ToString());

                return await _dbOperations.SelectAsync(finalQuery, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetMediaBudgetDetails (Parameters: {@Request})..", request);
                throw;
            }
        }
        public async Task<DataTable> CheckMediaBudgetDetailsExists(AddMediaBudgetDetailsRequest? request)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@CampusCode", request?.CampusCode ?? string.Empty),
                    new SQLParameters( "@TypeId", request?.MediaTypeId ?? 0),
                    new SQLParameters( "@Session", request?.Session ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.CHECK_MEDIA_BUDGET_DETAILS_EXISTS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckMediaBudgetDetailsExists (Parameters: {@Request})..", request);
                throw;
            }
        }
        public async Task<DataTable> CheckMediaBudgetDetailsExistsById(long? id)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@Id", id?? 0),
                };

                return await _dbOperations.SelectAsync(MediaSql.CHECK_MEDIA_BUDGET_DETAILS_EXISTS_BY_ID, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckMediaBudgetDetailsExistsById (Parameters: {@Id})..", id);
                throw;
            }
        }
        public async Task<DataTable> CheckMediaHeadTypeDetailsIsValid(AddMediaBudgetDetailsRequest? request)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@Id", request?.MediaTypeId ?? 0),
                    new SQLParameters( "@ScheduleType", request?.ScheduleType ?? string.Empty),
                    new SQLParameters( "@MediaType", request?.MediaType ?? string.Empty),
                    new SQLParameters( "@Head", request?.Head ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.CHECK_MEDIA_TYPE_DETAILS_ISVALID, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckMediaHeadTypeDetailsIsValid (Parameters: {@Request})..", request);
                throw;
            }
        }
        public async Task<int> AddMediaBudgetDetails(AddMediaBudgetDetailsRequest? request, string? employeeId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@CampusCode", request?.CampusCode ?? string.Empty),
                    new SQLParameters( "@TypeId", request?.MediaTypeId ?? 0),
                    new SQLParameters( "@ScheduleType", request?.ScheduleType ?? string.Empty),
                    new SQLParameters( "@MediaType", request?.MediaType ?? string.Empty),
                    new SQLParameters( "@Head", request?.Head ?? string.Empty),
                    new SQLParameters( "@Amount", request?.Amount ?? 0),
                    new SQLParameters( "@Session", request?.Session ?? string.Empty),
                    new SQLParameters( "@UpdatedFrom", _general.GetIpAddress() ?? string.Empty),
                    new SQLParameters( "@UpdatedBy", employeeId ?? string.Empty),
                };

                return await _dbOperations.DeleteInsertUpdateAsync(MediaSql.ADD_MEDIA_BUDGET_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During AddMediaBudgetDetailsExists (Parameters: {@Request})..", request);
                throw;
            }
        }
        public async Task<int> UpdateMediaBudgetDocumentDetails(string? id)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@Id", id ?? string.Empty),
                };

                return await _dbOperations.DeleteInsertUpdateAsync(MediaSql.UPDATE_MEDIA_BUDGET_DOCUMENT_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateMediaBudgetDocumentDetails (Parameters: {@Id})..", id);
                throw;
            }
        }
        public async Task<int> DeleteMediaBudgetDetailsById(long? id)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@Id", id ?? 0),
                };

                return await _dbOperations.DeleteInsertUpdateAsync(MediaSql.DELETE_MEDIA_BUDGET_DETAILS_BY_ID, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During DeleteMediaBudgetDetailsById (Parameters: {@Id})..", id);
                throw;
            }
        }


        public async Task<DataTable> GetMediaScheduleManagerSessions()
        {
            try
            {
                return await _dbOperations.SelectAsync(MediaSql.GET_MEDIA_SCHEDULES_SESSION, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetMediaScheduleManagerSessions..");
                throw;
            }
        }
        public async Task<DataTable> GetMediaScheduleMediaTypes()
        {
            try
            {
                return await _dbOperations.SelectAsync(MediaSql.GET_MEDIA_SCHEDULES_MEDIA_TYPES, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetMediaScheduleMediaTypes..");
                throw;
            }
        }
        public Task<DataTable> GetMediaScheduleManagerMediaTitles(GetMediaScheduleManagerMediaTitlesRequest? request)
        {
            try
            {
                string additionalCondition = string.Empty;
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@LimitItems", request?.NoOfRecords ?? 0),
                    new SQLParameters( "@OffSetItems", request?.RecordFrom ?? 0),
                };

                if (!string.IsNullOrEmpty(request?.Title))
                {
                    additionalCondition = " where MediaTitle LIKE CONCAT('%', @MediaTitle, '%') ";
                    parameters.Add(new SQLParameters("@MediaTitle", request.Title));
                }

                string finalQuery = MediaSql.GET_MEDIA_SCHEDULES_MEDIA_TITLES.Replace("@AdditionalCondition", additionalCondition);

                return _dbOperations.SelectAsync(finalQuery, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetMediaScheduleManagerMediaTitles (Parameters: {@Request})..", request);
                throw;
            }
        }
        public async Task<DataTable> GetMediaScheduleManagerCreateReleaseOrderAuthorities(GetMediaScheduleManagerCreateReleaseOrderAuthoritiesRequest? request)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@AuthorityType", request?.AuthorityType ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_MEDIA_SCHEDULES_RO_AUTHORITY, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetMediaScheduleManagerCreateReleaseOrderAuthorities (Parameters: {@Request})..", request);
                throw;
            }
        }



        public async Task<DataTable> GetEmployeeOnlyMediaPermission(string? employeeId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@EmployeeId", employeeId ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_EMPLOYEE_ONLY_MEDIA_PERMISSION_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetEmployeeOnlyMediaPermission (Parameters: {@Id})..", employeeId);
                throw;
            }
        }
        public async Task<DataTable> GetAdvertisementVendors(GetAdvertisementVendorsRequest? request)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@LimitItems", request?.NoOfRecords ?? 0),
                    new SQLParameters( "@OffSetItems", request?.RecordFrom ?? 0),
                };
                string additionalCondition = string.Empty;
                if (!string.IsNullOrEmpty(request?.VendorName))
                {
                    additionalCondition = " AND VendorName LIKE CONCAT('%', @VendorName, '%') ";
                    parameters.Add(new SQLParameters("@VendorName", request.VendorName));
                }

                string finalQuery = MediaSql.GET_ADVERTISEMNT_VENDORS.Replace("@AdditionalCondition", additionalCondition);

                return await _dbOperations.SelectAsync(finalQuery, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvertisementVendors (Parameters: {@Request})..", request);
                throw;
            }
        }
        public async Task<DataTable> GetNewScheduleCreateMediaTitles(GetScheduleCreateMediaTitles? request, string? employeeId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@LimitItems", request?.NoOfRecords ?? 0),
                    new SQLParameters( "@OffSetItems", request?.RecordFrom ?? 0),
                };
                string additionalCondition = string.Empty;
                if (!string.IsNullOrEmpty(request?.Title))
                {
                    additionalCondition = " AND Title LIKE CONCAT('%', @Title, '%') ";
                    parameters.Add(new SQLParameters("@Title", request.Title));
                }

                string finalQuery = MediaSql.GET_MEDIA_SCHEDULES_NEW_CREATE_MEDIA_TYPES.Replace("@AdditionalCondition", additionalCondition);

                return await _dbOperations.SelectAsync(finalQuery, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetNewScheduleCreateMediaTitles (Parameters: Request: {@Request}, EmployeeId: {@EmployeeId})..", request, employeeId);
                throw;
            }
        }
        public async Task<DataTable> CheckMediaBudgetDetailsExistsWithAmount(GetCanCreateMediaReleaseOrderDetailsRequest? request)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@CampusCode", request?.CampusCode ?? string.Empty),
                    new SQLParameters( "@Session", request?.Session ?? string.Empty),
                    new SQLParameters( "@Media", request?.MediaType ?? string.Empty),
                    new SQLParameters( "@Title", request?.Title ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.CHECK_MEDIA_BUDGET_DETAILS_EXISTS_WITH_AMOUNT, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckMediaBudgetDetailsExistsWithAmount (Parameters: Request: {@Request})..", request);
                throw;
            }
        }
        public async Task<DataTable> GetPendingReleaseOrdersScheduleIds()
        {
            try
            {
                return await _dbOperations.SelectAsync(MediaSql.GET_PENDING_RELEASE_ORDERS_SCHEDULE_IDS, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetPendingReleaseOrdersScheduleIds ..");
                throw;
            }
        }
        public async Task<DataTable> GetPendingMediaSchedules(GetCanCreateMediaReleaseOrderDetailsRequest? request, string? scheduleIds)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@CampusCode", request?.CampusCode ?? string.Empty),
                    new SQLParameters( "@Session", request?.Session ?? string.Empty),
                    new SQLParameters( "@MediaType", request?.MediaType ?? string.Empty),
                    new SQLParameters( "@Title", request?.Title ?? string.Empty),
                    new SQLParameters( "@ScheduleIds", scheduleIds ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_PENDING_MEDIA_SCHEDULES_RO_GENERATED, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetPendingMediaSchedules ..");
                throw;
            }
        }
        public async Task<DataTable> GetMediaBudgetAmount(GetCanCreateMediaReleaseOrderDetailsRequest? request)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@CampusCode", request?.CampusCode ?? string.Empty),
                    new SQLParameters( "@Session", request?.Session ?? string.Empty),
                    new SQLParameters( "@MediaType", request?.MediaType ?? string.Empty),
                    new SQLParameters( "@Title", request?.Title ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_MEDIA_BUDGET_AMOUNT, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetMediaBudgetAmount ..");
                throw;
            }
        }
        public async Task<DataTable> GetScheduledMediaBudgetAmount(GetCanCreateMediaReleaseOrderDetailsRequest? request)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@CampusCode", request?.CampusCode ?? string.Empty),
                    new SQLParameters( "@Session", request?.Session ?? string.Empty),
                    new SQLParameters( "@MediaType", request?.MediaType ?? string.Empty),
                    new SQLParameters( "@Title", request?.Title ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_SCHEDULED_MEDIA_BUDGET_AMOUNT, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetScheduledMediaBudgetAmount ..");
                throw;
            }
        }
        public async Task<DataTable> GetApprovedReleaseOrders(string? session)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@Session", session ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_APPROVED_RELEASE_ORDERS_BY_SESSION, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetApprovedReleaseOrders ..");
                throw;
            }
        }
        public async Task<DataTable> GetusedScheduledAmount(GetCanCreateMediaReleaseOrderDetailsRequest? request, string? scheduleIds)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@CampusCode", request?.CampusCode ?? string.Empty),
                    new SQLParameters( "@Session", request?.Session ?? string.Empty),
                    new SQLParameters( "@MediaType", request?.MediaType ?? string.Empty),
                    new SQLParameters( "@Title", request?.Title ?? string.Empty),
                    new SQLParameters( "@ScheduleIds", scheduleIds ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_USED_SCHEDULED_AMOUNT, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetusedScheduledAmount ..");
                throw;
            }
        }
        public async Task<DataTable> GetMediaSchedules(GetMediaSchedulesRequest? request, string? onlyMediaPermission)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@CampusCode", request?.CampusCode ?? string.Empty),
                    new SQLParameters( "@Session", request?.Session ?? string.Empty),
                    new SQLParameters( "@ScheduleType", request?.ScheduleType ?? string.Empty),
                    new SQLParameters( "@LimitItems", request?.NoOfRecords ?? 0),
                    new SQLParameters( "@OffSetItems", request?.RecordFrom ?? 0),
                };

                StringBuilder stringBuilder = new StringBuilder();
                string orderBy = string.Empty;

                if (!string.IsNullOrEmpty(request?.MediaType))
                {
                    stringBuilder.Append(" AND Type = @MediaType ");
                    parameters.Add(new SQLParameters("@MediaType", request.MediaType));
                }

                if (!string.IsNullOrEmpty(request?.Title))
                {
                    stringBuilder.Append(" AND MediaTitle= @Title ");
                    parameters.Add(new SQLParameters("@Title", request.Title));
                }

                if (!string.IsNullOrEmpty(onlyMediaPermission))
                {
                    stringBuilder.Append(" And Type in (@OnlyMediaType) ");
                    parameters.Add(new SQLParameters("@OnlyMediaType", onlyMediaPermission));
                }

                if (request?.SortBy == "DateWiseTypeWise")
                {
                    orderBy = " ORDER BY schedules.`Schedule`,Type,MediaTitle";
                }
                else if (request?.SortBy == "TypeWiseDateWise")
                {
                    orderBy = " ORDER BY Type,MediaTitle,schedules.`Schedule` ";
                }

                string finalQuery = MediaSql.GET_MEDIA_SCHEDULES.Replace("@AdditionalCondition", stringBuilder.ToString()).Replace("@OrderByCondition", orderBy);

                return await _dbOperations.SelectAsync(finalQuery, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetMediaSchedules (Parameters: {@Request}, {@OnlyMediaPermission})..", request, onlyMediaPermission);
                throw;
            }
        }
        public async Task<DataTable> GetExternalReleaseOrderDetailsByScheduleId(string? scheduleId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@ScheduleId", scheduleId ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_EXTERNAL_RELEASE_ORDER_DETAILS_BY_SCHEDULE_ID, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetReleaseOrderDetailsByScheduleId ..");
                throw;
            }
        }
        public async Task<DataTable> GetInternalReleaseOrderDetailsByScheduleId(string? scheduleId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@ScheduleId", scheduleId ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_INTERNAL_RELEASE_ORDER_DETAILS_BY_SCHEDULE_ID, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetInternalReleaseOrderDetailsByScheduleId ..");
                throw;
            }
        }
        public async Task<DataTable> GetMediaScheduleById(string? id)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@Id", id ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_MEDIA_SCHEDULES_BY_ID, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetMediaScheduleById ..");
                throw;
            }
        }

        public async Task<DataTable> GetExternalPendingOrAPprovedReleaseOrderDetailsByScheduleId(string? scheduleId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@ScheduleId", scheduleId ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(MediaSql.GET_EXTERNAL_RELEASE_ORDER_PENDING_OR_APPROVED_DETAILS_BY_SCHEDULE_ID, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetExternalPendingOrAPprovedReleaseOrderDetailsByScheduleId ..");
                throw;
            }
        }


        public async Task<DataTable> GetNewMediaScheduleId()
        {
            try
            {
                return await _dbOperations.SelectAsync(MediaSql.GET_NEW_SCHEDULE_ID, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetNewMediaScheduleId ..");
                throw;
            }
        }

        public async Task<string> AddMediaSchedule(AddMediaScheduleRequest? request, string? currentSession, string? employeeId)
        {
            try
            {
                string scheduleId = string.Empty;
                using DataTable dtScheduleId = await GetNewMediaScheduleId();
                if (dtScheduleId.Rows.Count > 0)
                {
                    scheduleId = dtScheduleId.Rows[0]["NewScheduleId"].ToString() ?? string.Empty;
                }

                if (string.IsNullOrEmpty(scheduleId))
                {
                    return string.Empty;
                }

                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@Id", scheduleId),
                    new SQLParameters( "@CampusCode", request?.CampusCode ?? string.Empty),
                    new SQLParameters( "@ScheduleType", request?.ScheduleType ?? string.Empty),
                    new SQLParameters( "@MediaType", request?.MediaType ?? string.Empty),
                    new SQLParameters( "@ScheduleDate", request?.ScheduleDate ?? string.Empty),
                    new SQLParameters( "@MediaTitle", request?.MediaTitle ?? string.Empty),
                    new SQLParameters( "@AdvertisementTypes", request?.AdvertisementTypes ?? string.Empty),
                    new SQLParameters( "@Editions", request?.Editions ?? string.Empty),
                    new SQLParameters( "@SizeW", request?.SizeW ?? 0),
                    new SQLParameters( "@SizeH", request?.SizeH ?? 0),
                    new SQLParameters( "@Amount", request?.Amount ?? 0),
                    new SQLParameters( "@Discount", request?.Discount ?? 0),
                    new SQLParameters( "@FinalAmount", request?.FinalAmount ?? 0),
                    new SQLParameters( "@Session", request?.Session ?? string.Empty),
                    new SQLParameters( "@AddedBy", employeeId ?? string.Empty),
                    new SQLParameters( "@Tax", request?.Tax ?? 0),
                    new SQLParameters( "@PageNo", request?.PageNo ?? 0),
                    new SQLParameters( "@Rate", request?.Rate ?? 0),
                    new SQLParameters( "@AddingSession", currentSession ?? string.Empty),
                    new SQLParameters( "@MyBillUpto", request?.BillUpTo ?? string.Empty),
                };

                int rowsAffected = await _dbOperations.DeleteInsertUpdateAsync(MediaSql.ADD_NEW_SCHEDULE, parameters, DBConnections.Advance);

                if (rowsAffected > 0)
                {
                    return scheduleId;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During AddMediaSchedule (Parameters: {@Request}, {@CurrentSession}, {@EmployeeId})..", request, currentSession, employeeId);
                throw;
            }
        }

        public async Task<int> UpdateScheduleDocumentId(string? id)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters( "@Id", id ?? string.Empty),
                };

                return await _dbOperations.DeleteInsertUpdateAsync(MediaSql.UPDATE_NEW_SCHEDULE_DOCUMENT_ID, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateScheduleDocumentId ..");
                throw;
            }
        }
    }
}
