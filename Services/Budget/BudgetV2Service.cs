using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Budget;
using System.Data;
using System.Reflection.Emit;

namespace AdvanceAPI.Services.Budget
{
    public class BudgetV2Service : IBudgetV2
    {

        private readonly IBudgetV2Repository _budgetRepository;
        private readonly IGeneral _general;
        public BudgetV2Service(IBudgetV2Repository budgetRepository, IGeneral general)
        {
            _budgetRepository = budgetRepository;
            _general = general;
        }
        public async Task<ApiResponse> GetBudgetFilterSessions(string? campus)
        {
            if (string.IsNullOrEmpty(campus))
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Campus Is Required");
            }

            using (DataTable d = await _budgetRepository.GetBudgetFilterSessions(campus))
            {
                List<string> sessions = new List<string>();
                foreach (DataRow session in d.Rows)
                {
                    sessions.Add(session["Session"]?.ToString() ?? string.Empty);
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", sessions);
            }
        }
        public async Task<ApiResponse> GetBudgetSessionAmountSummary(BudgetSessionAmountSummaryRequest? summaryRequest)
        {
            if (summaryRequest == null)
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Request Found...");
            }

            using (DataTable d = await _budgetRepository.GetBudgetSessionAmountSummary(summaryRequest))
            {
                List<BudgetSessionAmountSummaryResponse> summaryResponses = new List<BudgetSessionAmountSummaryResponse>();
                foreach (DataRow summaryRow in d.Rows)
                {
                    summaryResponses.Add(new BudgetSessionAmountSummaryResponse(summaryRow));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", summaryResponses);
            }
        }
        public async Task<ApiResponse> UpdateBudgetSessionAmountSummary(UpdateBudgetSessionAmountRequest? updateRequest, string? employeeId)
        {
            if (updateRequest == null)
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Request Found...");
            }

            using (DataTable dtIsExists = await _budgetRepository.GetBudgetSessionEditableSummaryExists(updateRequest.BudgetId))
            {
                if (dtIsExists.Rows.Count == 0)
                {
                    return new ApiResponse(StatusCodes.Status404NotFound, "Error", "Sorry!! No Valid Budget Details Found To Update...");
                }

                long existingAmount = _general.StringToLong(dtIsExists.Rows[0]["BudgetAmount"]?.ToString() ?? "0");

                if (existingAmount == updateRequest.BudgetAmount)
                {
                    return new ApiResponse(StatusCodes.Status200OK, "Success", "Sorry!! No Changes Detected In Budget Amount...");
                }

                await _budgetRepository.UpdateBudgetSessionSummaryAmount(updateRequest, existingAmount, employeeId);

                return new ApiResponse(StatusCodes.Status200OK, "Success", "Budget Amount Updated Successfully...");
            }

        }
        public async Task<ApiResponse> AddBudgetSessionAmountSummary(CreateNewBudgetSessionAmountSummaryRequest? createRequest, string? employeeId)
        {
            if (createRequest == null)
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Request Found...");
            }

            using (DataTable dtIsExists = await _budgetRepository.CheckIsBudgetSessionAmountSummaryExists(createRequest.Session, createRequest.CampusCode))
            {
                if (dtIsExists.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status409Conflict, "Error", "Sorry!! Budget Details Already Exists For The Selected Session And Campus...");
                }
            }

            await _budgetRepository.AddBudgetSessionSummaryAmount(createRequest, employeeId);

            return new ApiResponse(StatusCodes.Status200OK, "Success", "Budget Details Added Successfully...");

        }
        public async Task<ApiResponse> DeleteBudgetSessionAmountSummary(DeleteBudgetSessionSummaryAmountRequest? deleteRequest, string? employeeId)
        {
            if (deleteRequest == null)
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Request Found...");
            }

            using (DataTable dtIsExists = await _budgetRepository.GetBudgetSessionEditableSummaryExists(deleteRequest.BudgetId))
            {
                if (dtIsExists.Rows.Count == 0)
                {
                    return new ApiResponse(StatusCodes.Status404NotFound, "Error", "Sorry!! No Valid Budget Details Found To Delete...");
                }
                await _budgetRepository.DeleteBudgetSessionSummaryAmount(deleteRequest, employeeId);

                return new ApiResponse(StatusCodes.Status200OK, "Success", "Budget Details Deleted Successfully...");
            }

        }
        public async Task<ApiResponse> LockBudgetSessionAmountSummary(string? budgetId, string? employeeId)
        {
            if (string.IsNullOrWhiteSpace(budgetId))
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Request Found...");
            }

            using (DataTable dtIsExists = await _budgetRepository.GetBudgetSessionEditableSummaryExists(budgetId))
            {
                if (dtIsExists.Rows.Count == 0)
                {
                    return new ApiResponse(StatusCodes.Status404NotFound, "Error", "Sorry!! No Valid Budget Details Found To Lock...");
                }

                await _budgetRepository.LockBudgetSessionSummaryAmount(budgetId, employeeId);

                return new ApiResponse(StatusCodes.Status200OK, "Success", "Budget Locked Successfully...");
            }

        }
        public async Task<ApiResponse> GetBudgetSummaryNewSessions()
        {
            List<string> sessions = new List<string>();
            using (DataTable d = await _budgetRepository.GetAllSessionsOfBudgetSessionSummary())
            {
                foreach (DataRow session in d.Rows)
                {
                    sessions.Add(session["Session"]?.ToString() ?? string.Empty);
                }

                string? financeSession = _general.GetFinancialSession(DateTime.Now);

                if (!sessions.Contains(financeSession))
                {
                    sessions.Add(financeSession);
                }
                string? NextFinanceSession = _general.GetFinancialSession(DateTime.Now.AddMonths(12));
                if (!sessions.Contains(NextFinanceSession))
                {
                    sessions.Add(NextFinanceSession);
                }
                sessions = sessions.OrderByDescending(s => s).ToList();

                return new ApiResponse(StatusCodes.Status200OK, "Success", sessions);
            }
        }
        public async Task<ApiResponse> GetMaadForfilter(string Maad)
        {
            using (DataTable dt = await _budgetRepository.GetBudgetMaadListFilter(Maad))
            {
                List<string> filters = new List<string>();
                foreach (DataRow filterRow in dt.Rows)
                {
                    filters.Add(filterRow[0].ToString() ?? string.Empty);
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", filters);
            }
        }
        public async Task<ApiResponse> AddDepartmentDetails(string EmpCode, AddDepartmentSummaryRequest request)
        {
            using (DataTable dt = await _budgetRepository.CheckAlreadyDepartmentBudgetDetails(request))
            {
                if (dt.Rows.Count > 0)
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Record Already Exists");
            }
            //add maad in list if not add
            using (DataTable dt = await _budgetRepository.CheckMaadListRecord(request.BudgetMaad ?? string.Empty))
            {
                if (dt.Rows.Count <= 0)
                    await _budgetRepository.AddMaadInList(EmpCode, request.BudgetMaad ?? string.Empty);
            }
            //Task<int> AddDepartmentList(string EmpCode, AddDepartmentSummaryRequest request)
            int ins = await _budgetRepository.AddDepartmentList(EmpCode, request);

            return new ApiResponse(StatusCodes.Status200OK, "Success", $"`{ins}` Record Insert Successfully");
        }
        public async Task<ApiResponse> GetDepartmentBudgetDetails(string RefNo)
        {
            using (DataTable dt = await _budgetRepository.GetDepartmentDetails(RefNo))
            {
                List<DepartmentDetailsResponse> lst = new List<DepartmentDetailsResponse>();
                foreach (DataRow row in dt.Rows)
                {
                    lst.Add(new DepartmentDetailsResponse(row));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }
        }
        public async Task<ApiResponse> UpdateDepartmentBudgetDetails(string EmpCode, AddDepartmentSummaryUpdateRequest request)
        {
            // Task<int> UpdateDepartmentDetails(string EmpCode, AddDepartmentSummaryUpdateRequest request)
            int ins = await _budgetRepository.UpdateDepartmentDetails(EmpCode, request);
            return new ApiResponse(StatusCodes.Status200OK, "Success", $"`{ins}` Update Successfully");
        }
        public async Task<ApiResponse> GetBudgetHeadMapping(BudgetHeadMappingRequest? mappingRequest)
        {
            if (mappingRequest == null)
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Request Found...");
            }

            using (DataTable dtIsExists = await _budgetRepository.GetBudgetTypeHeadMapping(mappingRequest))
            {
                List<BudgetHeadMappingResponse> mappingResponses = new List<BudgetHeadMappingResponse>();
                foreach (DataRow mappingRow in dtIsExists.Rows)
                {
                    mappingResponses.Add(new BudgetHeadMappingResponse(mappingRow));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", mappingResponses);
            }
        }
        public async Task<ApiResponse> AddBudgetHeadMapping(AddBudgetTypeHeadRequest? addRequest, string? employeeId)
        {
            if (addRequest == null)
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Request Found...");
            }

            using (DataTable dtIsExists = await _budgetRepository.CheckBudgetTypeHeadMappingAlreadyExists(addRequest))
            {
                if (dtIsExists.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status409Conflict, "Error", "Sorry!! Budget Type Head Mapping Already Exists...");
                }

                await _budgetRepository.AddBudgetTypeHeadMappingAlreadyExists(addRequest, employeeId);

                return new ApiResponse(StatusCodes.Status200OK, "Success", "Budget Type Head Mapping Added Successfully...");

            }
        }
        public async Task<ApiResponse> DeleteBudgetHeadMapping(DeleteBudgetHeadMappingRequest? deleteRequest, string? employeeId)
        {
            if (deleteRequest == null)
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Request Found...");
            }

            using (DataTable dtIsExists = await _budgetRepository.CheckBudgetTypeHeadMappingAlreadyExistsByid(deleteRequest?.HeadMappingId))
            {
                if (dtIsExists.Rows.Count == 0)
                {
                    return new ApiResponse(StatusCodes.Status404NotFound, "Error", "Sorry!! No Valid Budget Type Head Mapping Found To Delete...");
                }
            }

            using (DataTable dtIsExists = await _budgetRepository.CheckBudgetTypeHeadMappingUsedOrNOtByid(deleteRequest?.HeadMappingId))
            {
                if (dtIsExists.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status409Conflict, "Error", "Sorry!! Budget Type Head Mapping Is Used By Departments And Cannot Be Deleted...");
                }
            }

            await _budgetRepository.DeleteBudgetTypeHeadMappingAlreadyExists(deleteRequest, employeeId);

            return new ApiResponse(StatusCodes.Status200OK, "Success", "Budget Type Head Mapping Deleted Successfully...");
        }
        public async Task<ApiResponse> GetCreateBudgetSummaryDepartments(string? employeeId, string? campusCode)
        {
            if (!_general.IsValidCampusCode(campusCode))
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Campus Found...");
            }

            bool alldepartments = (await _budgetRepository.CheckAllDepartmentBudgetAllowed(employeeId)).Rows.Count > 0;

            using (DataTable dt = await _budgetRepository.GetBudgetDepartments(employeeId, campusCode, alldepartments))
            {
                List<string> departments = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    departments.Add(dr["name"]?.ToString() ?? string.Empty);
                }

                return new ApiResponse(StatusCodes.Status200OK, "Success", departments);
            }



        }
        public async Task<ApiResponse> CreateBudgetSummary(string? employeeId, CreateDepartmentBudgetSummaryV2Request? budgetRequest)
        {

            if (budgetRequest == null)
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Budget Details Found...");
            }

            using (DataTable dtDepartmentExists = await _budgetRepository.CheckIsValidDepartmentBudgetDepartment(budgetRequest))
            {
                if (dtDepartmentExists.Rows.Count == 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Department Found For Budget Summary...");
                }
            }

            using (DataTable dtExists = await _budgetRepository.CheckIsDepartmentBudgetSummaryExists(budgetRequest))
            {

                if (dtExists.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", $"Sorry!! Budget Details Already Exists.");
                }

                string newBudgetReferenceNo = string.Empty;

                using (DataTable dtNewBudgetNo = await _budgetRepository.GetNewDepartmentBudgetSummaryReferenceNo())
                {
                    if (dtNewBudgetNo.Rows.Count > 0)
                    {
                        newBudgetReferenceNo = dtNewBudgetNo.Rows[0]["ReferenceNo"]?.ToString() ?? string.Empty;
                    }
                }

                if (string.IsNullOrWhiteSpace(newBudgetReferenceNo))
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", $"Sorry!! Unable to generate Budget Reference No at this moment. Please try again.");
                }

                await _budgetRepository.CreateNewBudgetSummary(employeeId, newBudgetReferenceNo, _general.GetIpAddress(), budgetRequest);

                return new ApiResponse(StatusCodes.Status200OK, "Success", newBudgetReferenceNo);

            }
        }

        public async Task<ApiResponse> GetBudgetSummary(string? employeeId, GetDepartmentBudgetSummaryV2Request? budgetRequest)
        {

            if (budgetRequest == null)
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Budget Details Found...");
            }

            bool alldepartments = (await _budgetRepository.CheckAllDepartmentBudgetAllowed(employeeId)).Rows.Count > 0;

            using (DataTable dtRecords = await _budgetRepository.GetDepartmentBudgetSummary(budgetRequest, employeeId, alldepartments))
            {

                List<DepartmentBudgetSummaryResponse> budgetSummaries = new List<DepartmentBudgetSummaryResponse>();
                foreach (DataRow dr in dtRecords.Rows)
                {
                    budgetSummaries.Add(new DepartmentBudgetSummaryResponse(dr));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", budgetSummaries);
            }

        }
        public async Task<ApiResponse> DeleteDepartmentBudgetDetails(string EmpCode, int Id, string RefNo)
        {
            using (DataTable dt = await _budgetRepository.ValidDepartmentDetailsForDelete(Id, RefNo))
            {
                if (dt.Rows.Count <= 0)
                {

                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Invalid Request/Invalid refno");
                }

            }
            int ins = await _budgetRepository.deleteDepartment(EmpCode, Id);
            return new ApiResponse(StatusCodes.Status200OK, "Success", $"`{ins}` Record Delete Successfully");
        }
    }
}
