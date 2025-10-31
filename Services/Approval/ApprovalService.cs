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
        public ApprovalService(IApprovalRepository approvalRepository, IGeneral generalService, IInclusiveService inclusiveService)
        {
            _approvalRepository = approvalRepository;
            _generalService = generalService;
            _inclusiveService = inclusiveService;
        }
        public async Task<ApiResponse> AddItemDraft(AddStockItemRequest AddStockItem,string EmpCode)
        {
            
            
             string RefNo=string.Empty;
             DataTable refNoDataTable=await _approvalRepository.GetDraftItemRefNo(EmpCode,AddStockItem.ApprovalType);
             if (refNoDataTable.Rows.Count > 0)
             {
                 RefNo=refNoDataTable.Rows[0][0].ToString();
             }
             else
             {
                 refNoDataTable = await _approvalRepository.GetAutoDraftItemRefNo();
                 if (refNoDataTable.Rows.Count > 0)
                 {
                     RefNo = refNoDataTable.Rows[0][0].ToString();
                 }
             }
             int AddItem=await _approvalRepository.AddDraftItem(RefNo,AddStockItem,EmpCode);
             if (AddItem > 0)
             {
                 return new ApiResponse(StatusCodes.Status200OK,"Success","Item Add Successfully");
             }
             else
             {
                 return new ApiResponse(StatusCodes.Status500InternalServerError,"Error","Add Failed");
             }
             
        }

        public async Task<ApiResponse> GetDraftedItem(string EmpCode, string AppType, string CampusCode)
        {
            DataTable itms=await _approvalRepository.GetDraftedItem(EmpCode,AppType,CampusCode);
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
                    Campus =  item["CampusCode"].ToString()
                });
            }
            return new ApiResponse(StatusCodes.Status200OK,"Success",items);
        }

        public async Task<ApiResponse> GetDraftItemSummary(string EmpCode, string AppType, string CampusCode)
        {
            DataTable summary=await _approvalRepository.GetDraftedSummary(EmpCode,AppType,CampusCode);
            DraftedSummaryResponse result = new DraftedSummaryResponse();
            if (summary.Rows.Count > 0)
            {
                result.ReferenceNo = summary.Rows[0]["ReferenceNo"].ToString();
                result.Campus = summary.Rows[0]["CampusCode"].ToString();
                result.Type = summary.Rows[0]["AppType"].ToString();
                result.Total = summary.Rows[0]["Total"].ToString();
                result.Amt = summary.Rows[0]["Amt"].ToString();
            }
            return new ApiResponse(StatusCodes.Status200OK,"Success",result);
        }

        public async Task<ApiResponse> GenerateApproval(string EmpCode,GeneratePurchaseApprovalRequest GeneratePurchaseApproval)
        {
            DataTable PurchaseApprovalRefNo =await _approvalRepository.GeneratePurchaseApprovalRefNo();
           
            string ApprovalRefNo=string.Empty;
           
            if (PurchaseApprovalRefNo.Rows.Count > 0)
            {
                ApprovalRefNo = PurchaseApprovalRefNo?.Rows[0][0].ToString() ?? string.Empty;
            }

            if (Convert.ToDateTime(GeneratePurchaseApproval.ApprovalDate) > Convert.ToDateTime(GeneratePurchaseApproval.ApprovalTillDate))
            {
                if (await _inclusiveService.IsUserAllowed(EmpCode,UserRolePermission.OpenPostFacto))
                {
                    GeneratePurchaseApproval.ApprovalType = "Post Facto - " + GeneratePurchaseApproval.ApprovalType;
                }
                else
                {
                  return  new ApiResponse(StatusCodes.Status422UnprocessableEntity,"Error","Sorry! Post Facto Approval Not Allowed In Your Case. For Post Facto Please Take Permission From Concern Authority.");
                }
            }

            int totdays = Convert.ToInt32((Convert.ToDateTime(GeneratePurchaseApproval.ApprovalTillDate) - Convert.ToDateTime(GeneratePurchaseApproval.ApprovalDate)).TotalDays);
            int insresult=await _approvalRepository.SubmitPurchaseBill(EmpCode,GeneratePurchaseApproval,ApprovalRefNo,totdays.ToString());
            
            
            return new ApiResponse(StatusCodes.Status200OK,"Success","Appproval Generate Successfully");
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
            using (DataTable dt=await _approvalRepository.CheckDeletedDraftedItem(ItemId))
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

                    DataTable dtIsComparisonDefined = await _approvalRepository.CheckIsApprovalComparisonDefined(approval.ReferenceNo ?? string.Empty);

                    if (dtIsComparisonDefined.Rows.Count > 0 || approval.CanDeleteApproval == true)
                    {
                        approval.OpenComparisionChart = true;
                    }

                    if (IsAllowedDisableBill && (approval.Status == "Pending" || approval.Status == "Approved") && string.IsNullOrWhiteSpace(approval.BillId) && approval.BillRequired == "Yes")
                    {
                        approval.CanLockBillStatus = true;
                    }

                    if (await _generalService.IsFileExists($"Uploads/Approvals/{approval.ReferenceNo}.xlsx"))
                    {
                        approval.CanOpenExcel = true;
                    }
                    approvals.Add(approval);
                }
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", approvals);

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
    }
}
