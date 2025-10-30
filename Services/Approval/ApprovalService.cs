using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Approval;
using AdvanceAPI.DTO.Inclusive;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Approval;
using System.Data;

namespace AdvanceAPI.Services.Approval
{
    public class ApprovalService : IApprovalService
    {
        private readonly IApprovalRepository _approvalRepository;
        private readonly IGeneral _generalService;
        public ApprovalService(IApprovalRepository approvalRepository, IGeneral generalService)
        {
            _approvalRepository = approvalRepository;
            _generalService = generalService;
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
                ApprovalRefNo=PurchaseApprovalRefNo.Rows[0][0].ToString();
            }

            if (Convert.ToDateTime(GeneratePurchaseApproval.ApprovalDate) > Convert.ToDateTime(GeneratePurchaseApproval.ApprovalTillDate))
            {
                string AllowPost = "0";
                if (await _generalService.CheckColumn("OpenPostFacto", EmpCode))
                {
                    GeneratePurchaseApproval.ApprovalType = "Post Facto - " + GeneratePurchaseApproval.ApprovalType;
                }
                else
                {
                    new ApiResponse(StatusCodes.Status422UnprocessableEntity,"Error","Sorry! Post Facto Approval Not Allowed In Your Case. For Post Facto Please Take Permission From Concern Authority.");
                }
            }
            
            
            
            
            return new ApiResponse(StatusCodes.Status200OK,"Success",PurchaseApprovalRefNo);
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
        public async Task<ApiResponse> GetApprovalFinalAuthorities(string? campusCode)
        {
            List<FinalAuthoritiesResponse> authorities = new List<FinalAuthoritiesResponse>();
            if (!_generalService.IsValidCampusCode(campusCode))
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", authorities);
            }

            using (DataTable authoritiesList = await _approvalRepository.GetApprovalFinalAuthority(campusCode))
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

    }
}
