using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;

namespace AdvanceAPI.IServices.Budget
{
    public interface IBudgetV2
    {
        Task<ApiResponse> GetBudgetFilterSessions(string? campus);
        Task<ApiResponse> GetBudgetSessionAmountSummary(BudgetSessionAmountSummaryRequest? summaryRequest);
    }
}
