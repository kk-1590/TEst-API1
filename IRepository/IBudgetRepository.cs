using AdvanceAPI.DTO.Budget;
using System.Data;

namespace AdvanceAPI.IRepository
{
    public interface IBudgetRepository
    {
        Task<int> AddDetails(string EmpCode, MapNewMaad mapNewMaad);
        Task<DataTable> CheckAlreadyAdded(MapNewMaad mapNewMaad);

        Task<int> UpdateMaadForBudget(string EmpCode, UpdateMaadBudegtRequest updateMaadBudegtRequest);
        Task<DataTable> GetMaadBudgetDetails(int Limit, int OffSet,string Campus,string Session, int BudgetRequired);
    }
}
