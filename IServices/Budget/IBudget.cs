using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;

namespace AdvanceAPI.IServices.Budget
{
    public interface IBudget
    {
        Task<ApiResponse> AddItemWithSession(string EmpCode, MapNewMaad mapNewMaad);
        Task<ApiResponse> UpdateBudgetMaad(string EmpCode, UpdateMaadBudegtRequest updateMaadBudegtRequest);
        Task<ApiResponse> GetRecord(int Limit, int Offset, string CampusCode, string Session, int BudgetRequired);
        Task<ApiResponse> GetMaadNonBudgetRequired(LimitRequest limitRequest);
        Task<ApiResponse> GetMaadBudgetRequired(LimitRequest limitRequest);
        Task<ApiResponse> GetAddedMaad(string RefNo);
        Task<ApiResponse> UpdateaadDetails(string EmpCode, UpdateBudgetDetails updateBudgetDetails);
        Task<ApiResponse> DeleteaadDetails(UpdateBudgetDetails updateBudgetDetails);

        Task<ApiResponse> GetCreateBudgetSummaryDepartments(string? employeeId, string? campusCode);
        Task<ApiResponse> CreateBudgetSummary(string? employeeId, CreateDepartmentBudgetSummaryRequest? budgetRequest);

    }
}
