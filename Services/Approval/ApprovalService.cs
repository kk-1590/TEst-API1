using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Approval;
using AdvanceAPI.DTO.Inclusive;
using AdvanceAPI.ENUMS.Inclusive;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Approval;
using AdvanceAPI.IServices.Inclusive;
using System.Data;

namespace AdvanceAPI.Services.Approval
{
    public class ApprovalService : IApprovalService
    {
        private readonly IApprovalRepository _approvalRepository;
        private readonly IGeneral _generalService;
        private readonly IInclusiveService _inclusiveService;
        private readonly IAccountRepository _accountRepository;
        public ApprovalService(IApprovalRepository approvalRepository, IGeneral generalService, IInclusiveService inclusiveService, IAccountRepository accountRepository)
        {
            _approvalRepository = approvalRepository;
            _generalService = generalService;
            _inclusiveService = inclusiveService;
            _accountRepository = accountRepository;
        }
        public async Task<ApiResponse> AddItemDraft(AddStockItemRequest AddStockItem, string EmpCode)
        {
            using (DataTable dt = await _approvalRepository.CheckAlreadyDraftedItem(EmpCode, AddStockItem.ApprovalType, AddStockItem.ItemCode, AddStockItem.RefNo))
            {
                if (dt.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", $"`{AddStockItem.ItemName}` already exists");
                }
            }

            string RefNo = string.Empty;
            DataTable refNoDataTable = await _approvalRepository.GetDraftItemRefNo(EmpCode, AddStockItem.ApprovalType, AddStockItem.RefNo);
            if (refNoDataTable.Rows.Count > 0)
            {
                AddStockItem.RefNo = refNoDataTable.Rows[0][0].ToString();
            }
            else
            {
                refNoDataTable = await _approvalRepository.GetAutoDraftItemRefNo();
                if (refNoDataTable.Rows.Count > 0)
                {
                    AddStockItem.RefNo = refNoDataTable.Rows[0][0].ToString();
                }
            }
            int AddItem = await _approvalRepository.AddDraftItem(RefNo, AddStockItem, EmpCode);
            if (AddItem > 0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", new { Message = "Item Add Successfully", RefNo = AddStockItem.RefNo });
            }
            else
            {
                return new ApiResponse(StatusCodes.Status500InternalServerError, "Error", "Add Failed");
            }

        }

        public async Task<ApiResponse> GetDraftedItem(string EmpCode, string AppType, string CampusCode, string RefNo)
        {
            DataTable itms = await _approvalRepository.GetDraftedItem(EmpCode, AppType, CampusCode, RefNo);
            List<DraftedItemResponse> items = new List<DraftedItemResponse>();
            foreach (DataRow item in itms.Rows)
            {
                items.Add(new DraftedItemResponse
                {
                    id = item["id"].ToString(),
                    ReferenceNo = item["ReferenceNo"].ToString(),
                    AppType = item["AppType"].ToString(),
                    ItemCode = item["ItemCode"].ToString(),
                    ItemName = item["ItemName"].ToString(),
                    Make = item["Make"].ToString(),
                    Size = item["Size"].ToString(),
                    Unit = item["Unit"].ToString(),
                    Balance = item["Balance"].ToString(),
                    Quantity = item["Quantity"].ToString(),
                    PrevRate = item["PrevRate"].ToString(),
                    CurRate = item["CurRate"].ToString(),
                    ChangeReason = item["ChangeReason"].ToString(),
                    WarIn = item["WarIn"].ToString(),
                    WarType = item["WarType"].ToString(),
                    ActualAmount = item["ActualAmount"].ToString(),
                    DisPer = item["DisPer"].ToString(),
                    VatPer = item["VatPer"].ToString(),
                    TotalAmount = item["TotalAmount"].ToString(),
                    IniOn = item["IniOn"].ToString(),
                    IniId = item["IniId"].ToString(),
                    Status = item["Status"].ToString(),
                    R_Total = item["R_Total"].ToString(),
                    R_Pending = item["R_Pending"].ToString(),
                    R_Status = item["R_Status"].ToString(),
                    SerialNo = item["SerialNo"].ToString(),
                    InitOn = item["InitOn"].ToString(),
                    Campus = item["CampusCode"].ToString()
                });
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", items);
        }

        public async Task<ApiResponse> GetDraftItemSummary(string EmpCode, string AppType, string CampusCode, string RefNo)
        {
            DataTable summary = await _approvalRepository.GetDraftedSummary(EmpCode, AppType, CampusCode, RefNo);
            DraftedSummaryResponse result = new DraftedSummaryResponse();
            if (summary.Rows.Count > 0)
            {
                result.ReferenceNo = summary.Rows[0]["ReferenceNo"].ToString();
                result.Campus = summary.Rows[0]["CampusCode"].ToString();
                result.Type = summary.Rows[0]["AppType"].ToString();
                result.Total = summary.Rows[0]["Total"].ToString();
                result.Amt = summary.Rows[0]["Amt"].ToString();
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", result);
        }

        public async Task<ApiResponse> GenerateApproval(string EmpCode, GeneratePurchaseApprovalRequest GeneratePurchaseApproval)
        {
            DataTable PurchaseApprovalRefNo = await _approvalRepository.GeneratePurchaseApprovalRefNo();

            string ApprovalRefNo = string.Empty;

            if (PurchaseApprovalRefNo.Rows.Count > 0)
            {
                ApprovalRefNo = PurchaseApprovalRefNo?.Rows[0][0].ToString() ?? string.Empty;
            }

            if (Convert.ToDateTime(GeneratePurchaseApproval.ApprovalDate) > Convert.ToDateTime(GeneratePurchaseApproval.ApprovalTillDate))
            {
                if (await _inclusiveService.IsUserAllowed(EmpCode, UserRolePermission.OpenPostFacto))
                {
                    GeneratePurchaseApproval.ApprovalType = "Post Facto - " + GeneratePurchaseApproval.ApprovalType;
                }
                else
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry! Post Facto Approval Not Allowed In Your Case. For Post Facto Please Take Permission From Concern Authority.");
                }
            }

            int totdays = Convert.ToInt32((Convert.ToDateTime(GeneratePurchaseApproval.ApprovalTillDate) - Convert.ToDateTime(GeneratePurchaseApproval.ApprovalDate)).TotalDays);
            int insresult = await _approvalRepository.SubmitPurchaseBill(EmpCode, GeneratePurchaseApproval, ApprovalRefNo, totdays.ToString());


            return new ApiResponse(StatusCodes.Status200OK, "Success", "Appproval Generate Successfully");
        }


        public async Task<ApiResponse> GetApprovalSessions()
        {
            using (DataTable sessionsList = await _approvalRepository.GetApprovalSessions())
            {
                List<string> sessions = new List<string>();
                foreach (DataRow row in sessionsList.Rows)
                {
                    sessions.Add(row["Session"]?.ToString() ?? string.Empty);
                }

                string currentFinanceSession = _generalService.GetFinancialSession(DateTime.Now);
                if (!sessions.Contains(currentFinanceSession))
                {
                    sessions.Insert(0, currentFinanceSession);
                }

                return new ApiResponse(StatusCodes.Status200OK, "Success", sessions);
            }

        }
        public async Task<ApiResponse> GetApprovalFinalAuthorities(GetApprovalFinalAuthoritiesRequest? requestDetails)
        {
            List<FinalAuthoritiesResponse> authorities = new List<FinalAuthoritiesResponse>();
            if (!_generalService.IsValidCampusCode(requestDetails?.CampusCode))
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", authorities);
            }

            using (DataTable authoritiesList = await _approvalRepository.GetApprovalFinalAuthority(requestDetails?.CampusCode))
            {
                foreach (DataRow row in authoritiesList.Rows)
                {
                    authorities.Add(new FinalAuthoritiesResponse
                    {
                        EmployeeCode = row["Employee_Code"]?.ToString() ?? string.Empty,
                        Name = row["Name"]?.ToString() ?? string.Empty,
                        Designation = row["Designation"]?.ToString() ?? string.Empty,
                        ApprovalCategory = row["ApprovalCategory"]?.ToString() ?? string.Empty,
                        LimitFrom = _generalService.StringToLong(row["LimitFrom"]?.ToString() ?? "0"),
                        LimitTo = _generalService.StringToLong(row["LimitTo"]?.ToString() ?? "0"),
                    });
                }


                if (!string.IsNullOrEmpty(requestDetails?.ApprovalType))
                {
                    authorities = authorities
                         .Where(a => a.ApprovalCategory != null && a.ApprovalCategory.Equals(requestDetails.ApprovalType, StringComparison.OrdinalIgnoreCase))
                         .ToList();

                }

                if (!string.IsNullOrEmpty(requestDetails?.Amount?.ToString()) && requestDetails?.Amount > 0 && requestDetails.Amount.HasValue)
                {
                    authorities = authorities
                        .Where(a => a.LimitFrom.HasValue && a.LimitTo.HasValue &&
                                     a.LimitFrom.Value <= requestDetails.Amount.Value &&
                                    a.LimitTo.Value >= requestDetails.Amount.Value)
                        .ToList();
                }

                return new ApiResponse(StatusCodes.Status200OK, "Success", authorities);
            }
        }

        public async Task<ApiResponse> GetApprovalNumber3Authorities(GetNumber3AuthorityRequest? search)
        {
            List<EmployeesListResponse> employeesList = new List<EmployeesListResponse>();
            if (string.IsNullOrEmpty(search?.CampusCode) || !_generalService.IsValidCampusCode(search?.CampusCode) || string.IsNullOrEmpty(search?.Name) || search?.Name?.Length < 3)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", employeesList);
            }
            bool OnlyNonMathuraAuthoritiesRequired = false;
            if (search?.CampusCode.Equals("101", StringComparison.OrdinalIgnoreCase) == false)
            {
                using (DataTable nonMathuraAuthoritiesDefined = await _approvalRepository.IsNonMathuraNumber3AuthoritiesDefined())
                {
                    if (nonMathuraAuthoritiesDefined.Rows.Count > 0)
                    {
                        OnlyNonMathuraAuthoritiesRequired = true;
                    }
                }
            }

            using (DataTable employeesDataTable = await _approvalRepository.GetApprovalNumber3Authorities(search?.Name, OnlyNonMathuraAuthoritiesRequired))
            {
                foreach (DataRow row in employeesDataTable.Rows)
                {
                    employeesList.Add(new EmployeesListResponse
                    {
                        Text = row["Text"]?.ToString() ?? string.Empty,
                        Value = row["Value"]?.ToString() ?? string.Empty,
                        EmployeeCode = row["employee_code"]?.ToString() ?? string.Empty
                    });
                }
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", employeesList);
        }

        public async Task<ApiResponse> DeleteApprovalDraft(string? emploeeId, DeleteApprovalDraftRequest? deleteRequest)
        {

            using (DataTable draftItemsTable = await _approvalRepository.CheckIsApprovalDraftItemsExists(emploeeId, deleteRequest))
            {
                if (draftItemsTable.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No Item exist to delete in selected criteria...");
                }

                int deletedCount = await _approvalRepository.DeleteApprovalDraftItems(emploeeId, deleteRequest);

                if (deletedCount > 0)
                {
                    return new ApiResponse(StatusCodes.Status200OK, "Success", $"{deletedCount} Draft Item(s) deleted successfully.");
                }
                else
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Failed to delete draft items. Please try again.");
                }
            }
        }

        public async Task<ApiResponse> DeleteDraftedItem(string ItemId)
        {
            using (DataTable dt = await _approvalRepository.CheckDeletedDraftedItem(ItemId))
            {
                if (dt.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No Item exist to delete in selected criteria...");
                }
                else
                {
                    int ins = await _approvalRepository.DeletedDraftedItem(ItemId);
                    return new ApiResponse(StatusCodes.Status200OK, "Success", $"{ins} Draft Item(s) deleted successfully.");
                }
            }
        }

        public async Task<ApiResponse> GetMyApprovals(string? emploeeId, string? type, AprrovalsListRequest? search)
        {

            List<MyApprovalReponse> approvals = new List<MyApprovalReponse>();

            bool IsAllowedDisableBill = await _inclusiveService.IsUserAllowed(emploeeId, ENUMS.Inclusive.UserRolePermission.AllowDisabledBill);

            bool OnlySelfApprovals = (type == "STORE" && !await _inclusiveService.IsUserAllowed(emploeeId, ENUMS.Inclusive.UserRolePermission.AllowAllApproval));

            using (DataTable approvalsTable = await _approvalRepository.GetMyApprovals(emploeeId, OnlySelfApprovals, search))
            {
                foreach (DataRow row in approvalsTable.Rows)
                {
                    MyApprovalReponse approval = new MyApprovalReponse(row);


                    approval.InitiatedByName = _generalService.ToTitleCase(approval.InitiatedByName ?? string.Empty);
                    approval.App1Name = _generalService.ToTitleCase(approval.App1Name ?? string.Empty);
                    approval.App2Name = _generalService.ToTitleCase(approval.App2Name ?? string.Empty);
                    approval.App3Name = _generalService.ToTitleCase(approval.App3Name ?? string.Empty);
                    approval.App4Name = _generalService.ToTitleCase(approval.App4Name ?? string.Empty);


                    DataTable dtIsComparisonDefined = await _approvalRepository.CheckIsApprovalComparisonDefined(approval.ReferenceNo ?? string.Empty);

                    if (dtIsComparisonDefined.Rows.Count > 0 || approval.CanDeleteApproval == true)
                    {
                        approval.OpenComparisionChart = true;
                    }

                    if (IsAllowedDisableBill && (approval.Status == "Pending" || approval.Status == "Approved") && string.IsNullOrWhiteSpace(approval.BillId) && approval.BillRequired == "Yes")
                    {
                        approval.CanLockBillStatus = true;
                    }

                    if (_generalService.IsFileExists($"Uploads/Approvals/{approval.ReferenceNo}.xlsx"))
                    {
                        approval.CanOpenExcel = true;
                    }
                    approvals.Add(approval);
                }
            }
            DataTable dts = await _approvalRepository.GetMyApprovalsCount(emploeeId, OnlySelfApprovals, search);
            int TotalRecord = 0;
            if (dts.Rows.Count > 0)
            {
                TotalRecord = Convert.ToInt32(dts.Rows[0][0].ToString());
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", new { Approvals = approvals, TotalRecord = TotalRecord });

        }

        public async Task<ApiResponse> DeleteApproval(string? emploeeId, string? referenceNo)
        {

            using (DataTable dtIsExist = await _approvalRepository.CheckIsApprovalExists(referenceNo))
            {
                if (dtIsExist.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No Valid approval exist with given reference no to delete.");
                }
            }


            int deletedCount = await _approvalRepository.DeleteApproval(emploeeId, referenceNo);

            if (deletedCount > 0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", $"Approval deleted successfully.");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Failed to delete approval. Please try again.");
            }
        }
        public async Task<ApiResponse> GETDRAFt(string emploeeId)
        {
            DataTable data = await _approvalRepository.GetDraft(emploeeId);
            List<DraftResponse> lst = new List<DraftResponse>();
            if (data.Rows.Count > 0)
            {
                //AppType,DraftName,CampusCode,COUNT(ReferenceNo) 'ItemCount',SUM(Balance) 'TotalBalance'
                foreach (DataRow dr in data.Rows)
                    lst.Add(new DraftResponse
                    {
                        AppType = dr["AppType"].ToString(),
                        DraftName = dr["DraftName"].ToString(),
                        CampusCode = dr["CampusCode"].ToString(),
                        ItemCount = Convert.ToInt32(dr["ItemCount"].ToString()),
                        Balance = (dr["TotalBalance"].ToString()),
                        ReferenceNo = (dr["ReferenceNo"].ToString()),
                        CampusName = _generalService.CampusNameByCode(dr["CampusCode"].ToString()!)

                    });
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }
            else
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }
        }
        public async Task<ApiResponse> GetPOApprovalDetails(string? type, string? referenceNo)
        {
            using (DataTable dtApproval = await _approvalRepository.GetPOApprovalDetails(referenceNo))
            {
                if (dtApproval.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No Approval Details Found");
                }

                PrintApprovalResponse printApprovalResponse = new PrintApprovalResponse(dtApproval.Rows[0]);

                using (DataTable dtIsComparisonDefined = await _approvalRepository.CheckIsApprovalComparisonLocked(referenceNo ?? string.Empty))
                {
                    if (dtIsComparisonDefined.Rows.Count > 0)
                    {
                        printApprovalResponse.CanViewComparisonChart = true;
                    }
                }

                if (!string.IsNullOrEmpty(type) && Array.IndexOf(new string[] { "ACCOUNTS", "FINANCE OFFICER", "ACCOUNTS OFFICER", "MANAGEMENT" }, type) >= 0)
                {
                    printApprovalResponse.CanEditNote = true;
                }

                using (DataTable dtBillExpenditureDetails = await _approvalRepository.GetPOApprovalBillExpenditureDetails(referenceNo ?? string.Empty))
                {
                    List<ApprovalBillExpenditureDetails> expenditureDetails = new List<ApprovalBillExpenditureDetails>();
                    foreach (DataRow row in dtBillExpenditureDetails.Rows)
                    {
                        expenditureDetails.Add(new ApprovalBillExpenditureDetails(row));
                    }
                    printApprovalResponse.BillExpenditureDetails = expenditureDetails;
                }

                using (DataTable dtPaymentDetails = await _approvalRepository.GetApprovalPaymentDetails(referenceNo ?? string.Empty))
                {
                    ApprovalPaymentDetails approvalPaymentDetails = new ApprovalPaymentDetails();

                    List<ApprovalPaymentDetailsList> paymentDetails = new List<ApprovalPaymentDetailsList>();
                    foreach (DataRow row in dtPaymentDetails.Rows)
                    {
                        paymentDetails.Add(new ApprovalPaymentDetailsList(row));
                    }

                    approvalPaymentDetails.TotalAmount = Convert.ToDouble(printApprovalResponse.TotalAmount ?? "0");
                    approvalPaymentDetails.TotalIssueAmount = paymentDetails.Sum(p => Convert.ToDouble(p.IssuedAmount ?? "0"));

                    if (approvalPaymentDetails.TotalAmount >= approvalPaymentDetails.TotalIssueAmount)
                    {
                        approvalPaymentDetails.PaymentDetailsComment = $"Total Issue Amount @ {approvalPaymentDetails.TotalIssueAmount} Rs./- And Balance Amount @ " + (approvalPaymentDetails.TotalAmount - approvalPaymentDetails.TotalIssueAmount) + " Rs./- ";
                    }
                    else
                    {
                        approvalPaymentDetails.PaymentDetailsComment = $"Total Issue Amount @ {approvalPaymentDetails.TotalIssueAmount} Rs./- And Over Amount @ " + (approvalPaymentDetails.TotalIssueAmount - approvalPaymentDetails.TotalAmount) + " Rs./- ";
                    }

                    approvalPaymentDetails.PaymentList = paymentDetails;
                    printApprovalResponse.PaymentDetails = approvalPaymentDetails;
                }

                using (DataTable dtWarrentyDetails = await _approvalRepository.GetApprovalWarrentyDetails(referenceNo ?? string.Empty))
                {
                    List<ApprovalWarrantyDetails> warrantyDetails = new List<ApprovalWarrantyDetails>();
                    foreach (DataRow row in dtWarrentyDetails.Rows)
                    {
                        warrantyDetails.Add(new ApprovalWarrantyDetails(row));
                    }
                    printApprovalResponse.WarrentyDetails = warrantyDetails;
                }

                using (DataTable dtRepairMaintinaceDetails = await _approvalRepository.GetApprovalRepairMaintinanceDetails(referenceNo ?? string.Empty))
                {
                    List<ApprovalRepairMaintinanceDetails> repairMaintinanceDetails = new List<ApprovalRepairMaintinanceDetails>();
                    foreach (DataRow row in dtRepairMaintinaceDetails.Rows)
                    {
                        repairMaintinanceDetails.Add(new ApprovalRepairMaintinanceDetails(row));
                    }
                    printApprovalResponse.RepairMaintinaceDetails = repairMaintinanceDetails;
                }

                printApprovalResponse!.RelativePersonName = _generalService.ToTitleCase(printApprovalResponse?.RelativePersonName ?? string.Empty);
                printApprovalResponse!.FirmName = _generalService.ToTitleCase(printApprovalResponse?.FirmName ?? string.Empty);
                printApprovalResponse!.FirmAddress = _generalService.ToTitleCase(printApprovalResponse?.FirmAddress ?? string.Empty);
                printApprovalResponse!.App1Name = _generalService.ToTitleCase(printApprovalResponse?.App1Name ?? string.Empty);
                printApprovalResponse!.App2Name = _generalService.ToTitleCase(printApprovalResponse?.App2Name ?? string.Empty);
                printApprovalResponse!.App3Name = _generalService.ToTitleCase(printApprovalResponse?.App3Name ?? string.Empty);
                printApprovalResponse!.App4Name = _generalService.ToTitleCase(printApprovalResponse?.App4Name ?? string.Empty);

                printApprovalResponse!.App1Designation = _generalService.ToTitleCase(printApprovalResponse?.App1Designation ?? string.Empty);
                printApprovalResponse!.App2Designation = _generalService.ToTitleCase(printApprovalResponse?.App2Designation ?? string.Empty);
                printApprovalResponse!.App3Designation = _generalService.ToTitleCase(printApprovalResponse?.App3Designation ?? string.Empty);
                printApprovalResponse!.App4Designation = _generalService.ToTitleCase(printApprovalResponse?.App4Designation ?? string.Empty);

                printApprovalResponse!.ForDepartment = _generalService.ToTitleCase(printApprovalResponse?.ForDepartment ?? string.Empty);
                printApprovalResponse!.AdditionalName = _generalService.ToTitleCase(printApprovalResponse?.AdditionalName ?? string.Empty);

                printApprovalResponse!.TotalAmount = _generalService.ConvertToTwoDecimalPlaces(printApprovalResponse?.TotalAmount ?? "0.00");
                printApprovalResponse!.DiscountOverAll = _generalService.ConvertToTwoDecimalPlaces(printApprovalResponse?.DiscountOverAll ?? "0.00");
                printApprovalResponse!.Amount = _generalService.ConvertToTwoDecimalPlaces(printApprovalResponse?.Amount ?? "0.00");
                printApprovalResponse!.Other = _generalService.ConvertToTwoDecimalPlaces(printApprovalResponse?.Other ?? "0.00");

                printApprovalResponse!.DiscountAmount = _generalService.ConvertToTwoDecimalPlaces((_generalService.StringToLong(printApprovalResponse!.Amount) * Convert.ToDouble(printApprovalResponse!.DiscountOverAll) / 100).ToString());

                printApprovalResponse!.DiscountOverAll = $"{printApprovalResponse!.DiscountOverAll}%";

                using (DataTable dtItems = await _approvalRepository.GetApprovalItems(referenceNo))
                {

                    double totalTax = 0;
                    double totalAmount = 0;
                    double totalPay = 0;
                    double totalDiscount = 0;

                    List<ApprovalItemDetails>? items = new List<ApprovalItemDetails>();
                    foreach (DataRow row in dtItems.Rows)
                    {
                        ApprovalItemDetails item = new ApprovalItemDetails(row);

                        item.ItemName = _generalService.ToTitleCase(item.ItemName ?? string.Empty);
                        item.Unit = _generalService.ToTitleCase(item.Unit ?? string.Empty);

                        item.TotalAmount = _generalService.ConvertToTwoDecimalPlaces(item.TotalAmount ?? "0.00");
                        item.VatPer = _generalService.ConvertToTwoDecimalPlaces(item.VatPer ?? "0.00");
                        item.DisPer = _generalService.ConvertToTwoDecimalPlaces(item.DisPer ?? "0.00");

                        double actualAmount = _generalService.ConvertToDouble(item.ActualAmount ?? "0.00");
                        totalAmount += actualAmount;
                        double myDiscount = 0;
                        double dt = _generalService.ConvertToDouble(item.DisPer ?? "0.00");

                        if (dt > 0)
                        {
                            myDiscount = Math.Round(actualAmount * dt / 100, 2);
                            actualAmount = actualAmount - myDiscount;
                        }
                        double myTotalAmount = _generalService.ConvertToDouble(item.TotalAmount ?? "0.00");
                        totalPay += myTotalAmount;
                        totalDiscount += myDiscount;
                        totalTax = totalTax + (myTotalAmount - actualAmount);

                        using (DataTable dtPreviousDetails = await _approvalRepository.GetApprovalItemPreviousPurchaseDetails(item.ReferenceNo, referenceNo))
                        {
                            if (dtPreviousDetails.Rows.Count > 0)
                            {
                                ApprovalItemPreviousDetails approvalItemPreviousDetails = new ApprovalItemPreviousDetails(dtPreviousDetails.Rows[0]);
                                item.PreviosDetails = approvalItemPreviousDetails;
                            }
                        }
                        items.Add(item);
                    }

                    printApprovalResponse!.Items = items;

                    printApprovalResponse!.SumamryActualAmount = _generalService.ConvertToTwoDecimalPlaces(totalAmount.ToString());
                    printApprovalResponse!.SumamryDiscountAmount = _generalService.ConvertToTwoDecimalPlaces(totalDiscount.ToString());
                    printApprovalResponse!.SumamryAmountAfterDiscount = _generalService.ConvertToTwoDecimalPlaces((totalAmount - totalDiscount).ToString());
                    printApprovalResponse!.SumamryTaxAmount = _generalService.ConvertToTwoDecimalPlaces(totalTax.ToString());
                    printApprovalResponse!.SumamryAmountToPay = _generalService.ConvertToTwoDecimalPlaces((totalAmount - totalDiscount + totalTax).ToString());
                    printApprovalResponse!.SumamryCashDiscountPercentage = printApprovalResponse!.DiscountOverAll;
                    printApprovalResponse!.SumamryCashDiscount = printApprovalResponse!.DiscountAmount;
                    printApprovalResponse!.SumamryOtherDiscount = printApprovalResponse!.Other;
                    printApprovalResponse!.SummaryFinalPayableAmount = printApprovalResponse!.TotalAmount;

                }

                printApprovalResponse!.VatType = _generalService.ToTitleCase(printApprovalResponse!.VatType ?? string.Empty);
                printApprovalResponse!.AmountInWords = _generalService.AmountInWords(printApprovalResponse!.TotalAmount);

                if (_generalService.ConvertToDouble(printApprovalResponse!.TotalAmount) > 100000)
                {
                    printApprovalResponse!.App4Name = "Chancellor";
                }

                using (DataTable dtRecommendedByDesignationContact = await _approvalRepository.GetApprovalEmployeeDesignationContact(printApprovalResponse!.RelativePersonID))
                {
                    if (dtRecommendedByDesignationContact.Rows.Count > 0)
                    {
                        printApprovalResponse.RelativePersonDesignation = dtRecommendedByDesignationContact.Rows[0]["deisgnation"]?.ToString() ?? string.Empty;
                        printApprovalResponse.RelativePersonContact = dtRecommendedByDesignationContact.Rows[0]["contactno"]?.ToString() ?? string.Empty;
                    }
                }

                printApprovalResponse.ViewCurrentStock = _generalService.ViewCurrentStock(printApprovalResponse.MyType);

                printApprovalResponse.ViewPreviousRate = _generalService.ViewPreviousRate(printApprovalResponse.MyType);

                return new ApiResponse(StatusCodes.Status200OK, "Success", printApprovalResponse);

            }
        }


        public async Task<ApiResponse> UpdateApprovalNote(string? employeeId, string? referenceNo, string? note)
        {
            using (DataTable dtIsExist = await _approvalRepository.CheckApprovalExistsToUpdate(referenceNo))
            {
                if (dtIsExist.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No Valid approval exist with given reference no to update note.");
                }
            }

            await _approvalRepository.UpdateApprovalNote(employeeId, referenceNo, note);

            return new ApiResponse(StatusCodes.Status200OK, "Success", $"Approval note updated successfully.");
        }

        public async Task<ApiResponse> GetEditApprovalDetails(string? referenceNo)
        {
            using (DataTable dtApprovalDetails = await _approvalRepository.GetEditApprovalDetails(referenceNo))
            {
                if (dtApprovalDetails.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No approval exist with given reference no.");
                }
                EditGetApprovalDetailsResponse editGetApprovalDetailsResponse = new EditGetApprovalDetailsResponse(dtApprovalDetails.Rows[0]);
                editGetApprovalDetailsResponse.isExcelFileUploaded = _generalService.IsFileExists($"Uploads/Approvals/{editGetApprovalDetailsResponse.ReferenceNo}.xlsx");
                return new ApiResponse(StatusCodes.Status200OK, "Success", editGetApprovalDetailsResponse);

            }
        }

        public async Task<ApiResponse> EditApprovalDetails(string referenceNo, UpdateApprovalEditDetails details, string EmpCode)
        {
            int ins = await _approvalRepository.EditApprovalDetails(referenceNo, details, EmpCode);
            if (details.File != null && details.File.Length > 0)
            {
                await _inclusiveService.SaveFile(referenceNo, "Uploads/Approvals", details.File, ".xlsx");
            }
            if (ins > 0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", $"`{ins}` record updated successfully.");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", $"No Changes In Data.");
            }
        }

        public async Task<ApiResponse> GetPurchaseApproval(string EmpCode, string EmpCodeAdd, AprrovalsListRequest details)
        {
            //await _account.GetAdditionalEmployeeCode(loginRequest?.UserId);
            DataTable d = await _accountRepository.GetAdditionalEmployeeCode(EmpCode);
            if (d.Rows.Count > 0)
            {
                EmpCodeAdd = d.Rows[0][0].ToString();
            }


            //Task<DataTable> GetApprovalDetails(string EmpCode,string EmpCodeAdd,GetApprovalRequest details)
            using (DataTable dt = await _approvalRepository.GetApprovalDetails(EmpCode, EmpCodeAdd!, details))
            {
                List<PurchaseApprovalDetails> lst = new List<PurchaseApprovalDetails>();
                foreach (DataRow dr in dt.Rows)
                {
                    PurchaseApprovalDetails pd = new PurchaseApprovalDetails();
                    pd.IsRejectable = false;
                    if (dr["App4ID"].ToString() == "GLAVIVEK" && dr["App4Status"].ToString() == "Approved") // If Vivek sir approved then no body can reject this approval
                    {
                        pd.IsRejectable = false;
                    }
                    else
                    {
                        pd.IsRejectable = CanReject(dr, EmpCode, EmpCodeAdd);
                    }

                    pd.AuthorityNumber = GetAuthNo(dr, EmpCode, EmpCodeAdd);
                    pd.CanApproval = CanApprove(dr, EmpCode, EmpCodeAdd);
                    pd.CanCancel = CanCancel(dr, EmpCode, EmpCodeAdd);
                    if (_generalService.IsFileExists($"Uploads/Approvals/{dr["ReferenceNo"].ToString()}.xlsx"))
                    {
                        pd.IsExcelFile = true;
                        pd.ExcelFileUrl = $"Uploads/Approvals/{dr["ReferenceNo"].ToString()}.xlsx";
                    }
                    else
                    {
                        pd.IsExcelFile = false;
                        pd.ExcelFileUrl = "";
                    }

                    pd.EncRelativePersonID = _generalService.Encrypt(dr["RelativePersonID"].ToString() ?? string.Empty);
                    pd.OtherDetails = dr["Test"].ToString()!;
                    string[] splt = dr["Test"].ToString()!.Split('$');
                    pd.VendorName = splt[3];
                    pd.VendorId = splt[2];
                    pd.Department = splt[1];
                    pd.CampusName = dr["CampusName"].ToString() ?? string.Empty;
                    pd.ApprovalType = dr["MyType"].ToString() ?? string.Empty;
                    pd.Status = dr["Status"].ToString() ?? string.Empty;
                    pd.BudgetAmount = dr["BudgetAmount"].ToString() ?? string.Empty;
                    pd.prevTaken = dr["PreviousTaken"].ToString() ?? string.Empty;
                    pd.TotalAmount = dr["TotalAmount"].ToString() ?? string.Empty;
                    pd.BudgetStatus = dr["BudgetStatus"].ToString() ?? string.Empty;
                    pd.IniName = dr["IniName"].ToString() ?? string.Empty;

                    pd.RelativePersonName = dr["RelativePersonName"].ToString() ?? string.Empty;
                    pd.RelativePersonID = dr["RelativePersonID"].ToString() ?? string.Empty;
                    pd.ApprovalBillStatus = dr["ReferenceBillStatus"].ToString() ?? string.Empty;
                    pd.RecievingStatus = dr["RecievingStatus"].ToString() ?? string.Empty;
                    pd.Note = dr["Note"].ToString() ?? string.Empty;
                    pd.VendorNameShow = dr["FirmName"].ToString() ?? string.Empty;
                    pd.VendorPersonName = dr["FirmPerson"].ToString() ?? string.Empty;
                    pd.VendorContactNo = dr["FirmContactNo"].ToString() ?? string.Empty;
                    pd.Maad = dr["Maad"].ToString() ?? string.Empty;

                    pd.ReferenceNo = dr["ReferenceNo"].ToString() ?? string.Empty;
                    pd.PreviousCancelRemark = dr["PreviousCancelRemark"].ToString() ?? string.Empty;
                    pd.CancelledOn = dr["CancelledOn"].ToString() ?? string.Empty;
                    pd.CancelledReason = dr["CancelledReason"].ToString() ?? string.Empty;
                    pd.RejectedReason = dr["RejectedReason"].ToString() ?? string.Empty;
                    pd.CloseOn = dr["CloseOn"].ToString() ?? string.Empty;
                    pd.CloseReason = dr["CloseReason"].ToString() ?? string.Empty;
                    pd.ByPass = dr["ByPass"].ToString() ?? string.Empty;
                    pd.BillId = dr["BillId"].ToString() ?? string.Empty;

                    pd.App1Name = dr["App1Name"].ToString() ?? string.Empty;
                    pd.App1On = dr["App1On"].ToString() ?? string.Empty;
                    pd.App1Id = dr["App1ID"].ToString() ?? string.Empty;
                    pd.App1Status = dr["App1Status"].ToString() ?? string.Empty;

                    pd.App2Name = dr["App2Name"].ToString() ?? string.Empty;
                    pd.App2On = dr["App2On"].ToString() ?? string.Empty;
                    pd.App2Id = dr["App2ID"].ToString() ?? string.Empty;
                    pd.App2Status = dr["App2Status"].ToString() ?? string.Empty;

                    pd.App3Name = dr["App3Name"].ToString() ?? string.Empty;
                    pd.App3On = dr["App3On"].ToString() ?? string.Empty;
                    pd.App3Id = dr["App3ID"].ToString() ?? string.Empty;
                    pd.App3Status = dr["App3Status"].ToString() ?? string.Empty;

                    pd.App4Name = dr["App4Name"].ToString() ?? string.Empty;
                    pd.App4On = dr["App4On"].ToString() ?? string.Empty;
                    pd.App4Id = dr["App4ID"].ToString() ?? string.Empty;
                    pd.App4Status = dr["App4Status"].ToString() ?? string.Empty;

                    pd.FinalStat = dr["FinalStat"].ToString() ?? string.Empty;
                    //ExeOn,AppDate,IniOn
                    pd.IniOn = dr["IniOn"].ToString() ?? string.Empty;
                    pd.ApprovalDate = dr["AppDate"].ToString() ?? string.Empty;
                    pd.UpTo = dr["ExeOn"].ToString() ?? string.Empty;
                    pd.Purpose = dr["Purpose"].ToString() ?? string.Empty;
                    pd.TotalItem = dr["TotalItem"].ToString() ?? string.Empty;



                    lst.Add(pd);
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }

        }

        public static int GetAuthNo(DataRow dr, string EmpCode, string EmpCodeAdd)
        {
            if (!string.IsNullOrEmpty(dr["App1ID"].ToString()) && (dr["App1ID"].ToString() == EmpCode || dr["App1ID"].ToString() == EmpCodeAdd))
            {
                return 1;
            }
            else
            {
                if (!string.IsNullOrEmpty(dr["App2ID"].ToString()) && (dr["App2ID"].ToString() == EmpCode || dr["App2ID"].ToString() == EmpCodeAdd))
                {
                    return 2;
                }
                else
                {
                    if (!string.IsNullOrEmpty(dr["App3ID"].ToString()) && (dr["App3ID"].ToString() == EmpCode || dr["App3ID"].ToString() == EmpCodeAdd))
                    {
                        return 3;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dr["App4ID"].ToString()) && (dr["App4ID"].ToString() == EmpCode || dr["App4ID"].ToString() == EmpCodeAdd))
                        {
                            return 4;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
        }

        public static bool CanApprove(DataRow dr, string EmpCode, string EmpCodeAdd)
        {
            if ((!string.IsNullOrEmpty(dr["App1ID"].ToString()) && (dr["App1ID"].ToString() == EmpCode || dr["App1ID"].ToString() == EmpCodeAdd) && dr["App1Status"].ToString() == "Pending") ||
                (!string.IsNullOrEmpty(dr["App2ID"].ToString()) && (dr["App2ID"].ToString() == EmpCode || dr["App2ID"].ToString() == EmpCodeAdd) && dr["App2Status"].ToString() == "Pending") ||
                (!string.IsNullOrEmpty(dr["App3ID"].ToString()) && (dr["App3ID"].ToString() == EmpCode || dr["App3ID"].ToString() == EmpCodeAdd) && dr["App3Status"].ToString() == "Pending") ||
                (!string.IsNullOrEmpty(dr["App4ID"].ToString()) && (dr["App4ID"].ToString() == EmpCode || dr["App4ID"].ToString() == EmpCodeAdd) && dr["App4Status"].ToString() == "Pending"))
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CanCancel(DataRow dr, string EmpCode, string AdditinalCode)
        {
            if (dr["Status"].ToString() == "Approved" && dr["BillId"]?.ToString()?.Length <= 0)
            {
                if (!string.IsNullOrEmpty(dr["App2ID"].ToString()) && ((dr["App2ID"].ToString() == EmpCode || dr["App2ID"].ToString() == AdditinalCode)) && !dr["ByPass"].ToString()!.Contains("Member2,"))
                {
                    return true;
                }
                if (!string.IsNullOrEmpty(dr["App3ID"].ToString()) && ((dr["App3ID"].ToString() == EmpCode || dr["App3ID"].ToString() == AdditinalCode)) && !dr["ByPass"].ToString()!.Contains("Member2,"))
                {
                    return true;
                }
                if (!string.IsNullOrEmpty(dr["App4ID"].ToString()) && ((dr["App4ID"].ToString() == EmpCode || dr["App4ID"].ToString() == AdditinalCode)) && !dr["ByPass"].ToString()!.Contains("Member2,"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public static bool CanReject(DataRow dr, string EmpCode, string AdditinalCode)
        {
            // if (dr["Status"].ToString() == "Approved" && dr["BillId"]?.ToString()?.Length <= 0)
            {
                if (!string.IsNullOrEmpty(dr["App2ID"].ToString()) && ((dr["App2ID"].ToString() == EmpCode || dr["App2ID"].ToString() == AdditinalCode)) && (dr["App2Status"].ToString() == "Pending") && dr["Status"].ToString() == "Pending")
                {
                    return true;
                }
                if (!string.IsNullOrEmpty(dr["App3ID"].ToString()) && ((dr["App3ID"].ToString() == EmpCode || dr["App3ID"].ToString() == AdditinalCode)) && (dr["App3Status"].ToString() == "Pending") && dr["Status"].ToString() == "Pending")
                {
                    return true;
                }
                if (!string.IsNullOrEmpty(dr["App4ID"].ToString()) && ((dr["App4ID"].ToString() == EmpCode || dr["App4ID"].ToString() == AdditinalCode)) && (dr["App4Status"].ToString() == "Pending") && dr["Status"].ToString() == "Pending")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //else
            //{
            //    return false;
            //}
        }



        public async Task<ApiResponse> ValidateRepairWarrnty(string CampusCode, string SRNo)
        {
            string baseurl = "http://hostel.glauniversity.in:84/inventoryservices.asmx/warrentyservice";
            using (DataTable d = await _approvalRepository.GetBaseUrl())
            {
                DataRow[] dr = d.Select("Type='WebService' and Tag='BaseUrlWarrenty'");
                if (dr.Length > 0)
                {
                    baseurl = dr[0][1].ToString();
                }

                string result = _inclusiveService.CallWebService(baseurl, SRNo, "@1@", "", CampusCode.ToString());
                return new ApiResponse(StatusCodes.Status200OK, "Success", result);
            }
        }

        public async Task<ApiResponse> PassPurchaseApproval(string? employeeCode, PassApprovalRequest? passRequest)
        {

            using (DataTable dtIsExist = await _approvalRepository.CheckPassApprovalValidExists(employeeCode, passRequest?.ReferenceNo, passRequest?.AuthorityNumber))
            {
                if (dtIsExist.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No Valid approval exist with given reference no to approve.");
                }
            }

            EmployeeDetails employeeDetails = await _inclusiveService.GetEmployeeDetailsByEmployeeCode(employeeCode ?? string.Empty);

            await _approvalRepository.ApproveApprovalRequest(employeeCode, employeeDetails?.Name, employeeDetails?.Designation, passRequest);

            using (DataTable dt = await _approvalRepository.GetStatusApprovalForFinalStatus(passRequest?.ReferenceNo))
            {
                if (dt.Rows.Count > 0 && dt.Rows[0][0]?.ToString() == "Approved")
                {
                    await _approvalRepository.UpdateApprovalFinalApproved(passRequest?.ReferenceNo);
                }
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success");
        }

        public async Task<ApiResponse> RejectPurchaseApproval(string? employeeCode, RejectACancelpprovalRequest? rejectRequest)
        {

            using (DataTable dtIsExist = await _approvalRepository.CheckPassApprovalValidExists(employeeCode, rejectRequest?.ReferenceNo, rejectRequest?.AuthorityNumber))
            {
                if (dtIsExist.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No Valid approval exist with given reference no to reject.");
                }
            }

            using (DataTable dtVivekSirApproved = await _approvalRepository.CheckIsVivekSirApprovedApproval(rejectRequest?.ReferenceNo))
            {
                if (dtVivekSirApproved.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! You cannot delete this record due to final approval, Approved by concerned authority.");
                }
            }

            EmployeeDetails employeeDetails = await _inclusiveService.GetEmployeeDetailsByEmployeeCode(employeeCode ?? string.Empty);

            await _approvalRepository.RejectApprovalRequest(employeeCode, employeeDetails?.Name, employeeDetails?.Designation, rejectRequest);


            return new ApiResponse(StatusCodes.Status200OK, "Success");
        }

        public async Task<ApiResponse> CancelPurchaseApproval(string? employeeCode, RejectACancelpprovalRequest? cancelRequest)
        {
            using (DataTable dtIsExist = await _approvalRepository.CheckCanCancelApproval(employeeCode, cancelRequest?.ReferenceNo, cancelRequest?.AuthorityNumber))
            {
                if (dtIsExist.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No Valid approval exist with given reference no to cancel.");
                }
            }

            EmployeeDetails employeeDetails = await _inclusiveService.GetEmployeeDetailsByEmployeeCode(employeeCode ?? string.Empty);

            await _approvalRepository.CancelApprovalRequest(employeeCode, employeeDetails?.Name, cancelRequest);

            return new ApiResponse(StatusCodes.Status200OK, "Success");
        }

        public async Task<ApiResponse> GetApprovalItems(string? referenceNo)
        {
            using (DataTable dtItems = await _approvalRepository.GetApprovalItems(referenceNo))
            {

                List<ApprovalItemDetails>? items = new List<ApprovalItemDetails>();
                foreach (DataRow row in dtItems.Rows)
                {
                    ApprovalItemDetails item = new ApprovalItemDetails(row);

                    item.ItemName = _generalService.ToTitleCase(item.ItemName ?? string.Empty);
                    item.Unit = _generalService.ToTitleCase(item.Unit ?? string.Empty);

                    item.TotalAmount = _generalService.ConvertToTwoDecimalPlaces(item.TotalAmount ?? "0.00");
                    item.VatPer = _generalService.ConvertToTwoDecimalPlaces(item.VatPer ?? "0.00");
                    item.DisPer = _generalService.ConvertToTwoDecimalPlaces(item.DisPer ?? "0.00");
                    items.Add(item);
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", items);
            }


        }

        public async Task UpdatePurchaseApprovalSummaryDetailsAfterEditItem(string? referenceNo)
        {
            using (DataTable dtSummary = await _approvalRepository.GetApprovalSummaryAmountDetails(referenceNo))
            {
                if (dtSummary.Rows.Count > 0)
                {
                    double discountOverAll = _generalService.ConvertToDouble(dtSummary.Rows[0]["Discount"]?.ToString() ?? "0");
                    double OtherCharges = _generalService.ConvertToDouble(dtSummary.Rows[0]["OtherCharges"]?.ToString() ?? "0");

                    using (DataTable dtNewItemsDetails = await _approvalRepository.GetApprovalTotalItemsAndAmount(referenceNo))
                    {
                        if (dtNewItemsDetails.Rows.Count > 0)
                        {
                            int newTotalItems = Convert.ToInt32(dtNewItemsDetails.Rows[0]["TotalItems"]?.ToString() ?? "0");
                            double newItemsAmount = _generalService.ConvertToDouble(dtNewItemsDetails.Rows[0]["TotalAmount"]?.ToString() ?? "0");
                            int newPayableAmount = (int)Math.Round((newItemsAmount + OtherCharges) - discountOverAll);

                            if (newPayableAmount < 0)
                            {
                                newPayableAmount = 0;
                            }
                            await _approvalRepository.UpdateApprovalSummaryItemsCountAmount(referenceNo, newTotalItems, newItemsAmount, newPayableAmount);
                        }
                    }

                }
            }

        }

        public async Task<ApiResponse> DeleteItemFromApproval(string? employeeId, DeleteApprovalItemRequest? deleteRequest)
        {
            using (DataTable dtIsExist = await _approvalRepository.GetApprovalIsCompletePending(deleteRequest?.ReferenceNo))
            {
                if (dtIsExist.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No Valid approval exists with given reference no to delete item.");
                }
            }

            using (DataTable dtIsItemExist = await _approvalRepository.GetApprovalHasItemCode(deleteRequest?.ReferenceNo, deleteRequest?.ItemCode))
            {
                if (dtIsItemExist.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No Valid item exists with given item code in the approval to delete.");
                }
            }

            using (DataTable dtIsOtherItemsExist = await _approvalRepository.GetApprovalHasOtherItems(deleteRequest?.ReferenceNo, deleteRequest?.ItemCode))
            {
                if (dtIsOtherItemsExist.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Approval has no other items. So you cannot delete this last item.");
                }
            }

            await _approvalRepository.DeleteItemFromApproval(employeeId, deleteRequest);

            await UpdatePurchaseApprovalSummaryDetailsAfterEditItem(deleteRequest?.ReferenceNo);

            return new ApiResponse(StatusCodes.Status200OK, "Success", "Item deleted successfully from approval.");
        }
        public async Task<ApiResponse> AddItemInCreatedApproval(string? employeeId, AddUpdateItemInApprovalRequest? addRequest)
        {
            using (DataTable dtIsExist = await _approvalRepository.GetApprovalIsCompletePending(addRequest?.ReferenceNo))
            {
                if (dtIsExist.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No Valid approval exists with given reference no to add item.");
                }
            }

            using (DataTable dtIsItemExist = await _approvalRepository.GetApprovalHasItemCode(addRequest?.ReferenceNo, addRequest?.ItemCode))
            {
                if (dtIsItemExist.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Item already exists in the approval.");
                }
            }

            ItemDetails itemDetails = await _inclusiveService.GetItemDetailsByItemCode(addRequest?.ItemCode ?? string.Empty);
            if (string.IsNullOrWhiteSpace(itemDetails?.ItemCode))
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid item code provided to add item in approval.");
            }

            await _approvalRepository.AddItemInCreatedApproval(employeeId, addRequest, itemDetails);

            await UpdatePurchaseApprovalSummaryDetailsAfterEditItem(addRequest?.ReferenceNo);

            return new ApiResponse(StatusCodes.Status200OK, "Success", "Item added successfully in approval.");
        }

        public async Task<ApiResponse> UpdateItemInCreatedApproval(string? employeeId, AddUpdateItemInApprovalRequest? updateRequest)
        {
            using (DataTable dtIsExist = await _approvalRepository.GetApprovalIsCompletePending(updateRequest?.ReferenceNo))
            {
                if (dtIsExist.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! No Valid approval exists with given reference no to update item.");
                }
            }

            using (DataTable dtIsItemExist = await _approvalRepository.GetApprovalHasItemCode(updateRequest?.ReferenceNo, updateRequest?.ItemCode))
            {
                if (dtIsItemExist.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Approval does not contain the specified item.");
                }
            }

            await _approvalRepository.UpdateItemInCreatedApproval(employeeId, updateRequest);

            await UpdatePurchaseApprovalSummaryDetailsAfterEditItem(updateRequest?.ReferenceNo);

            return new ApiResponse(StatusCodes.Status200OK, "Success", "Item updated successfully in approval.");
        }

    }
}
