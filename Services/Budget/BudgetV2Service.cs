using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Budget;
using System.Data;

namespace AdvanceAPI.Services.Budget
{
    public class BudgetV2Service : IBudgetV2
    {

        private readonly IBudgetV2Repository _budgetRepository;
        private readonly IGeneral _general;
        public BudgetV2Service(IBudgetV2Repository budgetRepository, IGeneral general)
        {
            _budgetRepository = budgetRepository;
            _general = general;
        }

        public async Task<ApiResponse> GetBudgetFilterSessions(string? campus)
        {
            if (string.IsNullOrEmpty(campus))
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Campus Is Required");
            }

            using (DataTable d = await _budgetRepository.GetBudgetFilterSessions(campus))
            {
                List<string> sessions = new List<string>();
                foreach (DataRow session in d.Rows)
                {
                    sessions.Add(session["Session"]?.ToString() ?? string.Empty);
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", sessions);
            }
        }

    }
}
