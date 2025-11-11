using AdvanceAPI.DTO.Budget;
using System.Data;
using System.Threading.Tasks;

namespace AdvanceAPI.IRepository
{
    public interface IBudgetV2Repository
    {
        Task<DataTable> GetBudgetFilterSessions(string? campusCode);
        Task<DataTable> GetBudgetSessionAmountSummary(BudgetSessionAmountSummaryRequest? summaryRequest);
        Task<DataTable> GetBudgetSessionEditableSummaryExists(string? budgetId);
        Task<int> UpdateBudgetSessionSummaryAmount(UpdateBudgetSessionAmountRequest? updateRequest, long? oldBudgetAmount, string? employeeCode);
        Task<DataTable> CheckIsBudgetSessionAmountSummaryExists(string? session, string? campusCode);
        Task<int> AddBudgetSessionSummaryAmount(CreateNewBudgetSessionAmountSummaryRequest? createRequest, string? employeeCode);
        Task<int> DeleteBudgetSessionSummaryAmount(DeleteBudgetSessionSummaryAmountRequest? deleteRequest, string? employeeCode);
        Task<int> LockBudgetSessionSummaryAmount(string? budgetId, string? employeeCode);
        Task<DataTable> GetBudgetMaadListFilter(string Maad);
        Task<DataTable> CheckAlreadyDepartmentBudgetDetails(AddDepartmentSummaryRequest request);
        Task<DataTable> CheckMaadListRecord(string Maad);
    }
}
