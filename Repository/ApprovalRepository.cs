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
                parametersList.Add(new SQLParameters("@SerialNo", AddStock.GstPer));
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

    }
}
