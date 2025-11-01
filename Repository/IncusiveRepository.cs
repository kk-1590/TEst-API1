using AdvanceAPI.DTO.DB;
using AdvanceAPI.DTO.Inclusive;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.ENUMS.Inclusive;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.Services.Account;
using AdvanceAPI.SQL.Account;
using AdvanceAPI.SQLConstants.Inclusive;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace AdvanceAPI.Repository
{
    public class IncusiveRepository(ILogger<IncusiveRepository> logger, IDBOperations dbContext, IHttpContextAccessor httpContextAccessor) : IIncusiveRepository
    {
        private readonly ILogger<IncusiveRepository> _logger = logger;
        private readonly IDBOperations _dbContext = dbContext;

        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<DataTable> GetEmployeeCampus(string? userId)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@EmployeeCode", userId ?? string.Empty),
                };
                return await _dbContext.SelectAsync(InclusiveSql.GET_ALLOWED_CAMPUS, parameters, DBConnections.Salary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetEmployeeCampus.");
                throw;
            }
        }
        public async Task<DataTable> GetApprovalType()
        {
            try
            {
                return await _dbContext.SelectAsync(InclusiveSql.GET_APPROVAL_TYPE, DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error during GetApprovalType.");
                throw;
            }
        }
        public async Task<DataTable> GetPurchaseDepartment()
        {
            try
            {
                return await _dbContext.SelectAsync(InclusiveSql.Get_Purchase_Department, DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error during GetPurchaseDepartment.");
                throw;
            }
        }
        public async Task<DataTable> GetItems(GetPurchaseItemRequest getPurchaseItemRequest)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder(string.Empty);
                List<SQLParameters> parameters = new List<SQLParameters>();   
                if(getPurchaseItemRequest.DeptCode!="All")
                {
                    stringBuilder.Append($" and DepartmentCode=@DeptCode");
                    parameters.Add(new SQLParameters("@DeptCode",getPurchaseItemRequest.DeptCode!));
                }
                if(getPurchaseItemRequest.SearchBy!="All")
                {
                    stringBuilder.Append($" and "+getPurchaseItemRequest.SearchBy!.Replace(" ","")+" like   '%"+ getPurchaseItemRequest.ItemName + "%'");
                    //parameters.Add(new SQLParameters("@ItemName", getPurchaseItemRequest.ItemName!));
                }
                else
                {
                    stringBuilder.Append($" and (ItemName LIKE '%"+ getPurchaseItemRequest.ItemName + "%' OR Size LIKE '%"+ getPurchaseItemRequest.ItemName + "%' OR Make LIKE '%"+ getPurchaseItemRequest.ItemName + "%')");
                    //parameters.Add(new SQLParameters("@ItemNames", getPurchaseItemRequest.ItemName!));
                }

                return await _dbContext.SelectAsync(InclusiveSql.Get_ITEM_NAME.Replace("@Condition", stringBuilder.ToString()),parameters, DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error during GetItems.");
                throw;
            }
        }
        public async Task<DataTable> GetBaseUrlFromTable()
        {
            try
            {
                return await _dbContext.SelectAsync(InclusiveSql.Get_Base_Url_Table,DBConnections.Advance);
                
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error during GetBaseUrlFromTable.");
                throw;
            }
        }
        public async Task<DataTable> GetItemDetails(string ItemCode)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters( "@itemcode",ItemCode));
                return await _dbContext.SelectAsync(InclusiveSql.GET_ITEM_BY_CODE,parameters,DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error During GetItemDetails");
                throw;
            }
        }
        public async Task<DataTable> GetItemMakeCode(string ItemName,string Size,string Unit)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                //itemname,size,unit
                parameters.Add(new SQLParameters("@itemname", ItemName));
                parameters.Add(new SQLParameters("@size", Size));
                parameters.Add(new SQLParameters("@unit", Unit));
                return await _dbContext.SelectAsync(InclusiveSql.GET_ITEM_MAKE_CODE,parameters,DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error During GetItemMakeCode");
                throw;
            }
        }
        public async Task<DataTable> GetItemsDetails(string ItemName,string Make)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                //itemcode,make
                parameters.Add(new SQLParameters("@itemcode", ItemName));
                parameters.Add(new SQLParameters("@make", Make));
                
                return await _dbContext.SelectAsync(InclusiveSql.GET_ITEM_DETAILS, parameters,DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error During GetItemsDetails");
                throw;
            }
        }


        public async Task<DataTable> GetEmployeeCampusCodes(string? userId)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@EmployeeCode", userId ?? string.Empty),
                };
                return await _dbContext.SelectAsync(InclusiveSql.GET_ALLOWED_CAMPUS_CODES, parameters, DBConnections.Salary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetEmployeeCampus.");
                throw;
            }
        }

        public async Task<DataTable> GetAllMaad()
        {
            try
            {
                return await _dbContext.SelectAsync(InclusiveSql.GET_ALL_MAAD, null, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetAllMaad.");
                throw;
            }

        }

        public async Task<DataTable> GetAllDepartments(List<string>? campusCodes)
        {
            try
            {
                StringBuilder changeCondition = new StringBuilder(string.Empty);
                List<SQLParameters> parameters = new List<SQLParameters>();

                if (campusCodes != null && campusCodes.Count > 0)
                {
                    for (int i = 0; i < campusCodes.Count; i++)
                    {
                        if (i > 0)
                        {
                            changeCondition.Append(" OR ");
                        }
                        changeCondition.Append($" FIND_IN_SET(@CampusCodeParameter{i},CampusCodes)");

                        parameters.Add(new SQLParameters($"@CampusCodeParameter{i}", campusCodes[i]));
                    }

                    string query = InclusiveSql.GET_ALL_DEPARTMENTS.Replace("@ChangeCondition", changeCondition.ToString());

                    return await _dbContext.SelectAsync(query, parameters, DBConnections.Advance);

                }

                return await _dbContext.SelectAsync(InclusiveSql.GET_ALL_MAAD, null, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetAllDepartments.");
                throw;
            }
        }

        public async Task<DataTable> GetVendors(string? search)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@VendorName", search ?? string.Empty),
                };

                return await _dbContext.SelectAsync(InclusiveSql.GET_ALL_VENDORS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetAllMaad.");
                throw;
            }
        }

        public async Task<DataTable> GetVendorSubFirms(int? vendorId)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@VendorID", vendorId.ToString() ?? string.Empty),
                };

                return await _dbContext.SelectAsync(InclusiveSql.GET_VENDOR_SUBFIRMS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetAllMaad.");
                throw;
            }
        }

        public async Task<DataTable> GetAllEmployees(string? search)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@EmployeeName", search ?? string.Empty),
                    new SQLParameters("@EmployeeCode", search ?? string.Empty),
                };

                return await _dbContext.SelectAsync(InclusiveSql.GET_ALL_EMPLOYEES, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetAllEmployees.");
                throw;
            }
        }

        public async Task<DataTable> GetIsVendorBudgetEnable(string? vendorId)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@VendorId", vendorId ?? string.Empty),
                };

                return await _dbContext.SelectAsync(InclusiveSql.GET_VENDOR_BUDGET_ENABLE, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetIsVendorBudgetEnable.");
                throw;
            }
        }

        public async Task<DataTable> IsFirmBudgetExist(GetFirmBudgetRequest? firm)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@VendorId", firm?.VendorId ?? string.Empty),
                    new SQLParameters("@SubFirm", firm?.SubFirm ?? string.Empty),
                };

                return await _dbContext.SelectAsync(InclusiveSql.CHECK_BUGET_EXISTENSE, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during IsFirmBudgetExist.");
                throw;
            }
        }

        public async Task<DataTable> IsFirmBudgetPending(GetFirmBudgetRequest? firm, string? fromDate, string? toDate, ApprovalTypes approvalTypes)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@VendorId", firm?.VendorId ?? string.Empty),
                    new SQLParameters("@SubFirm", firm?.SubFirm ?? string.Empty),
                    new SQLParameters("@FromDate", fromDate ?? string.Empty),
                    new SQLParameters("@ToDate", toDate ?? string.Empty),
                };

                return await _dbContext.SelectAsync(approvalTypes == ApprovalTypes.OtherApproval ? InclusiveSql.CHECK_IS_VENDOR_BUGET_PENDING_OTHER_APPOROVALS : InclusiveSql.CHECK_IS_VENDOR_BUGET_PENDING_PURCHASE_APPOROVALS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during IsFirmBudgetExist.");
                throw;
            }
        }

        public async Task<DataTable> GetVendorBudgetDetails(GetFirmBudgetRequest? firm, string? fromDate, string? toDate)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@VendorId", firm?.VendorId ?? string.Empty),
                    new SQLParameters("@SubFirm", firm?.SubFirm ?? string.Empty),
                    new SQLParameters("@FromDate", fromDate ?? string.Empty),
                    new SQLParameters("@ToDate", toDate ?? string.Empty),
                };

                return await _dbContext.SelectAsync(InclusiveSql.GET_VENDOR_BUDGET_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during IsFirmBudgetExist.");
                throw;
            }
        }
        public async Task<DataTable> CheckUserRole(string? employeeCode, UserRolePermission userRolePermission)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@EmployeeCode", employeeCode ?? string.Empty),
                    new SQLParameters("@ColumnName", userRolePermission.ToString() ?? string.Empty)
                };

                return await _dbContext.SelectAsync(InclusiveSql.CHECK_USER_ROLE, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CheckUserRole.");
                throw;
            }
        }
        public async Task<DataTable> GetFileKey()
        {
            try
            {
                return await _dbContext.SelectAsync(InclusiveSql.GET_FILE_ENC_KEY,  DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CheckUserRole.");
                throw;
            }
        }
    }
}
