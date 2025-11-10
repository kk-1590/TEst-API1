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
        public async Task<DataTable> GetRequiredBudget(string CampusCode,string Session)
        {
            try
            {
                List<SQLParameters> sQLParameters = new List<SQLParameters>();
                sQLParameters.Add(new SQLParameters("@CampusCode",CampusCode));
                sQLParameters.Add(new SQLParameters("@Session",Session));
                return await _dbOperations.SelectAsync(BudgetSql.GET_REQUIRED_BUDGET_MAAD,sQLParameters,DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error during GetNonRequiredBudget... ");
                throw;
            }
        }
        public async Task<DataTable> GetNonRequiredBudget(string CampusCode,string Session)
        {
            try
            {
                List<SQLParameters> sQLParameters = new List<SQLParameters>();
                sQLParameters.Add(new SQLParameters("@CampusCode",CampusCode));
                sQLParameters.Add(new SQLParameters("@Session",Session));
                return await _dbOperations.SelectAsync(BudgetSql.GET_NON_REQUIRED_BUDGET_MAAD,sQLParameters,DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error during GetNonRequiredBudget... ");
                throw;
            }
        }
        public async Task<DataTable> GetMaadAddedDetails(string RefNo)
        {
            try
            {
                List<SQLParameters> sQLParameters1 = new List<SQLParameters>();
                sQLParameters1.Add(new SQLParameters ("@RefNo",RefNo));
                return await _dbOperations.SelectAsync(BudgetSql.GET_ADDED_MAAD, sQLParameters1, DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error during GetMaadAddedDetails..");
                throw;
            }
        
        }
        public async Task<int> UpdateBudgetDetails(string EmpCode,UpdateBudgetDetails updateBudgetDetails)
        {
            try
            {
                List<SQLParameters> sQLParameters = new List<SQLParameters>();
                sQLParameters.Add(new SQLParameters("@BudgetAmount", updateBudgetDetails.BudgetAmount!));
                sQLParameters.Add(new SQLParameters("@IpAddress", _igeneral.GetIpAddress()));
                sQLParameters.Add(new SQLParameters("@EmpCode", EmpCode));
                sQLParameters.Add(new SQLParameters("@Id", updateBudgetDetails.Id));
                sQLParameters.Add(new SQLParameters("@RefNo", updateBudgetDetails.RefrenceNo!));
                return await _dbOperations.DeleteInsertUpdateAsync(BudgetSql.UPDATE_BUDGET_DETAILS, sQLParameters,DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error During UpdateBudgetDetails");
                throw;
            }
        }
        public async Task<int> DetailsBudgetDetails(string EmpCode,UpdateBudgetDetails updateBudgetDetails)
        {
            try
            {
                List<SQLParameters> sQLParameters = new List<SQLParameters>();
                sQLParameters.Add(new SQLParameters("@BudgetAmount", updateBudgetDetails.BudgetAmount!));
                sQLParameters.Add(new SQLParameters("@IpAddress", _igeneral.GetIpAddress()));
                sQLParameters.Add(new SQLParameters("@EmpCode", EmpCode));
                sQLParameters.Add(new SQLParameters("@Id", updateBudgetDetails.Id));
                sQLParameters.Add(new SQLParameters("@RefNo", updateBudgetDetails.RefrenceNo!));
                return await _dbOperations.DeleteInsertUpdateAsync(BudgetSql.UPDATE_BUDGET_DETAILS, sQLParameters,DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error During UpdateBudgetDetails");
                throw;
            }
        }
        public async Task<int> DeleteDetails(UpdateBudgetDetails updateBudgetDetails)
        {
            try
            {
                List<SQLParameters> sQLParameters =new List<SQLParameters>();
                sQLParameters.Add(new SQLParameters("@Id", updateBudgetDetails.Id));
                sQLParameters.Add(new SQLParameters("@RefNo", updateBudgetDetails.RefrenceNo!));
                return await _dbOperations.DeleteInsertUpdateAsync(BudgetSql.DELETE_BUDGET_DETAILS,sQLParameters,DBConnections.Advance);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error During DeleteDetails");
                throw;
            }
        }
        public async Task<DataTable> CheckAllreadyAddedDetails(List<AddBudgetDetailsRequest> add)
        {
            try
            {
                List<SQLParameters> sQLParameters1 =new List<SQLParameters>();
                sQLParameters1.Add(new SQLParameters("@RefNo", add[0].ReferenceNo!));
                string MaadStr =string.Join(",", add.AsEnumerable().Select(x =>"'"+ x.Maad+"'").ToList());
                return await _dbOperations.SelectAsync(BudgetSql.CHECK_ALREADY_ADDED_ITEM.Replace("@Maad",MaadStr), sQLParameters1,DBConnections.Advance);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error During CheckAllreadyAddedDetails");
                throw;
            }
        }

        public async Task<DataTable> CheckAllDepartmentBudgetAllowed(string? employeeCode)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>(1);
                sqlParameters.Add(new SQLParameters("@EmployeeId", employeeCode ?? string.Empty));

                return await _dbOperations.SelectAsync(BudgetSql.CHECK_ALL_DEPARTMENT_BUDGET_ALLOWED, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckAllDepartmentBudgetAllowed");
                throw;
            }
        }

        public async Task<DataTable> GetBudgetDepartments(string? employeeCode, string? campusCode, bool allDepartmentsAllowed)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>(1);
                sqlParameters.Add(new SQLParameters("@CampusCode", campusCode ?? string.Empty));
                if (!allDepartmentsAllowed)
                {
                    sqlParameters.Add(new SQLParameters("@EmployeeId", employeeCode ?? string.Empty));
                }

                return await _dbOperations.SelectAsync(allDepartmentsAllowed ? BudgetSql.GET_ALL_BUGDET_DEPARTMENT : BudgetSql.GET_ALLOWED_BUGDET_DEPARTMENT, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBudgetDepartments");
                throw;
            }
        }

        public async Task<DataTable> CheckIsDepartmentBudgetSummaryExists(CreateDepartmentBudgetSummaryRequest? budgetRequest)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Session", budgetRequest?.Session ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@CampusCode", budgetRequest?.CampusCode ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@Department", budgetRequest?.Department ?? string.Empty));

                return await _dbOperations.SelectAsync(BudgetSql.CHECK_IS_DEPARTMENT_BUDGET_SUMMARY_EXISTS, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckIsDepartmentBudgetSummaryExists");
                throw;
            }
        }

        public async Task<DataTable> CheckIsValidDepartmentBudgetDepartment(CreateDepartmentBudgetSummaryRequest? budgetRequest)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@CampusCode", budgetRequest?.CampusCode ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@Department", budgetRequest?.Department ?? string.Empty));

                return await _dbOperations.SelectAsync(BudgetSql.GET_IS_VALID_BUGDET_DEPARTMENT, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckIsValidDepartmentBudgetDepartment");
                throw;
            }
        }

        public async Task<DataTable> GetNewDepartmentBudgetSummaryReferenceNo()
        {
            try
            {
                return await _dbOperations.SelectAsync(BudgetSql.GET_NEW_DEPARTMENT_BUDGET_SUMMARY_REFERENCE_NO, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetNewDepartmentBudgetSummaryReferenceNo");
                throw;
            }
        }
        public async Task<int> CreateNewBudgetSummary(string? employeeId, string? referenceNo, string? ipAddress, CreateDepartmentBudgetSummaryRequest? budgetRequest)
        {
            try
            {

                //@ReferenceNo,@Session,@CampusCode,@Department,@BudgetName,@BudgetAmount,@InitiatedBy,NOW(),@InitiatedFrom

                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@Session", budgetRequest?.Session ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@CampusCode", budgetRequest?.CampusCode ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@Department", budgetRequest?.Department ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@BudgetName", budgetRequest?.BudgetName ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@BudgetAmount", budgetRequest?.BudgetAmount ?? 0));
                sqlParameters.Add(new SQLParameters("@InitiatedBy", employeeId ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@InitiatedFrom", ipAddress ?? string.Empty));

                return await _dbOperations.DeleteInsertUpdateAsync(BudgetSql.CREATE_NEW_BUDGET_SUMMARY, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CreateNewBudgetSummary");
                throw;
            }
        }

        public async Task<DataTable> GetDepartmentBudgetSummary(string? employeeId, bool allDepartmentAllowed, GetDepartmentBudgetSummaryRequest? budgetRequest)
        {
            try
            {
                string extraCondition = string.Empty;
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Session", budgetRequest?.Session ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@CampusCode", budgetRequest?.CampusCode ?? string.Empty));

                if (!allDepartmentAllowed)
                {
                    sqlParameters.Add(new SQLParameters("@EmployeeId", employeeId ?? string.Empty));
                }

                if (!string.IsNullOrWhiteSpace(budgetRequest?.Department))
                {
                    extraCondition += " AND A.Department=@Department ";
                    sqlParameters.Add(new SQLParameters("@Department", budgetRequest?.Department ?? string.Empty));
                }

                sqlParameters.Add(new SQLParameters("@LimitItems", budgetRequest?.NoOfRecords ?? 0));
                sqlParameters.Add(new SQLParameters("@OffSetItems", budgetRequest?.RecordFrom ?? 0));

                string query = allDepartmentAllowed ? BudgetSql.GET_DEPARTMENT_BUDGET_SUMMARY_ALL : BudgetSql.GET_DEPARTMENT_BUDGET_SUMMARY_RESTRICTED;
                query = query.Replace("@AdditionalCondition", extraCondition);

                return await _dbOperations.SelectAsync(query, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetNewDepartmentBudgetSummaryReferenceNo");
                throw;
            }
        }

    }
}
