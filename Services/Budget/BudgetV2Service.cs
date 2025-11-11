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

            return new ApiResponse(StatusCodes.Status201Created, "Success", "Budget Details Added Successfully...");

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

        public async Task<ApiResponse> AddDepartmentDetails(string EmpCode,AddDepartmentSummaryRequest request)
        {
            using (DataTable dt = await _budgetRepository.CheckAlreadyDepartmentBudgetDetails(request))
            {
                if (dt.Rows.Count > 0)
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Record Already Exists");
            }
            //add maad in list if not add
            using (DataTable dt = await _budgetRepository.CheckMaadListRecord(request.BudgetMaad??string.Empty))
            {
                if (dt.Rows.Count <= 0)
                    await _budgetRepository.AddMaadInList(EmpCode, request.BudgetMaad ?? string.Empty);
            }
            //Task<int> AddDepartmentList(string EmpCode, AddDepartmentSummaryRequest request)
            int ins =await _budgetRepository.AddDepartmentList(EmpCode,request);

            return new ApiResponse(StatusCodes.Status200OK, "Success", $"`{ins}` Record Insert Successfully");
        }

    }
}
