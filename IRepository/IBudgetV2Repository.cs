using AdvanceAPI.DTO.Budget;
using System.Data;
using System.Threading.Tasks;

namespace AdvanceAPI.IRepository
{
    public interface IBudgetV2Repository
    {
        Task<DataTable> GetBudgetFilterSessions(string? campusCode);
    }
}
