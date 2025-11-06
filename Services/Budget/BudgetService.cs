using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;
using AdvanceAPI.IServices.Budget;

namespace AdvanceAPI.Services.Budget
{
    public class BudgetService : IBudget
    {


        public BudgetService() { }

        public async Task<ApiResponse> InsertBudgetMaad(string EmpCode, MapNewMaad mapNewMaad)
        {
           return new ApiResponse(StatusCodes.Status200OK, null);
        }

    }
}
