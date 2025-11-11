using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Account;
using AdvanceAPI.DTO.Inclusive;
using AdvanceAPI.ENUMS.Inclusive;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Inclusive;

using System.Data;
using System.Net;
using System.Web;

namespace AdvanceAPI.Services.Inclusive
{
    public class InclusiveService : IInclusiveService
    {
        private readonly ILogger<InclusiveService> _logger;
        private readonly IGeneral _general;
        private readonly IIncusiveRepository _inclusive;

        public InclusiveService(ILogger<InclusiveService> logger, IGeneral general, IIncusiveRepository inclusive)
        {
            _logger = logger;
            _general = general;
            _inclusive = inclusive;
        }

        public async Task<ApiResponse> GetCampusList(string? employeeId)
        {
            DataTable campusList = await _inclusive.GetEmployeeCampus(employeeId);

            List<CampusResponse> campusResponses = new List<CampusResponse>();
            foreach (DataRow row in campusList.Rows)
            {
                CampusResponse campus = new CampusResponse
                {
                    CampusCode = row["CampusCode"]?.ToString() ?? string.Empty,
                    CampusName = row["CampusName"]?.ToString() ?? string.Empty
                };
                campusResponses.Add(campus);
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", campusResponses);
        }

        public async Task<ApiResponse> GetApprovalType()
        {
            using (DataTable approvalList = await _inclusive.GetApprovalType())
            {
                List<ApprovalTypeResponse> approvalTypeResponses = new List<ApprovalTypeResponse>();
                foreach (DataRow row in approvalList.Rows)
                {
                    approvalTypeResponses.Add(new ApprovalTypeResponse
                    {
                        ApprovalName = row[0].ToString() ?? string.Empty,
                    });
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", approvalTypeResponses);
            }

        }

        public async Task<ApiResponse> GetPurchaseDepartment()
        {
            using (DataTable department = await _inclusive.GetPurchaseDepartment())
            {
                List<PurchaseDepartmentResponse> purchaseDepartmentResponses = new List<PurchaseDepartmentResponse>();
                foreach (DataRow row in department.Rows)
                {
                    purchaseDepartmentResponses.Add(new PurchaseDepartmentResponse
                    {
                        DepartmentName = row[0].ToString() ?? string.Empty,
                    });
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", purchaseDepartmentResponses);
            }
        }
        public async Task<ApiResponse> GetItems(GetPurchaseItemRequest getPurchaseItemRequest)
        {

            List<ItemListResponse> itemslst = new List<ItemListResponse>();
            using (DataTable Items = await _inclusive.GetItems(getPurchaseItemRequest))
            {
                foreach (DataRow row in Items.Rows)
                {
                    itemslst.Add(new ItemListResponse
                    {
                        //ItemCode,ItemName,Size,Make,Unit,DepartmentCode
                        ItemCode = (row["ItemCode"].ToString()),
                        ItemName = row["ItemName"].ToString() ?? string.Empty,
                        Size = (row["Size"].ToString()),
                        Make = row["Make"].ToString(),
                        Unit = (row["Unit"].ToString()),
                        DeptCode = row["DepartmentCode"].ToString()
                    });
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", itemslst);
            }
        }

        public async Task<ApiResponse> GetStock(string itemCode, string CampusCode)
        {
            string BaseUrl = "http://hostel.glauniversity.in:84/inventoryservices.asmx/currentstock";
            using (DataTable dt = await _inclusive.GetBaseUrlFromTable())
            {
                DataRow[] dr = dt.Select("Type='WebService' and Tag='BaseUrlStock'");
                if (dr.Length > 0)
                {
                    BaseUrl = dr[0][1].ToString() ?? "";
                }
            }
            List<StockDetailsResponse> lst = new List<StockDetailsResponse>();
            using (DataTable dt = await _inclusive.GetItemDetails(itemCode))
            {
                if (dt.Rows.Count > 0)
                {
                    DataTable MakeCode = await _inclusive.GetItemMakeCode(dt.Rows[0]["ItemName"].ToString()!,
                        dt.Rows[0]["Size"].ToString()!, dt.Rows[0]["Unit"].ToString()!);
                    foreach (DataRow row in MakeCode.Rows)
                    {
                        DataTable details =
                            await _inclusive.GetItemsDetails(row["ItemCode"].ToString()!, row["Make"].ToString()!);
                        if (details.Rows.Count > 0)
                        {
                            string stock = CallWebService2(BaseUrl, details.Rows[0]["ItemCode"].ToString()!, CampusCode,
                                "@1@", "", "");
                            lst.Add(new StockDetailsResponse
                            {
                                Make = details.Rows[0]["Make"].ToString(),
                                ItemCode = details.Rows[0]["ItemCode"].ToString(),
                                ItemName = details.Rows[0]["ItemName"].ToString(),
                                Size = details.Rows[0]["Size"].ToString(),
                                PrevPurchase = details.Rows[0]["LastPur"].ToString(),
                                PrevRate = details.Rows[0]["CurRate"].ToString(),
                                Unit = details.Rows[0]["Unit"].ToString(),
                                Stock = stock.Replace("\r\n", "")
                            });
                        }


                    }

                    if (MakeCode.Rows.Count <= 0 && lst.Count <= 0)
                    {
                        string stock = CallWebService2(BaseUrl, itemCode, CampusCode,
                            "@1@", "", "");
                        lst.Add(new StockDetailsResponse
                        {
                            //ItemName,Size,Unit,Make
                            Make = dt.Rows[0]["Make"]?.ToString() ?? string.Empty,
                            ItemCode = itemCode,
                            ItemName = dt.Rows[0]["ItemName"]?.ToString() ?? string.Empty,
                            Size = dt.Rows[0]["Size"]?.ToString() ?? string.Empty,
                            PrevPurchase = "0.00",
                            PrevRate = "0.00",
                            Unit = dt.Rows[0]["Unit"]?.ToString() ?? string.Empty,
                            Stock = stock.Replace("\r\n", "")
                        });
                    }
                }

            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
        }
        public string CallWebService2(string url, string rno, string campusCode, string host, string mnth, string yr)
        {
            string strPost = "itemcode=" + rno + "&Campus=" + campusCode;
            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url + "?" + strPost);
            //BTN_SAVE.Text = url + "?" + strPost;

            objRequest.Method = "POST";
            objRequest.Timeout = 200000; // this should be greater than IIS / web server's timeout
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";
            objRequest.KeepAlive = false;
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch (Exception)
            {
                return "";
            }
            finally
            {
                myWriter.Close();
            }
            myWriter.Close();
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            StreamReader sr = new StreamReader(objResponse.GetResponseStream());
            string result;
            {
                result = sr.ReadToEnd();
                result = result.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace("<string xmlns=\"http://tempuri.org/\">", "").Replace("</string>", "");

                result = HttpUtility.HtmlDecode(result);
                sr.Close();
            }

            //BTN_DISPLAY.Text = result;
            return result;
        }


        public string CallWebService(string url, string rno, string host, string mnth, string yr)
        {
            string result = "";
            string strPost = "Serialno=" + rno + "&Campus=" + yr;
            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url + "?" + strPost);
            //BTN_SAVE.Text = url + "?" + strPost;

            objRequest.Method = "POST";
            objRequest.Timeout = 200000; // this should be greater than IIS / web server's timeout
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";
            objRequest.KeepAlive = false;
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);

            }
            catch (Exception e)
            {
                return "";
            }
            finally
            {
                myWriter.Close();
            }
            myWriter.Close();
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            StreamReader sr = new StreamReader(objResponse.GetResponseStream());
            {
                result = sr.ReadToEnd();
                result = result.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace("<string xmlns=\"http://tempuri.org/\">", "").Replace("</string>", "");

                result = HttpUtility.HtmlDecode(result);
                sr.Close();
            }

            //BTN_DISPLAY.Text = result;
            return result;
        }

        public async Task<ApiResponse> GetAllMaad()
        {
            DataTable maadList = await _inclusive.GetAllMaad();
            List<TextValue> allMaads = new List<TextValue>();
            foreach (DataRow row in maadList.Rows)
            {
                TextValue maad = new TextValue
                {
                    Text = row["Text"]?.ToString() ?? string.Empty,
                    Value = row["Value"]?.ToString() ?? string.Empty
                };
                allMaads.Add(maad);
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", allMaads);

        }

        public async Task<ApiResponse> GetAllDepartments(string? employeeId)
        {
            DataTable allowedCampusList = await _inclusive.GetEmployeeCampusCodes(employeeId);
            List<string> campusCodes = new List<string>();
            foreach (DataRow row in allowedCampusList.Rows)
            {
                string campusCode = row["CampusCode"]?.ToString() ?? string.Empty;
                campusCodes.Add(campusCode);
            }

            DataTable departments = await _inclusive.GetAllDepartments(campusCodes);
            List<string> allDepartments = new List<string>();
            foreach (DataRow row in departments.Rows)
            {
                allDepartments.Add(row["name"]?.ToString() ?? string.Empty);
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", allDepartments);
        }

        public async Task<ApiResponse> GetVendors(string? search)
        {
            List<TextValue> vendorsList = new List<TextValue>();
            if (string.IsNullOrEmpty(search) || search.Length < 3)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", vendorsList);
            }

            DataTable vendors = await _inclusive.GetVendors(search);
            foreach (DataRow row in vendors.Rows)
            {
                TextValue vendor = new TextValue
                {
                    Text = row["Text"]?.ToString() ?? string.Empty,
                    Value = row["Value"]?.ToString() ?? string.Empty
                };
                vendorsList.Add(vendor);
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", vendorsList);
        }

        public async Task<ApiResponse> GetVendorSubFirms(int? vendorId)
        {
            List<string> subFirmList = new List<string>();

            if (vendorId == null || vendorId <= 0)
            {
                subFirmList.Add("----");
                return new ApiResponse(StatusCodes.Status200OK, "Success", subFirmList);
            }

            DataTable subFirms = await _inclusive.GetVendorSubFirms(vendorId);
            foreach (DataRow row in subFirms.Rows)
            {
                subFirmList.Add(row["Offices"]?.ToString() ?? string.Empty);
            }
            if (subFirmList.Count == 0)
            {
                subFirmList.Add("----");
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", subFirmList);
        }

        public async Task<ApiResponse> GetAllEmployees(string? search)
        {
            List<EmployeesListResponse> employeesList = new List<EmployeesListResponse>();
            if (string.IsNullOrEmpty(search) || search.Length < 3)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", employeesList);
            }

            DataTable employees = await _inclusive.GetAllEmployees(search);
            foreach (DataRow row in employees.Rows)
            {
                EmployeesListResponse employee = new EmployeesListResponse
                {
                    Text = row["Text"]?.ToString() ?? string.Empty,
                    Value = row["Value"]?.ToString() ?? string.Empty,
                    EmployeeCode = row["employee_code"]?.ToString() ?? string.Empty
                };
                employeesList.Add(employee);
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", employeesList);
        }

        public async Task<ApiResponse> GetBudget(GetFirmBudgetRequest? firm)
        {
            FirmBudgetResponse budgetResponse = new FirmBudgetResponse();

            if (firm?.SubFirm?.Contains("*") == true)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", budgetResponse);
            }

            DataTable isBudgetEnable = await _inclusive.GetIsVendorBudgetEnable(firm?.VendorId);
            if (isBudgetEnable.Rows.Count > 0)
            {
                DataTable isBudgetExists = await _inclusive.IsFirmBudgetExist(firm);
                if (isBudgetExists.Rows.Count > 0)
                {
                    if (isBudgetExists.Rows[0]["Status"]?.ToString() == "Approved")
                    {
                        DataTable isBudgetPending = await _inclusive.IsFirmBudgetPending(firm, isBudgetExists.Rows[0]["Fr"]?.ToString(), isBudgetExists.Rows[0]["To"]?.ToString(), ENUMS.Inclusive.ApprovalTypes.OtherApproval);
                        if (isBudgetPending.Rows.Count > 0)
                        {
                            budgetResponse.ApprovalAuthorized = false;

                            budgetResponse.BudgetRequestStatus = $"Sorry! Previous Advance Approval Is Still Pending To Approve Under Same Budget Approval Initiated By {isBudgetPending.Rows[0]["IniName"]?.ToString() ?? string.Empty} on {isBudgetPending.Rows[0]["Date"]?.ToString() ?? string.Empty} of Amount : {isBudgetPending.Rows[0]["Amount"]?.ToString() ?? string.Empty} Rs/-";

                            budgetResponse.BudgetAmont = "N/A";
                            budgetResponse.ReleasedAmount = "N/A";
                            budgetResponse.BudgetReferenceNo = "Not Required";
                        }
                        else
                        {
                            isBudgetPending = await _inclusive.IsFirmBudgetPending(firm, isBudgetExists.Rows[0]["Fr"]?.ToString(), isBudgetExists.Rows[0]["To"]?.ToString(), ENUMS.Inclusive.ApprovalTypes.PurchaseApproval);
                            if (isBudgetPending.Rows.Count > 0)
                            {
                                budgetResponse.ApprovalAuthorized = false;

                                budgetResponse.BudgetRequestStatus = $"Sorry! Previous Approval Is Still Pending To Approve Under Same Budget Approval Initiated By {isBudgetPending.Rows[0]["IniName"]?.ToString() ?? string.Empty} on {isBudgetPending.Rows[0]["Date"]?.ToString() ?? string.Empty} of Amount : {isBudgetPending.Rows[0]["Amount"]?.ToString() ?? string.Empty} Rs/-";
                                budgetResponse.BudgetAmont = "N/A";
                                budgetResponse.ReleasedAmount = "N/A";
                                budgetResponse.BudgetReferenceNo = "Not Required";
                            }
                            else
                            {
                                string budgetAmount = "0";
                                DataTable vendorBudget = await _inclusive.GetVendorBudgetDetails(firm, isBudgetExists.Rows[0]["Fr"]?.ToString(), isBudgetExists.Rows[0]["To"]?.ToString());
                                if (vendorBudget.Rows.Count > 0)
                                {
                                    budgetAmount = vendorBudget.Rows[0][0]?.ToString() ?? "0";
                                }

                                budgetResponse.BudgetRequestStatus = $"Ok! Budget Required And Approved";
                                budgetResponse.BudgetAmont = isBudgetExists.Rows[0]["Amount"]?.ToString() ?? string.Empty;
                                budgetResponse.ReleasedAmount = budgetAmount;
                                budgetResponse.BudgetReferenceNo = isBudgetExists.Rows[0]["ReferenceNo"]?.ToString() ?? string.Empty;
                            }
                        }
                    }
                    else
                    {
                        budgetResponse.BudgetRequestStatus = "Sorry! Budget Required But Not Approved";
                        budgetResponse.BudgetAmont = isBudgetExists.Rows[0]["Amount"]?.ToString() ?? string.Empty;
                        budgetResponse.ReleasedAmount = "0";
                        budgetResponse.BudgetReferenceNo = isBudgetExists.Rows[0]["ReferenceNo"]?.ToString() ?? string.Empty;
                    }
                }
                else
                {
                    budgetResponse.BudgetRequestStatus = "Sorry! Budget Required But Not Found";
                    budgetResponse.BudgetAmont = "N/A";
                    budgetResponse.ReleasedAmount = "N/A";
                    budgetResponse.BudgetReferenceNo = "0";
                }
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", budgetResponse);
        }

        public async Task<bool> IsUserAllowed(string? employeeId, UserRolePermission userRolePermission)
        {
            var result = await _inclusive.CheckUserRole(employeeId, userRolePermission);
            return result.Rows.Count > 0;
        }

        public async Task<string> GetEnCryptedKey()
        {
            DataTable dt = await _inclusive.GetFileKey();
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString() ?? string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }
        public async Task<string> SaveFile(string FileName, string FilePath, IFormFile file, string Ext)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();

                //   FileName =_general.EncryptWithKey(FileName,await GetEnCryptedKey());
                path = Path.Combine(path, FilePath);
                if (!Path.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, FileName + Ext);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return "Success";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message, "Error During saving file");
                throw;
            }
        }
        public async Task<ApiResponse> GetApprovalCancellationReasons()
        {
            DataTable reasonsList = await _inclusive.GetApprovalCancellationReasons();
            List<string> allReasons = new List<string>();
            foreach (DataRow row in reasonsList.Rows)
            {
                string reason = row["Value"]?.ToString() ?? string.Empty;
                allReasons.Add(reason);
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", allReasons);

        }

        public async Task<EmployeeDetails> GetEmployeeDetailsByEmployeeCode(string? employeeCode)
        {
            DataTable employeeTableList = await _inclusive.GetEmployeeDetails(employeeCode);
            EmployeeDetails employeeDetails = new EmployeeDetails();
            if (employeeTableList?.Rows.Count > 0)
            {
                employeeDetails = new EmployeeDetails(employeeTableList.Rows[0]);
            }
            return employeeDetails;


        }

        public async Task<ItemDetails> GetItemDetailsByItemCode(string? itemCode)
        {
            DataTable itemDetailsList = await _inclusive.GetItemDetailsByItemCode(itemCode);
            ItemDetails itemDetail = new ItemDetails();
            if (itemDetailsList?.Rows.Count > 0)
            {
                itemDetail = new ItemDetails(itemDetailsList.Rows[0]);
            }
            return itemDetail;
        }

        public async Task<string> GetEmployeePhotoUrl(string? employeeId)
        {
            using (DataTable dtEmployeeEncryptedId = await _inclusive.GetEmployeeEncryptedId(employeeId))
            {
                if (dtEmployeeEncryptedId.Rows.Count > 0)
                {
                    return $" https://glauniversity.in:8088/assets/profiles/{dtEmployeeEncryptedId.Rows[0]["EncID"].ToString() ?? string.Empty}.jpg";
                }
                else
                {
                    return string.Empty;

                }
            }
        }
    }
}
