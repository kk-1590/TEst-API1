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
using System.Security.Cryptography.Xml;
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

                return await _dbOperations.SelectAsync(BudgetV2Sql.GET_BUDGET_MAAD_FILTER.Replace("@Maad",Maad), sqlParameters, DBConnections.Advance);
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
        public async Task<DataTable> GetAllSessionsOfBudgetSessionSummary()
        {
            try
            {
                return await _dbOperations.SelectAsync(BudgetV2Sql.GET_ALL_BUDGET_SESSION_SUMMARY_AMOUNT_SESSIONS, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAllSessionsOfBudgetSessionSummary");
                throw;
            }
        }
        public async Task<int> AddMaadInList(string EmpCode, string Maad)
        {

            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                //@Maad,NOW(),@IpAddress,@AddBy
                param.Add(new SQLParameters("@Maad", Maad));
                param.Add(new SQLParameters("@IpAddress", _general.GetIpAddress()));
                param.Add(new SQLParameters("@AddBy", EmpCode));

                return await _dbOperations.DeleteInsertUpdateAsync(BudgetV2Sql.ADD_MAAD_IN_LIST, param, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During AddMaadInList");
                throw;
            }
        }
        public async Task<int> AddDepartmentList(string EmpCode, AddDepartmentSummaryRequest request)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                // (@ReferenceNo, @BudgetType, @BudgetHead, @BudgetMaad, @BudgetAmount, @AllowOverBudgetApproval, NOW(), @AddedFrom, @AddedBy)
                param.Add(new SQLParameters("@ReferenceNo", request.ReferenceNo ?? string.Empty));
                param.Add(new SQLParameters("@BudgetType", request.BudgetType ?? string.Empty));
                param.Add(new SQLParameters("@BudgetHead", request.BudgetHead ?? string.Empty));
                param.Add(new SQLParameters("@BudgetMaad", request.BudgetMaad ?? string.Empty));
                param.Add(new SQLParameters("@BudgetAmount", request.BudgetAmount.ToString() ?? string.Empty));
                param.Add(new SQLParameters("@AllowOverBudgetApproval", request.AllowOverBudgetApproval));
                param.Add(new SQLParameters("@AddedFrom", _general.GetIpAddress()));
                param.Add(new SQLParameters("@AddedBy", EmpCode));
                return await _dbOperations.DeleteInsertUpdateAsync(BudgetV2Sql.ADD_DEPARTMENT_BUDGET_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During AddDepartmentList");
                throw;
            }
        }
        public async Task<DataTable> GetDepartmentDetails(string RefNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.SelectAsync(BudgetV2Sql.GET_DEPARMENT_BUDGET_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetDepartmentDetails");
                throw;
            }
        }
        public async Task<int> UpdateDepartmentDetails(string EmpCode, AddDepartmentSummaryUpdateRequest request)
        {
            //UPDATE department_budget_details SET BudgetType=@BudgetType,BudgetHead=@BudgetHead,BudgetMaad=@BudgetMaad,BudgetAmount=@BudgetAmount,AllowOverBudgetApproval=@AllowOverBudgetApproval,UpdatedBy=@UpdateBy,UpdatedOn=NOW(),UpdatedFrom=@IP WHERE Id=@Id
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@BudgetType", request.BudgetType ?? string.Empty));
                param.Add(new SQLParameters("@BudgetHead", request.BudgetHead ?? string.Empty));
                param.Add(new SQLParameters("@BudgetMaad", request.BudgetMaad ?? string.Empty));
                param.Add(new SQLParameters("@BudgetAmount", request.BudgetAmount.ToString() ?? string.Empty));
                param.Add(new SQLParameters("@AllowOverBudgetApproval", request.AllowOverBudgetApproval));
                param.Add(new SQLParameters("@IP", _general.GetIpAddress()));
                param.Add(new SQLParameters("@UpdateBy", EmpCode));
                param.Add(new SQLParameters("@Id", request.Id));
                return await _dbOperations.DeleteInsertUpdateAsync(BudgetV2Sql.UPDATE_DEPARTMENT_BUDGET_DETAILS, param, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateDepartmentDetails");
                throw;
            }
        }

        public async Task<DataTable> GetBudgetTypeHeadMapping(BudgetHeadMappingRequest? mappingRequest)
        {
            try
            {
                string extraCondition = "";

                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Session", mappingRequest?.Session ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@CampusCode", mappingRequest?.CampusCode ?? string.Empty));

                if (!string.IsNullOrEmpty(mappingRequest?.BudgetType))
                {
                    extraCondition = " AND A.BudgetType=@BudgetType ";
                    sqlParameters.Add(new SQLParameters("@BudgetType", mappingRequest?.BudgetType ?? string.Empty));
                }

                sqlParameters.Add(new SQLParameters("@OffSetItems", mappingRequest?.RecordFrom ?? 0));
                sqlParameters.Add(new SQLParameters("@LimitItems", mappingRequest?.NoOfRecords ?? 0));

                string query = BudgetV2Sql.GET_BUDGET_TYPE_HEAD_MAPPING.Replace("@ExtraCondition", extraCondition);

                return await _dbOperations.SelectAsync(query, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBudgetTypeHeadMapping");
                throw;
            }
        }

        public async Task<DataTable> CheckBudgetTypeHeadMappingAlreadyExists(AddBudgetTypeHeadRequest? addRequest)
        {
            try
            {

                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Session", addRequest?.Session ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@CampusCode", addRequest?.CampusCode ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@BudgetType", addRequest?.BudgetType ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@BudgetHead", addRequest?.BudgetHead ?? string.Empty));


                return await _dbOperations.SelectAsync(BudgetV2Sql.CHECK_BUDGET_TYPE_HEAD_MAPPING_ALREADY_EXISTS, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckBudgetTypeHeadMappingAlreadyExists");
                throw;
            }
        }

        public async Task<DataTable> AddBudgetTypeHeadMappingAlreadyExists(AddBudgetTypeHeadRequest? addRequest, string? employeeId)
        {
            try
            {

                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Session", addRequest?.Session ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@CampusCode", addRequest?.CampusCode ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@BudgetType", addRequest?.BudgetType ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@BudgetHead", addRequest?.BudgetHead ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@AddedBy", employeeId ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@AddedFrom", _general.GetIpAddress() ?? string.Empty));


                return await _dbOperations.SelectAsync(BudgetV2Sql.ADD_BUDGET_TYPE_HEAD_MAPPING, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During AddBudgetTypeHeadMappingAlreadyExists");
                throw;
            }
        }

        public async Task<DataTable> CheckBudgetTypeHeadMappingAlreadyExistsByid(string? headMapId)
        {
            try
            {

                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@HeadMappingId", headMapId ?? string.Empty));

                return await _dbOperations.SelectAsync(BudgetV2Sql.CHECK_BUDGET_TYPE_HEAD_MAPPING_EXISTS_BY_ID, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckBudgetTypeHeadMappingAlreadyExistsByid");
                throw;
            }
        }
        public async Task<DataTable> CheckBudgetTypeHeadMappingUsedOrNOtByid(string? headMapId)
        {
            try
            {

                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@HeadMappingId", headMapId ?? string.Empty));

                return await _dbOperations.SelectAsync(BudgetV2Sql.CHECK_BUDGET_TYPE_HEAD_MAPPING_USED_OR_NOT, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckBudgetTypeHeadMappingUsedOrNOtByid");
                throw;
            }
        }
        public async Task<DataTable> DeleteBudgetTypeHeadMappingAlreadyExists(DeleteBudgetHeadMappingRequest? deleteRequest, string? employeeId)
        {
            try
            {
                string updateRemark = $"Budget Head Mapping Deleted By: {employeeId} On: {DateTime.Now.ToString("F")} From: {_general.GetIpAddress()} With Reason: {deleteRequest?.Reason}";

                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@DeletedBy", employeeId ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@DeletedFrom", _general.GetIpAddress() ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@UpdateRemark", updateRemark ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@HeadMapId", deleteRequest?.HeadMappingId ?? string.Empty));

                return await _dbOperations.SelectAsync(BudgetV2Sql.DELETE_BUDGET_TYPE_HEAD_MAPPING, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckBudgetTypeHeadMappingUsedOrNOtByid");
                throw;
            }
        }

        public async Task<DataTable> CheckAllDepartmentBudgetAllowed(string? employeeCode)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>(1);
                sqlParameters.Add(new SQLParameters("@EmployeeId", employeeCode ?? string.Empty));

                return await _dbOperations.SelectAsync(BudgetV2Sql.CHECK_ALL_DEPARTMENT_BUDGET_ALLOWED, sqlParameters, DBConnections.Advance);
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

                return await _dbOperations.SelectAsync(allDepartmentsAllowed ? BudgetV2Sql.GET_ALL_BUGDET_DEPARTMENT : BudgetV2Sql.GET_ALLOWED_BUGDET_DEPARTMENT, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBudgetDepartments");
                throw;
            }
        }

        public async Task<DataTable> CheckIsDepartmentBudgetSummaryExists(CreateDepartmentBudgetSummaryV2Request? budgetRequest)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Session", budgetRequest?.Session ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@CampusCode", budgetRequest?.CampusCode ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@Department", budgetRequest?.Department ?? string.Empty));

                return await _dbOperations.SelectAsync(BudgetV2Sql.CHECK_IS_DEPARTMENT_BUDGET_SUMMARY_EXISTS, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CheckIsDepartmentBudgetSummaryExists");
                throw;
            }
        }

        public async Task<DataTable> CheckIsValidDepartmentBudgetDepartment(CreateDepartmentBudgetSummaryV2Request? budgetRequest)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@CampusCode", budgetRequest?.CampusCode ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@Department", budgetRequest?.Department ?? string.Empty));

                return await _dbOperations.SelectAsync(BudgetV2Sql.GET_IS_VALID_BUGDET_DEPARTMENT, sqlParameters, DBConnections.Advance);
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
                return await _dbOperations.SelectAsync(BudgetV2Sql.GET_NEW_DEPARTMENT_BUDGET_SUMMARY_REFERENCE_NO, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetNewDepartmentBudgetSummaryReferenceNo");
                throw;
            }
        }
        public async Task<int> CreateNewBudgetSummary(string? employeeId, string? referenceNo, string? ipAddress, CreateDepartmentBudgetSummaryV2Request? budgetRequest)
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
                sqlParameters.Add(new SQLParameters("@InitiatedBy", employeeId ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@InitiatedFrom", ipAddress ?? string.Empty));
                return await _dbOperations.DeleteInsertUpdateAsync(BudgetV2Sql.CREATE_NEW_BUDGET_SUMMARY, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During CreateNewBudgetSummary");
                throw;
            }
        }

        public async Task<DataTable> GetDepartmentBudgetSummary(GetDepartmentBudgetSummaryV2Request? budgetRequest, string? employeeId, bool allDepartmentsAllowed)
        {
            try
            {
                StringBuilder changeCondition = new StringBuilder("");
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Session", budgetRequest?.Session ?? string.Empty));
                sqlParameters.Add(new SQLParameters("@CampusCode", budgetRequest?.CampusCode ?? string.Empty));
                if (!string.IsNullOrWhiteSpace(budgetRequest?.Department))
                {
                    changeCondition.Append(" AND A.Department=@Department ");
                    sqlParameters.Add(new SQLParameters("@Department", budgetRequest?.Department ?? string.Empty));
                }
                if (!string.IsNullOrWhiteSpace(budgetRequest?.ReferenceNo))
                {
                    changeCondition.Append(" AND A.ReferenceNo=@ReferenceNo ");
                    sqlParameters.Add(new SQLParameters("@ReferenceNo", budgetRequest?.ReferenceNo ?? string.Empty));
                }
                if (!allDepartmentsAllowed)
                {
                    changeCondition.Append(" AND ( A.InitiatedBy=@EmployeeId  OR A.Department IN ((SELECT DISTINCT B.Department FROM salary_management.emp_department A, salary_management.departmenthod B WHERE A.`name`=B.Department AND FIND_IN_SET(@CampusCode,A.CampusCodes) AND B.EmployeeCode=@EmployeeId AND B.`Status` ='Activated')))");

                    sqlParameters.Add(new SQLParameters("@EmployeeId", employeeId ?? string.Empty));
                }

                sqlParameters.Add(new SQLParameters("@OffSetItems", budgetRequest?.RecordFrom ?? 0));
                sqlParameters.Add(new SQLParameters("@LimitItems", budgetRequest?.NoOfRecords ?? 0));

                string query = BudgetV2Sql.GET_DEPARTMENT_BUDGET_SUMMARY.Replace("@ChangeCondition", changeCondition.ToString());

                return await _dbOperations.SelectAsync(query, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetDepartmentBudgetSummary");
                throw;
            }
        }



        public async Task<DataTable> ValidDepartmentDetailsForDelete(int id, string RefNo)
        {
            List<SQLParameters> parameters = new List<SQLParameters>();
            try
            {
                parameters.Add(new SQLParameters("@Id", id));
                parameters.Add(new SQLParameters("@refNo", RefNo));
                return await _dbOperations.SelectAsync(BudgetV2Sql.ISVALIDDEPARMENTFORDELETE, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During ValidDepartmentDetailsForDelete");
                throw;
            }

        }
        public async Task<int> deleteDepartment(string EmpCode, int Id)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@DeletedBy", EmpCode));
                parameters.Add(new SQLParameters("@Id", Id));
                parameters.Add(new SQLParameters("@Ip", _general.GetIpAddress()));
                return await _dbOperations.DeleteInsertUpdateAsync(BudgetV2Sql.DELETE_DEPARMENT_BUDGETDETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During deleteDepartment");
                throw;
            }
        }

        public async Task<DataTable> GetDepartMentDetails(string RefNo)
        {
            try
            {
                List<SQLParameters> param= new List<SQLParameters>();
                param.Add(new SQLParameters("@ReferenceNo", RefNo));
                return await _dbOperations.SelectAsync(BudgetV2Sql.GET_BUDGETDETAILS,param,DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error During GetDepartMentDetails");
                throw;
            }
        }
        public async Task<int> UpdateBudgetSummary(string RefNo,int BudgetAmount,int RecurringAmount,int NonRecurringAmount)
        {
            try
            {
                //UPDATE department_budget_summary SET BudgetAmount=@BudgetAmount,RecurringBudgetAmount=@RecurringBudgetAmount,NonRecurringBudgetAmount=@NonRecurringBudgetAmount,BudgetAmountRemaining=@BudgetAmountRemaining,RecurringBudgetAmountRemaining=@RecurringBudgetAmountRemaining,NonRecurringBudgetAmountRemaining=@NonRecurringBudgetAmountRemaining,BudgetStatus='Created' WHERE ReferenceNo=@ReferenceNo AND BudgetStatus='Initiated' AND `Status`='Active'
                List<SQLParameters> lst= new List<SQLParameters>();
                lst.Add(new SQLParameters("@BudgetAmount",BudgetAmount));
                lst.Add(new SQLParameters("@RecurringBudgetAmount", RecurringAmount));
                lst.Add(new SQLParameters("@NonRecurringBudgetAmount", NonRecurringAmount));
                lst.Add(new SQLParameters("@NonRecurringBudgetAmount", NonRecurringAmount));
                lst.Add(new SQLParameters("@BudgetAmountRemaining", BudgetAmount));
                lst.Add(new SQLParameters("@RecurringBudgetAmountRemaining", RecurringAmount));
                lst.Add(new SQLParameters("@NonRecurringBudgetAmountRemaining", NonRecurringAmount));
                lst.Add(new SQLParameters("@ReferenceNo", RefNo));
                return await _dbOperations.DeleteInsertUpdateAsync(BudgetV2Sql.UPDATE_BUDGET_SUMMARY,lst,DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateBudgetSummary");
                throw;
            }
        }
       

        public async Task<int> UpdateDetails(string RefNo,string EmpCode)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@ReferenceNo", RefNo));
                parameters.Add(new SQLParameters("@EmpCode", EmpCode));
                parameters.Add(new SQLParameters("@Ip", _general.GetIpAddress()));
                return await _dbOperations.DeleteInsertUpdateAsync(BudgetV2Sql.UPDATE_BUDGET_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error During UpdateDetails");
                throw;
            }

        }
        public async Task<DataTable> ValidDetails(string RefNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@ReferenceNo", RefNo));
                return await _dbOperations.SelectAsync(BudgetV2Sql.IsValidDetails,parameters,DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During ValidDetails");
                throw;
            }
        }

        public async Task<DataTable> GetHeadDetails(string Type)
        {
            List<SQLParameters> lst = new List<SQLParameters>();
            lst.Add(new SQLParameters("@Type", Type));
            try
            {
                return await _dbOperations.SelectAsync(BudgetV2Sql.GETBUDGETHEADFILTER, lst, DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error During GetHeadDetails");
                throw;
            }
        }
    }
}
