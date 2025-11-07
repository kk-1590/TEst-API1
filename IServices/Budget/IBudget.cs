using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;

namespace AdvanceAPI.IServices.Budget
{
    public interface IBudget
    {
        Task<ApiResponse> AddItemWithSession(string EmpCode, MapNewMaad mapNewMaad);
        Task<ApiResponse> UpdateBudgetMaad(string EmpCode, UpdateMaadBudegtRequest updateMaadBudegtRequest);
        Task<ApiResponse> GetRecord(int Limit, int Offset);
    }
}
