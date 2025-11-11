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
        private readonly IGeneral _general;
        private readonly IDBOperations _dbOperations;
        public BudgetV2Repository(ILogger<BudgetV2Repository> logger, IGeneral general, IDBOperations dBOperations)
        {
            _logger = logger;
            _general = general;
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
        public async Task<DataTable> GetBudgetSessionEditableSummaryExists(string? budgetId)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>(1)
                {
                    new SQLParameters("@BudgetId", budgetId ?? string.Empty)
                };

                return await _dbOperations.SelectAsync(BudgetV2Sql.GET_BUDGET_SESSION_EDITABLE_SUMMARY_EXISTS, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBudgetSessionEditableSummaryExists");
                throw;
            }
        }

        public async Task<int> UpdateBudgetSessionSummaryAmount(UpdateBudgetSessionAmountRequest? updateRequest, long? oldBudgetAmount, string? employeeCode)
        {
            try
            {
                string updateRemark = $"Budget Amount Changed from {oldBudgetAmount} to {updateRequest?.BudgetAmount} By: {employeeCode} On: {DateTime.Now.ToString("F")} From: {_general.GetIpAddress()} With Reason: {updateRequest?.Reason}";

                List<SQLParameters> sqlParameters = new List<SQLParameters>()
                {
                    new SQLParameters("@BudgetAmount", updateRequest?.BudgetAmount ?? 0),
                    new SQLParameters("@UpdatedBy", employeeCode ?? string.Empty),
                    new SQLParameters("@UpdatedFrom", _general.GetIpAddress() ?? string.Empty),
                    new SQLParameters("@UpdateRemark", updateRemark ?? string.Empty),
                    new SQLParameters("@BudgetId", updateRequest?.BudgetId ?? string.Empty),
                };

                await _dbOperations.DeleteInsertUpdateAsync(BudgetV2Sql.UPDATE_BUDGET_SESSION_SUMMARY_AMOUNT, sqlParameters, DBConnections.Advance);

                sqlParameters = new List<SQLParameters>()
                {
                    new SQLParameters("@BudgetId", updateRequest?.BudgetId ?? string.Empty)
                };

                return await _dbOperations.DeleteInsertUpdateAsync(BudgetV2Sql.UPDATE_BUDGET_SESSION_SUMMARY_AMOUNT_CALCULATIONS, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateBudgetSessionSummaryAmount");
                throw;
            }
        }

        public async Task<DataTable> CheckIsBudgetSessionAmountSummaryExists(string? session, string? campusCode)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>(2)
                {
                    new SQLParameters("@Session", session ?? string.Empty),
                    new SQLParameters("@CampusCode", campusCode ?? string.Empty),
                };

                return await _dbOperations.SelectAsync(BudgetV2Sql.GET_BUDGET_SESSION_SUMMARY_EXISTS, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckIsBudgetSessionAMountSummaryExists");
                throw;
            }
        }

        public async Task<int> AddBudgetSessionSummaryAmount(CreateNewBudgetSessionAmountSummaryRequest? createRequest, string? employeeCode)
        {
            try
            {

                List<SQLParameters> sqlParameters = new List<SQLParameters>()
                {
                    new SQLParameters("@Session", createRequest?.Session ?? string.Empty),
                    new SQLParameters("@CampusCode", createRequest?.CampusCode ?? string.Empty),
                    new SQLParameters("@BudgetAmount", createRequest?.BudgetAmount ?? 0),
                    new SQLParameters("@AddedBy", employeeCode ?? string.Empty),
                    new SQLParameters("@AddedFrom", _general.GetIpAddress() ?? string.Empty),
                };

                return await _dbOperations.DeleteInsertUpdateAsync(BudgetV2Sql.ADD_BUDGET_SESSION_SUMMARY_AMOUNT, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During AddBudgetSessionSummaryAmount");
                throw;
            }
        }

        public async Task<int> DeleteBudgetSessionSummaryAmount(DeleteBudgetSessionSummaryAmountRequest? deleteRequest, string? employeeCode)
        {
            try
            {
                string updateRemark = $"Budget Deleted By: {employeeCode} On: {DateTime.Now.ToString("F")} From: {_general.GetIpAddress()} With Reason: {deleteRequest?.Reason}";

                List<SQLParameters> sqlParameters = new List<SQLParameters>()
                {
                    new SQLParameters("@DeletedBy", employeeCode ?? string.Empty),
                    new SQLParameters("@DeletedFrom", _general.GetIpAddress() ?? string.Empty),
                    new SQLParameters("@UpdateRemark", updateRemark ?? string.Empty),
                    new SQLParameters("@BudgetId", deleteRequest?.BudgetId ?? string.Empty),
                };

                return await _dbOperations.DeleteInsertUpdateAsync(BudgetV2Sql.DELETE_BUDGET_SESSION_SUMMARY_AMOUNT, sqlParameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During DeleteBudgetSessionSummaryAmount");
                throw;
            }
        }
        public async Task<int> LockBudgetSessionSummaryAmount(string? budgetId, string? employeeCode)
        {
            try
            {
                string updateRemark = $"Budget Locked By: {employeeCode} On: {DateTime.Now.ToString("F")} From: {_general.GetIpAddress()}";

                List<SQLParameters> sqlParameters = new List<SQLParameters>()
                {
                    new SQLParameters("@UpdatedBy", employeeCode ?? string.Empty),
                    new SQLParameters("@UpdatedFrom", _general.GetIpAddress() ?? string.Empty),
                    new SQLParameters("@UpdateRemark", updateRemark ?? string.Empty),
                    new SQLParameters("@BudgetId", budgetId ?? string.Empty),
                };

                return await _dbOperations.DeleteInsertUpdateAsync(BudgetV2Sql.LOCK_BUDGET_SESSION_SUMMARY_AMOUNT, sqlParameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During LockBudgetSessionSummaryAmount");
                throw;
            }
        }

        public async Task<DataTable> GetBudgetMaadListFilter(string Maad)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Maad", Maad));

                return await _dbOperations.SelectAsync(BudgetV2Sql.GET_BUDGET_MAAD_FILTER, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBudgetMaadListFilter");
                throw;
            }

        }
        public async Task<DataTable> CheckAlreadyDepartmentBudgetDetails(AddDepartmentSummaryRequest request)
        {
            try
            {
                //ReferenceNo=@RefNo AND BudgetHead=@BudgetHead AND BudgetMaad=@BudgetMaad AND BudgetType=@BudgetType
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", request.ReferenceNo ?? string.Empty));
                param.Add(new SQLParameters("@BudgetHead", request.BudgetHead ?? string.Empty));
                param.Add(new SQLParameters("@BudgetMaad", request.BudgetMaad ?? string.Empty));
                param.Add(new SQLParameters("@BudgetType", request.BudgetType ?? string.Empty));
                return await _dbOperations.SelectAsync(BudgetV2Sql.CHECK_ALREADY_ADDED_DEPARTMENT_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckAlreadyDepartmentBudgetDetails");
                throw;
            }
        }
        public async Task<DataTable> CheckMaadListRecord(string Maad)
        {
            try
            {
                List<SQLParameters> sQLParameters = new List<SQLParameters>();
                sQLParameters.Add(new SQLParameters("@BudgetMaad", Maad));
                return await _dbOperations.SelectAsync(BudgetV2Sql.GET_ALREADY_ADDED_MAAD, sQLParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckMaadListRecord");
                throw;
            }
        }


        public async Task<int> AddMaadInList()
        {

            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During AddMaadInList");
                throw;
            }
        }
    }
}
