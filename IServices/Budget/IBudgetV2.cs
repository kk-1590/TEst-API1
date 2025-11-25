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
        Task<ApiResponse> GetDepartmentBudgetDetails(string RefNo);
        Task<ApiResponse> UpdateDepartmentBudgetDetails(string EmpCode, AddDepartmentSummaryUpdateRequest request);
        Task<ApiResponse> GetBudgetHeadMapping(BudgetHeadMappingRequest? mappingRequest);
        Task<ApiResponse> AddBudgetHeadMapping(AddBudgetTypeHeadRequest? addRequest, string? employeeId);
        Task<ApiResponse> DeleteBudgetHeadMapping(DeleteBudgetHeadMappingRequest? deleteRequest, string? employeeId);
        Task<ApiResponse> GetCreateBudgetSummaryDepartments(string? employeeId, string? campusCode);
        Task<ApiResponse> CreateBudgetSummary(string? employeeId, CreateDepartmentBudgetSummaryV2Request? budgetRequest);
        Task<ApiResponse> GetBudgetSummary(string? employeeId, GetDepartmentBudgetSummaryV2Request? budgetRequest);
        Task<ApiResponse> DeleteDepartmentBudgetDetails(string EmpCode, int Id, string REfNo);
        Task<ApiResponse> LockDepartmentDetails(string RefNo, string EmpCode);
        Task<ApiResponse> GetHeadFilter(string Type);
    }
}
