using AdvanceAPI.DTO.Budget;
using System.Data;

namespace AdvanceAPI.IRepository
{
    public interface IBudgetRepository
    {
        Task<int> AddDetails(string EmpCode, MapNewMaad mapNewMaad);
        Task<DataTable> CheckAlreadyAdded(MapNewMaad mapNewMaad);

        Task<int> UpdateMaadForBudget(string EmpCode, UpdateMaadBudegtRequest updateMaadBudegtRequest);
        Task<DataTable> GetMaadBudgetDetails(int Limit, int OffSet, string Campus, string Session, int BudgetRequired);
        Task<DataTable> GetRequiredBudget(string CampusCode, string Session);
        Task<DataTable> GetNonRequiredBudget(string CampusCode, string Session);
        Task<DataTable> GetMaadAddedDetails(string RefNo);
        Task<int> UpdateBudgetDetails(string EmpCode, UpdateBudgetDetails updateBudgetDetails);
        Task<int> DeleteDetails(UpdateBudgetDetails updateBudgetDetails);
        Task<DataTable> CheckAllreadyAddedDetails(List<AddBudgetDetailsRequest> add);
        Task<DataTable> CheckAllDepartmentBudgetAllowed(string? employeeCode);
        Task<DataTable> GetBudgetDepartments(string? employeeCode, string? campusCode, bool allDepartmentsAllowed);
        Task<DataTable> CheckIsValidDepartmentBudgetDepartment(CreateDepartmentBudgetSummaryRequest? budgetRequest);
        Task<DataTable> CheckIsDepartmentBudgetSummaryExists(CreateDepartmentBudgetSummaryRequest? budgetRequest);
        Task<DataTable> GetNewDepartmentBudgetSummaryReferenceNo();
        Task<int> CreateNewBudgetSummary(string? employeeId, string? referenceNo, string? ipAddress, CreateDepartmentBudgetSummaryRequest? budgetRequest);
        Task<DataTable> GetDepartmentBudgetSummary(string? employeeId, bool allDepartmentAllowed, GetDepartmentBudgetSummaryRequest? budgetRequest);
    }
}
