using AdvanceAPI.DTO.Approval;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.SQLConstants.Approval;
using System.Data;
using System.Text;

namespace AdvanceAPI.Repository
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly ILogger<ApprovalRepository> _logger;
        private readonly IDBOperations _dbContext;
        private readonly IGeneral _general;

        public ApprovalRepository(ILogger<ApprovalRepository> logger, IDBOperations dbContext, IGeneral general)
        {
            _logger = logger;
            _dbContext = dbContext;
            _general = general;
        }

        public async Task<DataTable> GetDraftItemRefNo(string EmpCode, string AppType,string RefNo)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@EmpCode", EmpCode ?? string.Empty),
                    new SQLParameters("@AppType", AppType ?? string.Empty),
                    new SQLParameters("@ReferenceNo", RefNo ?? string.Empty),
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
                parametersList.Add(new SQLParameters("@ReferenceNo", AddStock.RefNo ?? string.Empty));
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
                parametersList.Add(new SQLParameters("@DraftName", AddStock.DraftName));
                return await _dbContext.DeleteInsertUpdateAsync(ApprovalSql.INERT_DRAFT, parametersList, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during AddDraftItem.");
                throw;
            }
        }

        public async Task<DataTable> GetDraftedItem(string EmpCode, string AppType,string CampusCode,string RefNo)
        {
            try
            {
                //GET_DRAFTED_ITEM
                List<SQLParameters> parametersList = new List<SQLParameters>();
                parametersList.Add(new SQLParameters("@EmpCode", EmpCode));
                parametersList.Add(new SQLParameters("@ApprovalType", AppType));
                parametersList.Add(new SQLParameters("@CampusCode", CampusCode ));
                parametersList.Add(new SQLParameters("@ReferenceNo",RefNo?? string.Empty));
                return await _dbContext.SelectAsync(ApprovalSql.GET_DRAFTED_ITEM,parametersList, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetDraftedItem.");
                throw;
            }
        }

        public async Task<DataTable> GetDraftedSummary(string EmpCode, string AppType, string CampusCode, string RefNo)
        {
            try
            {
                List<SQLParameters> parametersList = new List<SQLParameters>();
                parametersList.Add(new SQLParameters("@EmpCode", EmpCode));
                parametersList.Add(new SQLParameters("@AppType", AppType));
                parametersList.Add(new SQLParameters("@CampusCode", CampusCode));
                parametersList.Add(new SQLParameters("@ReferenceNo", RefNo));
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
                    new SQLParameters("@ReferenceNo", deleteRequest?.ReferenceNo ?? string.Empty),
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
                    new SQLParameters("@ReferenceNo", deleteRequest?.ReferenceNo ?? string.Empty),
                };

                return await _dbContext.DeleteInsertUpdateAsync(ApprovalSql.DELETE_DRAFT_APPROVAL_ITEMS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during DeleteApprovalDraftItems.");
                throw;
            }
        }

        public async Task<DataTable> GetVenderRegister(string VenderCode)
        {
            try
            {
                List<SQLParameters> sqlParametersList = new List<SQLParameters>();
                sqlParametersList.Add(new SQLParameters("@VenderID",VenderCode));
                return await _dbContext.SelectAsync(ApprovalSql.GET_VENDER_REGISTER, sqlParametersList,DBConnections.Advance);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e, "Error during GetVenderRegister.");
                throw;
            }
        }

        public async Task<int> SubmitPurchaseBill(string EmpCode,GeneratePurchaseApprovalRequest generatePurchaseApprovalRequest,string RefNo,string TotalDays)
        {
            DataTable VenderDetails =
                await GetVenderRegister(generatePurchaseApprovalRequest.ApprovalMessers.ToString());
            string FermName = "";
            string FcontcatName = "";
            string Fermcno = "";
            string FermEmail = "";
            string FermAlter = "";
            string FermAdd = "";
            if (VenderDetails != null && VenderDetails.Rows.Count > 0)
            {
                FermName = VenderDetails.Rows[0]["VendorName"].ToString()??string.Empty;
                FcontcatName = VenderDetails.Rows[0]["ContactPersons"].ToString()??string.Empty;
                Fermcno = VenderDetails.Rows[0]["ContactNo"].ToString()??string.Empty;
                FermEmail = VenderDetails.Rows[0]["EmailID"].ToString()??string.Empty;
                FermAlter = VenderDetails.Rows[0]["AlternateContactNo"].ToString()??string.Empty;
                FermAdd = VenderDetails.Rows[0]["Address"].ToString().ToUpper().Replace("<BR/>", " ")??string.Empty;
            }
            string[] App1 = generatePurchaseApprovalRequest.Approval1.Split("#");
            string[] App2 = generatePurchaseApprovalRequest.Approval2.Split("#");
            string[] App3 = generatePurchaseApprovalRequest.Approval3.Split("#");
            string[] App4 = generatePurchaseApprovalRequest.FinalApprovalAuth.Split("#");
            string[] IniBy = generatePurchaseApprovalRequest.InitBy.Split("#");
            string BudgetRequired = generatePurchaseApprovalRequest.BudgetDet, BudgetAmount = generatePurchaseApprovalRequest.BudgetAmount, PreviousTaken = generatePurchaseApprovalRequest.BudgetReleaseAmount;
            string BudgetStatus = "Not Required";
            string CurStatus = "Not Required";
            int b_;
            if (int.TryParse(BudgetAmount, out b_))
            {
                int c_ = Convert.ToInt32(PreviousTaken);
                int diff = b_ - (c_ + Convert.ToInt32(generatePurchaseApprovalRequest.TotalAmount));
                CurStatus = diff.ToString();
                BudgetStatus = diff < 0 ? Math.Abs(diff) + " Rs/- OverBudget" : "UnderBudget";
            }
            string Session = _general.GetFinancialSession(DateTime.Now);
            //@RefNo,@Session,@MyType,@Note,@Purpose,@BillUptoValue,@BillUptoType,@UploadById,@UploadByName,NOW(),@IpAddress,@ForDepartment,
// @FirmName,@FirmPerson,@FirmEmail,@FirmPanNo,@FirmAddress,
// @FirmContactNo,@FirmAlternateContactNo,@TotalItem,@Amount,@VatPer,@TotalAmount,'Pending',@CashPer,@Other,@FinalApprovalId,@FinalApprovalName,@FinalApprovalDesignation,
// 'Pending',NULL,@ApprovalCategory,'Pending',@VendorID,@IniID,
// @IniName,@IniDesignation,@IniDepartment,@AppDate,@ReferenceBillStatus,@BillTill,@ExtendedBillDate,'0.00',@ApprovalItemCount,'Pending',@BillRequired,@Maad,@BudgetBalanceAmount,@BudgetRequired,@BudgetAmount,
// @PreviousTaken,@CurStatus,@BudgetStatus,@BudgetReferenceNo,@CampusCode,@CampusName
//@App1ID,@App1Name,@App1Designation,'Pending',NULL,@App2ID,@App2Name,@App2Designation,'Pending',NULL,@App3ID,@App3Name,@App3Designation,'Pending',NULL
            List<SQLParameters> sqlParametersList = new List<SQLParameters>();
            sqlParametersList.Add(new SQLParameters("@EmpCode",EmpCode));
            sqlParametersList.Add(new SQLParameters("@RefNo",RefNo));
            sqlParametersList.Add(new SQLParameters("@Session",Session));
            sqlParametersList.Add(new SQLParameters("@MyType",generatePurchaseApprovalRequest.ApprovalType ?? string.Empty));
            sqlParametersList.Add(new SQLParameters("@Note",_general.GetReplace(generatePurchaseApprovalRequest.ApprovalNote)));
            sqlParametersList.Add(new SQLParameters("@Purpose",_general.GetReplace(generatePurchaseApprovalRequest.ApprovalPurpose)));
            sqlParametersList.Add(new SQLParameters("@BillUptoValue",TotalDays));
            sqlParametersList.Add(new SQLParameters("@BillUptoType","Day"));
            sqlParametersList.Add(new SQLParameters("@UploadById",EmpCode));
            sqlParametersList.Add(new SQLParameters("@UploadByName",await _general.GetEmpName(EmpCode))); ///GET EMP NAME
            sqlParametersList.Add(new SQLParameters("@IpAddress",_general.GetIpAddress()));
            sqlParametersList.Add(new SQLParameters("@ForDepartment",generatePurchaseApprovalRequest.ApprovalDepartment));
            sqlParametersList.Add(new SQLParameters("@FirmName",FermName));
            sqlParametersList.Add(new SQLParameters("@FirmPerson",FcontcatName));
            sqlParametersList.Add(new SQLParameters("@FirmPanNo",""));
            sqlParametersList.Add(new SQLParameters("@FirmAddress",FermAdd));
            sqlParametersList.Add(new SQLParameters("@FirmContactNo",Fermcno));
            sqlParametersList.Add(new SQLParameters("@FirmEmail",FermEmail));
            sqlParametersList.Add(new SQLParameters("@FirmAlternateContactNo",FermAlter));
            sqlParametersList.Add(new SQLParameters("@TotalItem",generatePurchaseApprovalRequest.DraftedItems.Count));
            sqlParametersList.Add(new SQLParameters("@Amount",generatePurchaseApprovalRequest.DraftedItems.Sum(x=>Convert.ToInt32(x.ActualAmount))));
            sqlParametersList.Add(new SQLParameters("@VatPer",generatePurchaseApprovalRequest.OverAllGST));
            sqlParametersList.Add(new SQLParameters("@TotalAmount",generatePurchaseApprovalRequest.AmountInDigit));
            sqlParametersList.Add(new SQLParameters("@CashPer",generatePurchaseApprovalRequest.OverAllDiscount));
            sqlParametersList.Add(new SQLParameters("@Other",generatePurchaseApprovalRequest.OtherCharges));
            if (generatePurchaseApprovalRequest.AmountInDigit >= 1000)
            {
                sqlParametersList.Add(new SQLParameters("@ReferenceBillStatus","Open"));
                sqlParametersList.Add(new SQLParameters("@BillRequired","Yes"));
                sqlParametersList.Add(new SQLParameters("@App1ID",App1[0]));
                sqlParametersList.Add(new SQLParameters("@App1Name",App1[1]));
                sqlParametersList.Add(new SQLParameters("@App1Designation",App1[2]));
                sqlParametersList.Add(new SQLParameters("@App2ID",App2[0]));
                sqlParametersList.Add(new SQLParameters("@App2Name",App2[1]));
                sqlParametersList.Add(new SQLParameters("@App2Designation",App2[2]));
                sqlParametersList.Add(new SQLParameters("@App3ID",App3[0]));
                sqlParametersList.Add(new SQLParameters("@App3Name",App3[1]));
                sqlParametersList.Add(new SQLParameters("@App3Designation",App3[2]));
                sqlParametersList.Add(new SQLParameters("@FinalApprovalId",App4[0]));
                sqlParametersList.Add(new SQLParameters("@FinalApprovalName",App4[1]));
                sqlParametersList.Add(new SQLParameters("@FinalApprovalDesignation",App4[2]));
            }
            else
            {
                sqlParametersList.Add(new SQLParameters("@FinalApprovalId",App1[0]));
                sqlParametersList.Add(new SQLParameters("@FinalApprovalName",App1[1]));
                sqlParametersList.Add(new SQLParameters("@FinalApprovalDesignation",App1[2]));
                // sqlParametersList.Add(new SQLParameters("@FinalApprovalId",App1[0]));
                sqlParametersList.Add(new SQLParameters("@ReferenceBillStatus","Closed"));
                sqlParametersList.Add(new SQLParameters("@BillRequired","No"));
                
            }
            sqlParametersList.Add(new SQLParameters("@ApprovalCategory",generatePurchaseApprovalRequest.ApprovalCategory));
            sqlParametersList.Add(new SQLParameters("@VendorID",generatePurchaseApprovalRequest.ApprovalMessers));
            sqlParametersList.Add(new SQLParameters("@IniID",IniBy[0]));
            sqlParametersList.Add(new SQLParameters("@IniName",IniBy[1]));
            sqlParametersList.Add(new SQLParameters("@IniDesignation",IniBy[2]));
            sqlParametersList.Add(new SQLParameters("@IniDepartment",IniBy[3]));
            sqlParametersList.Add(new SQLParameters("@AppDate",generatePurchaseApprovalRequest.ApprovalDate));
            sqlParametersList.Add(new SQLParameters("@BillTill",generatePurchaseApprovalRequest.ApprovalTillDate));
            sqlParametersList.Add(new SQLParameters("@ExtendedBillDate",generatePurchaseApprovalRequest.ApprovalTillDate));
            sqlParametersList.Add(new SQLParameters("@ApprovalItemCount",generatePurchaseApprovalRequest.DraftedItems.Count));
            sqlParametersList.Add(new SQLParameters("@Maad",generatePurchaseApprovalRequest.Maad));
            sqlParametersList.Add(new SQLParameters("@BudgetBalanceAmount",generatePurchaseApprovalRequest.BudgetBalanceAmount));
            sqlParametersList.Add(new SQLParameters("@BudgetRequired",generatePurchaseApprovalRequest.BudgetDet));
            sqlParametersList.Add(new SQLParameters("@BudgetAmount",generatePurchaseApprovalRequest.BudgetAmount));
            sqlParametersList.Add(new SQLParameters("@PreviousTaken",generatePurchaseApprovalRequest.BudgetReleaseAmount));
            sqlParametersList.Add(new SQLParameters("@CurStatus",CurStatus));
            sqlParametersList.Add(new SQLParameters("@BudgetStatus",BudgetStatus));
            sqlParametersList.Add(new SQLParameters("@BudgetReferenceNo",generatePurchaseApprovalRequest.BudgetRefNo));
            sqlParametersList.Add(new SQLParameters("@CampusCode",generatePurchaseApprovalRequest.campus));
            sqlParametersList.Add(new SQLParameters("@CampusName",await _general.CampusNameByCode(generatePurchaseApprovalRequest.campus.ToString())));

            try
            {
                int ins = await _dbContext.DeleteInsertUpdateAsync(
                    generatePurchaseApprovalRequest.AmountInDigit >= 1000
                        ? ApprovalSql.INSERT_FINAL_PURCHASE_APPROVAL_ABOVE_1000
                        : ApprovalSql.INSERT_FINAL_PURCHASE_APPROVAL_BELOW_1000, sqlParametersList, DBConnections.Advance);

                sqlParametersList.Clear();
                //RefNo,AppType,CampusCode,EmpCode
                //sqlParametersList.Add(new SQLParameters("@RefNo",RefNo));
                sqlParametersList.Add(new SQLParameters("@AppType",generatePurchaseApprovalRequest.ApprovalType));
                sqlParametersList.Add(new SQLParameters("@CampusCode",generatePurchaseApprovalRequest.campus));
                sqlParametersList.Add(new SQLParameters("@ReferenceNo",generatePurchaseApprovalRequest.RefNo));
                sqlParametersList.Add(new SQLParameters("@EmpCode",EmpCode));
                {
                    ins = await _dbContext.DeleteInsertUpdateAsync(
                        ApprovalSql.INSER_APPROVAL_DETAILS.Replace("@RefNo", RefNo), sqlParametersList,
                        DBConnections.Advance);
                }
                {
                    // @EmpCode,@CampusCode,@AppType
                    ins=await _dbContext.DeleteInsertUpdateAsync(ApprovalSql.DELETE_APPROVAL_DRAFT_DETAILS, sqlParametersList, DBConnections.Advance);
                }
                return ins;
            }
            catch (Exception e)
            {
                sqlParametersList.Clear();
                sqlParametersList.Add(new SQLParameters("@ReferenceNo",RefNo));
                int ins=await _dbContext.DeleteInsertUpdateAsync(ApprovalSql.DELETE_PURCHASE_DETAILS, sqlParametersList, DBConnections.Advance);
                ins=await _dbContext.DeleteInsertUpdateAsync(ApprovalSql.DELETE_PURCHASE_SUMMARY, sqlParametersList, DBConnections.Advance);
                _logger.LogError(e, "An error occurred while Generate Final Purchase Approval.");
                Console.WriteLine(e);
                throw;
            }
           
            
        }

        public async Task<DataTable> CheckDeletedDraftedItem(string ItemId)
        {
            try
            {
                List<SQLParameters> sqlParametersList = new List<SQLParameters>();
                sqlParametersList.Add(new SQLParameters("@Id",ItemId));
                return await _dbContext.SelectAsync(ApprovalSql.GETDRAFTEDITEM,sqlParametersList,DBConnections.Advance);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> DeletedDraftedItem(string ItemId)
        {
            try
            {
                List<SQLParameters> sqlParametersList = new List<SQLParameters>();
                sqlParametersList.Add(new SQLParameters("@Id",ItemId));
                int ins=await _dbContext.DeleteInsertUpdateAsync(ApprovalSql.DELETE_DRAFTED_ITEM, sqlParametersList, DBConnections.Advance); 
                return ins;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e, "An error occurred while DeletedDraftedItem");
                throw;
            }
        }
        public async Task<DataTable> GetMyApprovals(string? emploeeId, bool OnlySelfApprovals, AprrovalsListRequest? search)
        {
            try
            {


                StringBuilder extraCondition = new StringBuilder();
                var parameters = new List<SQLParameters>();
                if (OnlySelfApprovals)
                {
                    extraCondition.Append($" And (IniId = @EmployeeCode or RelativePersonID= @EmployeeCode)");
                    parameters.Add(new SQLParameters("@EmployeeCode", emploeeId ?? string.Empty));
                }

                if (!string.IsNullOrEmpty(search?.ReferenceNo))
                {
                    extraCondition.Append(" And ReferenceNo= @ReferenceNo");
                    parameters.Add(new SQLParameters("@ReferenceNo", search.ReferenceNo ?? string.Empty));
                }
                else
                {
                    extraCondition.Append($"  And `Session`=@Session And `Status`=@Status ");
                    parameters.Add(new SQLParameters("@Session", search?.Session ?? string.Empty));
                    parameters.Add(new SQLParameters("@Status", search?.Status ?? string.Empty));

                    if (!string.IsNullOrEmpty(search?.CampusCode))
                    {
                        extraCondition.Append(" And CampusCode= @CampusCode");
                        parameters.Add(new SQLParameters("@CampusCode", search.CampusCode ?? string.Empty));
                    }
                }
                
                parameters.Add(new SQLParameters("@LimitItems", search?.NoOfItems ?? 0));
                parameters.Add(new SQLParameters("@OffSetItems", search?.ItemsFrom ?? 0));

                string sqlQuery = ApprovalSql.GET_MY_APPROVALS.Replace("@Condition", extraCondition.ToString());

                return await _dbContext.SelectAsync(sqlQuery, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetMyApprovals.");
                throw;
            }
        }public async Task<DataTable> GetMyApprovalsCount(string? emploeeId, bool OnlySelfApprovals, AprrovalsListRequest? search)
        {
            try
            {


                StringBuilder extraCondition = new StringBuilder();
                var parameters = new List<SQLParameters>();
                if (OnlySelfApprovals)
                {
                    extraCondition.Append($" And (IniId = @EmployeeCode or RelativePersonID= @EmployeeCode)");
                    parameters.Add(new SQLParameters("@EmployeeCode", emploeeId ?? string.Empty));
                }

                if (!string.IsNullOrEmpty(search?.ReferenceNo))
                {
                    extraCondition.Append(" And ReferenceNo= @ReferenceNo");
                    parameters.Add(new SQLParameters("@ReferenceNo", search.ReferenceNo ?? string.Empty));
                }
                else
                {
                    extraCondition.Append($"  And `Session`=@Session And `Status`=@Status ");
                    parameters.Add(new SQLParameters("@Session", search?.Session ?? string.Empty));
                    parameters.Add(new SQLParameters("@Status", search?.Status ?? string.Empty));

                    if (!string.IsNullOrEmpty(search?.CampusCode))
                    {
                        extraCondition.Append(" And CampusCode= @CampusCode");
                        parameters.Add(new SQLParameters("@CampusCode", search.CampusCode ?? string.Empty));
                    }
                }
                
                parameters.Add(new SQLParameters("@LimitItems", search?.NoOfItems ?? 0));
                parameters.Add(new SQLParameters("@OffSetItems", search?.ItemsFrom ?? 0));

                string sqlQuery = ApprovalSql.GET_MY_APPROVAL_COUNT.Replace("@Condition", extraCondition.ToString());

                return await _dbContext.SelectAsync(sqlQuery, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetMyApprovals.");
                throw;
            }
        }

        public async Task<DataTable> CheckIsApprovalComparisonDefined(string? referenceNo)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty)
                };
                return await _dbContext.SelectAsync(ApprovalSql.CHECK_IS_APPROVAL_COMPARISON_DEFINED, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CheckIsApprovalComparisonDefined.");
                throw;
            }
        }
        public async Task<DataTable> CheckIsApprovalExists(string? referenceNo)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty)
                };
                return await _dbContext.SelectAsync(ApprovalSql.CHECK_APPROVAL_EXISTS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CheckIsApprovalComparisonDefined.");
                throw;
            }
        }

        public async Task<int> DeleteApproval(string? employeeId, string? referenceNo)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty),
                    new SQLParameters("@EmployeeId", employeeId ?? string.Empty),
                    new SQLParameters("@DeleteFrom", _general.GetIpAddress() ?? string.Empty),
                };
                return await _dbContext.DeleteInsertUpdateAsync(ApprovalSql.DELETE_APPROVAL, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during DeleteApproval.");
                throw;
            }
        }

        public async Task<DataTable> CheckAlreadyDraftedItem(string EmpCode, string AppType,string ItemCode,string RefNo)
        {
            try
            {
                List<SQLParameters>  sqlParametersList = new List<SQLParameters>();
                sqlParametersList.Add(new SQLParameters("@EmpCode", EmpCode ?? string.Empty));
                sqlParametersList.Add(new SQLParameters("@AppType", AppType ?? string.Empty));
                sqlParametersList.Add(new SQLParameters("@ItemCode", ItemCode ?? string.Empty));
                sqlParametersList.Add(new SQLParameters("@ReferenceNo", RefNo ?? string.Empty));
                return await _dbContext.SelectAsync(ApprovalSql.CHECK_ALREADY_ITEM,sqlParametersList, DBConnections.Advance);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e, "Error during CheckAlreadyDraftedItem.");
                throw;
            }
        }

        public async Task<DataTable> GetDraft(string EmpCode)
        {
            try
            {
                List<SQLParameters> sqlParametersList = new List<SQLParameters>();
                sqlParametersList.Add(new SQLParameters("@EmpCode", EmpCode ?? string.Empty));
                return await _dbContext.SelectAsync(ApprovalSql.GET_DRAFT,sqlParametersList, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CheckAlreadyDraftedItem.");
                throw;
            }
        }

        public async Task<DataTable> GetPOApprovalDetails(string? referenceNo)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty)
                };
                return await _dbContext.SelectAsync(ApprovalSql.GET_PO_APPROVAL_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetPOApprovalDetails.");
                throw;
            }
        }

        public async Task<DataTable> CheckIsApprovalComparisonLocked(string? referenceNo)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty)
                };
                return await _dbContext.SelectAsync(ApprovalSql.CHECK_IS_APPPROVAL_COMPARISON_LOCKED, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CheckIsApprovalComparisonLocked.");
                throw;
            }
        }
        public async Task<DataTable> GetPOApprovalBillExpenditureDetails(string? referenceNo)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty)
                };
                return await _dbContext.SelectAsync(ApprovalSql.GET_APPROVAL_BILL_EXPENDITURE_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetPOApprovalBillExpenditureDetails.");
                throw;
            }
        }
        public async Task<DataTable> GetApprovalPaymentDetails(string? referenceNo)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty)
                };
                return await _dbContext.SelectAsync(ApprovalSql.GET_APPROVAL_PAYMENT_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetApprovalPaymentDetails.");
                throw;
            }
        }
        public async Task<DataTable> GetApprovalWarrentyDetails(string? referenceNo)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty)
                };
                return await _dbContext.SelectAsync(ApprovalSql.GET_APPROVAL_WARRENTY_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetApprovalWarrentyDetails.");
                throw;
            }
        }
        public async Task<DataTable> GetApprovalRepairMaintinanceDetails(string? referenceNo)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty)
                };
                return await _dbContext.SelectAsync(ApprovalSql.GET_APPROVAL_REPAIR_MAINTINANCE_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetApprovalRepairMaintinanceDetails.");
                throw;
            }
        }

        public async Task<DataTable> GetApprovalItems(string? referenceNo)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty)
                };
                return await _dbContext.SelectAsync(ApprovalSql.GET_APPROVAL_ITEMS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetApprovalItems.");
                throw;
            }
        }
        public async Task<DataTable> GetApprovalItemPreviousPurchaseDetails(string? referenceNo, string? itemCode)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ItemCode", itemCode ?? string.Empty),
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty),
                };
                return await _dbContext.SelectAsync(ApprovalSql.GET_APPROVAL_ITEMS_PREVIOUS_PURCHASE_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetApprovalItemPreviousPurchaseDetails.");
                throw;
            }
        }

        public async Task<DataTable> GetApprovalEmployeeDesignationContact(string? employeeId)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@EmployeeId", employeeId ?? string.Empty),
                };
                return await _dbContext.SelectAsync(ApprovalSql.GET_APPROVAL_EMPLOYEES_DESIGNATION_DEPARTMENT, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetApprovalItemPreviousPurchaseDetails.");
                throw;
            }
        }

        public async Task<DataTable> CheckApprovalExistsToUpdate(string? referenceNo)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty),
                };
                return await _dbContext.SelectAsync(ApprovalSql.CHECK_IS_APPROVAL_EXISTS_TO_UPDATE, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during CheckApprovalExists.");
                throw;
            }
        }


        public async Task<int> UpdateApprovalNote(string? employeeId, string? referenceNo, string? note)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@EmployeeId", employeeId ?? string.Empty),
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty),
                    new SQLParameters("@Note", note?.Trim()?.Replace("\n", " ")?.ToUpper() ?? string.Empty)
                };
                return await _dbContext.DeleteInsertUpdateAsync(ApprovalSql.UPDATE_APPROVAL_NOTE, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during UpdateApprovalNote.");
                throw;
            }
        }

        public async Task<DataTable> GetEditApprovalDetails(string? referenceNo)
        {
            try
            {
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@ReferenceNo", referenceNo ?? string.Empty),
                };
                return await _dbContext.SelectAsync(ApprovalSql.GET_EDIT_APPROVAL_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetEditApprovalDetails.");
                throw;
            }
        }
        public async Task<int> EditApprovalDetails(string? referenceNo,UpdateApprovalEditDetails details,string EmpCode)
        {
            try
            {
                
                List<SQLParameters> sqlParametersList = new List<SQLParameters>();
                sqlParametersList.Add(new SQLParameters("@VenderID",details.VendorId));
                string FermName = "";
                string FcontcatName = "";
                string Fermcno = "";
                string FermEmail = "";
                string FermAlter = "";
                string FermAdd = "";
               
                using (DataTable dt=await _dbContext.SelectAsync(ApprovalSql.GET_VENDER_REGISTER,DBConnections.Advance))
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        FermName = dt.Rows[0]["VendorName"].ToString()??string.Empty;
                        FcontcatName = dt.Rows[0]["ContactPersons"].ToString()??string.Empty;
                        Fermcno = dt.Rows[0]["ContactNo"].ToString()??string.Empty;
                        FermEmail = dt.Rows[0]["EmailID"].ToString()??string.Empty;
                        FermAlter = dt.Rows[0]["AlternateContactNo"].ToString()??string.Empty;
                        FermAdd = dt.Rows[0]["Address"].ToString().ToUpper().Replace("<BR/>", " ")??string.Empty;
                    }
                }
               
                if (details.MyType.Contains("Post Facto -"))
                {
                    details.MyType=details.MyType.Replace("Post Facto -", "").Trim();
                }
                var parameters = new List<SQLParameters>() {
                    new SQLParameters("@RefNo", referenceNo ?? string.Empty),
                    new SQLParameters("@Maad", details.Maad ?? string.Empty),
                    new SQLParameters("@DepartMent", details.DepartMent ?? string.Empty),
                    new SQLParameters("@Vendorid", details.VendorId.ToString() ?? string.Empty),
                    new SQLParameters("@FirmName", FermName ?? string.Empty),
                    new SQLParameters("@Address", FermAdd ?? string.Empty),
                    new SQLParameters("@FirmContactNo", Fermcno ?? string.Empty),
                    new SQLParameters("@FirmPerson", FcontcatName ?? string.Empty),
                    new SQLParameters("@FirmEmail", FermEmail ?? string.Empty),
                    new SQLParameters("@AltContactNo", FermAlter ?? string.Empty),
                    new SQLParameters("@Purpose", details.Purpose ?? string.Empty),
                    new SQLParameters("@Note", details.Note ?? string.Empty),
                    new SQLParameters("@AppDate", details.AppDate ?? string.Empty),
                    new SQLParameters("@BillDate", details.BillTill ?? string.Empty),
                    new SQLParameters("@ExtendedBillDate", details.ExtendedBillDate ?? string.Empty),
                    new SQLParameters("@MyType", Convert.ToDateTime(details.AppDate)>Convert.ToDateTime(details.BillTill)?"Post Facto - "+details.MyType:details.MyType),
                };
                //@Type,@ChangeUniqueNo,@ChangeIn,@FromData,@ToData,@Operation,now(),@DoneBy,@IpAddress
                List<SQLParameters> sqlParametersList2 = new List<SQLParameters>();
                sqlParametersList2.Add(new SQLParameters("@Type", details.MyType??string.Empty));
                sqlParametersList2.Add(new SQLParameters("@ChangeUniqueNo", referenceNo));
                sqlParametersList2.Add(new SQLParameters("@ChangeIn",details.MyType));
                sqlParametersList2.Add(new SQLParameters("@FromData", ""));
                sqlParametersList2.Add(new SQLParameters("@ToData", ""));
                sqlParametersList2.Add(new SQLParameters("@DoneBy", EmpCode));
                sqlParametersList2.Add(new SQLParameters("@IpAddress", _general.GetIpAddress()));
                int ins = await _dbContext.DeleteInsertUpdateAsync(ApprovalSql.UPDATE_LOG, sqlParametersList2,DBConnections.Advance);
                
                return await _dbContext.DeleteInsertUpdateAsync(ApprovalSql.UPDATE_APPROVAL_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during EditApprovalDetails.");
                throw;
            }
        }

        public async Task<DataTable> GetApprovalDetails(string EmpCode,string EmpCodeAdd,GetApprovalRequest details)
        {
            try
            {
                List<SQLParameters> sqlParametersList = new List<SQLParameters>();
                string Condition = " And `Session`=@Session And ( App1ID=@EmpCode || App2ID=@EmpCode  || App3ID=@EmpCode  || App4ID=@EmpCode || App1ID=@EmpCodeAdd || App2ID=@EmpCodeAdd  || App3ID=@EmpCodeAdd  || App4ID=@EmpCodeAdd )";
                sqlParametersList.Add(new SQLParameters("@Session", details.Session));
                sqlParametersList.Add(new SQLParameters("@EmpCode", EmpCode));
                sqlParametersList.Add(new SQLParameters("@EmpCodeAdd", EmpCodeAdd));
                sqlParametersList.Add(new SQLParameters("@Limit", details.Limit));
                sqlParametersList.Add(new SQLParameters("@Offset", details.Page));
                if (!string.IsNullOrEmpty(details.CampusCode.ToString())) //camppus code
                {
                    Condition += " AND CampusCode=@CampusCode";
                    sqlParametersList.Add(new SQLParameters("@CampusCode", EmpCode));
                }

                if (!string.IsNullOrEmpty(details.Department))
                {
                    Condition += " And ForDepartment==@Department";
                    sqlParametersList.Add(new SQLParameters("@Department", details.Department));
                }

                if (!string.IsNullOrEmpty(details.Status))
                {
                    switch (details.Status)
                    {
                        case "Pending My":
                            Condition += " And And `Status`='Pending' And ( (App1ID=@EmpCode && App1Status='Pending') || (App2ID=@EmpCode && App2Status='Pending')  || (App3ID=@EmpCode && App3Status='Pending')  || (App4ID=@EmpCode && App4Status='Pending') || (App1ID=@EmpCodeAdd && App1Status='Pending') || (App2ID=@EmpCodeAdd && App2Status='Pending')  || (App3ID=@EmpCodeAdd && App3Status='Pending')  || (App4ID=@EmpCodeAdd && App4Status='Pending') )";
                        break;
                        case "Pending All":
                            Condition += " And `Status`=@Status";
                            sqlParametersList.Add(new SQLParameters("@Status", "Pending"));
                            break;
                        default:
                            Condition += " And `Status`=@Status";
                            sqlParametersList.Add(new SQLParameters("@Status", details.Status));
                            break;
                    }
                }
                return await _dbContext.SelectAsync(ApprovalSql.GET_APPROVAL_DETAILS.Replace("@AdditinalQuery",Condition),sqlParametersList,DBConnections.Advance);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e, "Error during GetApprovalDetails.");
                throw;
            }
        }

    }
}
