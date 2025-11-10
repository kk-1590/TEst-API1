using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.SQLConstants.Budget;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using System.Data;
using System.Text;

namespace AdvanceAPI.Repository
{
    public class BudgetV2Repository : IBudgetV2Repository
    {
        private ILogger<BudgetV2Repository> _logger;
        private readonly IGeneral _igeneral;
        private readonly IDBOperations _dbOperations;
        public BudgetV2Repository(ILogger<BudgetV2Repository> logger, IGeneral general, IDBOperations dBOperations)
        {
            _logger = logger;
            _igeneral = general;
            _dbOperations = dBOperations;
        }

        public async Task<DataTable> GetBudgetFilterSessions(string? campusCode)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@CampusCode", campusCode ?? string.Empty));
                return await _dbOperations.SelectAsync(BudgetV2Sql.GET_BUDGET_SESSIONS_FOR_FILTER, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBudgetFilterSessions");
                throw;
            }
        }

        public async Task<DataTable> GetBudgetSessionAmountSummary(BudgetSessionAmountSummaryRequest? summaryRequest)
        {
            try
            {
                StringBuilder changeCondition = new StringBuilder("");
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                if (!string.IsNullOrEmpty(summaryRequest?.CampusCode))
                {
                    changeCondition.Append(" AND A.CampusCode=@CampusCode ");
                    sqlParameters.Add(new SQLParameters("@CampusCode", summaryRequest?.CampusCode ?? string.Empty));
                }
                if (!string.IsNullOrEmpty(summaryRequest?.Session))
                {
                    changeCondition.Append(" AND A.Session=@Session ");
                    sqlParameters.Add(new SQLParameters("@Session", summaryRequest?.Session ?? string.Empty));
                }

                sqlParameters.Add(new SQLParameters("@OffSetItems", summaryRequest?.RecordFrom ?? 0));
                sqlParameters.Add(new SQLParameters("@LimitItems", summaryRequest?.NoOfRecords ?? 0));


                string query = BudgetV2Sql.GET_BUDGET_SESSIONS_AMOUNT_SUMMARY.Replace("@ChangeCondition", changeCondition.ToString());

                return await _dbOperations.SelectAsync(query, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBudgetSessionAmountSummary");
                throw;
            }
        }
    }
}
