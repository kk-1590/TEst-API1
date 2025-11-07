using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.SQLConstants.Budget;
using Microsoft.Extensions.Logging;
using System.Data;

namespace AdvanceAPI.Repository
{
    public class BudgetRepository : IBudgetRepository
    {
        private ILogger<BudgetRepository> _logger;
        private readonly IGeneral _igeneral;
        private readonly IDBOperations _dbOperations;
        public BudgetRepository(ILogger<BudgetRepository> logger,IGeneral general,IDBOperations dBOperations) 
        {
            _logger = logger;
            _igeneral = general;
            _dbOperations = dBOperations;
        }

        public async Task<DataTable> CheckAlreadyAdded(MapNewMaad mapNewMaad)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Session", _igeneral.GetFinancialSession(DateTime.Now)));
                sqlParameters.Add(new SQLParameters("@Maad", mapNewMaad.Maad!));
                sqlParameters.Add(new SQLParameters("@CampusCode", mapNewMaad.CampusCode));
                return await _dbOperations.SelectAsync(BudgetSql.CHECK_ALREADY_MAAD, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error During CheckAlreadyAdded");
                throw;
            }
        }

        public async Task<int> AddDetails(string EmpCode, MapNewMaad mapNewMaad)
        {
            try
            {
                //@Session,@CampusCode,@Maad,@IsBudgetRequired,@AddedBy,NOW()
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Session",_igeneral.GetFinancialSession(DateTime.Now)));
                sqlParameters.Add(new SQLParameters("@CampusCode", mapNewMaad.CampusCode));
                sqlParameters.Add(new SQLParameters("@Maad", mapNewMaad.Maad!));
                sqlParameters.Add(new SQLParameters("@IsBudgetRequired", mapNewMaad.BusgetRequired));
                sqlParameters.Add(new SQLParameters("@AddedBy", EmpCode));
                sqlParameters.Add(new SQLParameters("@AddedFrom", _igeneral.GetIpAddress()));
                string q=BudgetSql.INS__QUERY;
                int ins=await _dbOperations.DeleteInsertUpdateAsync(q, sqlParameters,DBConnections.Advance);

                return ins;
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "Error During... AddDetails");
                throw;
            }
        }


        public async Task<int> UpdateMaadForBudget(string EmpCode, UpdateMaadBudegtRequest updateMaadBudegtRequest)
        {
           
            List<SQLParameters> sQLParameters = new List<SQLParameters>();
            sQLParameters.Add(new SQLParameters("@Status",updateMaadBudegtRequest.Status));
            sQLParameters.Add(new SQLParameters("@Id",updateMaadBudegtRequest.Id));
            sQLParameters.Add(new SQLParameters("@IsBudgetRequired", updateMaadBudegtRequest.BudgetRequired));
            sQLParameters.Add(new SQLParameters("@UpdatedBy", EmpCode));
            sQLParameters.Add(new SQLParameters("@UpdatedFrom", _igeneral.GetIpAddress()));
            sQLParameters.Add(new SQLParameters("@Session", updateMaadBudegtRequest.Session!));
            sQLParameters.Add(new SQLParameters("@Maap", updateMaadBudegtRequest.Maad!));
            sQLParameters.Add(new SQLParameters("@CampusCode", updateMaadBudegtRequest.CampusCode));
            sQLParameters.Add(new SQLParameters("@UpdateRemark", updateMaadBudegtRequest.Remark!));
            try
            {
                int ins = await _dbOperations.DeleteInsertUpdateAsync(BudgetSql.UPDATE_MAAD_BUDGET_MAPPING, sQLParameters, DBConnections.Advance);
                return ins;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateMaadForBudget");
                throw;
            }

        }

        public async Task<DataTable> GetMaadBudgetDetails(int Limit, int OffSet, string CampusCode, string Session,int BudgetRequired)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Limit", Limit));
                sqlParameters.Add(new SQLParameters("@OffSet", OffSet));
                sqlParameters.Add(new SQLParameters("@CampusCode", CampusCode));
                sqlParameters.Add(new SQLParameters("@Session",Session));
                string Cond = "";
                if (BudgetRequired>=0) 
                {
                    Cond += " And IsBudgetRequired=@IsBudgetRequired";
                    sqlParameters.Add(new SQLParameters("@IsBudgetRequired", BudgetRequired));
                }
                return await _dbOperations.SelectAsync(BudgetSql.GET_BUDGET_MAAD.Replace("@Condition",Cond),sqlParameters,DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error: {ex}", "Error During GetMaadBudgetDetails");
                throw;
            }
        }

    }
}
