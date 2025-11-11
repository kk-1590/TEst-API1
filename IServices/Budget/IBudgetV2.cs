using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;

namespace AdvanceAPI.IServices.Budget
{
    public interface IBudgetV2
    {
        Task<ApiResponse> GetBudgetFilterSessions(string? campus);
        Task<ApiResponse> GetBudgetSessionAmountSummary(BudgetSessionAmountSummaryRequest? summaryRequest);
        Task<ApiResponse> UpdateBudgetSessionAmountSummary(UpdateBudgetSessionAmountRequest? updateRequest, string? employeeId);
        Task<ApiResponse> AddBudgetSessionAmountSummary(CreateNewBudgetSessionAmountSummaryRequest? createRequest, string? employeeId);
        Task<ApiResponse> DeleteBudgetSessionAmountSummary(DeleteBudgetSessionSummaryAmountRequest? deleteRequest, string? employeeId);
        Task<ApiResponse> LockBudgetSessionAmountSummary(string? budgetId, string? employeeId);
        Task<ApiResponse> GetMaadForfilter(string Maad);
        Task<ApiResponse> AddDepartmentDetails(string EmpCode, AddDepartmentSummaryRequest request);
        Task<ApiResponse> GetBudgetSummaryNewSessions();
    }
}
