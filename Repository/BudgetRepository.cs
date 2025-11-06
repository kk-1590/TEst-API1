using AdvanceAPI.DTO.Budget;
using AdvanceAPI.DTO.DB;
using Microsoft.Extensions.Logging;

namespace AdvanceAPI.Repository
{
    public class BudgetRepository
    {
        private ILogger<BudgetRepository> _logger;
        public BudgetRepository(ILogger<BudgetRepository> logger) 
        {
            _logger = logger;
        }

        public async Task<int> AddDetails(string EmpCode, MapNewMaad mapNewMaad)
        {
            try
            {
                //@Session,@CampusCode,@Maad,@IsBudgetRequired,@AddedBy,NOW()
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Session",""));
                sqlParameters.Add(new SQLParameters("@CampusCode", ""));
                sqlParameters.Add(new SQLParameters("@Maad", mapNewMaad.Maad));
                sqlParameters.Add(new SQLParameters("@IsBudgetRequired", mapNewMaad.BusgetRequired));
                sqlParameters.Add(new SQLParameters("@AddedBy", EmpCode));


                return 0;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Error During... AddDetails");
                throw;
            }
        }
    }
}
