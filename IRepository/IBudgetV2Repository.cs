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
        Task<int> AddMaadInList(string EmpCode, string Maad);
        Task<int> AddDepartmentList(string EmpCode, AddDepartmentSummaryRequest request);
        Task<DataTable> GetAllSessionsOfBudgetSessionSummary();
        Task<DataTable> GetDepartmentDetails(string RefNo);
        Task<int> UpdateDepartmentDetails(string EmpCode, AddDepartmentSummaryUpdateRequest request);

        Task<DataTable> GetBudgetTypeHeadMapping(BudgetHeadMappingRequest? mappingRequest);
        Task<DataTable> CheckBudgetTypeHeadMappingAlreadyExists(AddBudgetTypeHeadRequest? addRequest);
        Task<DataTable> AddBudgetTypeHeadMappingAlreadyExists(AddBudgetTypeHeadRequest? addRequest, string? employeeId);
        Task<DataTable> CheckBudgetTypeHeadMappingAlreadyExistsByid(string? headMapId);
        Task<DataTable> CheckBudgetTypeHeadMappingUsedOrNOtByid(string? headMapId);
        Task<DataTable> DeleteBudgetTypeHeadMappingAlreadyExists(DeleteBudgetHeadMappingRequest? deleteRequest, string? employeeId);
    }
}
