using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Advance;
using AdvanceAPI.DTO.Media;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Inclusive;
using AdvanceAPI.IServices.Media;
using Microsoft.AspNetCore.Mvc.Formatters;
using NuGet.Protocol.Plugins;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.Text;
using ZstdSharp.Unsafe;

namespace AdvanceAPI.Services.Media
{
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly IGeneral _general;
        private readonly IInclusiveService _inclusiveService;
        public MediaService(IMediaRepository mediaRepository, IGeneral general, IInclusiveService inclusiveService)
        {
            _mediaRepository = mediaRepository;
            _general = general;
            _inclusiveService = inclusiveService;
        }

        public async Task<ApiResponse> GetMediaScheduleTypes()
        {
            using DataTable dtScheduleTypes = await _mediaRepository.GetAllMediaScheduleTypes();

            HashSet<string> scheduleTypes = new HashSet<string>();
            for (int i = 0; i < dtScheduleTypes.Rows.Count; i++)
            {
                scheduleTypes.Add(dtScheduleTypes.Rows[i]["Types"]?.ToString() ?? string.Empty);
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", scheduleTypes);
        }
        public async Task<ApiResponse> GetMediaTypes(string? scheduleType)
        {
            using DataTable dtMediaTypes = await _mediaRepository.GetAllMediaTypes(scheduleType);
            HashSet<string> mediaTypes = new HashSet<string>();
            for (int i = 0; i < dtMediaTypes.Rows.Count; i++)
            {
                mediaTypes.Add(dtMediaTypes.Rows[i]["Type"]?.ToString() ?? string.Empty);
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", mediaTypes);
        }
        public async Task<ApiResponse> GetMediaTypeHeads(GetMediaTypeHeadsRequest? request)
        {
            using DataTable dtMediaTypeHeads = await _mediaRepository.GetMediaTypeHeads(request);

            HashSet<MediaTypeHeadsResponse> mediaTypeHeadsResponses = new HashSet<MediaTypeHeadsResponse>();

            foreach (DataRow row in dtMediaTypeHeads.Rows)
            {
                mediaTypeHeadsResponses.Add(new MediaTypeHeadsResponse(row));
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", mediaTypeHeadsResponses);
        }
        public async Task<ApiResponse> AddMediaTypeHeads(AddNewMediaTypeHeadRequest? request)
        {
            using DataTable dtExists = await _mediaRepository.CheckMediaTypeHeadExists(request);
            if (dtExists.Rows.Count > 0)
            {
                return new ApiResponse(StatusCodes.Status409Conflict, "Media Type Head already exists");
            }
            int rowsAffected = await _mediaRepository.AddMediaTypeHead(request);

            if (rowsAffected > 0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Media Type Head added successfully.");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, "Failed to add Media Type Head. Try After Some Time.");
            }
        }
        public async Task<ApiResponse> DeleteMediaTypeHead(int? id)
        {
            using DataTable dtExists = await _mediaRepository.CheckMediaTypeHeadExistsByid(id);

            if (dtExists.Rows.Count == 0)
            {
                return new ApiResponse(StatusCodes.Status404NotFound, "Sorry!! Media Type Head not found...");
            }
            int rowsAffected = await _mediaRepository.DeleteMediaTypeHeadByid(id);
            if (rowsAffected > 0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Media Type Head deleted successfully.");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, "Failed to delete Media Type Head. Try After Some Time.");

            }
        }



        public async Task<ApiResponse> GetEditionAdvertisementTypes(string? scheduleType)
        {
            using DataTable dtEditionAdTypes = await _mediaRepository.GetAllEditionAdvertisementEdition(scheduleType);

            HashSet<string> editionAdTypes = new HashSet<string>();

            for (int i = 0; i < dtEditionAdTypes.Rows.Count; i++)
            {
                editionAdTypes.Add(dtEditionAdTypes.Rows[i]["Type"]?.ToString() ?? string.Empty);
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", editionAdTypes);
        }
        public async Task<ApiResponse> GetEditionAdvertisementHeads(GetEditionAdvertisementHeadsRequest? request)
        {
            using DataTable dtEditionAdvertisementHeads = await _mediaRepository.GetEditionAdvertisementHeads(request);

            HashSet<EditionAdvertisementHeadsResponse> editionAdvertisementHeadsResponses = new HashSet<EditionAdvertisementHeadsResponse>();

            foreach (DataRow row in dtEditionAdvertisementHeads.Rows)
            {
                editionAdvertisementHeadsResponses.Add(new EditionAdvertisementHeadsResponse(row));
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", editionAdvertisementHeadsResponses);
        }
        public async Task<ApiResponse> AddEditionAdvertisementHead(AddEditionAdvertisementHeadsRequest? request)
        {
            using DataTable dtExists = await _mediaRepository.CheckEditionAdvertisementHeadExists(request);

            if (dtExists.Rows.Count > 0)
            {
                return new ApiResponse(StatusCodes.Status409Conflict, "Edition Advertisement Head already exists");
            }

            int rowsAffected = await _mediaRepository.AddEditionAdvertisementHead(request);
            if (rowsAffected > 0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Edition Advertisement Head added successfully.");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, "Failed to add Edition Advertisement Head. Try After Some Time.");
            }
        }
        public async Task<ApiResponse> DeleteEditionAdvertisementHead(int? id)
        {
            using DataTable dtExists = await _mediaRepository.CheckEditionAdvertisementHeadExistsByid(id);

            if (dtExists.Rows.Count == 0)
            {
                return new ApiResponse(StatusCodes.Status404NotFound, "Sorry!! Edition Advertisement Head not found...");
            }

            int rowsAffected = await _mediaRepository.DeleteEditionAdvertisementHeadByid(id);

            if (rowsAffected > 0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Edition Advertisement Head deleted successfully.");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, "Failed to delete Edition Advertisement Head. Try After Some Time.");
            }

        }



        public async Task<ApiResponse> GetMediaBudgetSessions()
        {
            HashSet<string> budgetSessions = new HashSet<string>();

            for (int i = DateTime.Now.Year + 1; i >= 2010; i--)
            {
                budgetSessions.Add((i + "-" + (i + 1).ToString().Substring(2, 2)));
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", budgetSessions);
        }
        public async Task<ApiResponse> GetMediaBudgetDetails(GetMediaBudgetDetailsRequest? request)
        {
            using DataTable dtMediaBudgetDetails = await _mediaRepository.GetMediaBudgetDetails(request);

            HashSet<MediaBudgetDetailsResponse> mediaBudgetDetailsResponses = new HashSet<MediaBudgetDetailsResponse>();

            foreach (DataRow row in dtMediaBudgetDetails.Rows)
            {
                MediaBudgetDetailsResponse mediaBudgetDetailsResponse = new MediaBudgetDetailsResponse(row);

                if (_general.IsFileExists($"MediaDeals/{mediaBudgetDetailsResponse?.Id!}_{mediaBudgetDetailsResponse?.MediaType!}.pdf"))
                {
                    mediaBudgetDetailsResponse!.IsPdfExists = true;
                }

                mediaBudgetDetailsResponses.Add(mediaBudgetDetailsResponse!);
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", mediaBudgetDetailsResponses);
        }
        public async Task<ApiResponse> AddMediaBudgetDetails(AddMediaBudgetDetailsRequest? request, string? employeeId)
        {

            using DataTable dtHeadExists = await _mediaRepository.CheckMediaHeadTypeDetailsIsValid(request);
            if (dtHeadExists.Rows.Count == 0)
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, "Invalid Media Head Details Found. Please provide valid details.");
            }

            using DataTable dtExists = await _mediaRepository.CheckMediaBudgetDetailsExists(request);
            if (dtExists.Rows.Count > 0)
            {
                return new ApiResponse(StatusCodes.Status409Conflict, "Media Budget Details already exists");
            }


            int rowsAffected = await _mediaRepository.AddMediaBudgetDetails(request, employeeId);

            if (rowsAffected > 0)
            {
                using DataTable dtAddedExists = await _mediaRepository.CheckMediaBudgetDetailsExists(request);

                if (dtAddedExists.Rows.Count > 0 && request?.SupportingDocument != null)
                {
                    string budgetId = dtAddedExists.Rows[0][0]?.ToString() ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(budgetId))
                    {
                        string filePath = "MediaDeals";

                        string fileName = $"{budgetId}_{request.MediaTypeId}.pdf";

                        await _general.UploadFile(request.SupportingDocument, filePath, fileName);

                        await _mediaRepository.UpdateMediaBudgetDocumentDetails(budgetId);
                    }
                }

                return new ApiResponse(StatusCodes.Status200OK, "Media Budget Details added successfully.");
            }

            else
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, "Failed to add Media Budget Details. Try After Some Time.");

            }
        }
        public async Task<ApiResponse> DeleteMediaBudgetDetails(long? id)
        {
            using DataTable dtExists = await _mediaRepository.CheckMediaBudgetDetailsExistsById(id);

            if (dtExists.Rows.Count == 0)
            {
                return new ApiResponse(StatusCodes.Status404NotFound, "Sorry!! Media Budget Details not found...");
            }

            string? col1 = dtExists.Rows[0]["Col1"]?.ToString();
            if (!string.IsNullOrWhiteSpace(col1))
            {
                string filePath = "MediaDeals";
                string fileName = $"{col1}.pdf";
                await _general.DeleteFile(filePath, fileName);
            }

            int affectedRows = await _mediaRepository.DeleteMediaBudgetDetailsById(id);

            if (affectedRows > 0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Media Budget Details deleted successfully.");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, "Failed to delete Media Budget Details. Try After Some Time.");
            }
        }




        public async Task<ApiResponse> GetMediaScheduleManagerSessions()
        {

            List<string> sessions = new List<string>();
            using DataTable dtSessions = await _mediaRepository.GetMediaScheduleManagerSessions();

            foreach (DataRow session in dtSessions.Rows)
            {
                sessions.Add(session["Session"]?.ToString() ?? string.Empty);
            }

            string financialSession = _general.GetFinancialSession(DateTime.Now);
            if (!sessions.Contains(financialSession))
            {
                sessions.Insert(0, financialSession);
            }

            financialSession = (DateTime.Now.Year + "-" + (DateTime.Now.Year + 1).ToString().Substring(2, 2));

            if (!sessions.Contains(financialSession))
            {
                sessions.Insert(0, financialSession);
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", sessions);

        }
        public async Task<ApiResponse> GetMediaScheduleManagerMediaTypes()
        {
            using DataTable dtMediaTypes = await _mediaRepository.GetMediaScheduleMediaTypes();
            HashSet<string> mediaTypes = new HashSet<string>();
            for (int i = 0; i < dtMediaTypes.Rows.Count; i++)
            {
                mediaTypes.Add(dtMediaTypes.Rows[i]["Type"]?.ToString() ?? string.Empty);
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", mediaTypes);
        }
        public async Task<ApiResponse> GetMediaScheduleManagerMediaTitles(GetMediaScheduleManagerMediaTitlesRequest? request)
        {
            using DataTable dtMediaTitles = await _mediaRepository.GetMediaScheduleManagerMediaTitles(request);
            HashSet<string> mediaTitles = new HashSet<string>();
            for (int i = 0; i < dtMediaTitles.Rows.Count; i++)
            {
                mediaTitles.Add(dtMediaTitles.Rows[i]["MediaTitle"]?.ToString() ?? string.Empty);
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", mediaTitles);

        }
        public async Task<ApiResponse> GetMediaScheduleManagerCreateReleaseOrderAuthorities(GetMediaScheduleManagerCreateReleaseOrderAuthoritiesRequest? request)
        {
            using DataTable dtAuthorities = await _mediaRepository.GetMediaScheduleManagerCreateReleaseOrderAuthorities(request);
            HashSet<MediaScheduleManagerCreateReleaseOrderAuthoritiesResponse> mediaScheduleManagerCreateReleaseOrderAuthoritiesResponses = new HashSet<MediaScheduleManagerCreateReleaseOrderAuthoritiesResponse>();

            foreach (DataRow row in dtAuthorities.Rows)
            {
                mediaScheduleManagerCreateReleaseOrderAuthoritiesResponses.Add(new MediaScheduleManagerCreateReleaseOrderAuthoritiesResponse(row));
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", mediaScheduleManagerCreateReleaseOrderAuthoritiesResponses);

        }
        public async Task<ApiResponse> GetAdvertisementVendors(GetAdvertisementVendorsRequest? request)
        {
            using DataTable dtVendors = await _mediaRepository.GetAdvertisementVendors(request);

            HashSet<AdvertisementVendorsResponse> advertisementVendorsResponses = new HashSet<AdvertisementVendorsResponse>();

            foreach (DataRow row in dtVendors.Rows)
            {
                advertisementVendorsResponses.Add(new AdvertisementVendorsResponse(row));
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", advertisementVendorsResponses);
        }
        public async Task<ApiResponse> GetNewScheduleCreateMediaTitles(GetScheduleCreateMediaTitles? request, string? employeeId)
        {
            using DataTable dtOnlyMediaPermission = await _mediaRepository.GetEmployeeOnlyMediaPermission(employeeId);
            string? onlyMediaPermission = string.Empty;
            if (dtOnlyMediaPermission.Rows.Count > 0)
            {
                onlyMediaPermission = dtOnlyMediaPermission.Rows[0]["onlymedia"]?.ToString() ?? string.Empty;
            }

            using DataTable dtMediaTitles = await _mediaRepository.GetNewScheduleCreateMediaTitles(request, onlyMediaPermission);
            HashSet<MediaTypeHeadsResponse> mediaTypeHeadsResponses = new HashSet<MediaTypeHeadsResponse>();
            foreach (DataRow row in dtMediaTitles.Rows)
            {
                mediaTypeHeadsResponses.Add(new MediaTypeHeadsResponse(row));
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", mediaTypeHeadsResponses);
        }
        public async Task<ApiResponse> GetCanCreateMediaRODetails(GetCanCreateMediaReleaseOrderDetailsRequest? request)
        {
            CanCreateMediaReleaseOrderDetailsResponse canCreateMediaReleaseOrderDetailsResponse = new CanCreateMediaReleaseOrderDetailsResponse();

            if (request != null && !string.IsNullOrWhiteSpace(request?.CampusCode) && !string.IsNullOrWhiteSpace(request?.Session) && !string.IsNullOrWhiteSpace(request?.ScheduleType) && !string.IsNullOrWhiteSpace(request?.MediaType) && !string.IsNullOrWhiteSpace(request?.Title))
            {
                canCreateMediaReleaseOrderDetailsResponse.CanUploadReleaseOrder = true;

                DataTable dtBudgetAmountExists = await _mediaRepository.CheckMediaBudgetDetailsExistsWithAmount(request);

                if (dtBudgetAmountExists.Rows.Count <= 0)
                {
                    canCreateMediaReleaseOrderDetailsResponse.CanUploadReleaseOrder = false;
                    canCreateMediaReleaseOrderDetailsResponse.Instruction = "Budget Not Defined For Selected Campus > Session > Media > Title.. Can Not Create New Release Order For Same";
                }

                using DataTable dtPendingReleaseOrders = await _mediaRepository.GetPendingReleaseOrdersScheduleIds();
                if (dtPendingReleaseOrders.Rows.Count > 0)
                {
                    StringBuilder releaseOrders = new StringBuilder();
                    foreach (DataRow DR in dtPendingReleaseOrders.Rows)
                    {
                        releaseOrders.Append(DR[0]?.ToString()?.Substring(1, (DR[0]?.ToString() ?? string.Empty).Length - 1));
                    }

                    string pendingScheduleOrders = releaseOrders.ToString();
                    pendingScheduleOrders = pendingScheduleOrders.Substring(0, pendingScheduleOrders.Length - 1);

                    if (!string.IsNullOrWhiteSpace(pendingScheduleOrders))
                    {
                        using DataTable dtPendingMediaSchedules = await _mediaRepository.GetPendingMediaSchedules(request, pendingScheduleOrders);
                        if (dtPendingMediaSchedules.Rows.Count > 0)
                        {
                            canCreateMediaReleaseOrderDetailsResponse.Instruction = "Release Order Pending For Approval/Rejection .. But You Can Create New Release Order For Same";
                        }
                    }
                }


                DataTable dtBudgetSummary = new DataTable();
                dtBudgetSummary.Columns.Add("SNO");
                dtBudgetSummary.Columns.Add("Type");
                dtBudgetSummary.Columns.Add("Amount");
                dtBudgetSummary.Columns.Add("Remark");
                double budamt = Convert.ToDouble(0);

                {
                    DataRow DR = dtBudgetSummary.NewRow();
                    DR[0] = "1.";
                    DR[1] = "Budget";
                    DR[2] = "0";
                    DR[3] = "Sorry! Budget Not Found";
                    using DataTable T1 = await _mediaRepository.GetMediaBudgetAmount(request);

                    if (T1.Rows.Count > 0 && (T1.Rows[0][1]?.ToString() ?? string.Empty).Length > 0)
                    {
                        DR[2] = T1.Rows[0][1].ToString();
                        if (T1.Rows[0][1].ToString() != "0")
                            DR[3] = "Ok! Budget Found";
                    }
                    budamt = Convert.ToDouble(DR[2]);
                    dtBudgetSummary.Rows.Add(DR);
                }

                {
                    DataRow DR = dtBudgetSummary.NewRow();
                    DR[0] = "2.";
                    DR[1] = "Scheduled";
                    DR[2] = "0";
                    DR[3] = "Ok! No Scheduled Found";
                    using DataTable T1 = await _mediaRepository.GetScheduledMediaBudgetAmount(request);

                    if (T1.Rows.Count > 0 && (T1.Rows[0][1]?.ToString() ?? string.Empty).Length > 0)
                    {
                        DR[2] = T1.Rows[0][1].ToString();
                        if (T1.Rows[0][1].ToString() != "0")
                        {
                            double schamt = Convert.ToDouble(DR[2]);
                            double diff = budamt - schamt;
                            if (diff >= 0)
                            {
                                DR[3] = "Ok! You Have " + diff + " Rs./- More To Schedule.";
                            }
                            else
                            {
                                DR[3] = "Sorry! You Have Exceeded " + Math.Abs(diff) + " Rs./- In Schedule.";
                            }

                        }
                    }

                    dtBudgetSummary.Rows.Add(DR);
                }

                {
                    DataRow DR = dtBudgetSummary.NewRow();
                    DR[0] = "3.";
                    DR[1] = "Release Order";
                    DR[2] = "0";
                    DR[3] = "Ok! No Release Order Found";
                    {

                        double used = 0;

                        using DataTable dtApprovedReleaseOrders = await _mediaRepository.GetApprovedReleaseOrders(request?.Session);

                        if (dtApprovedReleaseOrders.Rows.Count > 0)
                        {
                            string ss = "";
                            foreach (DataRow drRO in dtApprovedReleaseOrders.Rows)
                            {
                                ss = string.Concat(ss, drRO[1].ToString().AsSpan(1, (drRO[1]?.ToString() ?? string.Empty).Length - 1));
                            }
                            ss = ss.Substring(0, ss.Length - 1);

                            using DataTable dtUsedAmount = await _mediaRepository.GetusedScheduledAmount(request, ss);
                            if (dtUsedAmount.Rows.Count > 0)
                            {
                                used = Convert.ToDouble(dtUsedAmount.Rows[0][0].ToString());
                            }
                        }

                        DR[2] = used;
                        if (DR[2].ToString() != "0")
                        {
                            double resamt = Convert.ToDouble(DR[2]);
                            double diff = budamt - resamt;
                            if (diff >= 0)
                            {
                                DR[3] = "Ok! You Have " + diff + " Rs./- More To Design Release Order.";
                            }
                            else
                            {
                                DR[3] = "Sorry! You Have Exceeded " + Math.Abs(diff) + " Rs./- In Release Order.";
                                canCreateMediaReleaseOrderDetailsResponse.IsOverBudget = true;
                            }

                        }
                    }

                    dtBudgetSummary.Rows.Add(DR);
                }

                HashSet<MediaBudgetSummaryBudgetScheduledRO> mediaBudgetSummaryBudgetScheduledROs = new HashSet<MediaBudgetSummaryBudgetScheduledRO>();

                foreach (DataRow row in dtBudgetSummary.Rows)
                {
                    mediaBudgetSummaryBudgetScheduledROs.Add(new MediaBudgetSummaryBudgetScheduledRO(row));
                }

                canCreateMediaReleaseOrderDetailsResponse.HeaderString = $"Budget & Expenditure @ {await _inclusiveService.GetCampusNameByCampusCode(request?.CampusCode)} -> {request?.Session} -> {request?.MediaType} -> {request?.Title}";

                canCreateMediaReleaseOrderDetailsResponse.SummaryList = mediaBudgetSummaryBudgetScheduledROs;

                return new ApiResponse(StatusCodes.Status200OK, "Success", canCreateMediaReleaseOrderDetailsResponse);
            }
            else
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, "Invalid Request Data");
            }
        }
        public async Task<ApiResponse> GetMediaSchedules(GetMediaSchedulesRequest? request, string? employeeId)
        {
            using DataTable dtOnlyMediaPermission = await _mediaRepository.GetEmployeeOnlyMediaPermission(employeeId);
            string? onlyMediaPermission = string.Empty;
            if (dtOnlyMediaPermission.Rows.Count > 0)
            {
                onlyMediaPermission = dtOnlyMediaPermission.Rows[0]["onlymedia"]?.ToString() ?? string.Empty;
            }

            using DataTable dtMediaSchedules = await _mediaRepository.GetMediaSchedules(request, onlyMediaPermission);

            HashSet<SchedulesResponse> schedulesResponses = new HashSet<SchedulesResponse>();

            foreach (DataRow row in dtMediaSchedules.Rows)
            {
                SchedulesResponse schedulesResponse = new SchedulesResponse(row);

                DateTime dtm = Convert.ToDateTime(schedulesResponse.ScheduleDate);

                if (schedulesResponse.ExecutedOn != "----")
                {
                    schedulesResponse.CanEdit = false;
                    schedulesResponse.LockIt = "Yes";
                }

                if (dtm < DateTime.Now.Date)
                {
                    if (schedulesResponse.ExecutedOn == "----")
                        schedulesResponse.BackColor = System.Drawing.Color.LightPink;
                    else
                        schedulesResponse.BackColor = System.Drawing.Color.LightSkyBlue;
                }
                if (dtm == DateTime.Now.Date)
                {
                    if (schedulesResponse.ExecutedOn == "----")
                        schedulesResponse.BackColor = System.Drawing.Color.LightYellow;
                    else
                        schedulesResponse.BackColor = System.Drawing.Color.LightSkyBlue;
                }

                if (schedulesResponse.Billinitiated != "N/A")
                {
                    schedulesResponse.BackColor = System.Drawing.Color.LightGreen;
                    schedulesResponse.StoredBillRecieptLink = $"'Reports/StoredBillReceipt.aspx?TransactionID={schedulesResponse.Billinitiated}&SequenceID=-1";
                }
                schedulesResponse.Edition = schedulesResponse!.Edition!.Replace("+", "+\n");

                if (_general.IsFileExists($"Upload_Advertisement/{schedulesResponse.Id}.pdf"))
                {
                    schedulesResponse.AdvertisementFileExists = true;
                }

                using DataTable dtExternalReleaseOrderExists = await _mediaRepository.GetExternalReleaseOrderDetailsByScheduleId(schedulesResponse.Id);

                if (dtExternalReleaseOrderExists.Rows.Count > 0)
                {
                    schedulesResponse.CanGenerateReleaseOrder = false;
                    schedulesResponse.CanDelete = false;
                    schedulesResponse.IsReleaseOrderGenerated = true;

                    schedulesResponse.ReleaseOrderPrintLink = $"Reports/ReleaseOrder.aspx?Type=Old&Id={dtExternalReleaseOrderExists.Rows[0]["OrderId"].ToString()}&Vdr=&ForSession={request?.Session}&Extra=&OrderNo=";

                    if (_general.IsFileExists($"Upload_Release_Order/{dtExternalReleaseOrderExists.Rows[0]["OrderId"].ToString()}.pdf"))
                    {
                        schedulesResponse.ReleaseOrderDownloadLink = $"Upload_Release_Order/{dtExternalReleaseOrderExists.Rows[0]["OrderId"].ToString()}.pdf";
                    }
                }
                else
                {
                    using DataTable dtInternalReleaseOrderExists = await _mediaRepository.GetInternalReleaseOrderDetailsByScheduleId(schedulesResponse.Id);

                    if (dtInternalReleaseOrderExists.Rows.Count > 0)
                    {
                        schedulesResponse.CanDelete = false;

                        schedulesResponse.IsReleaseOrderGenerated = true;
                    }
                }

                if (schedulesResponse.ExecutedOn != "" && (schedulesResponse.ExecutedOn != "----"))
                {
                    schedulesResponse.CanDelete = false;
                }


                schedulesResponses.Add(schedulesResponse);
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", schedulesResponses);

        }
        public async Task<ApiResponse> GetMediaScheduleToEdit(string? id)
        {
            using DataTable dtMediaSchedule = await _mediaRepository.GetMediaScheduleById(id);

            if (dtMediaSchedule.Rows.Count == 0)
            {
                return new ApiResponse(StatusCodes.Status404NotFound, "Sorry!! Media Schedule not found...");
            }

            EditMediaScheduleResponse schedulesResponse = new EditMediaScheduleResponse(dtMediaSchedule.Rows[0]);

            DateTime dtm = Convert.ToDateTime(schedulesResponse.ScheduleDate);

            if (schedulesResponse.ExecutedOn != "----")
            {
                schedulesResponse.CanEdit = false;
                schedulesResponse.LockIt = "Yes";
            }

            if (dtm < DateTime.Now.Date)
            {
                if (schedulesResponse.ExecutedOn == "----")
                    schedulesResponse.BackColor = System.Drawing.Color.LightPink;
                else
                    schedulesResponse.BackColor = System.Drawing.Color.LightSkyBlue;
            }
            if (dtm == DateTime.Now.Date)
            {
                if (schedulesResponse.ExecutedOn == "----")
                    schedulesResponse.BackColor = System.Drawing.Color.LightYellow;
                else
                    schedulesResponse.BackColor = System.Drawing.Color.LightSkyBlue;
            }

            if (schedulesResponse.Billinitiated != "N/A")
            {
                schedulesResponse.BackColor = System.Drawing.Color.LightGreen;
                schedulesResponse.StoredBillRecieptLink = $"'Reports/StoredBillReceipt.aspx?TransactionID={schedulesResponse.Billinitiated}&SequenceID=-1";
            }
            schedulesResponse.Edition = schedulesResponse!.Edition!.Replace("+", "+\n");

            if (_general.IsFileExists($"Upload_Advertisement/{schedulesResponse.Id}.pdf"))
            {
                schedulesResponse.AdvertisementFileExists = true;
            }

            using DataTable dtExternalReleaseOrderExists = await _mediaRepository.GetExternalReleaseOrderDetailsByScheduleId(schedulesResponse.Id);

            if (dtExternalReleaseOrderExists.Rows.Count > 0)
            {
                schedulesResponse.CanGenerateReleaseOrder = false;
                schedulesResponse.CanDelete = false;
                schedulesResponse.IsReleaseOrderGenerated = true;
            }
            else
            {
                using DataTable dtInternalReleaseOrderExists = await _mediaRepository.GetInternalReleaseOrderDetailsByScheduleId(schedulesResponse.Id);

                if (dtInternalReleaseOrderExists.Rows.Count > 0)
                {
                    schedulesResponse.CanDelete = false;

                    schedulesResponse.IsReleaseOrderGenerated = true;
                }
            }

            if (schedulesResponse.ExecutedOn != "" && (schedulesResponse.ExecutedOn != "----"))
            {
                schedulesResponse.CanDelete = false;
            }

            string[] selectedEditions = schedulesResponse.Edition!.Replace(", ", "?").Split('?');
            string[] selectedAdvertisementTypes = schedulesResponse.AdvertisementType!.Replace(", ", "?").Split('?');

            schedulesResponse.Editions = selectedEditions.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            schedulesResponse.Advertisements = selectedAdvertisementTypes.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (schedulesResponse.LockIt == "Yes")
            {
                schedulesResponse.CanEditScheduleOn = false;
                schedulesResponse.CanEditMediaType = false;
                schedulesResponse.CanEditAmount = false;
                schedulesResponse.CanEditWidth = false;
                schedulesResponse.CanEditHeight = false;
                schedulesResponse.CanEditRate = false;
                schedulesResponse.CanEditDiscount = false;
                schedulesResponse.CanEditTax = false;
                schedulesResponse.CanEditActualPaid = false;
                schedulesResponse.CanEditPageNo = false;
            }
            else
            {
                schedulesResponse.CanEditScheduleOn = true;
                schedulesResponse.CanEditMediaType = true;
                schedulesResponse.CanEditAmount = true;
                schedulesResponse.CanEditWidth = true;
                schedulesResponse.CanEditHeight = true;
                schedulesResponse.CanEditRate = true;
                schedulesResponse.CanEditDiscount = true;
                schedulesResponse.CanEditTax = true;
                schedulesResponse.CanEditActualPaid = true;
                schedulesResponse.CanEditPageNo = true;
            }

            using DataTable dtApprovedOrPendingReleaseOrders = await _mediaRepository.GetExternalPendingOrAPprovedReleaseOrderDetailsByScheduleId(schedulesResponse.Id);

            if (dtApprovedOrPendingReleaseOrders.Rows.Count > 0)
            {
                schedulesResponse.CanEditScheduleOn = true;
                schedulesResponse.CanEditPageNo = true;
                schedulesResponse.CanEditMediaType = false;
                schedulesResponse.CanEditAmount = false;
                schedulesResponse.CanEditWidth = false;
                schedulesResponse.CanEditHeight = false;
                schedulesResponse.CanEditRate = false;
                schedulesResponse.CanEditDiscount = false;
                schedulesResponse.CanEditTax = false;
                schedulesResponse.CanEditActualPaid = false;
            }
            else
            {
                schedulesResponse.CanEditScheduleOn = true;
                schedulesResponse.CanEditMediaType = true;
                schedulesResponse.CanEditAmount = true;
                schedulesResponse.CanEditWidth = true;
                schedulesResponse.CanEditHeight = true;
                schedulesResponse.CanEditRate = true;
                schedulesResponse.CanEditDiscount = true;
                schedulesResponse.CanEditTax = true;
                schedulesResponse.CanEditActualPaid = true;
                schedulesResponse.CanEditPageNo = true;
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", schedulesResponse);
        }
        public async Task<ApiResponse> AddMediaSchedule(AddMediaScheduleRequest? request, string? employeeId)
        {
            DateTime currentDateTime = DateTime.Now;

            string currentSession = (currentDateTime.Month >= 4 ? currentDateTime.Year.ToString() + "-" + (currentDateTime.Year + 1).ToString().Substring(2, 2) : (currentDateTime.Year - 1).ToString() + "-" + (currentDateTime.Year).ToString().Substring(2, 2));

            string scheduleId = await _mediaRepository.AddMediaSchedule(request, currentSession, employeeId);

            if (!string.IsNullOrEmpty(scheduleId))
            {
                if (request?.SupportingDocument != null)
                {
                    string filePath = "Upload_Advertisement";
                    string fileName = $"{scheduleId}.pdf";
                    await _general.UploadFile(request.SupportingDocument, filePath, fileName);
                    await _mediaRepository.UpdateScheduleDocumentId(scheduleId);
                }

                return new ApiResponse(StatusCodes.Status201Created, "Media schedule created successfully.");
            }
            return new ApiResponse(StatusCodes.Status500InternalServerError, "Error creating media schedule. Try again later.");
        }
        public async Task<ApiResponse> DeleteMediaSchedule(string? id)
        {
            using DataTable dtExists = await _mediaRepository.CheckIsMediaScheduleExistsById(id);

            if (dtExists.Rows.Count <= 0)
            {
                return new ApiResponse(StatusCodes.Status404NotFound, "Sorry!! Media Schedule not found...");
            }

            int affectedRows = await _mediaRepository.DeleteMediaScheduleById(id);

            if (affectedRows > 0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Media Schedule deleted successfully.");
            }

            else
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, "Failed to delete Media Schedule. Try After Some Time.");
            }
        }
        public async Task<ApiResponse> EditMediaSchedule(EditMediaScheduleRequest? request, string? employeeId)
        {
            using DataTable dtExists = await _mediaRepository.CheckIsMediaScheduleExistsById(request?.Id);

            if (dtExists.Rows.Count <= 0)
            {
                return new ApiResponse(StatusCodes.Status404NotFound, "Sorry!! Media Schedule not found...");
            }

            EditMediaScheduleOldDetails? editMediaScheduleOldDetails = new EditMediaScheduleOldDetails(dtExists.Rows[0]);

            int affectedRows = await _mediaRepository.EditMediaSchedule(request, editMediaScheduleOldDetails, employeeId);

            if (affectedRows == int.MaxValue)
            {
                return new ApiResponse(StatusCodes.Status409Conflict, "Sorry!! No changes detected in the Media Schedule.");
            }

            if (affectedRows > 0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Media Schedule updated successfully.");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status400BadRequest, "Failed to update Media Schedule. Try After Some Time.");
            }
        }
        public async Task<ApiResponse> EditMediaScheduleFile(EditMediaScheduleFileRequest? request)
        {
            using DataTable dtExists = await _mediaRepository.CheckIsMediaScheduleExistsById(request?.Id);

            if (dtExists.Rows.Count <= 0)
            {
                return new ApiResponse(StatusCodes.Status404NotFound, "Sorry!! Media Schedule not found...");
            }
            if (request?.SupportingDocument != null)
            {
                string filePath = "Upload_Advertisement";
                string fileName = $"{request?.Id}.pdf";
                await _general.UploadFile(request!.SupportingDocument, filePath, fileName);
                await _mediaRepository.UpdateScheduleDocumentId(request?.Id);
            }

            return new ApiResponse(StatusCodes.Status200OK, "Media Schedule Document updated successfully.");

        }
    }
}