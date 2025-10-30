using System.Data;
using AdvanceAPI.DTO.Approval;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.SQLConstants.Approval;

namespace AdvanceAPI.Repository
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly ILogger<ApprovalRepository> _logger;
        private readonly IDBOperations _dbContext;


        public ApprovalRepository(ILogger<ApprovalRepository> logger, IDBOperations dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<DataTable> GetDraftItemRefNo(string EmpCode, string AppType)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@EmpCode", EmpCode ?? string.Empty),
                    new SQLParameters("@AppType", AppType ?? string.Empty),
                };
                return await _dbContext.SelectAsync(ApprovalSql.GET_ITEM_REFERENCE_NO, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetDraftItemRefNo.");
                throw;
            }
        }
        public async Task<DataTable> GetAutoDraftItemRefNo()
        {
            try
            {
                return await _dbContext.SelectAsync(ApprovalSql.GET_AUTO_ITEM_REFNO, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetAutoDraftItemRefNo.");
                throw;
            }
        }
        public async Task<int> AddDraftItem(string RefNo, AddStockItemRequest AddStock, string EmpCode)
        {
            try
            {
                List<SQLParameters> parametersList = new List<SQLParameters>();
                //@ReferenceNo,@AppType,@ItemCode,@ItemName,@Make,@Size,@Unit,@Balance,@Quantity,@PrevRate,@CurRate,@ChangeReason,@TotalAmount,NOW(),@IniId,'Pending',@WarIn,@WarType,@ActualAmount,@VatPer,@R_Total,@R_Pending,'Pending',@SerialNo,@DisPer,@CampusCode
                parametersList.Add(new SQLParameters("@ReferenceNo", RefNo));
                parametersList.Add(new SQLParameters("@AppType", AddStock.ApprovalType));
                parametersList.Add(new SQLParameters("@ItemCode", AddStock.ItemCode));
                parametersList.Add(new SQLParameters("@ItemName", AddStock.ItemName));
                parametersList.Add(new SQLParameters("@Make", AddStock.Make));
                parametersList.Add(new SQLParameters("@Size", AddStock.Size));
                parametersList.Add(new SQLParameters("@Unit", AddStock.Unit));
                parametersList.Add(new SQLParameters("@Balance", AddStock.Balance));
                parametersList.Add(new SQLParameters("@Quantity", AddStock.Quantity));
                parametersList.Add(new SQLParameters("@PrevRate", AddStock.PrevRate));
                parametersList.Add(new SQLParameters("@CurRate", AddStock.CurrentRate));
                parametersList.Add(new SQLParameters("@ChangeReason", AddStock.ChangeReason));
                parametersList.Add(new SQLParameters("@TotalAmount", AddStock.TotalAmount));
                parametersList.Add(new SQLParameters("@IniId", EmpCode));
                parametersList.Add(new SQLParameters("@WarIn", AddStock.Warranty));
                parametersList.Add(new SQLParameters("@WarType", AddStock.WarrantyType));
                parametersList.Add(new SQLParameters("@ActualAmount", AddStock.ActualAmount));
                parametersList.Add(new SQLParameters("@VatPer", AddStock.GstPer));
                parametersList.Add(new SQLParameters("@R_Total", "0.00"));
                parametersList.Add(new SQLParameters("@R_Pending", AddStock.Quantity));
                parametersList.Add(new SQLParameters("@SerialNo", string.Empty));
                parametersList.Add(new SQLParameters("@DisPer", AddStock.DiscountPercent));
                parametersList.Add(new SQLParameters("@CampusCode", AddStock.CampusCode));
                return await _dbContext.DeleteInsertUpdateAsync(ApprovalSql.INERT_DRAFT, parametersList, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during AddDraftItem.");
                throw;
            }
        }

        public async Task<DataTable> GetDraftedItem(string EmpCode, string AppType,string CampusCode)
        {
            try
            {
                //GET_DRAFTED_ITEM
                List<SQLParameters> parametersList = new List<SQLParameters>();
                parametersList.Add(new SQLParameters("@EmpCode", EmpCode));
                parametersList.Add(new SQLParameters("@ApprovalType", AppType));
                parametersList.Add(new SQLParameters("@CampusCode", CampusCode ));
                return await _dbContext.SelectAsync(ApprovalSql.GET_DRAFTED_ITEM,parametersList, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetDraftedItem.");
                throw;
            }
        }

        public async Task<DataTable> GetDraftedSummary(string EmpCode, string AppType, string CampusCode)
        {
            try
            {
                List<SQLParameters> parametersList = new List<SQLParameters>();
                parametersList.Add(new SQLParameters("@EmpCode", EmpCode));
                parametersList.Add(new SQLParameters("@AppType", AppType));
                parametersList.Add(new SQLParameters("@CampusCode", CampusCode));
                return await _dbContext.SelectAsync(ApprovalSql.Get_Draft_Summary, parametersList,
                    DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetDraftedSummary.");
                throw;
            }
        }

        public async Task<DataTable> GeneratePurchaseApprovalRefNo()
        {
            try
            {
                return await _dbContext.SelectAsync(ApprovalSql.Generate_RefNo_Purchaseapprovalsummary, DBConnections.Advance);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e, "Error during GeneratePurchaseApprovalRefNo.");
                throw;
            }
        }

        public async Task<DataTable> GetApprovalSessions()
        {
            try
            {
                return await _dbContext.SelectAsync(ApprovalSql.GET_APPROVALS_SESSION, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetApprovalSessions.");
                throw;
            }
        }
        public async Task<DataTable> GetApprovalFinalAuthority(string? campusCode)
        {
            try
            {
                return await _dbContext.SelectAsync((campusCode ?? string.Empty) == "101" ? ApprovalSql.GET_APPROVAL_FINAL_AUTHORITIES_MATHURA : ApprovalSql.GET_APPROVAL_FINAL_AUTHORITIES_NON_MATHURA, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetApprovalSessions.");
                throw;
            }
        }
        public async Task<DataTable> IsNonMathuraNumber3AuthoritiesDefined()
        {
            try
            {
                return await _dbContext.SelectAsync(ApprovalSql.GET_APPROVAL_NUMBER_3_NON_MATHURA_AUTHORITIES_DEFINED, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during IsNonMathuraNumber3AuthoritiesDefined.");
                throw;
            }
        }
        public async Task<DataTable> GetApprovalNumber3Authorities(string? search, bool isNonMathuraAuthoritiesRequired)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@EmployeeName", search ?? string.Empty),
                    new SQLParameters("@EmployeeCode", search ?? string.Empty),
                };

                return await _dbContext.SelectAsync(isNonMathuraAuthoritiesRequired ? ApprovalSql.GET_APPROVAL_NUMBER_3_NON_MATHURA_AUTHORITIES : ApprovalSql.GET_APPROVAL_NUMBER_3_MATHURA_AUTHORITIES, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during IsNonMathuraNumber3AuthoritiesDefined.");
                throw;
            }
        }

        public async Task<DataTable> CheckIsApprovalDraftItemsExists(string? emploeeId, DeleteApprovalDraftRequest? deleteRequest)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@IniId", emploeeId ?? string.Empty),
                    new SQLParameters("@AppType", deleteRequest?.ApprovalType ?? string.Empty),
                    new SQLParameters("@CampusCode", deleteRequest?.CampusCode ?? string.Empty),
                };

                return await _dbContext.SelectAsync(ApprovalSql.CHEKC_IS_DRAFT_APPROVAL_ITEMS_EXISTS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CheckIsApprovalDraftItemsExists.");
                throw;
            }
        }

        public async Task<int> DeleteApprovalDraftItems(string? emploeeId, DeleteApprovalDraftRequest? deleteRequest)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@IniId", emploeeId ?? string.Empty),
                    new SQLParameters("@AppType", deleteRequest?.ApprovalType ?? string.Empty),
                    new SQLParameters("@CampusCode", deleteRequest?.CampusCode ?? string.Empty),
                };

                return await _dbContext.DeleteInsertUpdateAsync(ApprovalSql.DELETE_DRAFT_APPROVAL_ITEMS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CheckIsApprovalDraftItemsExists.");
                throw;
            }
        }
    }
}
