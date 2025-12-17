using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Advance;
using AdvanceAPI.DTO.Advance.Bill;
using AdvanceAPI.DTO.Advance.BillApproval;
using AdvanceAPI.DTO.Advance.TimeLine;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.DTO.Inclusive;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Advance;
using AdvanceAPI.IServices.Inclusive;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace AdvanceAPI.Services.Advance
{
    public class AdvanceService : IAdvanceService
    {
        private IAdvanceRepository _advanceRepository;
        private IGeneral _general;
        private readonly IInclusiveService _inclusiveService;
        public AdvanceService(IAdvanceRepository advanceRepository, IGeneral general, IInclusiveService inclusiveService)
        {
            _advanceRepository = advanceRepository;
            _general = general;
            _inclusiveService = inclusiveService;
        }
        public async Task<ApiResponse> GetApprovals(string RefNo)
        {
            purchaseApprovalDetailsResponse details = new purchaseApprovalDetailsResponse();
            DataTable ApprovalDetails = await _advanceRepository.PurchaseApprovalDetails(RefNo);

            if (ApprovalDetails.Rows.Count <= 0)
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Invalid Access");
            }

            using (DataTable dt = await _advanceRepository.CheckPreviousPendingBill(RefNo))
            {
                if (dt.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status200OK, "Warning", "Sorry! Previous Advance Approval Is Still Pending To Approve For Same Purchase Approval Initiated By " + dt.Rows[0]["IniName"].ToString() + " on " + dt.Rows[0]["AppDate"].ToString() + " of Amount : " + dt.Rows[0]["Amount"].ToString() + " Rs/-");
                }
                //Task<DataTable> GetVendorTextValue(string VendorId)
                DataTable d = await _advanceRepository.GetVendorTextValue(ApprovalDetails.Rows[0]["VendorID"].ToString() ?? string.Empty);
                List<TextValues> VendorDetails = new List<TextValues>();
                foreach (DataRow row in d.Rows)
                {
                    VendorDetails.Add(new TextValues(row));
                }
                List<TextValues> VendorOffice = new List<TextValues>();
                if (ApprovalDetails.Rows[0]["PExtra3"].ToString() != "")
                {
                    DataTable OfficeData = await _advanceRepository.getVendorDetails(ApprovalDetails.Rows[0]["VendorID"].ToString()!, ApprovalDetails.Rows[0]["PExtra3"].ToString()!);
                    foreach (DataRow row in OfficeData.Rows)
                    {
                        VendorOffice.Add(new TextValues(row));
                    }
                }
                details.Department = ApprovalDetails.Rows[0]["ForDepartment"].ToString();
                details.Purpose = ApprovalDetails.Rows[0]["ForDepartment"].ToString();
                details.Note = ApprovalDetails.Rows[0]["ForDepartment"].ToString();

                DataTable BalanceDetails = await _advanceRepository.GetBalanceDetails(RefNo);
                DataTable PreviousDetails = await _advanceRepository.GetVendorDetails(RefNo);
                if (BalanceDetails.Rows.Count > 0)
                {
                    if (BalanceDetails.Rows[0][0].ToString()!.Length > 0)
                    {

                        details.BalanceAmount = ((Convert.ToInt32(ApprovalDetails.Rows[0]["TotalAmount"].ToString()!.Replace(".00", "")) - Convert.ToInt32(BalanceDetails.Rows[0][0].ToString())) >= 0 ? (Convert.ToInt32(ApprovalDetails.Rows[0]["TotalAmount"].ToString()!.Replace(".00", "")) - Convert.ToInt32(BalanceDetails.Rows[0][0].ToString())) : 0).ToString();
                    }
                    else
                    {
                        details.BalanceAmount = ApprovalDetails.Rows[0]["TotalAmount"].ToString()!.Replace(".00", "");
                    }
                }
                else
                {
                    details.BalanceAmount = ApprovalDetails.Rows[0]["TotalAmount"].ToString()!.Replace(".00", "");
                }
                details.Vendorlst = VendorDetails;
                details.VendorOffice = VendorOffice;
                return new ApiResponse(StatusCodes.Status200OK, "Success", details);
            }
        }
        public async Task<ApiResponse> GetDepartmentHod(string Dept)
        {
            List<TextValues> textValues = new List<TextValues>();
            using (DataTable dt = await _advanceRepository.GetDepartmentHod(Dept))
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        textValues.Add(new TextValues(dr));

                    }
                    return new ApiResponse(StatusCodes.Status200OK, "Success", textValues);
                }
                else
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Hod Not Found");
                }
            }
        }

        public async Task<ApiResponse> GenerateAdvance(GenerateAdvancerequest req, string EmpCode, string EmpName, string RefNo)
        {

            using (DataTable dt = await _advanceRepository.CheckPreviousPendingBill(RefNo))
            {
                if (dt.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status200OK, "Warning", "Sorry! Previous Advance Approval Is Still Pending To Approve For Same Purchase Approval Initiated By " + dt.Rows[0]["IniName"].ToString() + " on " + dt.Rows[0]["AppDate"].ToString() + " of Amount : " + dt.Rows[0]["Amount"].ToString() + " Rs/-");
                }
            }

            req.ApprovalType = req.ApprovalType + " Advance Form";
            if (Convert.ToDateTime(req.AppDate) > Convert.ToDateTime(req.BillTill))
            {
                req.ApprovalType = "Post Facto - " + req.ApprovalType;
            }
            int totdays = Convert.ToInt32((Convert.ToDateTime(req.BillTill) - Convert.ToDateTime(req.AppDate)).TotalDays);
            req.BillUptoValue = totdays.ToString();
            string fname = "";
            string fcontcatname = "";
            string fcno = "";
            string femail = "";
            string falter = "";
            string fadd = "";
            using (DataTable dt = await _advanceRepository.GetVendorDetails(req.VendorID!))
            {
                if (dt.Rows.Count > 0)
                {
                    fname = dt.Rows[0]["VendorName"].ToString() ?? string.Empty;
                    fcontcatname = dt.Rows[0]["ContactPersons"].ToString() ?? string.Empty;
                    fcno = dt.Rows[0]["ContactNo"].ToString() ?? string.Empty;
                    femail = dt.Rows[0]["EmailID"].ToString() ?? string.Empty;
                    falter = dt.Rows[0]["AlternateContactNo"].ToString() ?? string.Empty;
                    fadd = dt.Rows[0]["Address"].ToString() ?? string.Empty.ToUpper().Replace("<BR/>", " ");
                }
                else
                {
                    req.VendorID = "0";
                }

            }
            string RefrenceNo = "";
            using (DataTable dt = await _advanceRepository.GetAdvanceRefNo())
            {
                RefrenceNo = dt.Rows[0][0].ToString() ?? string.Empty;
            }
            string PurChaseBackValue = string.Empty;
            using (DataTable dt = await _advanceRepository.GetBackValue(req.ApprovalType))
            {
                PurChaseBackValue = dt.Rows[0][0].ToString() ?? string.Empty;
            }
            req.BudgetStatus = "Not Required";
            req.CurStatus = "Not Required";
            req.PreviousTaken = "0";
            int b_ = 0;
            req.Session = _general.GetFinancialSession(DateTime.Now);
            string PurchaseApprovalDetails = "";
            if (int.TryParse(req.BudgetAmount, out b_))
            {
                if (req.ApprovalType.Contains("Against"))
                {
                    using (DataTable dt = await _advanceRepository.GetPurchaseApprovalPValues(RefNo))
                    {
                        string PrevPurchase = dt.Rows[0]["Value"].ToString() ?? string.Empty;
                        PurchaseApprovalDetails = PrevPurchase;
                        string[] pvals = PrevPurchase.Split('#');
                        int temp = 0;
                        int.TryParse(pvals[2].Replace(".00", ""), out temp);
                        int c_ = Convert.ToInt32(req.PreviousTaken);
                        c_ = c_ - temp;
                        req.PreviousTaken = c_.ToString();
                        int diff = b_ - (c_ + temp);
                        req.CurStatus = diff.ToString();
                        req.BudgetStatus = diff < 0 ? Math.Abs(diff) + " Rs/- OverBudget" : "UnderBudget";

                    }
                }
                else
                {
                    int c_ = Convert.ToInt32(req.PreviousTaken);
                    int diff = b_ - (c_ + Convert.ToInt32(req.Amount));
                    req.CurStatus = diff.ToString();
                    req.BudgetStatus = diff < 0 ? Math.Abs(diff) + " Rs/- OverBudget" : "UnderBudget";
                }
            }

            //Task<int> SaveAdvance(GenerateAdvancerequest req, string EmpCode, string EmpName, string RefNo,string FirmName,string FirmPerson,string FirmEmail,string FirmPanNo,string FirmAddress,string FirmContactNo,string FirmAlternateContactNo,string BackValue,string PurchaseApprovalDetails)
            int ins = await _advanceRepository.SaveAdvance(req, EmpCode, EmpName, RefrenceNo, fname, fcontcatname, femail, "", fadd, fcno, falter, PurChaseBackValue, PurchaseApprovalDetails, RefNo);

            return new ApiResponse(StatusCodes.Status200OK, "Success", "Record Insert Successfully");

        }


        public async Task<ApiResponse> GetFinalAuth(string CampusCode)
        {
            //Task<DataTable> FinalAuth(string CampusCode);
            using (DataTable dt = await _advanceRepository.FinalAuth(CampusCode))
            {
                List<TextValues> values = new List<TextValues>();
                foreach (DataRow dr in dt.Rows)
                {
                    values.Add(new TextValues(dr));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", values);
            }

        }
        //Task<DataTable> GetMyAdvanceDetails(string Status,string Session,string CampusCode,string RefNo)
        public async Task<ApiResponse> GetMyAdvanceReq(string Status, string Session, string CampusCode, string RefNo, string Type, string EmpCode, string Department, string itemsFrom, string noOfItems)
        {
            List<MyRequestResponse> lst = new List<MyRequestResponse>();
            using (DataTable dt = await _advanceRepository.GetMyAdvanceDetails(Status, Session, CampusCode, RefNo, Type, EmpCode, Department, itemsFrom, noOfItems))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MyRequestResponse MyReq = new MyRequestResponse();

                    if (dr["App1Status"].ToString() != "Pending" || dr["App2Status"].ToString() != "Pending" || dr["App4Status"].ToString() != "Pending")
                    {
                        MyReq.CanDelete = false;
                    }
                    else
                    {
                        MyReq.CanDelete = true;
                    }
                    MyReq.CanUploadBill = false;
                    using (DataTable ds = await _advanceRepository.GetrefnoForGenerateBillAdvance(Type, EmpCode, dr["ReferenceNo"].ToString()))
                    {
                        MyReq.CanUploadBill = ds.Rows.Count > 0;
                    }
                    MyReq.Status = dr["Status"].ToString();
                    MyReq.CampusName = dr["CampusName"].ToString();
                    MyReq.BudgetAmount = dr["BudgetAmount"].ToString();
                    MyReq.App1Status = dr["App1Status"].ToString();
                    MyReq.App2Status = dr["App2Status"].ToString();
                    MyReq.App3Status = dr["App3Status"].ToString();
                    MyReq.App4Status = dr["App4Status"].ToString();
                    MyReq.PreviousTaken = dr["PreviousTaken"].ToString();
                    MyReq.MyType = dr["MyType"].ToString();
                    MyReq.CurStatus = dr["CurStatus"].ToString();
                    MyReq.BudgetStatus = dr["BudgetStatus"].ToString();
                    MyReq.ReferenceNo = dr["ReferenceNo"].ToString();
                    MyReq.App1Name = dr["App1Name"].ToString();
                    MyReq.App2Name = dr["App2Name"].ToString();
                    MyReq.App3Name = dr["App3Name"].ToString();
                    MyReq.App4Name = dr["App4Name"].ToString();
                    MyReq.App1On = dr["App1On"].ToString();
                    MyReq.App2On = dr["App2On"].ToString();
                    MyReq.App3On = dr["App3On"].ToString();
                    MyReq.App4On = dr["App4On"].ToString();
                    MyReq.RejectedReason = dr["RejectedReason"].ToString();
                    MyReq.IniOn = dr["IniOn"].ToString();
                    MyReq.Session = dr["Session"].ToString();
                    MyReq.Purpose = dr["Purpose"].ToString();
                    MyReq.TotalAmount = dr["TotalAmount"].ToString();
                    MyReq.IniName = dr["IniName"].ToString();
                    MyReq.AppDate = dr["AppDate"].ToString();
                    MyReq.BudgetReferenceNo = dr["BudgetReferenceNo"].ToString();
                    MyReq.ExtraDetails = dr["MyExtra"].ToString();
                    MyReq.BillUpTo = dr["ExeOn"].ToString();
                    MyReq.App1ID = dr["App1ID"].ToString();
                    MyReq.App2ID = dr["App2ID"].ToString();
                    MyReq.App3ID = dr["App3ID"].ToString();
                    MyReq.App4ID = dr["App4ID"].ToString();
                    MyReq.IniID = dr["IniId"].ToString();
                    MyReq.App1Photo = await _inclusiveService.GetEmployeePhotoUrl(dr["App1ID"].ToString() ?? string.Empty);
                    MyReq.App2Photo = await _inclusiveService.GetEmployeePhotoUrl(dr["App2ID"].ToString() ?? string.Empty);
                    MyReq.App3Photo = await _inclusiveService.GetEmployeePhotoUrl(dr["App3ID"].ToString() ?? string.Empty);
                    MyReq.App4Photo = await _inclusiveService.GetEmployeePhotoUrl(dr["App4ID"].ToString() ?? string.Empty);
                    MyReq.IniByPhoto = await _inclusiveService.GetEmployeePhotoUrl(dr["IniId"].ToString() ?? string.Empty);
                    lst.Add(MyReq);
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }




        }
        public async Task<ApiResponse> DeleteAdvance(string RefNo, string EmpCode)
        {
            using (DataTable dt = await _advanceRepository.GetMyAdvanceDetails("", "", "", RefNo, "", EmpCode, "", "0", "10"))
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["App1Status"].ToString() != "Pending" || dt.Rows[0]["App2Status"].ToString() != "Pending" || dt.Rows[0]["App4Status"].ToString() != "Pending")
                    {
                        return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry! You Cannot Delete This Approval");
                    }

                }
                else
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Invalid Access");
                }
            }


            int ins = await _advanceRepository.DeleteAdvance(RefNo, EmpCode);
            return new ApiResponse(StatusCodes.Status200OK, "Success", "Record Delete Successfully");
        }
        //Task<DataTable> GetPassApprovalDetails(string EmpCode,string EmpCodeAdd,string Type, GetMyAdvanceRequest req)
        public async Task<ApiResponse> GetApprovalDetails(string EmpCode, string EmpCodeAdd, string Type, GetMyAdvanceRequest req)
        {
            List<AdvanceApprovalResponse> lst = new List<AdvanceApprovalResponse>();
            using (DataTable dt = await _advanceRepository.GetPassApprovalDetails(EmpCode, EmpCodeAdd, Type, req))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    bool IsRejectable = false;
                    IsRejectable = CanReject(dr, EmpCode, EmpCodeAdd);
                    if (dr["App4ID"].ToString() == "GLAVIVEK" && dr["App4Status"].ToString() == "Approved") // If Vivek sir approved then no body can reject this approval
                    {
                        IsRejectable = false;
                    }
                    lst.Add(new AdvanceApprovalResponse
                    {
                        CanReject = IsRejectable,
                        MyType = dr["MyType"].ToString(),
                        Status = dr["Status"].ToString(),
                        ByPass = dr["ByPass"].ToString(),
                        Test = dr["Test"].ToString(),
                        BudgetAmount = dr["BudgetAmount"].ToString(),
                        TotalAmount = dr["TotalAmount"].ToString(),
                        PreviousTaken = dr["PreviousTaken"].ToString(),
                        BudgetStatus = dr["BudgetStatus"].ToString(),
                        ReferenceNo = dr["ReferenceNo"].ToString(),
                        App1Status = dr["App1Status"].ToString(),
                        App2Status = dr["App2Status"].ToString(),
                        App3Status = dr["App3Status"].ToString(),
                        App4Status = dr["App4Status"].ToString(),
                        App1ID = dr["App1ID"].ToString(),
                        App2ID = dr["App2ID"].ToString(),
                        App3ID = dr["App3ID"].ToString(),
                        App4ID = dr["App4ID"].ToString(),
                        App1Name = dr["App1Name"].ToString(),
                        App2Name = dr["App2Name"].ToString(),
                        App3Name = dr["App3Name"].ToString(),
                        App4Name = dr["App4Name"].ToString(),
                        App1On = dr["App1On"].ToString(),
                        App2On = dr["App2On"].ToString(),
                        App3On = dr["App3On"].ToString(),
                        App4On = dr["App4On"].ToString(),
                        FinalStat = dr["FinalStat"].ToString(),
                        RejectedReason = dr["RejectedReason"].ToString(),
                        CancelledReason = dr["CancelledReason"].ToString(),
                        CancelledBy = dr["CancelledBy"].ToString(),
                        CancelledOn = dr["CancelledOn"].ToString(),
                        BillId = dr["BillId"].ToString(),
                        IniBy = dr["IniName"].ToString(),
                        RelativePerson = dr["RelativePersonName"].ToString(),
                        CampusName = dr["CampusName"].ToString(),
                        IniOn = dr["IniOn"].ToString(),
                        IniById = dr["IniId"].ToString(),
                        DepartMent = dr["ForDepartment"].ToString(),
                        RelativePersonID = dr["RelativePersonID"].ToString(),
                        RelativeDepartment = dr["RelativeDepartment"].ToString(),
                        VendorId = dr["VendorId"].ToString(),
                        FirmName = dr["FirmName"].ToString(),
                        Purpose = dr["Purpose"].ToString(),
                        Extra = dr["MyExtra"].ToString(),
                        BillUpTo = dr["ExeOn"].ToString(),
                        AppDate = dr["AppDate"].ToString(),
                        App1Photo = await _inclusiveService.GetEmployeePhotoUrl(dr["App1ID"].ToString() ?? string.Empty),
                        App2Photo = await _inclusiveService.GetEmployeePhotoUrl(dr["App2ID"].ToString() ?? string.Empty),
                        App3Photo = await _inclusiveService.GetEmployeePhotoUrl(dr["App3ID"].ToString() ?? string.Empty),
                        App4Photo = await _inclusiveService.GetEmployeePhotoUrl(dr["App4ID"].ToString() ?? string.Empty),
                        IniByPhoto = await _inclusiveService.GetEmployeePhotoUrl(dr["IniId"].ToString() ?? string.Empty),
                        CanApprove = CanApproval(dr, EmpCode, EmpCodeAdd),
                        ApprovalNo = GetApprovalNo(EmpCode, EmpCodeAdd, dr)
                    });
                }
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
        }

        public int GetApprovalNo(string EmpCode, string AddEmpCode, DataRow dr)
        {
            if (dr["App1ID"].ToString() == EmpCode || dr["App1ID"].ToString() == AddEmpCode)
            {
                return 1;
            }
            if (dr["App2ID"].ToString() == EmpCode || dr["App2ID"].ToString() == AddEmpCode)
            {
                return 2;
            }
            if (dr["App3ID"].ToString() == EmpCode || dr["App3ID"].ToString() == AddEmpCode)
            {
                return 3;
            }
            if (dr["App4ID"].ToString() == EmpCode || dr["App4ID"].ToString() == AddEmpCode)
            {
                return 4;
            }
            return 0;
        }

        public bool CanReject(DataRow dr, string EmpCode, string AddEmpCode)
        {
            if ((dr["App1ID"].ToString() == EmpCode || dr["App1ID"].ToString() == AddEmpCode) && dr["App1Status"].ToString() == "Pending" && dr["Status"].ToString() == "Pending")
            {
                return true;
            }
            if (dr["App2Status"].ToString() == "Pending" || dr["App2Status"].ToString() == "Rejected")
            {
                if ((dr["App2ID"].ToString() == EmpCode || dr["App2ID"].ToString() == AddEmpCode) && dr["App2Status"].ToString() == "Pending" && dr["Status"].ToString() == "Pending")
                {
                    return true;
                }
            }
            if (dr["App3Name"].ToString()?.Length > 0)
            {
                if (dr["App3ID"].ToString() == EmpCode && dr["App3Status"].ToString() == "Pending" && dr["Status"].ToString() == "Pending")
                {
                    return true;
                }
            }
            if ((dr["App4ID"].ToString() == EmpCode || dr["App4ID"].ToString() == AddEmpCode) && dr["Status"].ToString() == "Pending")
            {
                return true;
            }
            return false;
        }

        public bool CanApproval(DataRow dr, string EmpCode, string AddEmpCode)
        {
            if ((dr["App1ID"].ToString() == EmpCode || dr["App1ID"].ToString() == AddEmpCode) && dr["App1Status"].ToString() == "Pending" && dr["Status"].ToString() == "Pending")
            {
                return true;
            }
            if ((dr["App2ID"].ToString() == EmpCode.ToString() || dr["App2ID"].ToString() == AddEmpCode) && dr["App2Status"].ToString() == "Pending" && dr["Status"].ToString() == "Pending")
            {
                return true;
            }

            if ((dr["App3ID"].ToString() == EmpCode.ToString() && dr["App3Status"].ToString() == "Pending" && dr["Status"].ToString() == "Pending") && dr["App3Name"].ToString()?.Length > 0)
            {
                return true;
            }
            if ((dr["App1Status"].ToString() == "Approved" && dr["App2Status"].ToString() == "Approved" && (dr["App4ID"].ToString() == EmpCode || dr["App4ID"].ToString() == AddEmpCode) && dr["App4Status"].ToString() == "Pending" && dr["Status"].ToString() == "Pending") || (dr["FinalStat"].ToString() == "Y" && (dr["App4ID"].ToString() == EmpCode || dr["App4ID"].ToString() == AddEmpCode) && dr["App4Status"].ToString() == "Pending" && dr["Status"].ToString() == "Pending"))

            {
                return true;
            }
            return false;
        }

        public async Task<ApiResponse> GetAdvanceType()
        {
            List<TextValues> values = new List<TextValues>();
            using (DataTable d = await _advanceRepository.GetApprovalType())
            {
                foreach (DataRow dr in d.Rows)
                {
                    values.Add(new TextValues(dr));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", values);
            }
        }

        //public async Task<ApiResponse> GetApproval(string AppTypeValue,string AppTypeText)
        //{
        //    if(AppTypeValue == "")
        //    {

        //    }


        //}


        //Task<int> AdvanceApproveDetails(string EmpCode, string EmpName, PassApprovalRequest req)
        public async Task<ApiResponse> PassAdvanceApproval(string EmpCode, string EmpName, PassApprovalRequest req)
        {
            int ins = await _advanceRepository.AdvanceApproveDetails(EmpCode, EmpName, req);
            if (ins > 0)
            {
                using (DataTable IsStatusApprove = await _advanceRepository.IsApproveDetails(EmpCode, EmpName, req))
                {
                    if (IsStatusApprove.Rows.Count > 0 && IsStatusApprove.Rows[0][0].ToString() == "Approved")
                    {
                        int UpdateStatus = await _advanceRepository.ApproveFinalStatus(EmpCode, EmpName, req);

                        int InsBillBase = await _advanceRepository.InsertBillBase(EmpCode, EmpName, req);

                        DataTable BillBaseDetails = await _advanceRepository.BillBaseDetails(EmpCode, EmpName, req);
                        if (BillBaseDetails.Rows.Count > 0)
                        {
                            //Task<int> InsertBillAuthority(string BillTransId,string RefNo)
                            int InsAuth = await _advanceRepository.InsertBillAuthority(BillBaseDetails.Rows[0]["TransactionID"].ToString() ?? "", req.RefNo!);

                            int UpdateBillStatus = await _advanceRepository.UpdateBillStatus(BillBaseDetails.Rows[0]["TransactionID"].ToString() ?? "", req.RefNo!);

                        }
                    }
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", "Record Approved Successfully");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Invalid Request/Record Not found");
            }

        }

        public async Task<ApiResponse> RejectApproval(string EmpCode, string EmpName, PassApprovalRequest req)
        {
            int ins = await _advanceRepository.RejectAdvanceApproval(EmpCode, EmpName, req);
            if (ins > 0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", "Record Reject Successfully");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Invalid Access/Record Not Found");
            }
        }


        public async Task<ApiResponse> GetBasicDetailsForGenerateBill(string RefNo)
        {
            //Task<string> CheckWarrentyUploadeOrNot(string RefNo)
            string chk = await _advanceRepository.CheckWarrentyUploadeOrNot(RefNo);
            if (chk != "")
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", chk);
            }


            PurchaseDetailsForGenerateBillREsponse lst = new PurchaseDetailsForGenerateBillREsponse();
            using (DataTable dt = await _advanceRepository.GetPurchaseBasicForBill(RefNo))
            {
                lst.FirmName = dt.Rows[0]["FirmName"].ToString() ?? string.Empty;
                lst.Department = dt.Rows[0]["ForDepartment"].ToString() ?? string.Empty;
                lst.FirmContactName = dt.Rows[0]["FirmPerson"].ToString() ?? string.Empty;
                lst.FirmEmail = dt.Rows[0]["FirmEmail"].ToString() ?? string.Empty;
                lst.FirmPANNo = dt.Rows[0]["FirmPANNo"].ToString() ?? string.Empty;
                lst.FirmAddress = dt.Rows[0]["FirmAddress"].ToString() ?? string.Empty;
                lst.FirmContactNo = dt.Rows[0]["FirmContactNo"].ToString() ?? string.Empty;
                lst.FirmAlternate = dt.Rows[0]["FirmAlternateContactNo"].ToString() ?? string.Empty;
                lst.CampusCode = dt.Rows[0]["CampusCode"].ToString() ?? string.Empty;
                lst.CampusName = dt.Rows[0]["CampusName"].ToString() ?? string.Empty;
                lst.RelativePerson = dt.Rows[0]["RelativePersonName"].ToString() ?? string.Empty;
                lst.AdditionalName = dt.Rows[0]["AdditionalName"].ToString() ?? string.Empty;
                lst.Purpose = dt.Rows[0]["Purpose"].ToString() ?? string.Empty;
                lst.InitiateOn = dt.Rows[0]["AppDateDisp"].ToString() ?? string.Empty;
                lst.ExpBillDate = dt.Rows[0]["ExpDate"].ToString() ?? string.Empty;
                lst.Actualmount = dt.Rows[0]["TotalAmount"].ToString();
                lst.AmountSantioned = dt.Rows[0]["TotalAmount"].ToString() ?? string.Empty;
                lst.AmountDiscount = dt.Rows[0]["Dis"].ToString() ?? string.Empty;


                //Load Vendor

                List<TextValues> VendorOffice = new List<TextValues>();

                // if (dt.Rows[0]["PExtra3"].ToString() != "")
                {
                    //Task<DataTable> GetOffices(string VendorId)
                    DataTable OfficeData = await _advanceRepository.GetOffices(dt.Rows[0]["VendorID"].ToString() ?? "");
                    if (OfficeData.Rows.Count > 0)
                    {
                        foreach (DataRow row in OfficeData.Rows)
                        {
                            VendorOffice.Add(new TextValues(row));
                        }
                    }
                    else
                    {
                        DataRow DR = OfficeData.NewRow();
                        DR = OfficeData.NewRow();
                        DR[0] = "----";
                        OfficeData.Rows.Add(DR);
                        VendorOffice.Add(new TextValues(DR));
                    }
                    lst.Offices = VendorOffice;

                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }
        }

        public async Task<ApiResponse> GetAuthority(string RefNo)
        {
            List<TextValues> App1List = new List<TextValues>();
            List<TextValues> App2List = new List<TextValues>();
            List<TextValues> App3List = new List<TextValues>();
            List<TextValues> App4List = new List<TextValues>();

            using (DataTable dt = await _advanceRepository.GetPurchaseBasicForBill(RefNo))
            {


                if (dt.Rows[0]["App3ID"].ToString()!.Length > 0)
                {
                    App1List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App3Name"].ToString() + " - " + dt.Rows[0]["App3Designation"].ToString(),
                        Value = dt.Rows[0]["App3ID"].ToString() + "#" + dt.Rows[0]["App3Name"].ToString()
                    });
                    App1List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App2Name"].ToString() + " - " + dt.Rows[0]["App2Designation"].ToString(),
                        Value = dt.Rows[0]["App2ID"].ToString() + "#" + dt.Rows[0]["App2Name"].ToString()
                    });
                    App1List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App1Name"].ToString() + " - " + dt.Rows[0]["App1Designation"].ToString(),
                        Value = dt.Rows[0]["App1ID"].ToString() + "#" + dt.Rows[0]["App1Name"].ToString()
                    });
                    App2List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App3Name"].ToString() + " - " + dt.Rows[0]["App3Designation"].ToString(),
                        Value = dt.Rows[0]["App3ID"].ToString() + "#" + dt.Rows[0]["App3Name"].ToString()
                    });
                    App2List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App2Name"].ToString() + " - " + dt.Rows[0]["App2Designation"].ToString(),
                        Value = dt.Rows[0]["App2ID"].ToString() + "#" + dt.Rows[0]["App2Name"].ToString()
                    });
                    App2List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App1Name"].ToString() + " - " + dt.Rows[0]["App1Designation"].ToString(),
                        Value = dt.Rows[0]["App1ID"].ToString() + "#" + dt.Rows[0]["App1Name"].ToString()
                    });
                }
                if (dt.Rows[0]["App2ID"].ToString()!.Length > 0)
                {
                    App1List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App2Name"].ToString() + " - " + dt.Rows[0]["App2Designation"].ToString(),
                        Value = dt.Rows[0]["App2ID"].ToString() + "#" + dt.Rows[0]["App2Name"].ToString()
                    });
                    App1List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App1Name"].ToString() + " - " + dt.Rows[0]["App1Designation"].ToString(),
                        Value = dt.Rows[0]["App1ID"].ToString() + "#" + dt.Rows[0]["App1Name"].ToString()
                    });
                    App2List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App2Name"].ToString() + " - " + dt.Rows[0]["App2Designation"].ToString(),
                        Value = dt.Rows[0]["App2ID"].ToString() + "#" + dt.Rows[0]["App2Name"].ToString()
                    });
                    App2List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App1Name"].ToString() + " - " + dt.Rows[0]["App1Designation"].ToString(),
                        Value = dt.Rows[0]["App1ID"].ToString() + "#" + dt.Rows[0]["App1Name"].ToString()
                    });
                }
                else
                {
                    App1List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App1Name"].ToString() + " - " + dt.Rows[0]["App1Designation"].ToString(),
                        Value = dt.Rows[0]["App1ID"].ToString() + "#" + dt.Rows[0]["App1Name"].ToString()
                    });
                    App2List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App1Name"].ToString() + " - " + dt.Rows[0]["App1Designation"].ToString(),
                        Value = dt.Rows[0]["App1ID"].ToString() + "#" + dt.Rows[0]["App1Name"].ToString()
                    });
                }


                //get 3 and 4 th authority

                if (dt.Rows[0]["CampusCode"].ToString() != "101")
                {
                    DataTable ThirdAuth = await _advanceRepository.GetThirdAuth();
                    foreach (DataRow dr in ThirdAuth.Rows)
                    {
                        App3List.Add(new TextValues
                        {
                            EmpCode = dr["Value"].ToString(),
                            Value = dr["Value2"].ToString(),
                            Text = dr["Text"].ToString(),
                        });
                    }
                    DataTable Forth = await _advanceRepository.Get4Auth();
                    foreach (DataRow dr in ThirdAuth.Rows)
                    {
                        App4List.Add(new TextValues
                        {
                            EmpCode = dr["Value"].ToString(),
                            Value = dr["Value2"].ToString(),
                            Text = dr["Text"].ToString(),
                        });
                    }
                }
                else
                {
                    DataTable AuthList = await _advanceRepository.GetFinanceAuthority();
                    foreach (DataRow dr in AuthList.Rows)
                    {
                        App3List.Add(new TextValues
                        {
                            EmpCode = dr["Value"].ToString(),
                            Value = dr["Value2"].ToString(),
                            Text = dr["Text"].ToString(),
                        });

                    }
                    App4List = App3List;
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", new { App1List, App2List, App3List, App4List });
            }

        }
        //Task <DataTable> CanGenerateBill()
        public async Task<ApiResponse> GeneratereNoForUploadBill(string Type, string EmpCode, bool IsThousand, string RefNo = "")
        {
            using (DataTable d = await _advanceRepository.CanGenerateBill(Type, EmpCode, IsThousand, RefNo))
            {
                List<TextValues> textValues = new List<TextValues>();
                foreach (DataRow dr in d.Rows)
                {
                    textValues.Add(new TextValues(dr));

                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", textValues);
            }
        }


        public async Task<ApiResponse> GeneratereNoForUploadBillAgainstAdvance(string Type, string EmpCode, string RefNo = "")
        {
            using (DataTable d = await _advanceRepository.GetrefnoForGenerateBillAdvance(Type, EmpCode, RefNo))
            {
                List<TextValues> textValues = new List<TextValues>();
                foreach (DataRow dr in d.Rows)
                {
                    textValues.Add(new TextValues(dr));

                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", textValues);
            }
        }


        public async Task<ApiResponse> GetPurchase(string Type, string Empcode)
        {
            using (DataTable dt = await _advanceRepository.LoadApprovalDetails(Type, Empcode))
            {
                List<TextValues> lst = new List<TextValues>();
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new TextValues(dr));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }
        }

        //public async Task<ApiResponse> GetPrintDetailsAdvance(string Id)
        //{
        //    using(DataTable dt=await _advanceRepository.GetbasicPrintDetails(Id))
        //    {
        //        DataTable ContactNo = await _advanceRepository.GetContactNo(dt.Rows[0]["RelativePersonID"].ToString()??string.Empty);

        //    }
        //}

        public async Task<ApiResponse> GetBasicDetailsVisit(string AppType, string Type, string EmpCode)
        {
            using (DataTable dt = await _advanceRepository.GetVisitBasicDetails(Type, EmpCode, "PLC"))
            {
                List<TextValues> lst = new List<TextValues>();
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new TextValues(dr));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }
        }
        public async Task<ApiResponse> GetartnerVisit(string AppType, string Type, string EmpCode)
        {
            using (DataTable dt = await _advanceRepository.GetPartnerVisit(Type, EmpCode, "CVV"))
            {
                List<TextValues> lst = new List<TextValues>();
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new TextValues(dr));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }
        }
        public async Task<ApiResponse> GetSchoolVisit(string AppType, string Type, string EmpCode)
        {
            using (DataTable dt = await _advanceRepository.GetSchoolVisit(Type, EmpCode, "ADM"))
            {
                List<TextValues> lst = new List<TextValues>();
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new TextValues(dr));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }
        }
        public async Task<ApiResponse> GetSubfirm(int VendorId)
        {
            using (DataTable dt = await _advanceRepository.LoadSubFirm(VendorId))
            {
                List<TextValues> textValues = new List<TextValues>();
                foreach (DataRow dr in dt.Rows)
                {
                    textValues.Add(new TextValues(dr));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", textValues);
            }
        }
        public async Task<ApiResponse> LoadOffices(string TypeId, string VendorId)
        {
            using (DataTable dt = await _advanceRepository.LoadOffices(TypeId, VendorId))
            {
                List<TextValues> lst = new List<TextValues>();
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new TextValues(dr));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }

        }


        public async Task<ApiResponse> GetBasicDetailsForGenerateAdvance(string Values, string Type)
        {
            purchaseApprovalDetailsResponse details = new purchaseApprovalDetailsResponse();
            string[] splt = Values.Split('#');
            using (DataTable dt = await _advanceRepository.CheckPreviousPendingBill(splt[0]))
            {
                List<TextValues> VendorDetails = new List<TextValues>();

                if (dt.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status200OK, "Warning", "Sorry! Previous Advance Approval Is Still Pending To Approve For Same Purchase Approval Initiated By " + dt.Rows[0]["IniName"].ToString() + " on " + dt.Rows[0]["AppDate"].ToString() + " of Amount : " + dt.Rows[0]["Amount"].ToString() + " Rs/-");
                }
                DataTable dtt = await _advanceRepository.GetVendorTextValue(splt[1]);

                foreach (DataRow row in dtt.Rows)
                {
                    VendorDetails.Add(new TextValues(row));
                }
                DataTable dts = await _advanceRepository.GetDepartDetails(Values, Type);
                if (dts.Rows.Count > 0)
                {
                    details.Department = dts.Rows[0]["ForDepartment"].ToString();
                    details.Purpose = dts.Rows[0]["ForDepartment"].ToString();
                    details.Note = dts.Rows[0]["ForDepartment"].ToString();
                }
                DataTable BalanceDetails = await _advanceRepository.GetBalanceDetails(splt[0]);
                //DataTable PreviousDetails = await _advanceRepository.GetVendorDetails(RefNo);
                if (BalanceDetails.Rows.Count > 0)
                {
                    if (BalanceDetails.Rows[0][0].ToString()!.Length > 0)
                    {

                        details.BalanceAmount = ((Convert.ToInt32(splt[2]!.Replace(".00", "")) - Convert.ToInt32(BalanceDetails.Rows[0][0].ToString())) >= 0 ? (Convert.ToInt32(splt[2].Replace(".00", "")) - Convert.ToInt32(BalanceDetails.Rows[0][0].ToString())) : 0).ToString();
                    }
                    else
                    {
                        details.BalanceAmount = splt[2].ToString()!.Replace(".00", "");
                    }
                }
                else
                {
                    details.BalanceAmount = splt[2].ToString()!.Replace(".00", "");
                }
                details.Vendorlst = VendorDetails;
            }


            return new ApiResponse(StatusCodes.Status200OK, "Success", details);
        }

        public async Task<ApiResponse> SaveBill(string EmpCode, string EmpName, AddBillGenerateRequest req, string Type)
        {
            if (!string.IsNullOrEmpty(req.RefNo))
            {
                if (req.ForTypeOf == "Advance Bill")
                {
                    DataTable cann = await _advanceRepository.GetrefnoForGenerateBillAdvance(Type, EmpCode, req.RefNo!);
                    if (cann.Rows.Count <= 0)
                    {
                        return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Invalid Access");
                    }
                    using (DataTable dt = await _advanceRepository.CheckPreviousPendingBill(req.RefNo!))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            return new ApiResponse(StatusCodes.Status200OK, "Warning", "Sorry! Previous Advance Approval Is Still Pending To Approve For Same Purchase Approval Initiated By " + dt.Rows[0]["IniName"].ToString() + " on " + dt.Rows[0]["AppDate"].ToString() + " of Amount : " + dt.Rows[0]["Amount"].ToString() + " Rs/-");
                        }
                    }
                }
                else
                {
                    DataTable dt = await _advanceRepository.CanGenerateBill(Type, EmpCode, Convert.ToInt32(req.Amount) > 1000 ? false : true, req.RefNo!);
                    if (dt.Rows.Count <= 0)
                    {
                        return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Invalid Access");
                    }
                }
            }

            string BillRefNo = "";
            using (DataTable GetBillBaseRefNo = await _advanceRepository.GetBillBaseRefNo())
            {
                if (GetBillBaseRefNo.Rows.Count > 0)
                {
                    string myid = GetBillBaseRefNo.Rows[0][0].ToString() ?? string.Empty;
                    string NewId = GetBillBaseRefNo.Rows[0][0].ToString() ?? string.Empty;
                    BillRefNo = GetBillBaseRefNo.Rows[0][0].ToString() ?? string.Empty;
                }
                //Task<int> SaveBillAuth(string EmpCode, string EmpName, AddBillGenerateRequest req, string BillBaseREfNo)
                //Task<int> SaveBillSave(string EmpCode,string EmpName, AddBillGenerateRequest req,string BillBaseREfNo)
                int saverecord = await _advanceRepository.SaveBillSave(EmpCode, EmpName, req, BillRefNo);
                int SaveAuth = await _advanceRepository.SaveBillAuth(EmpCode, EmpName, req, BillRefNo);



                //FileName = _general.EncryptWithKey(FileName, await _inclusiveService.GetEnCryptedKey());
                //string str = await _inclusiveService.SaveFile(FileName, FilePath, file, ".pdf");
                //check pdf file and delete
                string FilePath = Directory.GetCurrentDirectory();
                FilePath = Path.Combine(FilePath, "Upload_Bills");
                string FileName = BillRefNo + ".pdf";

                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }
                if (req.pdf != null && req.pdf.Length > 0)
                {
                    //delete file
                    if (File.Exists(Path.Combine(FilePath, FileName)))
                    {
                        File.Delete(Path.Combine(FilePath, FileName));
                    }
                    //save file
                    string str = await _inclusiveService.SaveFile(BillRefNo, FilePath, req.pdf, ".pdf");
                    int updatepdf = await _advanceRepository.UpdatePdf(BillRefNo);
                }
                //check Excel file and delete and save
                if (req.ExcelFile != null && req.ExcelFile!.Length > 0)
                {
                    FileName = BillRefNo + ".xlsx";
                    if (File.Exists(Path.Combine(FilePath, FileName)))
                    {
                        File.Delete(Path.Combine(FilePath, FileName));
                    }
                    //save file
                    string str = await _inclusiveService.SaveFile(BillRefNo, FilePath, req.ExcelFile!, ".xlsx");
                    int updateExcel = await _advanceRepository.UpdateExcel(BillRefNo);
                }

                return new ApiResponse(StatusCodes.Status200OK, "Success", "Bill Upload Successfully");
            }
        }
        public async Task<ApiResponse> GetDetails(string Cond)
        {
            using (DataTable dt = await _advanceRepository.GetFirm(Cond))
            {
                List<TextValues> values = new List<TextValues>();
                foreach (DataRow dr in dt.Rows)
                {
                    values.Add(new TextValues(dr));
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", values);
            }
        }
        public async Task<ApiResponse> GetLockDetails(string Type)
        {
            DateLockResponse lst = new DateLockResponse();
            using (DataTable dt = await _advanceRepository.GetDateLock(Type))
            {

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Type"].ToString() == "Initiation")
                    {
                        lst.InitiateOn = DateTime.Now.AddDays(-Convert.ToInt32(dr["MyLimit"].ToString()));
                        lst.TestingOnDate = DateTime.Now.AddDays(-Convert.ToInt32(dr["MyLimit"].ToString()));
                    }
                    if (dr["Type"].ToString() == "Bill")
                    {
                        lst.BillDate = DateTime.Now.AddDays(-Convert.ToInt32(dr["MyLimit"].ToString()));
                    }
                    if (dr["Type"].ToString() == "Bill On Gate")
                    {
                        lst.BillOnGate = DateTime.Now.AddDays(-Convert.ToInt32(dr["MyLimit"].ToString()));
                    }
                    if (dr["Type"].ToString() == "Gate")
                    {
                        lst.GateDate = DateTime.Now.AddDays(-Convert.ToInt32(dr["MyLimit"].ToString()));
                    }
                    if (dr["Type"].ToString() == "Department")
                    {
                        lst.DepartmentDate = DateTime.Now.AddDays(-Convert.ToInt32(dr["MyLimit"].ToString()));
                    }
                    if (dr["Type"].ToString() == "Store")
                    {
                        lst.StoreDate = DateTime.Now.AddDays(-Convert.ToInt32(dr["MyLimit"].ToString()));
                    }
                    lst.EndDate = DateTime.Now;

                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }
        }

        public async Task<ApiResponse> GetAdvanceApprovalPrint(string ReferenceNo, string EmployeeId, string EmployeeType)
        {
            using (DataTable dtApproval = await _advanceRepository.GetAdvancePrintApprovalDetails(ReferenceNo))
            {
                if (dtApproval.Rows.Count == 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Approval Details Found...");
                }

                HashSet<string> MissionAdmissionTypes = new HashSet<string>();
                using (DataTable dtMissionAdmissionTypes = await _advanceRepository.GetMissionAdmissionReturnTypes())
                {
                    foreach (DataRow dr in dtMissionAdmissionTypes.Rows)
                    {
                        MissionAdmissionTypes.Add(dr["Value"].ToString() ?? string.Empty);
                    }
                }

                AdvanceApprovalPrintDetailsResponse advanceApprovalPrintDetailsResponse = new AdvanceApprovalPrintDetailsResponse(dtApproval.Rows[0]);

                EmployeeDetails employeeDetails = await _inclusiveService.GetEmployeeDetailsByEmployeeCode(advanceApprovalPrintDetailsResponse.RelativePersonID);
                advanceApprovalPrintDetailsResponse.RelativePersonContactNo = employeeDetails.ContactNo ?? string.Empty;
                advanceApprovalPrintDetailsResponse.RelativePersonName = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.RelativePersonName ?? string.Empty);
                advanceApprovalPrintDetailsResponse.RelativeDepartment = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.RelativeDepartment ?? string.Empty);
                advanceApprovalPrintDetailsResponse.RelativeDesignation = _general.ToTitleCase(employeeDetails.Designation ?? string.Empty);



                if (!string.IsNullOrWhiteSpace(advanceApprovalPrintDetailsResponse.AdvFromShow))
                {
                    string leaveDetailsForCodes = advanceApprovalPrintDetailsResponse.RelativePersonID ?? string.Empty;

                    using (DataTable dtLeaveEmployeeCodes = await _advanceRepository.GetAdvanceOfficeMappingEmployeeCodes(advanceApprovalPrintDetailsResponse.PExtra3!, advanceApprovalPrintDetailsResponse.Session!))
                    {
                        if (dtLeaveEmployeeCodes.Rows.Count > 0 && !string.IsNullOrWhiteSpace(dtLeaveEmployeeCodes.Rows[0][0]?.ToString()))
                        {
                            leaveDetailsForCodes = dtLeaveEmployeeCodes.Rows[0][0]?.ToString() ?? string.Empty;
                        }
                    }
                    using (DataTable dtLeaves = await _advanceRepository.GetAdvanceEmployeeLeaveDetails(leaveDetailsForCodes, advanceApprovalPrintDetailsResponse.AdvFrom!, advanceApprovalPrintDetailsResponse.AdvTo!))
                    {
                        foreach (DataRow dr in dtLeaves.Rows)
                        {
                            AdvanceEmployeeLeaveDetails advanceEmployeeLeaveDetails = new AdvanceEmployeeLeaveDetails(dr);
                            advanceEmployeeLeaveDetails.Name = _general.ToTitleCase(advanceEmployeeLeaveDetails.Name ?? string.Empty);

                            advanceApprovalPrintDetailsResponse?.LeaveDetails?.Add(advanceEmployeeLeaveDetails);
                        }
                    }
                }

                using (DataTable dtBillDetails = await _advanceRepository.GetAdvanceBillSummaryDetails(ReferenceNo))
                {
                    foreach (DataRow dr in dtBillDetails.Rows)
                    {
                        AdvanceBillDetails advanceBillSummaryDetails = new AdvanceBillDetails(dr);
                        if (MissionAdmissionTypes.Contains(advanceBillSummaryDetails.Head ?? string.Empty))
                        {
                            advanceBillSummaryDetails.IsHighlighted = true;
                        }
                        advanceApprovalPrintDetailsResponse?.BillDetails?.Add(advanceBillSummaryDetails);
                    }

                    if (advanceApprovalPrintDetailsResponse?.BillDetails?.Count > 0)
                    {
                        advanceApprovalPrintDetailsResponse.BillDetailsTotal = advanceApprovalPrintDetailsResponse.BillDetails.Sum(s => s.Amount ?? 0);
                    }
                }

                if (Array.IndexOf(new string[] { "ACCOUNTS", "FINANCE OFFICER", "ACCOUNTS OFFICER", "MANAGEMENT" }, EmployeeType.ToUpper()) >= 0)
                {
                    advanceApprovalPrintDetailsResponse!.CanEditNote = true;
                }


                string paymentReferenceNo = "";
                string overAllMaxAmount = "";

                string[] paymentCheckStatus = new string[] { "Clr.", "Rcv.", "Sgn." };

                if (!string.IsNullOrWhiteSpace(advanceApprovalPrintDetailsResponse?.PDetails))
                {
                    string[]? paymentDetailsArray = advanceApprovalPrintDetailsResponse?.PDetails.Split('#');
                    if (paymentDetailsArray != null && paymentDetailsArray.Length >= 3 && !paymentDetailsArray[0].StartsWith("PLC") && !paymentDetailsArray[0].StartsWith("ADM"))
                    {
                        paymentReferenceNo = paymentDetailsArray[0];
                        overAllMaxAmount = paymentDetailsArray[2];
                    }
                }

                if (string.IsNullOrWhiteSpace(paymentReferenceNo))
                {
                    double acamt = Convert.ToDouble(advanceApprovalPrintDetailsResponse?.TotalAmount);
                    using (DataTable dtPaymentDetails = await _advanceRepository.GetAdvancePaymentDetailsByReferenceNo(paymentReferenceNo))
                    {
                        if (dtPaymentDetails.Rows.Count > 0)
                        {
                            advanceApprovalPrintDetailsResponse!.AdvanceOn = dtPaymentDetails.Rows[0]["Bank"]?.ToString() ?? "----";
                        }


                        foreach (DataRow dr in dtPaymentDetails.Rows)
                        {
                            AdvancePaymentDetails advancePaymentDetails = new AdvancePaymentDetails(dr);
                            if (Array.IndexOf(paymentCheckStatus, advancePaymentDetails.Status) >= 0)
                            {
                                advancePaymentDetails.ShowFinalImage = true;
                            }
                            advanceApprovalPrintDetailsResponse?.PaymentDetails?.Add(advancePaymentDetails);
                        }
                    }


                    long totam = advanceApprovalPrintDetailsResponse?.PaymentDetails?.Sum(s => long.TryParse(s.IssuedAmount, out long amt) ? amt : 0) ?? 0;
                    if (acamt >= totam)
                    {
                        advanceApprovalPrintDetailsResponse!.PaymentDetailsFinalRemark = "Total Issue Amount @ " + totam + " Rs./- And Balance Amount @ " + (acamt - totam) + " Rs./- ";
                    }
                    else
                    {
                        advanceApprovalPrintDetailsResponse!.PaymentDetailsFinalRemark = "Total Issue Amount @ " + totam + " Rs./- And Over Amount @ " + (totam - acamt) + " Rs./- ";
                    }

                    using (DataTable dtBillBaseTransactionIds = await _advanceRepository.GetAdvancePaymentTransactionIdsByReferenceNo(ReferenceNo))
                    {
                        if (dtBillBaseTransactionIds.Rows.Count > 0 && !string.IsNullOrWhiteSpace(dtBillBaseTransactionIds.Rows[0][0]?.ToString()))
                        {
                            using (DataTable dtBillDetails = await _advanceRepository.GetAdvanceBillDetailsByTransactionIds(dtBillBaseTransactionIds.Rows[0][0]?.ToString() ?? string.Empty))
                            {
                                int gtot = 0;

                                List<AdvanceApprovalBillDetailsPrintOut> advanceApprovalBillDetailsPrintOuts = new List<AdvanceApprovalBillDetailsPrintOut>();

                                foreach (DataRow dr in dtBillDetails.Rows)
                                {

                                    AdvanceApprovalBillDetailsPrintOut advanceApprovalBillDetailsPrintOut = new AdvanceApprovalBillDetailsPrintOut(dr);
                                    advanceApprovalBillDetailsPrintOut.LastUpdatedBy = _general.ToTitleCase(advanceApprovalBillDetailsPrintOut.LastUpdatedBy ?? string.Empty);
                                    advanceApprovalBillDetailsPrintOut.Status = _general.ToTitleCase(advanceApprovalBillDetailsPrintOut.Status ?? string.Empty);

                                    using (DataTable dtBillDisributionDetails = await _advanceRepository.GetAdvanceBillBillDistributionDetailsById(advanceApprovalBillDetailsPrintOut.SequenceID!))
                                    {
                                        int stot = 0;

                                        List<AdvanceBillDistributionDetails> distributionDetails = new List<AdvanceBillDistributionDetails>();

                                        foreach (DataRow drDist in dtBillDisributionDetails.Rows)
                                        {
                                            AdvanceBillDistributionDetails advanceBillDistributionDetails = new AdvanceBillDistributionDetails(drDist);
                                            stot += Convert.ToInt32(advanceBillDistributionDetails.Amount);
                                            gtot += Convert.ToInt32(advanceBillDistributionDetails.Amount);
                                            if (MissionAdmissionTypes.Contains(advanceBillDistributionDetails.Title!))
                                            {
                                                advanceBillDistributionDetails.IsHighlighted = true;
                                            }
                                            distributionDetails.Add(advanceBillDistributionDetails);
                                        }
                                        advanceApprovalBillDetailsPrintOut.DistributionDetails = distributionDetails;
                                        advanceApprovalBillDetailsPrintOut.DistributionDetailsTotal = stot;
                                    }

                                    advanceApprovalBillDetailsPrintOuts.Add(advanceApprovalBillDetailsPrintOut);
                                }

                                AdvanceBillAgainstBaseSummaryDetails advanceBillAgainstBaseSummaryDetails = new AdvanceBillAgainstBaseSummaryDetails();
                                advanceBillAgainstBaseSummaryDetails.BillAgainstBaseDetails = advanceApprovalBillDetailsPrintOuts;

                                advanceBillAgainstBaseSummaryDetails.TotalAdvance = totam.ToString() + " Rs./-";
                                advanceBillAgainstBaseSummaryDetails.BillAmount = gtot.ToString() + " Rs./-";
                                advanceBillAgainstBaseSummaryDetails.Difference = (totam - gtot) + " Rs./-";
                                advanceBillAgainstBaseSummaryDetails.FinalStatus = string.Empty;
                                advanceBillAgainstBaseSummaryDetails.ExcessBill = string.Empty;
                                if ((totam - gtot) >= 0)
                                {
                                    advanceBillAgainstBaseSummaryDetails.FinalStatus = "Final Status";
                                    advanceBillAgainstBaseSummaryDetails.ExcessBill = advanceBillAgainstBaseSummaryDetails.Difference + " ( Amount Left )";
                                }
                                else
                                {
                                    advanceBillAgainstBaseSummaryDetails.FinalStatus = "Final Status";
                                    advanceBillAgainstBaseSummaryDetails.ExcessBill = advanceBillAgainstBaseSummaryDetails.Difference + " ( Excess Bill )";
                                }
                                advanceBillAgainstBaseSummaryDetails.TotalBills = dtBillDetails.Rows.Count.ToString();
                                advanceApprovalPrintDetailsResponse!.BillAgainstBaseDetailsSummary = advanceBillAgainstBaseSummaryDetails;
                            }



                            using DataTable dtBillDistributionDetails = await _advanceRepository.GetAdvanceBillAgainstBaseDistributionDetailsByTransactionIds(dtBillBaseTransactionIds.Rows[0][0]?.ToString() ?? string.Empty);

                            using DataTable dtOtherApprovalDistributionDetails = await _advanceRepository.GetAdvanceOtherSummaryDistributionDetailsByReferenceNo(ReferenceNo);

                            if (dtBillDistributionDetails.Rows.Count > 0 && dtOtherApprovalDistributionDetails.Rows.Count > 0)
                            {
                                DataTable Vs = new DataTable();
                                Vs.Columns.Add("Head");
                                Vs.Columns.Add("Advance");
                                Vs.Columns.Add("Bill");
                                Vs.Columns.Add("Diff");

                                List<string> H = dtBillDistributionDetails.AsEnumerable().Select(p => p.Field<string>("Head")).ToList()!;
                                H.AddRange(dtOtherApprovalDistributionDetails.AsEnumerable().Select(p => p.Field<string>("Head")).ToList()!);

                                H = H.Distinct().ToList();
                                H.Sort();

                                foreach (string s in H)
                                {
                                    long BillAmt = (dtBillDistributionDetails.AsEnumerable().Where(p => p.Field<string>("Head") == s).Any() ? dtBillDistributionDetails.AsEnumerable().Where(p => p.Field<string>("Head") == s).Sum(p => p.Field<long>("Amount")) : 0);
                                    long AdvAmt = (dtOtherApprovalDistributionDetails.AsEnumerable().Where(p => p.Field<string>("Head") == s).Any() ? dtOtherApprovalDistributionDetails.AsEnumerable().Where(p => p.Field<string>("Head") == s).Sum(p => p.Field<long>("Amount")) : 0);

                                    DataRow DR = Vs.NewRow();
                                    DR[0] = s;
                                    DR[1] = AdvAmt;
                                    DR[2] = BillAmt;
                                    DR[3] = AdvAmt - BillAmt;

                                    Vs.Rows.Add(DR);
                                }

                                List<AdvanceDistributionVsSubmittedBillDistribution> distributionVsSubmittedBillDistributions = new List<AdvanceDistributionVsSubmittedBillDistribution>();
                                foreach (DataRow dr in Vs.Rows)
                                {
                                    AdvanceDistributionVsSubmittedBillDistribution advanceDistributionVsSubmittedBillDistribution = new AdvanceDistributionVsSubmittedBillDistribution(dr);
                                    if (MissionAdmissionTypes.Contains(advanceDistributionVsSubmittedBillDistribution.Head!))
                                    {
                                        advanceDistributionVsSubmittedBillDistribution.IsHighlighted = true;
                                    }
                                    distributionVsSubmittedBillDistributions.Add(advanceDistributionVsSubmittedBillDistribution);
                                }

                                AdvanceDistributionVsSubmittedBillDistributionSummary advanceDistributionVsSubmittedBillDistributionSummary = new AdvanceDistributionVsSubmittedBillDistributionSummary();
                                advanceDistributionVsSubmittedBillDistributionSummary.DistributionDetails = distributionVsSubmittedBillDistributions;
                                advanceDistributionVsSubmittedBillDistributionSummary.TotalAdvance = distributionVsSubmittedBillDistributions.Sum(s => s.Advance ?? 0);
                                advanceDistributionVsSubmittedBillDistributionSummary.TotalBill = distributionVsSubmittedBillDistributions.Sum(s => s.Bill ?? 0);
                                advanceDistributionVsSubmittedBillDistributionSummary.TotalDiff = distributionVsSubmittedBillDistributions.Sum(s => s.Diff ?? 0);

                                advanceApprovalPrintDetailsResponse!.AdvanceDistributionVsSubmittedBill = advanceDistributionVsSubmittedBillDistributionSummary;
                            }

                        }
                    }


                }
                else
                {
                    using (DataTable dtPaymentDetails = await _advanceRepository.GetAdvancePaymentDetailsByPaymentGroupId(paymentReferenceNo))
                    {
                        foreach (DataRow dr in dtPaymentDetails.Rows)
                        {
                            AdvancePaymentDetails advancePaymentDetails = new AdvancePaymentDetails(dr);

                            if (Array.IndexOf(paymentCheckStatus, advancePaymentDetails.Status) >= 0)
                            {
                                advancePaymentDetails.ShowFinalImage = true;
                            }

                            advanceApprovalPrintDetailsResponse?.PaymentDetails?.Add(advancePaymentDetails);
                        }

                        double acamt = Convert.ToDouble(overAllMaxAmount);
                        long totam = advanceApprovalPrintDetailsResponse?.PaymentDetails?.Sum(s => long.TryParse(s.IssuedAmount, out long amt) ? amt : 0) ?? 0;

                        if (acamt >= totam)
                        {
                            advanceApprovalPrintDetailsResponse!.PaymentDetailsFinalRemark = "Total Issue Amount @ " + totam + " Rs./- And Balance Amount @ " + (acamt - totam) + " Rs./- ";
                        }
                        else
                        {
                            advanceApprovalPrintDetailsResponse!.PaymentDetailsFinalRemark = "Total Issue Amount @ " + totam + " Rs./- And Over Amount @ " + (totam - acamt) + " Rs./- ";
                        }
                    }
                }


                if (!string.IsNullOrWhiteSpace(advanceApprovalPrintDetailsResponse?.PDetails))
                {
                    string[] sst = advanceApprovalPrintDetailsResponse?.PDetails?.ToString().Split('#')!;
                    if (sst.Length > 0)
                    {
                        if (sst[0].StartsWith("PLC"))
                        {
                            advanceApprovalPrintDetailsResponse!.AdvanceAgainstTextRemarkSummary = "This advance request is against the Corporate Visit Approval (Reference No. - <a href='#' style='font-weight:bold;' onclick=\"openPopUp('https://glauniversity.in:8105/main/reports/VO.aspx?RefNo=" + _general.Encrypt(sst[0].Replace("PLC", "")) + "',0,0); return false;\"><u>" + HttpUtility.HtmlDecode(sst[0]) + "</u></a>) of amount " + sst[2] + " Rs./- dated on " + sst[3] + ".";
                        }
                        else
                        {
                            if (sst[0].StartsWith("ADM"))
                            {
                                advanceApprovalPrintDetailsResponse!.AdvanceAgainstTextRemarkSummary = "This advance request is against the School Visit Approval (Reference No. - <a href='#' style='font-weight:bold;' onclick=\"openPopUp('https://glauniversity.in:8109/main/reports/VO.aspx?RefNo=" + _general.Encrypt(sst[0].Replace("ADM", "")) + "',0,0); return false;\"><u>" + HttpUtility.HtmlDecode(sst[0]) + "</u></a>) of amount " + sst[2] + " Rs./- dated on " + sst[3] + ".";
                            }
                            else
                            {
                                advanceApprovalPrintDetailsResponse!.AdvanceAgainstTextRemarkSummary = "This advance request is against the Purchase Approval (Reference No. - <u>" + sst[0] + "</u>) of amount " + sst[2] + " Rs./- dated on " + sst[3] + ".";
                            }
                        }
                    }
                }

                if (advanceApprovalPrintDetailsResponse?.BudgetAmount != "N/A" && Convert.ToInt32(advanceApprovalPrintDetailsResponse?.BudgetAmount ?? "0") > 0)
                {
                    if (advanceApprovalPrintDetailsResponse?.MyType?.Contains("Against") == true)
                    {
                        int curs = Convert.ToInt32(advanceApprovalPrintDetailsResponse?.BudgetAmount) - Convert.ToInt32(advanceApprovalPrintDetailsResponse?.PreviousTaken) + Convert.ToInt32(advanceApprovalPrintDetailsResponse?.CurStatus);

                        advanceApprovalPrintDetailsResponse!.AdvanceBudgetTextRemarkSummary = "Budget Reference No. -  <u>" + advanceApprovalPrintDetailsResponse?.BudgetReferenceNo + "</u><br/> Budget Amount : " + advanceApprovalPrintDetailsResponse?.BudgetAmount + " Rs./-<br/>Previous Sanctioned : " + advanceApprovalPrintDetailsResponse?.PreviousTaken + " Rs./-<br/>Approval Amount : " + (curs) + " Rs./-<br/>" + (advanceApprovalPrintDetailsResponse?.BudgetStatus!.Contains("Over") == true ? ("<font color='red'>Final Status : " + advanceApprovalPrintDetailsResponse?.BudgetStatus + "</font>") : ("<font color='green'>Final Status : " + advanceApprovalPrintDetailsResponse?.BudgetStatus + "</font>")) + "</font>";
                    }
                    else
                    {
                        advanceApprovalPrintDetailsResponse!.AdvanceBudgetTextRemarkSummary = "Budget Reference No. - <u>" + advanceApprovalPrintDetailsResponse?.BudgetReferenceNo + "</u><br/> Budget Amount : " + advanceApprovalPrintDetailsResponse?.BudgetAmount + " Rs./-<br/>Previous Sanctioned : " + advanceApprovalPrintDetailsResponse?.PreviousTaken + " Rs./-<br/>Requested Amount : " + advanceApprovalPrintDetailsResponse?.TotalAmount + " Rs./-<br/>" + (advanceApprovalPrintDetailsResponse?.BudgetStatus!.Contains("Over") == true ? ("<font color='red'>Final Status : " + advanceApprovalPrintDetailsResponse?.BudgetStatus + "</font>") : ("<font color='green'>Final Status : " + advanceApprovalPrintDetailsResponse?.BudgetStatus + "</font>")) + "</font>";
                    }



                    using (DataTable dtAmountIssuedAgainstBudget = await _advanceRepository.GetAdvanceAmountIssuedAgainstBudgetByReferenceNo(advanceApprovalPrintDetailsResponse?.BudgetReferenceNo!))
                    {
                        List<AdvanceAmountIssuedAgainstBudgetDetails> advanceAmountIssuedAgainstBudgetDetails = new List<AdvanceAmountIssuedAgainstBudgetDetails>();

                        int totis = 0;
                        foreach (DataRow dr in dtAmountIssuedAgainstBudget.Rows)
                        {
                            AdvanceAmountIssuedAgainstBudgetDetails advanceAmountIssuedAgainstBudgetDetail = new AdvanceAmountIssuedAgainstBudgetDetails(dr);
                            totis += (Convert.ToInt32(advanceAmountIssuedAgainstBudgetDetail.Amount) + Convert.ToInt32(advanceAmountIssuedAgainstBudgetDetail.ExcessUsed));

                            advanceAmountIssuedAgainstBudgetDetails.Add(advanceAmountIssuedAgainstBudgetDetail);
                        }

                        advanceApprovalPrintDetailsResponse!.amountIssuedAgainstBudgetSummary.AdvanceAmountIssuedAgainstBudgetDetails = advanceAmountIssuedAgainstBudgetDetails;
                        advanceApprovalPrintDetailsResponse!.amountIssuedAgainstBudgetSummary.SummaryText = $"Total Budget : {advanceApprovalPrintDetailsResponse?.BudgetAmount}, Issued Amount : {totis}";

                        int diff = Convert.ToInt32(advanceApprovalPrintDetailsResponse?.BudgetAmount) - totis;
                        if (diff >= 0)
                        {
                            advanceApprovalPrintDetailsResponse!.amountIssuedAgainstBudgetSummary.SummaryText += $", <b style='color:green; font-weight:bold;'>Remain Amt. : {diff}</b>";
                        }
                        else
                        {
                            advanceApprovalPrintDetailsResponse!.amountIssuedAgainstBudgetSummary.SummaryText += $", <b style='color:red; font-weight:bold;'>Over Amt. : {diff}</b>";
                        }
                    }

                    using (DataTable dtBudgetUsedSessionWise = await _advanceRepository.GetAdvanceBudgetUsageSessionWiseByReferenceNo(advanceApprovalPrintDetailsResponse?.BudgetReferenceNo!))
                    {
                        List<AdvanceBudgetUsageSessionSummaryDetails> advanceBudgetUsageSessionSummaryDetails = new List<AdvanceBudgetUsageSessionSummaryDetails>();

                        foreach (DataRow dr in dtBudgetUsedSessionWise.Rows)
                        {
                            AdvanceBudgetUsageSessionSummaryDetails advanceBudgetUsageSessionSummaryDetail = new AdvanceBudgetUsageSessionSummaryDetails(dr);

                            using (DataTable dtBudgetUsedMonthWise = await _advanceRepository.GetAdvanceBudgetUsageMonthWiseByReferenceNo(advanceBudgetUsageSessionSummaryDetail.ReferenceNo!))
                            {
                                DataTable Set = new DataTable();
                                Set.Columns.Add("Head");

                                Set.Columns.Add("Jan");
                                Set.Columns.Add("JanRemark");
                                Set.Columns.Add("Feb");
                                Set.Columns.Add("FebRemark");
                                Set.Columns.Add("Mar");
                                Set.Columns.Add("MarRemark");
                                Set.Columns.Add("Apr");
                                Set.Columns.Add("AprRemark");
                                Set.Columns.Add("May");
                                Set.Columns.Add("MayRemark");
                                Set.Columns.Add("Jun");
                                Set.Columns.Add("JunRemark");
                                Set.Columns.Add("Jul");
                                Set.Columns.Add("JulRemark");
                                Set.Columns.Add("Aug");
                                Set.Columns.Add("AugRemark");
                                Set.Columns.Add("Sep");
                                Set.Columns.Add("SepRemark");
                                Set.Columns.Add("Oct");
                                Set.Columns.Add("OctRemark");
                                Set.Columns.Add("Nov");
                                Set.Columns.Add("NovRemark");
                                Set.Columns.Add("Dec");
                                Set.Columns.Add("DecRemark");

                                Set.Columns.Add("Tot");

                                if (dtBudgetUsedMonthWise.Rows.Count > 0)
                                {
                                    List<string> H = dtBudgetUsedMonthWise.AsEnumerable().Select(p => p.Field<string>("Head")).Distinct().ToList()!;
                                    foreach (string Hs in H)
                                    {
                                        double totisn = 0;
                                        DataRow DR = Set.NewRow();
                                        DR[0] = Hs;
                                        for (int c = 1; c < Set.Columns.Count - 1; c += 2)
                                        {
                                            double Amt = 0;
                                            string Remark = string.Empty;
                                            if (dtBudgetUsedMonthWise.AsEnumerable().Where(p => p.Field<string>("Head") == Hs && p.Field<string>("Mon") == Set.Columns[c].ColumnName).Any())
                                            {
                                                Amt = dtBudgetUsedMonthWise.AsEnumerable().Where(p => p.Field<string>("Head") == Hs && p.Field<string>("Mon") == Set.Columns[c].ColumnName).Sum(p => p.Field<double>("Amount"));

                                                Remark = dtBudgetUsedMonthWise.AsEnumerable().Where(p => p.Field<string>("Head") == Hs && p.Field<string>("Mon") == Set.Columns[c].ColumnName).Max(p => p.Field<string>("Remark"))!;

                                                totisn += Amt;
                                                if (Remark.Length > 0)
                                                {
                                                    Remark = Remark.Replace("$", "<br/>").Trim();
                                                }
                                            }

                                            DR[c] = Amt.ToString();
                                            DR[c + 1] = Remark;
                                        }
                                        DR[Set.Columns.Count - 1] = totisn.ToString();
                                        Set.Rows.Add(DR);
                                    }
                                }

                                List<AdvanceBudgetUsageMonthSummaryDetails> advanceBudgetUsageMonthSummaryDetails = new List<AdvanceBudgetUsageMonthSummaryDetails>();

                                foreach (DataRow drt in Set.Rows)
                                {
                                    AdvanceBudgetUsageMonthSummaryDetails advanceBudgetUsageMonthSummaryDetail = new AdvanceBudgetUsageMonthSummaryDetails(drt);

                                    if (MissionAdmissionTypes.Contains(advanceBudgetUsageMonthSummaryDetail.Head!))
                                    {
                                        advanceBudgetUsageMonthSummaryDetail.IsHighlighted = true;
                                    }

                                    advanceBudgetUsageMonthSummaryDetails.Add(advanceBudgetUsageMonthSummaryDetail);
                                }

                                advanceBudgetUsageSessionSummaryDetail.UsageMonthSummaryDetails = advanceBudgetUsageMonthSummaryDetails;

                                advanceBudgetUsageSessionSummaryDetail.JanTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.Jan ?? 0);
                                advanceBudgetUsageSessionSummaryDetail.FebTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.Feb ?? 0);
                                advanceBudgetUsageSessionSummaryDetail.MarTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.Mar ?? 0);
                                advanceBudgetUsageSessionSummaryDetail.AprTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.Apr ?? 0);
                                advanceBudgetUsageSessionSummaryDetail.MayTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.May ?? 0);
                                advanceBudgetUsageSessionSummaryDetail.JunTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.Jun ?? 0);
                                advanceBudgetUsageSessionSummaryDetail.JulTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.Jul ?? 0);
                                advanceBudgetUsageSessionSummaryDetail.AugTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.Aug ?? 0);
                                advanceBudgetUsageSessionSummaryDetail.SepTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.Sep ?? 0);
                                advanceBudgetUsageSessionSummaryDetail.OctTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.Oct ?? 0);
                                advanceBudgetUsageSessionSummaryDetail.NovTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.Nov ?? 0);
                                advanceBudgetUsageSessionSummaryDetail.DecTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.Dec ?? 0);
                                advanceBudgetUsageSessionSummaryDetail.TotTotal = advanceBudgetUsageMonthSummaryDetails.Sum(s => s.Tot ?? 0);

                            }

                            advanceBudgetUsageSessionSummaryDetails.Add(advanceBudgetUsageSessionSummaryDetail);
                        }

                        advanceApprovalPrintDetailsResponse!.advanceBudgetUsageSessionSummaryDetails = advanceBudgetUsageSessionSummaryDetails;


                    }

                }
                else
                {
                    advanceApprovalPrintDetailsResponse!.AdvanceBudgetTextRemarkSummary = string.Empty;
                }


                if (!string.IsNullOrWhiteSpace(advanceApprovalPrintDetailsResponse!.NewExpDate))
                {
                    advanceApprovalPrintDetailsResponse!.NewExpTextRemarkSummary = $"I further request you that due to some reason, we are unable to submit the related vouchers till the above mentioned date. So I request to for the extension till {advanceApprovalPrintDetailsResponse!.NewExpDate}";
                }
                else
                {
                    advanceApprovalPrintDetailsResponse!.NewExpTextRemarkSummary = string.Empty;
                }

                if (advanceApprovalPrintDetailsResponse!.Status == "Cancelled")
                {
                    advanceApprovalPrintDetailsResponse!.IsCancelled = true;
                }

                if (!string.IsNullOrWhiteSpace(advanceApprovalPrintDetailsResponse!.App1On))
                {
                    if (advanceApprovalPrintDetailsResponse!.ByPass?.Contains("Member1,") == true)
                    {
                        advanceApprovalPrintDetailsResponse!.App1On = "By Passed";
                    }
                    if (advanceApprovalPrintDetailsResponse!.Status == "Cancelled" && advanceApprovalPrintDetailsResponse!.CancelBy == "1")
                    {
                        advanceApprovalPrintDetailsResponse!.App1On = advanceApprovalPrintDetailsResponse!.CancelOn;
                        advanceApprovalPrintDetailsResponse!.CancelByApp1 = true;
                    }
                }


                if (!string.IsNullOrWhiteSpace(advanceApprovalPrintDetailsResponse!.App2On))
                {
                    if (advanceApprovalPrintDetailsResponse!.ByPass?.Contains("Member2,") == true)
                    {
                        advanceApprovalPrintDetailsResponse!.App2On = "By Passed";
                    }
                    if (advanceApprovalPrintDetailsResponse!.Status == "Cancelled" && advanceApprovalPrintDetailsResponse!.CancelBy == "2")
                    {
                        advanceApprovalPrintDetailsResponse!.App2On = advanceApprovalPrintDetailsResponse!.CancelOn;
                        advanceApprovalPrintDetailsResponse!.CancelByApp2 = true;
                    }
                }

                advanceApprovalPrintDetailsResponse!.App3Name = _general.ToTitleCase(advanceApprovalPrintDetailsResponse!.App3Name ?? string.Empty);
                advanceApprovalPrintDetailsResponse!.App3Designation = $"({_general.ToTitleCase(advanceApprovalPrintDetailsResponse!.App3Designation ?? string.Empty)})";

                if (!string.IsNullOrWhiteSpace(advanceApprovalPrintDetailsResponse!.App3Status) && !string.IsNullOrWhiteSpace(advanceApprovalPrintDetailsResponse!.App3On))
                {
                    if (advanceApprovalPrintDetailsResponse!.ByPass?.Contains("Member3,") == true)
                    {
                        advanceApprovalPrintDetailsResponse!.App3On = "By Passed";
                    }
                    if (advanceApprovalPrintDetailsResponse!.Status == "Cancelled" && advanceApprovalPrintDetailsResponse!.CancelBy == "3")
                    {
                        advanceApprovalPrintDetailsResponse!.App3On = advanceApprovalPrintDetailsResponse!.CancelOn;
                        advanceApprovalPrintDetailsResponse!.CancelByApp3 = true;
                    }
                }


                if (!string.IsNullOrWhiteSpace(advanceApprovalPrintDetailsResponse!.App4On))
                {
                    if (advanceApprovalPrintDetailsResponse!.ByPass?.Contains("Member4,") == true)
                    {
                        advanceApprovalPrintDetailsResponse!.App4On = "By Passed";
                    }
                    if (advanceApprovalPrintDetailsResponse!.Status == "Cancelled" && advanceApprovalPrintDetailsResponse!.CancelBy == "4")
                    {
                        advanceApprovalPrintDetailsResponse!.App4On = advanceApprovalPrintDetailsResponse!.CancelOn;
                        advanceApprovalPrintDetailsResponse!.CancelByApp4 = true;
                    }

                }


                advanceApprovalPrintDetailsResponse.Purpose = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.Purpose ?? string.Empty);
                advanceApprovalPrintDetailsResponse.App1Name = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.App1Name ?? string.Empty);
                advanceApprovalPrintDetailsResponse.App2Name = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.App2Name ?? string.Empty);
                advanceApprovalPrintDetailsResponse.App3Name = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.App3Name ?? string.Empty);
                advanceApprovalPrintDetailsResponse.CampusName = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.CampusName ?? string.Empty);
                advanceApprovalPrintDetailsResponse.FirmName = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.FirmName ?? string.Empty);

                if (!string.IsNullOrWhiteSpace(advanceApprovalPrintDetailsResponse.PExtra3) && advanceApprovalPrintDetailsResponse.PExtra3 != "----")
                {
                    advanceApprovalPrintDetailsResponse.FirmName += $"({advanceApprovalPrintDetailsResponse.PExtra3})";
                }
                advanceApprovalPrintDetailsResponse.FirmAddress = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.FirmAddress ?? string.Empty);
                advanceApprovalPrintDetailsResponse.App1Name = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.App1Name ?? string.Empty);
                advanceApprovalPrintDetailsResponse.App2Name = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.App2Name ?? string.Empty);
                advanceApprovalPrintDetailsResponse.App4Designation = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.App4Designation ?? string.Empty);
                advanceApprovalPrintDetailsResponse.App1Designation = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.App1Designation ?? string.Empty);
                advanceApprovalPrintDetailsResponse.App2Designation = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.App2Designation ?? string.Empty);
                advanceApprovalPrintDetailsResponse.Note = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.Note ?? string.Empty);
                advanceApprovalPrintDetailsResponse.Purpose = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.Purpose ?? string.Empty);
                advanceApprovalPrintDetailsResponse.ForDepartment = _general.ToTitleCase(advanceApprovalPrintDetailsResponse.ForDepartment ?? string.Empty);
                advanceApprovalPrintDetailsResponse.TotalAmount = _general.ConvertToTwoDecimalPlaces(advanceApprovalPrintDetailsResponse.TotalAmount ?? string.Empty);
                advanceApprovalPrintDetailsResponse.AmountInWords = _general.AmountInWords(advanceApprovalPrintDetailsResponse.TotalAmount ?? string.Empty);


                if (Convert.ToDouble(advanceApprovalPrintDetailsResponse.TotalAmount) > 100000)
                {
                    advanceApprovalPrintDetailsResponse.ExtraAppName = "Chancellor";
                }

                if (advanceApprovalPrintDetailsResponse.MyType == "Against Purchase Approval Advance Form")
                {
                    advanceApprovalPrintDetailsResponse!.MyType = "Advance Against Purchase Approval Form";
                }

                return new ApiResponse(StatusCodes.Status200OK, "Success", advanceApprovalPrintDetailsResponse);
            }

        }


        public async Task<ApiResponse> GetBasicDetailsForGenerateBillAgainstAdvance(string RefNo)
        {
            //Task<string> CheckWarrentyUploadeOrNot(string RefNo)
            //string chk = await _advanceRepository.CheckWarrentyUploadeOrNot(RefNo);
            //if (chk != "")
            //{
            //    return new ApiResponse(StatusCodes.Status200OK, "Success", chk);
            //}


            PurchaseDetailsForGenerateBillREsponse lst = new PurchaseDetailsForGenerateBillREsponse();
            using (DataTable dt = await _advanceRepository.GetPurchaseBasicForBillAgainstAdvance(RefNo))
            {
                lst.FirmName = dt.Rows[0]["FirmName"].ToString() ?? string.Empty;
                lst.Department = dt.Rows[0]["ForDepartment"].ToString() ?? string.Empty;
                lst.FirmContactName = dt.Rows[0]["FirmPerson"].ToString() ?? string.Empty;
                lst.FirmEmail = dt.Rows[0]["FirmEmail"].ToString() ?? string.Empty;
                lst.FirmPANNo = dt.Rows[0]["FirmPANNo"].ToString() ?? string.Empty;
                lst.FirmAddress = dt.Rows[0]["FirmAddress"].ToString() ?? string.Empty;
                lst.FirmContactNo = dt.Rows[0]["FirmContactNo"].ToString() ?? string.Empty;
                lst.FirmAlternate = dt.Rows[0]["FirmAlternateContactNo"].ToString() ?? string.Empty;
                lst.CampusCode = dt.Rows[0]["CampusCode"].ToString() ?? string.Empty;
                lst.CampusName = dt.Rows[0]["CampusName"].ToString() ?? string.Empty;
                lst.RelativePerson = dt.Rows[0]["RelativePersonName"].ToString() ?? string.Empty;
                lst.AdditionalName = dt.Rows[0]["AdditionalName"].ToString() ?? string.Empty;
                lst.Purpose = dt.Rows[0]["Purpose"].ToString() ?? string.Empty;
                lst.InitiateOn = dt.Rows[0]["AppDateDisp"].ToString() ?? string.Empty;
                lst.ExpBillDate = dt.Rows[0]["ExpDate"].ToString() ?? string.Empty;
                lst.Actualmount = dt.Rows[0]["TotalAmount"].ToString();
                lst.AmountSantioned = dt.Rows[0]["TotalAmount"].ToString() ?? string.Empty;
                lst.AmountDiscount = dt.Rows[0]["Dis"].ToString() ?? string.Empty;
                lst.ExtraValues = dt.Rows[0]["PExtra3"].ToString() ?? ""; ;

                //Load Vendor

                List<TextValues> VendorOffice = new List<TextValues>();

                // if (dt.Rows[0]["PExtra3"].ToString() != "")
                {
                    //Task<DataTable> GetOffices(string VendorId)
                    DataTable OfficeData = await _advanceRepository.GetOffices(dt.Rows[0]["VendorID"].ToString() ?? "");
                    if (OfficeData.Rows.Count > 0)
                    {
                        foreach (DataRow row in OfficeData.Rows)
                        {
                            VendorOffice.Add(new TextValues(row));
                        }
                    }
                    else
                    {
                        DataRow DR = OfficeData.NewRow();
                        DR = OfficeData.NewRow();
                        DR[0] = "----";
                        OfficeData.Rows.Add(DR);
                        VendorOffice.Add(new TextValues(DR));
                    }
                    lst.Offices = VendorOffice;


                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }
        }

        public async Task<ApiResponse> GetAdvancebillAuth(string CampusCode)
        {
            List<TextValues> lst1 = new List<TextValues>();
            List<TextValues> lst2 = new List<TextValues>();
            List<TextValues> lst3 = new List<TextValues>();
            List<TextValues> lst4 = new List<TextValues>();

            if (CampusCode != "101")
            {
                using (DataTable dtAuuth1 = await _advanceRepository.GetAuthAdvanceBillAuth1(CampusCode))
                {

                    foreach (DataRow row in dtAuuth1.Rows)
                    {
                        lst1.Add(new TextValues
                        {
                            Text = row["Text"].ToString(),
                            Value = row["Value2"].ToString(),
                            EmpCode = row["Value"].ToString()
                        });
                    }
                }
                using (DataTable dtAuuth2 = await _advanceRepository.GetAuthAdvanceBillAuth2(CampusCode))
                {

                    foreach (DataRow row in dtAuuth2.Rows)
                    {
                        lst2.Add(new TextValues
                        {
                            Text = row["Text"].ToString(),
                            Value = row["Value2"].ToString(),
                            EmpCode = row["Value"].ToString()
                        });
                    }

                }

                using (DataTable dtAuuth4 = await _advanceRepository.GetAuthAdvanceBillAuth4(CampusCode))
                {

                    foreach (DataRow dr in dtAuuth4.Rows)
                    {
                        lst4.Add(new TextValues
                        {
                            Text = dr["Text"].ToString(),
                            Value = dr["Value2"].ToString(),
                            EmpCode = dr["Value"].ToString()
                        });
                    }
                }
            }
            else
            {
                using (DataTable dtAuuth4 = await _advanceRepository.GetAuthAdvanceBillAuth4(CampusCode))
                {

                    foreach (DataRow dr in dtAuuth4.Rows)
                    {
                        lst4.Add(new TextValues
                        {
                            Text = dr["Text"].ToString(),
                            Value = dr["Value2"].ToString(),
                            EmpCode = dr["Value"].ToString()
                        });
                    }
                    lst1 = lst4;
                    lst2 = lst4;
                }
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", new { app3List = lst1, app4List = lst2, app1List = lst3, app2List = lst4 });
        }
        public async Task<ApiResponse> GetPurchaseApprovalBill(string EmpCode, string Type, GetApprovalBillRequest req)
        {
            //Task<DataTable> GetApprovalBill(string EmpCode,string Type, GetApprovalBillRequest req)
            List<BillApprovalResponse> lst = new List<BillApprovalResponse>();
            using (DataTable dt = await _advanceRepository.GetApprovalBill(EmpCode, Type, req))
            {
                DataTable mytab = new DataTable();
                mytab = dt.Clone();
                mytab.Rows.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BillApprovalResponse ls = new BillApprovalResponse();
                    if (Convert.ToInt32(dt.Rows[i]["AmountRemaining"].ToString()) <= 0)
                    {
                        DataTable dts = await _advanceRepository.GetIssuedAmount(dt.Rows[i]["TransactionID"].ToString() ?? "");

                        if (dts.Rows.Count > 0)
                        {
                            //dtn.Tables[0].Rows[i][4] = dtn_.Tables[0].Rows[0][0].ToString();
                            if (dts.Rows[0][0].ToString().Trim().Length > 0)
                            {
                                if (Convert.ToInt32(dts.Rows[0][0].ToString()) < Convert.ToInt32(dt.Rows[i]["Santioned"].ToString()))
                                {
                                    //dtn.Tables[0].Rows[i][4] = Convert.ToInt32(dtn_.Tables[0].Rows[0][0].ToString()) + "#" + Convert.ToInt32(dtn.Tables[0].Rows[i]["Santioned"].ToString());
                                    mytab.ImportRow(dt.Rows[i]);
                                }
                                else
                                {
                                    /*dtn.Tables[0].Rows[i]["Purpose"] = "Completed";
                                    mytab.ImportRow(dtn.Tables[0].Rows[i]);*/
                                    continue;
                                }
                            }
                            else
                            {
                                mytab.ImportRow(dt.Rows[i]);
                            }
                        }
                        else
                        {
                            mytab.ImportRow(dt.Rows[i]);
                        }
                    }
                    else
                    {
                        //dtn.Tables[0].Rows[i][5] = dtn.Tables[0].Rows[i]["AmountRemaining"].ToString();
                        mytab.ImportRow(dt.Rows[i]);
                    }

                    ls.CampusName = mytab.Rows[i]["CampusName"].ToString();
                    ls.TransactionId = mytab.Rows[i]["TransactionID"].ToString();
                    ls.SenAmount = mytab.Rows[i]["Santioned"].ToString();
                    ls.Paid = mytab.Rows[i]["Paid"].ToString();
                    ls.InitOn = mytab.Rows[i]["INI"].ToString();
                    ls.IssuidName = dt.Rows[i]["IssuedName"].ToString();//IssuedBy,DATE_FORMAT(IssuedOn,'%d %b, %y') 'IssuedOn'
                    ls.IssuedBy = dt.Rows[i]["IssuedBy"].ToString();
                    ls.IssuedOn = dt.Rows[i]["IssuedOn"].ToString();
                    ls.IssImg = await _inclusiveService.GetEmployeePhotoUrl(dt.Rows[i]["IssuedBy"].ToString());
                    ls.Status = mytab.Rows[i]["Status"].ToString();
                    ls.IsSpacial = mytab.Rows[i]["IsSpecial"].ToString();
                    ls.NewGate = mytab.Rows[i]["NewGate"].ToString();
                    ls.InitBy = mytab.Rows[i]["Initiated By"].ToString();
                    ls.ExtraBill = mytab.Rows[i]["BillExtra3"].ToString();
                    ls.BillExtra6 = mytab.Rows[i]["BillExtra6"].ToString();
                    //ls.PaidOn = mytab.Rows[i]["On"].ToString();
                    ls.NewTest = mytab.Rows[i]["NewTest"].ToString();
                    ls.Purpose = mytab.Rows[i]["Purpose"].ToString();
                    ls.NewExp = mytab.Rows[i]["NewExp"].ToString();
                    ls.FirmName = mytab.Rows[i]["Firm Name"].ToString();
                    ls.NewBill = mytab.Rows[i]["NewBill"].ToString();
                    ls.CashDiscount = mytab.Rows[i]["CashDiscount"].ToString();
                    ls.Col5 = mytab.Rows[i]["col5"].ToString();
                    ls.NewIni = mytab.Rows[i]["NewIni"].ToString();
                    ls.AmountRemaining = mytab.Rows[i]["AmountRemaining"].ToString();
                    if (mytab.Rows[i]["BillExtra6"].ToString()!.Length > 0)
                    {
                        ls.PurposeLink = "http://hostel.glauniversity.in:84/warrenty.aspx?approvalid=" + mytab.Rows[i]["BillExtra6"].ToString();
                    }
                    else
                    {
                        long PurRef = 0;
                        if (Int64.TryParse(mytab.Rows[i]["BillExtra3"].ToString().Replace("MED", ""), out PurRef))
                        {
                            if (mytab.Rows[i]["BillExtra3"].ToString().Length > 0)
                            {
                                ls.PurposeLink = "Reports/PO.aspx?Id=" + mytab.Rows[i]["BillExtra3"].ToString() + "";
                            }
                        }
                    }

                    //check cond again
                    int apporrej = 0;

                    if (mytab.Rows[i]["Status"].ToString() == "Waiting For Bills Approval" || mytab.Rows[i]["Status"].ToString() == "Bill Rejected")
                    {



                        if ((Type != "STORE" && Type != "GUEST" && mytab.Rows[i]["NewBill"].ToString().Length > 0) || (apporrej <= 0 && mytab.Rows[i]["Status"].ToString() == "Waiting For Bills Approval" && mytab.Rows[i]["NewBill"].ToString().Length > 0))
                        {
                            ls.CanEditDate = true;
                        }
                        DataTable AuthDetails = await _advanceRepository.GetApprovalAuthority(mytab.Rows[i]["Trans. ID"].ToString());
                        List<Authorities> authorities = new List<Authorities>();
                        foreach (DataRow dr in AuthDetails.Rows)
                        {
                            authorities
                                .Add(new Authorities
                                {
                                    Name = dr["EmployeeDetails"].ToString(),
                                    On = dr["DB"].ToString(),
                                    Status = dr["Status"].ToString(),
                                    EmployeeId = dr["EmployeeID"].ToString(),
                                    Img = await _inclusiveService.GetEmployeePhotoUrl(dr["EmployeeID"].ToString() ?? "")
                                });
                        }
                        ls.AuthList = authorities;



                    }
                    else
                    {
                        if ((Type != "STORE" && Type != "GUEST" && mytab.Rows[i]["NewBill"].ToString().Length > 0))
                            ls.CanEditDate = true;
                    }
                    if (mytab.Rows[i]["Status"].ToString() == "Ready To Issue Amount")
                    {
                        if (mytab.Rows[i]["AmountRemaining"].ToString() == "0")
                            ls.Status = "Bill Fully Paid";
                        using (DataTable auth = await _advanceRepository.GetIssuedAmounREadyApproval(dt.Rows[i]["TransactionID"].ToString() ?? ""))
                        {
                            List<Authorities> authorities = new List<Authorities>();
                            foreach (DataRow dr in auth.Rows)
                            {
                                authorities
                                    .Add(new Authorities
                                    {
                                        Name = dr["EmployeeDetails"].ToString(),
                                        On = dr["DB"].ToString(),
                                        Status = dr["Status"].ToString(),
                                        EmployeeId = dr["EmployeeID"].ToString(),
                                        Img = await _inclusiveService.GetEmployeePhotoUrl(dr["EmployeeID"].ToString() ?? "")
                                    });
                            }
                            ls.AuthList = authorities;
                        }
                    }
                    if (mytab.Rows[i]["Status"].ToString() == "Waiting For Cheque Approval")
                    {
                        ls.Status = "Bill Approved";
                        using (DataTable auth = await _advanceRepository.GetIssuedAmounREadyApproval(dt.Rows[i]["TransactionID"].ToString() ?? ""))
                        {
                            List<Authorities> authorities = new List<Authorities>();
                            foreach (DataRow dr in auth.Rows)
                            {
                                authorities
                                    .Add(new Authorities
                                    {
                                        Name = dr["EmployeeDetails"].ToString(),
                                        On = dr["DB"].ToString(),
                                        Status = dr["Status"].ToString(),
                                        EmployeeId = dr["EmployeeID"].ToString(),
                                        Img = await _inclusiveService.GetEmployeePhotoUrl(dr["EmployeeID"].ToString() ?? "")
                                    });

                            }
                            ls.AuthList = authorities;
                        }


                    }
                    //color
                    if (mytab.Rows[i]["Status"].ToString() != "Bill Rejected")
                    {
                        using (DataTable colordt = await _advanceRepository.RowColor(mytab.Rows[i]["Trans. ID"].ToString()))
                        {
                            if (colordt.Rows.Count > 0)
                            {
                                if (mytab.Rows[i]["MyINICheck"].ToString().Length <= 0)
                                {
                                    ls.BackColor = "#FF5555";
                                }
                                else
                                {
                                    var iniRaw = mytab.Rows[i]["MyINICheck"]?.ToString();
                                    var limit = Convert.ToInt32(colordt.Rows[0]["Limit"]);

                                    var ini = DateTime.ParseExact(
                                        iniRaw,
                                        "MM/dd/yyyy",                    // ya "dd/MM/yyyy" jo actual ho
                                        CultureInfo.InvariantCulture);

                                    if (ini.AddDays(limit).Date == DateTime.Now.Date)
                                    {
                                        ls.BackColor = "#FFFFCC";
                                    }
                                    if (ini.AddDays(limit) < DateTime.Now.Date)
                                    {
                                        ls.BackColor = "#FF5555";
                                    }
                                }
                            }
                        }
                    }
                    if (mytab.Rows[i]["Status"].ToString() == "Ready To Issue Amount" || mytab.Rows[i]["Status"].ToString() == "Waiting For Cheque Approval")
                    {
                        using (DataTable rowcolo = await _advanceRepository.RowColorReadyAmount(mytab.Rows[i]["Trans. ID"].ToString()))
                        {


                            DateTime myDate;
                            if (DateTime.TryParseExact(
                                    mytab.Rows[i]["MyINICheck"].ToString(),
                                    "MM/dd/yyyy",
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    System.Globalization.DateTimeStyles.None,
                                    out myDate))
                            {
                                DateTime limitDate = myDate.AddDays(Convert.ToInt32(rowcolo.Rows[0]["Limit"].ToString()));

                                if (limitDate == DateTime.Now.Date)
                                    ls.BackColor = "#FFFFCC";
                                else if (limitDate < DateTime.Now.Date)
                                    ls.BackColor = "#FF5555";
                            }
                            else
                            {
                                // Handle invalid date string gracefully
                                ls.BackColor = "#CCCCCC"; // Gray fallback
                            }

                        }
                    }
                    if (mytab.Rows[i]["IsSpecial"].ToString() == "True")
                    {
                        ls.BackColor = "#8CDAFF";
                    }

                    if (mytab.Rows[i]["Status"].ToString() == "Waiting For Bills Approval" || mytab.Rows[i]["Status"].ToString() == "Bill Rejected" || mytab.Rows[i]["Status"].ToString() == "Bill Cancelled")
                    {
                        if (mytab.Rows[0]["Status"].ToString() == "Waiting For Bills Approval" && EmpCode == mytab.Rows[0]["IssuedBy"].ToString())
                        {
                            using (DataTable stst = await _advanceRepository.GetAuthStatusBill(ls.TransactionId))
                            {
                                if (stst.Rows.Count <= 0)
                                {
                                    ls.CanEdit = true;
                                    ls.CanDelete = true;
                                }
                                else
                                {
                                    ls.CanEdit = false;
                                    ls.CanDelete = false;
                                }
                            }
                        }
                        else
                        {
                            ls.CanEdit = false;
                            ls.CanDelete = false;
                        }
                    }
                    else
                    {
                        ls.CanEdit = false;
                        ls.CanDelete = false;
                        if(Convert.ToInt32(ls.AmountRemaining) > 0 && Type == "ACCOUNTS")
                        {
                            ls.CanUploadCheque=true;
                        }
                        else
                        {
                            ls.CanUploadCheque=false;
                        }
                    }

                    lst.Add(ls);
                }





            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
        }





        public async Task<ApiResponse> UpdatePurchaseBillDate(string empCode, UpdatePurchaseBillDateRequest req)
        {

            string store = "";
            string Update = "";
            List<SQLParameters> param = new List<SQLParameters>();
            if (Convert.ToDateTime(req.NewAppDate) != Convert.ToDateTime(req.OldAppDate))
            {
                store += "Ini. Dt. : " + req.OldAppDate.Replace("-", ".") + "->" + req.NewAppDate.Replace("-", ".") + ", ";
                Update += " Col2=@NewAppDate,";
                param.Add(new SQLParameters("@NewAppDate", req.NewAppDate));
            }
            if (Convert.ToDateTime(req.NewBillDate) != Convert.ToDateTime(req.OldBillDate))
            {
                store += " Bill Dt. : " + req.OldBillDate.Replace("-", ".") + "->" + req.NewBillDate.Replace("-", ".") + ", ";
                Update += " BillDate=@NewBillDate,";
                param.Add(new SQLParameters("@NewBillDate", req.NewBillDate));
            }
            if (Convert.ToDateTime(req.NewGateDate) != Convert.ToDateTime(req.OldGateDate))
            {
                store += " Gate Dt. : " + req.OldGateDate.Replace("-", ".") + "->" + req.NewGateDate.Replace("-", ".") + ", ";
                Update += " GateEntryOn=@NewGateDate,";
                param.Add(new SQLParameters("@NewGateDate", req.NewGateDate));
            }
            if (Convert.ToDateTime(req.NewExpDate) != Convert.ToDateTime(req.OldExpDate))
            {
                store += " Exp. Dt. : " + req.OldExpDate.Replace("-", ".") + "->" + req.NewExpDate.Replace("-", ".") + ", ";
                Update += " BillExtra1=@NewExpDate,";
                param.Add(new SQLParameters("@NewExpDate", req.NewExpDate));
            }
            if (Convert.ToDateTime(req.NewTestDate) != Convert.ToDateTime(req.OldTestDate))
            {
                store += " Test Dt. : " + req.OldTestDate.Replace("-", ".") + "->" + req.NewTestDate.Replace("-", ".") + ", ";
                Update += " BillExtra4=@NewTestDate,";
                param.Add(new SQLParameters("@NewTestDate", req.NewTestDate));
            }
            if (store.Length > 0)
            {
                store = "  Done By : " + empCode + " On " + DateTime.Now.ToLongDateString() + "$";
                Update += " LastUpdatedOn=now(),LastUpdatedBy='" + empCode + "',BillExtra2=CONCAT(IFNULL(BillExtra2,''),'" + store + "'),";
            }
            if (Update.Length > 0)
            {
                int ins = await _advanceRepository.UpdateBillBase(Update, req.TransId, req.NewWarrentyID, param);
            }

            //update Purpose
            if (req.NewPurpose != req.OldPurpose)
            {
                int ins = await _advanceRepository.UpdateRemark(req.NewPurpose ?? "", req.TransId ?? "");
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", "Record Update Successfully");

        }


        public async Task<ApiResponse> UpdateBillDetails(string RefNo)
        {
            using (DataTable dt = await _advanceRepository.GetEditBillDetails(RefNo))
            {
                if (dt.Rows.Count > 0)
                {
                    BillDetailsForEditResponse res = new BillDetailsForEditResponse();
                    res.AdditionalName = dt.Rows[0]["Col5"].ToString() ?? "";
                    res.ForOffice = dt.Rows[0]["Col3"].ToString() ?? "";
                    res.RefNo = dt.Rows[0]["BillExtra3"].ToString() ?? "";
                    res.Department = dt.Rows[0]["Col1"].ToString() ?? "";
                    res.FirmName = dt.Rows[0]["FirmName"].ToString() ?? "";
                    res.FirmContactName = dt.Rows[0]["FirmPerson"].ToString() ?? "";
                    res.FirmEmail = dt.Rows[0]["FirmEmail"].ToString() ?? "";
                    res.FirmContactNo = dt.Rows[0]["FirmContactNo"].ToString() ?? "";
                    res.FirmAlternate = dt.Rows[0]["FirmAlternateContactNo"].ToString() ?? "";
                    res.FirmAddress = dt.Rows[0]["FirmAddress"].ToString() ?? "";
                    res.FirmPANNo = dt.Rows[0]["FirmPanNo"].ToString() ?? "";
                    res.Purpose = dt.Rows[0]["Remark"].ToString() ?? "";
                    res.ForEmployee = dt.Rows[0]["RelativePersonName"].ToString() + " - " + dt.Rows[0]["RelativeDesignation"].ToString() + " [" + dt.Rows[0]["RelativeDepartment"].ToString() + "]";
                    res.Reamining = dt.Rows[0]["AmountRemaining"].ToString() ?? "";
                    res.Actualmount = dt.Rows[0]["AmountRequired"].ToString() ?? "";
                    res.AlreadyIssued = dt.Rows[0]["AmountPaid"].ToString() ?? "";
                    res.BillNo = dt.Rows[0]["BillNo"].ToString() ?? "";
                    res.GateDate = dt.Rows[0]["G"].ToString() ?? "";
                    res.StoreDate = dt.Rows[0]["S"].ToString() ?? "";
                    res.InitiateOn = dt.Rows[0]["Ini"].ToString() ?? "";
                    res.DepartmentDate = dt.Rows[0]["D"].ToString() ?? "";
                    res.CampusCode = dt.Rows[0]["CampusCode"].ToString() ?? "";
                    res.CampusName = dt.Rows[0]["CampusName"].ToString() ?? "";
                    if (dt.Rows[0]["ExpDt"].ToString() == "---")
                    {
                        res.ExpBillDate = "";
                    }
                    else
                    {
                        res.ExpBillDate = dt.Rows[0]["ExpDt"].ToString();
                    }
                    res.TestingUpto = dt.Rows[0]["TestDt"].ToString() ?? "";
                    res.BillOnGate = dt.Rows[0]["GT"].ToString() ?? "";
                    res.BillDate = dt.Rows[0]["BL"].ToString() ?? "";
                    res.AmountDiscount = dt.Rows[0]["CashDiscount"].ToString() ?? "";
                    res.Status = dt.Rows[0]["Status"].ToString() ?? "";
                    if (!string.IsNullOrEmpty(dt.Rows[0]["ExtendedBillDate"].ToString()) && dt.Rows[0]["ExtendedBillDate"].ToString().Length > 0)
                    {
                        res.NextBillTill = dt.Rows[0]["ExtendedBillDate"].ToString();
                    }

                    TextValues auth = new TextValues();
                    using (DataTable Authlst = await _advanceRepository.GetRefAuth(RefNo))
                    {
                        DataRow[] dr = Authlst.Select("ApprovalNo = 1");
                        if (dr.Length > 0)
                        {
                            auth = new TextValues
                            {
                                Text = dr[0]["EmployeeDetails"].ToString(),
                                Value = dr[0]["EmployeeID"].ToString() + "#" + dr[0]["EmployeeDetails"].ToString(),

                            };
                            res.App1Auth = auth;
                        }
                        dr = Authlst.Select("ApprovalNo = 2");
                        auth = new TextValues();
                        if (dr.Length > 0)
                        {
                            auth = new TextValues
                            {
                                Text = dr[0]["EmployeeDetails"].ToString(),
                                Value = dr[0]["EmployeeID"].ToString() + "#" + dr[0]["EmployeeDetails"].ToString(),

                            };
                            res.App2Auth = auth;
                        }
                        dr = Authlst.Select("ApprovalNo = 3");
                        auth = new TextValues();
                        if (dr.Length > 0)
                        {
                            auth = new TextValues
                            {
                                Text = dr[0]["EmployeeDetails"].ToString(),
                                Value = dr[0]["EmployeeID"].ToString() + "#" + dr[0]["EmployeeDetails"].ToString(),

                            };
                            res.App3Auth = auth;
                        }
                        dr = Authlst.Select("ApprovalNo = 4");
                        auth = new TextValues();
                        if (dr.Length > 0)
                        {
                            auth = new TextValues
                            {
                                Text = dr[0]["EmployeeDetails"].ToString(),
                                Value = dr[0]["EmployeeID"].ToString() + "#" + dr[0]["EmployeeDetails"].ToString(),

                            };
                            res.App4Auth = auth;
                        }
                    }
                    List<TextValues> Offices = new List<TextValues>();
                    using (DataTable loadOffice = await _advanceRepository.GetOffices(dt.Rows[0]["VendorID"].ToString()))
                    {
                        foreach (DataRow dr in loadOffice.Rows)
                        {
                            Offices.Add(new TextValues
                            {
                                Text = dr["Text"].ToString(),
                                Value = dr["Value"].ToString(),
                            });
                        }
                        if (Offices.Count == 0)
                        {
                            DataRow DR = loadOffice.NewRow();
                            DR[0] = "";
                            loadOffice.Rows.Add(DR);
                            DR = loadOffice.NewRow();
                            DR[0] = "----";
                            loadOffice.Rows.Add(DR);
                        }
                    }
                    res.Offices = Offices;


                    res.BillTransId = RefNo;
                    string parentPath = Directory.GetCurrentDirectory();


                    if (File.Exists(Path.Combine(parentPath, "Upload_Bills", "Approved", res.BillTransId + ".pdf")))
                    {
                        res.PdfFile = Path.Combine("Upload_Bills", "Approved", res.BillTransId + ".pdf");

                    }
                    else
                    {

                        if (File.Exists(Path.Combine(parentPath, "Upload_Bills", res.BillTransId + ".pdf")))
                        {
                            res.PdfFile = Path.Combine("Upload_Bills", res.BillTransId + ".pdf");
                        }
                    }


                    if (File.Exists(Path.Combine(parentPath, "Upload_Bills", "Approved", res.BillTransId + ".xlsx")))
                    {
                        res.ExceFile = Path.Combine("Upload_Bills", "Approved", res.BillTransId + ".xlsx");
                    }
                    else
                    {
                        if (File.Exists(Path.Combine(parentPath, "Upload_Bills", res.BillTransId + ".xlsx")))
                        {
                            res.ExceFile = Path.Combine("Upload_Bills", res.BillTransId + ".xlsx");
                        }
                    }
                    return new ApiResponse(StatusCodes.Status200OK, "Success", res);
                }
                else
                {
                    return new ApiResponse(StatusCodes.Status200OK, "Success", "No Record Found");
                }
            }



        }
        public async Task<ApiResponse> DeleteBill(string EmpCode, string TransId, int Opr = 0)
        {

            //Update PurchaseBillSummary
            if (Opr == 0)
            {
                int del = await _advanceRepository.UpdateDeleteStatus(TransId);
            }
            //insert backup
            int ins = await _advanceRepository.InsertDeleteLog(TransId, EmpCode);

            //deletebill base
            if (ins == 0)
            {
                int BillBase = await _advanceRepository.DeleteBillBase(TransId);
            }
            //deletebill Authority
            int BillAuth = await _advanceRepository.DeleteBillAuth(TransId);

            return new ApiResponse(StatusCodes.Status200OK, "Success", "Bill Delete Successfully");
        }

        public async Task<ApiResponse> UpdateBill(string EmpCode, string EmpName, AddBillGenerateRequest req, string Type)
        {



            if (req.ForTypeOf == "Advance Bill")
            {
                DataTable cann = await _advanceRepository.GetrefnoForGenerateBillAdvance(Type, EmpCode, req.RefNo!);
                if (cann.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Invalid Access");
                }
                using (DataTable dt = await _advanceRepository.CheckPreviousPendingBill(req.RefNo!))
                {
                    //if (dt.Rows.Count > 0)
                    //{
                    //    return new ApiResponse(StatusCodes.Status200OK, "Warning", "Sorry! Previous Advance Approval Is Still Pending To Approve For Same Purchase Approval Initiated By " + dt.Rows[0]["IniName"].ToString() + " on " + dt.Rows[0]["AppDate"].ToString() + " of Amount : " + dt.Rows[0]["Amount"].ToString() + " Rs/-");
                    //}
                }
            }
            else
            {
                DataTable dt = await _advanceRepository.CanGenerateBill(Type, EmpCode, Convert.ToInt32(req.Amount) > 1000 ? false : true, req.RefNo!);
                if (dt.Rows.Count <= 0)
                {
                    //return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Invalid Access");
                }
            }


            string BillRefNo = req.BillId;
            //using (DataTable GetBillBaseRefNo = await _advanceRepository.GetBillBaseRefNo())
            {
                //if (GetBillBaseRefNo.Rows.Count > 0)
                //{
                //    string myid = GetBillBaseRefNo.Rows[0][0].ToString() ?? string.Empty;
                //    string NewId = GetBillBaseRefNo.Rows[0][0].ToString() ?? string.Empty;
                //    BillRefNo = GetBillBaseRefNo.Rows[0][0].ToString() ?? string.Empty;
                //}
                //Task<int> SaveBillAuth(string EmpCode, string EmpName, AddBillGenerateRequest req, string BillBaseREfNo)
                //Task<int> SaveBillSave(string EmpCode,string EmpName, AddBillGenerateRequest req,string BillBaseREfNo)
                int saverecord = await _advanceRepository.UpdateBill(EmpCode, EmpName, req, BillRefNo);
                int SaveAuth = await _advanceRepository.SaveBillAuth(EmpCode, EmpName, req, BillRefNo);



                //FileName = _general.EncryptWithKey(FileName, await _inclusiveService.GetEnCryptedKey());
                //string str = await _inclusiveService.SaveFile(FileName, FilePath, file, ".pdf");
                //check pdf file and delete
                string FilePath = Directory.GetCurrentDirectory();
                FilePath = Path.Combine(FilePath, "Upload_Bills");
                string FileName = BillRefNo + ".pdf";

                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }
                if (req.pdf != null && req.pdf.Length > 0)
                {
                    //delete file
                    if (File.Exists(Path.Combine(FilePath, FileName)))
                    {
                        File.Delete(Path.Combine(FilePath, FileName));
                    }
                    //save file
                    string str = await _inclusiveService.SaveFile(BillRefNo, FilePath, req.pdf, ".pdf");
                    int updatepdf = await _advanceRepository.UpdatePdf(BillRefNo);
                }
                //check Excel file and delete and save
                if (req.ExcelFile != null && req.ExcelFile!.Length > 0)
                {
                    FileName = BillRefNo + ".xlsx";
                    if (File.Exists(Path.Combine(FilePath, FileName)))
                    {
                        File.Delete(Path.Combine(FilePath, FileName));
                    }
                    //save file
                    string str = await _inclusiveService.SaveFile(BillRefNo, FilePath, req.ExcelFile!, ".xlsx");
                    int updateExcel = await _advanceRepository.UpdateExcel(BillRefNo);
                }

                return new ApiResponse(StatusCodes.Status200OK, "Success", "Bill Update Successfully");
            }
        }
        public async Task<ApiResponse> GetBillDetails(string TransId, string EmpName)
        {
            using (DataTable dt = await _advanceRepository.GetBillDetails(TransId))
            {
                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    var culture = CultureInfo.CurrentCulture;
                    var textInfo = culture.TextInfo;

                    // ---------- basic DTO fill ----------
                    var dto = new BillDetailsResponse
                    {
                        TransactionId = row["TransactionID"]?.ToString(),
                        Session = row["Session"]?.ToString(),
                        CampusName = row["CampusName"]?.ToString(),
                        ForType = row["ForType"]?.ToString(),
                        RelativePersonId = row["RelativePersonID"]?.ToString(),
                        VendorId = row["VendorID"]?.ToString(),
                        Department = textInfo.ToTitleCase(row["Col1"]?.ToString()?.ToLower() ?? string.Empty),
                        Purpose = textInfo.ToTitleCase(row["Remark"]?.ToString()?.ToLower() ?? string.Empty),
                        FirmName = textInfo.ToTitleCase(row["MyFirmName"]?.ToString()?.ToLower() ?? string.Empty),
                        FirmAddress = textInfo.ToTitleCase(row["FirmAddress"]?.ToString()?.ToLower() ?? string.Empty),
                        FirmContactNo = row["FirmContactNo"]?.ToString(),
                        BillNo = row["BillNo"]?.ToString(),
                        BillStatus = row["MyStatus"]?.ToString(),
                        Discount = row["CashDiscount"]?.ToString(),
                        UploadOn = row["IO"]?.ToString(),
                        UploadBy = textInfo.ToTitleCase(row["IssuedName"]?.ToString()?.ToLower() ?? string.Empty),
                        RelativePersonName = textInfo.ToTitleCase(row["RelativePersonName"]?.ToString()?.ToLower() ?? string.Empty),
                        RelativeDesignation = textInfo.ToTitleCase(row["RelativeDesignation"]?.ToString()?.ToLower() ?? string.Empty),
                        ReportBy = textInfo.ToTitleCase((EmpName ?? "Guest User").ToLower()),
                        ReportDate = $"{DateTime.Now:dd.MM.yyyy}"
                    };

                    dto.AmountRequired = row["AmountRequired"] == DBNull.Value ? 0 : Convert.ToDecimal(row["AmountRequired"]);
                    dto.AmountPaid = row["AmountPaid"] == DBNull.Value ? 0 : Convert.ToDecimal(row["AmountPaid"]);
                    dto.AmountRemaining = row["AmountRemaining"] == DBNull.Value ? 0 : Convert.ToDecimal(row["AmountRemaining"]);

                    // Extra name (Col5)
                    var col5 = row["Col5"]?.ToString()?.Trim();
                    dto.AdditionalName = string.IsNullOrEmpty(col5) || col5 == "---"
                        ? "---"
                        : textInfo.ToTitleCase(col5.ToLower());

                    // ---------- date display logic ----------
                    dto.ExpBillDate = string.IsNullOrEmpty(row["Exp"]?.ToString()) ? "----" : row["Exp"].ToString();
                    dto.GateDate = string.IsNullOrEmpty(row["G"]?.ToString()) ? "" : row["G"].ToString();
                    dto.IniDate = string.IsNullOrEmpty(row["INI"]?.ToString()) ? "----" : row["INI"].ToString();
                    dto.DeptDate = string.IsNullOrEmpty(row["D"]?.ToString()) ? "----" : row["D"].ToString();
                    dto.BillDate = string.IsNullOrEmpty(row["BL"]?.ToString()) ? "----" : row["BL"].ToString();
                    dto.TestBillDate = row["Tst"]?.ToString() == row["BL"]?.ToString()
                                        ? "----"
                                        : row["Tst"]?.ToString();

                    // ---------- scan copy (pdf / xlsx) ----------
                    var tid = dto.TransactionId;
                    var root = Directory.GetCurrentDirectory();
                    var pdfApp = Path.Combine(root, "Upload_Bills", "Approved", $"{tid}.pdf");
                    var pdfNorm = Path.Combine(root, "Upload_Bills", $"{tid}.pdf");
                    var xlsx = Path.Combine(root, "Upload_Bills", $"{tid}.xlsx");

                    if (System.IO.File.Exists(pdfApp))
                        dto.ScanCopyLinkHtml = $"{pdfApp}";
                    else if (System.IO.File.Exists(pdfNorm))
                        dto.ScanCopyLinkHtml = $"{pdfNorm}";
                    else if (System.IO.File.Exists(xlsx))
                        dto.ScanCopyLinkHtml = $"{xlsx}";

                    // ---------- Vendor 827 (Imprest) ----------
                    if (dto.VendorId == "827")
                    {
                        using DataTable imp = await _advanceRepository.GetImprestByBillTransactionId(tid);
                        if (imp.Rows.Count > 0)
                        {
                            dto.ApprovalLinkHtml =
                                $"ImprestSummary.aspx?Id={_general.Encrypt($"{tid}#No")}";
                            dto.PageName = "Imprest Summary";
                        }
                    }

                    // ---------- BillExtra3 / Col4 based approval links ----------
                    var billExtra3 = row["BillExtra3"]?.ToString() ?? string.Empty;
                    var col4Ref = row["Col4"]?.ToString() ?? string.Empty;

                    if (!string.IsNullOrWhiteSpace(billExtra3))
                    {
                        if (billExtra3.Contains("SEC"))
                        {
                            dto.ApprovalLinkHtml =
                                $"http://glauniversity.in/RefundPrint.aspx?Id={_general.Encrypt(tid)}";
                            dto.PageName = "Security Refund";
                        }
                        else if (billExtra3.Contains("REF"))
                        {
                            dto.ApprovalLinkHtml =
                                $"http://glauniversity.in/studentinformationfee.aspx?Stu_ID={row["Col4"]}&User={_general.Encrypt("Security")}";
                            dto.PageName = "Sstudentinformationfee";
                        }
                        else if (dto.ForType == "Advance Bill")
                        {
                            dto.ApprovalLinkHtml =
                                $"POAdvance.aspx?Id={billExtra3}";
                            dto.PageName = "Advance Application";
                        }
                        else if (billExtra3.Contains("WRK"))
                        {
                            dto.ApprovalLinkHtml =
                                $"<a href='../WorkShop/WorkShopApprovalPrint.aspx?Id={_general.Encrypt(billExtra3.Replace("WRK", ""))}'>Click Here To Workshop/Conference Approval Details</a>";
                            dto.PageName = "Workshop/Conference Approval";
                            // workshopapproval_bill_details grid
                            using DataTable wrk = await _advanceRepository.GetWorkshopBillDetails(tid);
                            //dto.Issues.AddRange(BuildIssuesFromWorkshopTable(wrk));  // helper, optional
                        }
                        else if (billExtra3.Contains("PLC"))
                        {
                            dto.ApprovalLinkHtml =
                                $"https://glauniversity.in:8105/main/reports/VO.aspx?RefNo={_general.Encrypt(billExtra3.Replace("PLC", ""))}";
                            dto.PageName = "Visit Approval";
                        }
                        else if (billExtra3.Contains("MED"))
                        {
                            dto.ApprovalLinkHtml =
                                $"<a href='ReleaseOrder.aspx?Type=Old&Id={billExtra3.Replace("MED", "")}'>Click Here To View Release Order</a>";
                            dto.PageName = "Release Order";
                        }
                        else if (billExtra3.Contains("ADM"))
                        {
                            dto.ApprovalLinkHtml = $"http://glauniversity.in:8109/main/reports/VO.aspx?RefNo={_general.Encrypt(billExtra3.Replace("ADM", ""))}";
                            dto.PageName = "Visit Approval";
                            //$"<a href='http://glauniversity.in:8109/main/reports/VO.aspx?RefNo={_general.Encrypt(billExtra3.Replace("ADM", ""))}'>Click Here To View Visit Approval</a>";
                        }
                        else if (billExtra3.Contains("SAL"))
                        {
                            var tt = billExtra3.Replace("SAL", "");
                            dto.ApprovalLinkHtml = $"http://glauniversity.in:8088/PrintForms/Res_Feedback.aspx?RefNo={_general.Encrypt(tt)}";
                            dto.PageName = "Resignation Application";
                            //$"<a href='http://glauniversity.in:8088/PrintForms/Res_Feedback.aspx?RefNo={_general.Encrypt(tt)}'>Click Here To View Resignation Application</a>";
                        }
                        else if (billExtra3.Contains("RSC"))
                        {
                            var tt = billExtra3.Replace("RSC", "");
                            dto.ApprovalLinkHtml = $"https://glauniversity.in:8085/ASP/PhdScholarPrint.aspx?ReqData={_general.EncryptWithoutHour(tt)}";
                            dto.PageName = "Progress Application";
                            //$"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('https://glauniversity.in:8085/ASP/PhdScholarPrint.aspx?ReqData={_general.EncryptWithoutHour(tt)}',0,0); return false;\"><u>Click Here To View Progress Application</u></a>";
                        }
                        else
                        {
                            dto.ApprovalLinkHtml = billExtra3;
                            dto.PageName = "Purchase Approval";
                            //$"<a href='PO.aspx?Id={billExtra3}&Switch=Y'>Click Here To View Purchase Approval</a>";
                        }
                    }
                    else
                    {
                        if (col4Ref.Contains("-ADV"))
                        {
                            var tt = col4Ref.Replace("-ADV", "");
                            dto.ApprovalLinkHtml =
                                $"<a href='POAdvance.aspx?Id={tt}&Switch=Y'>Click Here To View Advance Application</a>";
                            dto.PageName = "Advance Application";
                        }
                        if (col4Ref.Contains("-MOB"))
                        {
                            var tt = col4Ref.Replace("-MOB", "");
                            dto.ApprovalLinkHtml =
                                $"http://glauniversity.in:8088/ExtraPages/MobileShowBillDetails.aspx?approvedid={_general.Encrypt($"{tid}#{tt}")}";
                            dto.PageName = "Mobile Bill Details";
                        }
                        if (col4Ref.Contains("-SAL"))
                        {
                            var tt = col4Ref.Replace("-SAL", "");
                            dto.ApprovalLinkHtml =
                                $"http://glauniversity.in:8088/PrintForms/Res_Feedback.aspx?RefNo={_general.Encrypt(tt)}";
                            dto.PageName = "Resignation Application";
                        }
                    }

                    // ---------- relative contact no ----------
                    using DataTable relDt = await _advanceRepository.GetRelativeContactNo(dto.RelativePersonId);
                    dto.RelativeContactNo = relDt.Rows.Count > 0
                        ? relDt.Rows[0]["contactno"].ToString()
                        : "N/A";

                    // ---------- approvals_authority summary ----------
                    using DataTable apprDt = await _advanceRepository.GetApprovalsAuthSummary(tid);
                    dto.ApprovedBySummary = apprDt.Rows.Count > 0
                        ? textInfo.ToTitleCase(apprDt.Rows[0][0]?.ToString()?.ToLower() ?? string.Empty)
                        : "N/A";

                    // ---------- bill_transaction_issue grid -> Issues ----------
                    using DataTable issueDt = await _advanceRepository.GetBillIssues(tid);
                    var rowNo = 1;
                    foreach (DataRow r in issueDt.Rows)
                    {
                        var issue = new BillIssueRowDto
                        {
                            RowNo = rowNo++,
                            CvName = r["CVName"]?.ToString(),
                            PaidAmount = r["PaidAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(r["PaidAmount"]),
                            TaxAmount = r["TaxAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(r["TaxAmount"]),
                            IssuedAmount = r["IssuedAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(r["IssuedAmount"]),
                            Tran = r["Tran"]?.ToString(),
                            IssuedOn = r["On"]?.ToString(),
                            By = r["By"]?.ToString(),
                            SignedOn = r["Sign"]?.ToString(),
                            ReceivedOn = r["Rcv"]?.ToString(),
                            BankOn = r["Bank"]?.ToString(),
                            Status = r["Status"]?.ToString()
                        };

                        var t = r["TransactionID"]?.ToString();
                        var s = r["SequenceID"]?.ToString();
                        issue.PdfUrl = $"/Upload_Bills/{t}_{s}.pdf";
                        issue.ExcelUrl = $"/Upload_Bills/{t}_{s}.xlsx";

                        dto.Issues.Add(issue);
                    }
                    return new ApiResponse(StatusCodes.Status200OK, "Success", dto);
                }
                else
                {
                    return new ApiResponse(StatusCodes.Status200OK, "Success", "No Record Found");
                }
            }
        }

        public async Task<ApiResponse> GetVendorDetails(string VendorId)
        {
            DataTable VenderDetails =
                await _advanceRepository.GetVendorDetails(VendorId);
            FirmDetailsResponse res = new FirmDetailsResponse();

            if (VenderDetails != null && VenderDetails.Rows.Count > 0)
            {
                res.FirmName = VenderDetails.Rows[0]["VendorName"].ToString() ?? string.Empty;
                res.FirmContactName = VenderDetails.Rows[0]["ContactPersons"].ToString() ?? string.Empty;
                res.FirmContactNo = VenderDetails.Rows[0]["ContactNo"].ToString() ?? string.Empty;
                res.FirmEmail = VenderDetails.Rows[0]["EmailID"].ToString() ?? string.Empty;
                res.FirmAlternateContactNo = VenderDetails.Rows[0]["AlternateContactNo"].ToString() ?? string.Empty;
                res.FirmAddress = VenderDetails.Rows[0]["Address"].ToString().ToUpper().Replace("<BR/>", " ") ?? string.Empty;
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", res);
        }

        public async Task<ApiResponse> LoadTransactionDetails(string TransId)
        {
            ApiResponseLoadTransactionDetails ls=new ApiResponseLoadTransactionDetails();
            List<LoadTransactionDetailsRespomse> lst=new List<LoadTransactionDetailsRespomse>();
            using (DataTable dt = await _advanceRepository.GetTransDetails(TransId))
            {
               DataTable billBase=await _advanceRepository.BillBaseDetails(TransId);
               
                string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload_Bills");
                foreach (DataRow dr in dt.Rows) 
                {
                    bool canDelete = false;
                    if (dr["SignedOn"].ToString()=="" && dr["ReceivedOn"].ToString()=="" && dr["ClearOn"].ToString()=="")
                    {
                        canDelete = true;
                    }


                    DataTable Auth = await _advanceRepository.CheucqAuth(TransId, dr["SequenceID"].ToString());
                    lst.Add(new LoadTransactionDetailsRespomse
                    {
                        //Remaining = dr["AmountRemaining"].ToString(),
                        //AlreadyIssued = dr["AmountPaid"].ToString(),
                        IssuedOn = dr["On"].ToString(),
                        Firm = dr["CVName"].ToString(),
                        Sub_Firm = dr["CVSubFirm"].ToString(),
                        Type = dr["IssuedType"].ToString(),
                        Issued = dr["IssuedAmount"].ToString(),
                        Tax = dr["TaxAmount"].ToString(),
                        Other = dr["OtherCut"].ToString(),
                        Paid = dr["PaidAmount"].ToString(),
                        Mode = dr["Mode"].ToString(),
                        TransNo = dr["TransactionNo"].ToString(),
                        Date = dr["On"].ToString(),
                        By = dr["IssuedByName"].ToString(),
                        SeqNo = dr["SequenceID"].ToString(),
                        FileUrl = File.Exists(Path.Combine(FolderPath, TransId + "_" + dr["SequenceID"].ToString() + ".pdf")) ? $"Upload_Bills/{TransId}_{dr["SequenceID"].ToString()}.pdf" : "",
                        CheckIssuedOn = dr["IssuedOn"].ToString(),
                        SignedOn = dr["SignedOn"].ToString(),
                        ReceivedOn = dr["ReceivedOn"].ToString(),
                        ClearOn = dr["ClearOn"].ToString(),
                        Status = Auth.Rows.Count > 0 ? Auth.Rows[0]["Status"].ToString() : "",
                        EmpDetails = Auth.Rows.Count > 0 ? Auth.Rows[0]["EmployeeDetails"].ToString() : "" ,
                        CanDelete= canDelete

                    });
                }
                ls.LoadTransactionDetailsRespomses = lst;
                ls.CanMessageSend = false;
                ls.EnableBillUpto = false;
                if (billBase.Rows.Count > 0)
                {
                    ls.Remaining = billBase.Rows[0]["AmountRemaining"].ToString();
                    ls.AlreadyIssued = billBase.Rows[0]["AmountPaid"].ToString();
                    ls.AdditionalName= billBase.Rows[0]["Col5"].ToString();
                    ls.Purpose= billBase.Rows[0]["Remark"].ToString();
                    ls.FirmName= billBase.Rows[0]["FirmName"].ToString();
                    ls.VendorId= billBase.Rows[0]["VendorID"].ToString();
                    ls.Sub_Firm= billBase.Rows[0]["col3"].ToString();
                    if (billBase.Rows[0]["ForType"].ToString()== "Advance Bill")
                    {
                        ls.EnableBillUpto = true;
                        DataTable IsSpecial = await _advanceRepository.GEtSpecialVendor(billBase.Rows[0]["VendorID"].ToString());
                        if(IsSpecial.Rows.Count>0)
                        {
                            ls.CanMessageSend = true;
                        }
                    }
                }

            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", ls);
        }

        public async Task<ApiResponse> getAuthForDirectBill(string CampusCode)
        {
            List<TextValues> PersonLst = new List<TextValues>();
            using(DataTable dt=await _advanceRepository.GetPerson(CampusCode))
            {
                foreach(DataRow dr in dt.Rows)
                {
                    PersonLst.Add(new TextValues
                    {
                        Text = dr["Text"].ToString(),
                        Value = dr["Value"].ToString(),
                        EmpCode = dr["employee_code"].ToString()
                    });
                }
            }
            List<TextValues> App3lst = new List<TextValues>();
            List<TextValues> App4lst = new List<TextValues>();
            List<TextValues> App1lst = new List<TextValues>();
            List<TextValues> App2lst = new List<TextValues>();
            if(CampusCode!="101")
            {
                using(DataTable dt=await _advanceRepository.GetThirdAuth())
                {
                    foreach(DataRow dr in dt.Rows)
                    App3lst.Add(new TextValues
                    {
                        Text = dr["Text"].ToString(),
                        Value = dr["Value2"].ToString(),
                        EmpCode = dr["Value"].ToString()
                    });
                }
                using(DataTable dt=await _advanceRepository.Get4Auth())
                {
                    foreach (DataRow dr in dt.Rows)
                        App4lst.Add(new TextValues
                        {
                            Text = dr["Text"].ToString(),
                            Value = dr["Value2"].ToString(),
                            EmpCode = dr["Value"].ToString()
                        });
                }

            }
            else
            {
                using(DataTable dt=await _advanceRepository.GetBillauth(CampusCode))
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        App4lst.Add(new TextValues
                        {
                            Text = dr["Text"].ToString(),
                            Value = dr["Value2"].ToString(),
                            EmpCode = dr["Value"].ToString()
                        });
                        App3lst.Add(new TextValues
                        {
                            Text = dr["Text"].ToString(),
                            Value = dr["Value2"].ToString(),
                            EmpCode = dr["Value"].ToString()
                        });
                    }
                }
            }
            using(DataTable dt=await _advanceRepository.GetFirstSecond(CampusCode))
            {
                foreach(DataRow dr in dt.Rows)
                {
                    App1lst.Add(new TextValues
                    {
                        Text = dr["Text"].ToString(),
                        Value = dr["Value2"].ToString(),
                        EmpCode = dr["Value"].ToString()
                    });
                }
                App2lst = App1lst;
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", new { PersonLst,App1List= App1lst,App2List= App2lst,App3List= App3lst,App4List= App4lst });
        }



        //public async Task<ApiResponse> SaveCheckDetails(string EmpCode,string EmpType)
        //{

        //}


        public async Task<ApiResponse> GetBillApprovalFilterSessions()
        {
            using DataTable dtSessions = await _advanceRepository.GetBillApprovalFilterSessions();
            List<string> Sessions = new List<string>();
            foreach (DataRow dr in dtSessions.Rows)
            {
                Sessions.Add(dr["Session"].ToString() ?? string.Empty);
            }
            string currentSession = _general.GetFinancialSession(DateTime.Now);

            if (!Sessions.Contains(currentSession))
            {
                Sessions.Insert(0, currentSession);
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", Sessions);
        }
        public async Task<ApiResponse> GetBillApprovalFilterInitiatedBy()
        {
            using DataTable dtInitiatedBy = await _advanceRepository.GetBillApprovalFilterInitiatedBy();

            List<TextValues> InitiatedBy = new List<TextValues>();

            foreach (DataRow dr in dtInitiatedBy.Rows)
            {
                InitiatedBy.Add(new TextValues
                {
                    Text = dr["Text"].ToString(),
                    Value = dr["Value"].ToString(),
                    EmpCode = dr["Value"].ToString()
                });
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", InitiatedBy);
        }
        public async Task<ApiResponse> GetBillApprovalFilterChequeBy()
        {
            using DataTable dtChequeBy = await _advanceRepository.GetBillApprovalFilterChequeBy();

            List<TextValues> ChequeBy = new List<TextValues>();

            foreach (DataRow dr in dtChequeBy.Rows)
            {
                ChequeBy.Add(new TextValues
                {
                    Text = dr["Text"].ToString(),
                    Value = dr["Value"].ToString(),
                    EmpCode = dr["Value"].ToString()
                });
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", ChequeBy);
        }

        public async Task<ApiResponse> GetBillApprovalDetails(GetBillApprovalRequest? getBillApprovalRequest, string employeeId, string role, string name)
        {
            using DataTable dtAgencyWork = await _advanceRepository.GetBillApprovalAgencyBillIds();

            bool AllowBillReject = await _inclusiveService.IsUserAllowed(employeeId, ENUMS.Inclusive.UserRolePermission.AllowBillReject);

            using DataTable dtBillApprovals = await _advanceRepository.GetBillApprovalDetails(getBillApprovalRequest, employeeId, role);

            List<BillChequeApprovalResponse> billApprovalResponses = new List<BillChequeApprovalResponse>();

            foreach (DataRow dr in dtBillApprovals.Rows)
            {
                BillChequeApprovalResponse billChequeApprovalResponse = new BillChequeApprovalResponse(dr);

                if (_general.IsFileExists($"Upload_Bills/Approved/{billChequeApprovalResponse?.TransID}.pdf"))
                {
                    billChequeApprovalResponse!.BillApprovedPdfExists = true;
                }
                else if (_general.IsFileExists($"Upload_Bills/{billChequeApprovalResponse?.TransID}.pdf"))
                {
                    billChequeApprovalResponse!.BillPdfExists = true;
                }
                if (_general.IsFileExists($"Upload_Bills/{billChequeApprovalResponse?.TransID}.xlsx"))
                {
                    billChequeApprovalResponse!.BillExcelExists = true;
                }

                if (!string.IsNullOrWhiteSpace(billChequeApprovalResponse?.SequenceID))
                {
                    if (_general.IsFileExists($"Upload_Bills/{billChequeApprovalResponse?.TransID}_{billChequeApprovalResponse?.SequenceID}.pdf"))
                    {
                        billChequeApprovalResponse!.SequenceBillPdfExists = true;
                    }
                    if (_general.IsFileExists($"Upload_Bills/{billChequeApprovalResponse?.TransID}_{billChequeApprovalResponse?.SequenceID}.xlsx"))
                    {
                        billChequeApprovalResponse!.SequenceBillExcelExists = true;
                    }
                }

                if (!string.IsNullOrWhiteSpace(billChequeApprovalResponse?.BillExtra3) && billChequeApprovalResponse?.BillExtra3.Contains("SEC") == false && billChequeApprovalResponse?.BillExtra3.Contains("REF") == false && billChequeApprovalResponse?.BillExtra3.Contains("SAL") == false)
                {
                    if (billChequeApprovalResponse?.ForType == "Advance Bill")
                    {
                        billChequeApprovalResponse!.MainPrintOutString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('Reports/POAdvance.aspx?Id={billChequeApprovalResponse?.BillExtra3}&Switch=Y',0,0); return false;\"><u>" + HttpUtility.HtmlDecode(billChequeApprovalResponse?.ForType) + "</u></a>";

                        billChequeApprovalResponse?.MainPrintOutDetails?.Link = $"Reports/POAdvance.aspx?Id={billChequeApprovalResponse?.BillExtra3}&Switch=Y";
                        billChequeApprovalResponse?.MainPrintOutDetails?.Text = billChequeApprovalResponse?.ForType;

                        DataTable dtAdvanceBudgetSummary = await _advanceRepository.GetBillApprovalAdvanceBudgetSummary(billChequeApprovalResponse?.BillExtra3 ?? string.Empty);

                        if (dtAdvanceBudgetSummary.Rows.Count > 0)
                        {
                            billChequeApprovalResponse!.BudgetString = (!string.IsNullOrWhiteSpace(dtAdvanceBudgetSummary.Rows[0]["BudgetCommentExtra"]?.ToString()) ? dtAdvanceBudgetSummary.Rows[0]["BudgetCommentExtra"].ToString() + "<br/><br/>" : "") + "Budget : " + dtAdvanceBudgetSummary.Rows[0]["BudgetAmount"].ToString() + " Rs./-<br/>Prev. : " + dtAdvanceBudgetSummary.Rows[0]["PreviousTaken"].ToString() + " Rs./-<br/>Cur. : " + dtAdvanceBudgetSummary.Rows[0]["Amount"].ToString() + " Rs./-<br/>Status : " + dtAdvanceBudgetSummary.Rows[0]["BudgetStatus"].ToString() + "<br/>";

                            billChequeApprovalResponse?.BudgetPrintOutDetails?.BudgetAmount = dtAdvanceBudgetSummary.Rows[0]["BudgetAmount"]?.ToString();
                            billChequeApprovalResponse?.BudgetPrintOutDetails?.PreviousTaken = dtAdvanceBudgetSummary.Rows[0]["PreviousTaken"]?.ToString();
                            billChequeApprovalResponse?.BudgetPrintOutDetails?.Amount = dtAdvanceBudgetSummary.Rows[0]["Amount"]?.ToString();
                            billChequeApprovalResponse?.BudgetPrintOutDetails?.BudgetStatus = dtAdvanceBudgetSummary.Rows[0]["BudgetStatus"]?.ToString();
                            billChequeApprovalResponse?.BudgetPrintOutDetails?.BudgetComment = dtAdvanceBudgetSummary.Rows[0]["BudgetCommentExtra"]?.ToString();

                        }
                        else
                        {
                            dtAdvanceBudgetSummary = await _advanceRepository.GetBillApprovalAdvanceExcludeMedBudgetSummary(billChequeApprovalResponse?.BillExtra3?.Replace("MED", "") ?? string.Empty);

                            if (dtAdvanceBudgetSummary.Rows[0]["MyType"].ToString() == "Against Purchase Approval Advance Form" && dtAdvanceBudgetSummary.Rows[0]["PExtra4"].ToString() != "No Purchase Approval" && dtAdvanceBudgetSummary.Rows[0]["PExtra4"]?.ToString()?.Split('#')[0].Length >= 9)
                            {
                                dtAdvanceBudgetSummary = await _advanceRepository.GetBillApprovalAdvanceBudgetSummaryApprovalDetails(dtAdvanceBudgetSummary.Rows[0]["PExtra4"]?.ToString()?.Split('#')[0]!);
                            }


                            billChequeApprovalResponse!.BudgetString = "Total Amount : " + dtAdvanceBudgetSummary.Rows[0]["TotalAmount"].ToString() + " Rs./-<br/>Total Paid : " + dtAdvanceBudgetSummary.Rows[0]["TotalPaid"].ToString() + " Rs./-<br/>Total Bal. : " + dtAdvanceBudgetSummary.Rows[0]["TotalBal"].ToString() + " Rs./-<br/>Status : " + dtAdvanceBudgetSummary.Rows[0]["Status"].ToString() + "<br/><br/>";

                            billChequeApprovalResponse!.BudgetStringToolTip = dtAdvanceBudgetSummary.Rows[0]["PaidComment"]?.ToString()?.Replace("@@", "\n") ?? string.Empty;

                            billChequeApprovalResponse?.BudgetPrintOutDetails?.TotalAmount = dtAdvanceBudgetSummary.Rows[0]["TotalAmount"]?.ToString();
                            billChequeApprovalResponse?.BudgetPrintOutDetails?.TotalPaid = dtAdvanceBudgetSummary.Rows[0]["TotalPaid"]?.ToString();
                            billChequeApprovalResponse?.BudgetPrintOutDetails?.TotalBalance = dtAdvanceBudgetSummary.Rows[0]["TotalBal"]?.ToString();
                            billChequeApprovalResponse?.BudgetPrintOutDetails?.Status = dtAdvanceBudgetSummary.Rows[0]["Status"]?.ToString();

                        }
                    }
                    else
                    {

                        if (billChequeApprovalResponse!.BillExtra3.Contains("MED"))
                        {
                            billChequeApprovalResponse!.MainPrintOutString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('Reports/ReleaseOrder.aspx?Type=Old&Id={(billChequeApprovalResponse!.BillExtra3!.ToString().Replace("MED", ""))}',0,0); return false;\"><u>" + HttpUtility.HtmlDecode(billChequeApprovalResponse?.ForType) + "</u></a>";

                            billChequeApprovalResponse?.MainPrintOutDetails?.Link = $"Reports/ReleaseOrder.aspx?Type=Old&Id={(billChequeApprovalResponse!.BillExtra3!.ToString().Replace("MED", ""))}";
                            billChequeApprovalResponse?.MainPrintOutDetails?.Text = billChequeApprovalResponse?.ForType;


                            DataTable dtReleaseOrder = await _advanceRepository.GetBillApprovalAdvanceMedReleaseOrder(billChequeApprovalResponse!.BillExtra3?.Replace("MED", "")!);
                            if (dtReleaseOrder.Rows.Count > 0)
                            {
                                if (dtReleaseOrder.Rows[0]["BudgetStatus"].ToString() == "Y")

                                {
                                    billChequeApprovalResponse!.BudgetString = (!string.IsNullOrWhiteSpace(dtReleaseOrder.Rows[0]["BudgetCommentExtra"]?.ToString()) ? dtReleaseOrder.Rows[0]["BudgetCommentExtra"].ToString() + "<br/><br/>" : "") + "Budget : " + dtReleaseOrder.Rows[0]["BudgetAmount"].ToString() + " Rs./-<br/>Prev. : " + dtReleaseOrder.Rows[0]["PreviousTaken"].ToString() + " Rs./-<br/>Cur. : " + dtReleaseOrder.Rows[0]["Amount"].ToString() + " Rs./-<br/>Status : " + dtReleaseOrder.Rows[0]["CurStatus"].ToString() + "<br/><br/>";

                                    billChequeApprovalResponse?.BudgetPrintOutDetails?.BudgetAmount = dtReleaseOrder.Rows[0]["BudgetAmount"]?.ToString();
                                    billChequeApprovalResponse?.BudgetPrintOutDetails?.PreviousTaken = dtReleaseOrder.Rows[0]["PreviousTaken"]?.ToString();
                                    billChequeApprovalResponse?.BudgetPrintOutDetails?.Amount = dtReleaseOrder.Rows[0]["Amount"]?.ToString();
                                    billChequeApprovalResponse?.BudgetPrintOutDetails?.BudgetStatus = dtReleaseOrder.Rows[0]["CurStatus"]?.ToString();
                                    billChequeApprovalResponse?.BudgetPrintOutDetails?.BudgetComment = dtReleaseOrder.Rows[0]["BudgetCommentExtra"]?.ToString();
                                }

                                billChequeApprovalResponse!.BudgetString += "Total Amount : " + dtReleaseOrder.Rows[0]["TotalAmount"].ToString() + " Rs./-<br/>Total Paid : " + dtReleaseOrder.Rows[0]["TotalPaid"].ToString() + " Rs./-<br/>Total Bal. : " + dtReleaseOrder.Rows[0]["TotalBal"].ToString() + " Rs./-<br/>Status : " + dtReleaseOrder.Rows[0]["Status"].ToString() + "<br/><br/>";

                                billChequeApprovalResponse?.BudgetPrintOutDetails?.TotalAmount = dtReleaseOrder.Rows[0]["TotalAmount"]?.ToString();
                                billChequeApprovalResponse?.BudgetPrintOutDetails?.TotalPaid = dtReleaseOrder.Rows[0]["TotalPaid"]?.ToString();
                                billChequeApprovalResponse?.BudgetPrintOutDetails?.TotalBalance = dtReleaseOrder.Rows[0]["TotalBal"]?.ToString();
                                billChequeApprovalResponse?.BudgetPrintOutDetails?.Status = dtReleaseOrder.Rows[0]["Status"]?.ToString();

                                billChequeApprovalResponse!.BudgetStringToolTip = dtReleaseOrder.Rows[0]["PaidComment"]?.ToString()?.Replace("@@", "\n") ?? string.Empty;
                            }
                        }
                        else if (!billChequeApprovalResponse!.BillExtra3.Contains("PLC") && !billChequeApprovalResponse!.BillExtra3.Contains("WRK") && !billChequeApprovalResponse!.BillExtra3.Contains("ADM") && !billChequeApprovalResponse!.BillExtra3.Contains("RSC"))
                        {
                            if (Int64.TryParse(billChequeApprovalResponse!.BillExtra3?.ToString()?.Replace("MED", ""), out _))
                            {
                                using DataTable dtApprovalSummary = await _advanceRepository.GetBillApprovalAdvanceBudgetSummaryApprovalDetails(billChequeApprovalResponse!.BillExtra3?.ToString()?.Replace("MED", "")!);
                                if (dtApprovalSummary.Rows.Count > 0)
                                {
                                    billChequeApprovalResponse!.BudgetString = "Total Amount : " + dtApprovalSummary.Rows[0]["TotalAmount"].ToString() + " Rs./-<br/>Total Paid : " + dtApprovalSummary.Rows[0]["TotalPaid"].ToString() + " Rs./-<br/>Total Bal. : " + dtApprovalSummary.Rows[0]["TotalBal"].ToString() + " Rs./-<br/>Status : " + dtApprovalSummary.Rows[0]["Status"].ToString() + "<br/><br/>";

                                    billChequeApprovalResponse?.BudgetPrintOutDetails?.TotalAmount = dtApprovalSummary.Rows[0]["TotalAmount"]?.ToString();
                                    billChequeApprovalResponse?.BudgetPrintOutDetails?.TotalPaid = dtApprovalSummary.Rows[0]["TotalPaid"]?.ToString();
                                    billChequeApprovalResponse?.BudgetPrintOutDetails?.TotalBalance = dtApprovalSummary.Rows[0]["TotalBal"]?.ToString();
                                    billChequeApprovalResponse?.BudgetPrintOutDetails?.Status = dtApprovalSummary.Rows[0]["Status"]?.ToString();

                                    billChequeApprovalResponse!.ItemReturnOnConsumableLinkString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('http://hostel.glauniversity.in:84/NewItemReturnnonconsumableprintforbill.aspx?approvalid={billChequeApprovalResponse!.BillExtra3!.ToString()}&Switch=Y',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse!.SequenceID)}</u></a>";

                                    billChequeApprovalResponse!.ItemReturnOnConsumableLinkDetails?.Link = $"http://hostel.glauniversity.in:84/NewItemReturnnonconsumableprintforbill.aspx?approvalid={billChequeApprovalResponse!.BillExtra3!.ToString()}&Switch=Y";
                                    billChequeApprovalResponse!.ItemReturnOnConsumableLinkDetails?.Text = billChequeApprovalResponse!.SequenceID;


                                    billChequeApprovalResponse!.MainPrintOutString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('Reports/PO.aspx?Id={billChequeApprovalResponse!.BillExtra3!.ToString()}&Switch=Y',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse?.ForType)}</u></a>";

                                    billChequeApprovalResponse!.MainPrintOutDetails!.Link = $"Reports/PO.aspx?Id={billChequeApprovalResponse!.BillExtra3!.ToString()}&Switch=Y";
                                    billChequeApprovalResponse!.MainPrintOutDetails!.Text = billChequeApprovalResponse?.ForType;


                                    billChequeApprovalResponse!.MainGateRecievingLinkString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('http://hostel.glauniversity.in:84/MainGateMaterialReceivingdetail.aspx?referenceno={billChequeApprovalResponse!.BillExtra3}&Switch=Y',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse!.INI)}</u></a>";

                                    billChequeApprovalResponse!.MainGatePrintOutDetails!.Link = $"http://hostel.glauniversity.in:84/MainGateMaterialReceivingdetail.aspx?referenceno={billChequeApprovalResponse!.BillExtra3}&Switch=Y";
                                    billChequeApprovalResponse!.MainGatePrintOutDetails!.Text = billChequeApprovalResponse!.INI;



                                    billChequeApprovalResponse!.BudgetStringToolTip = dtApprovalSummary.Rows[0]["PaidComment"]?.ToString()?.Replace("@@", "\n") ?? string.Empty;
                                }
                            }
                        }

                        if (billChequeApprovalResponse!.BillExtra3!.Contains("PLC"))
                        {
                            billChequeApprovalResponse!.MainPrintOutString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('https://glauniversity.in:8105/main/reports/VO.aspx?RefNo={_general.Encrypt(billChequeApprovalResponse!.BillExtra3!.Replace("PLC", ""))}',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse?.ForType)}</u></a>";

                            billChequeApprovalResponse!.MainPrintOutDetails!.Link = $"https://glauniversity.in:8105/main/reports/VO.aspx?RefNo={_general.Encrypt(billChequeApprovalResponse!.BillExtra3!.Replace("PLC", ""))}";
                            billChequeApprovalResponse!.MainPrintOutDetails!.Text = billChequeApprovalResponse?.ForType;

                        }
                        if (billChequeApprovalResponse!.BillExtra3!.Contains("WRK"))
                        {
                            billChequeApprovalResponse!.MainPrintOutString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('Workshop/WorkShopApprovalPrint.aspx?Id={_general.Encrypt(billChequeApprovalResponse!.BillExtra3!.Replace("WRK", ""))}',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse?.ForType)}</u></a>";

                            billChequeApprovalResponse!.MainPrintOutDetails!.Link = $"Workshop/WorkShopApprovalPrint.aspx?Id={_general.Encrypt(billChequeApprovalResponse!.BillExtra3!.Replace("WRK", ""))}";
                            billChequeApprovalResponse!.MainPrintOutDetails!.Text = billChequeApprovalResponse?.ForType;
                        }
                        if (billChequeApprovalResponse!.BillExtra3!.Contains("ADM"))
                        {
                            billChequeApprovalResponse!.MainPrintOutString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('https://glauniversity.in:8109/main/reports/VO.aspx?RefNo={_general.Encrypt(billChequeApprovalResponse!.BillExtra3!.Replace("ADM", ""))}',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse?.ForType)}</u></a>";

                            billChequeApprovalResponse!.MainPrintOutDetails!.Link = $"https://glauniversity.in:8109/main/reports/VO.aspx?RefNo={_general.Encrypt(billChequeApprovalResponse!.BillExtra3!.Replace("ADM", ""))}";
                            billChequeApprovalResponse!.MainPrintOutDetails!.Text = billChequeApprovalResponse?.ForType;
                        }
                        if (billChequeApprovalResponse!.BillExtra3!.Contains("RSC"))
                        {
                            billChequeApprovalResponse!.MainPrintOutString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('https://glauniversity.in:8085/ASP/PhdScholarPrint.aspx?ReqData={_general.EncryptWithoutHour(billChequeApprovalResponse!.BillExtra3!.ToString().Replace("RSC", ""))}',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse?.ForType)}</u></a>";

                            billChequeApprovalResponse!.MainPrintOutDetails!.Link = $"https://glauniversity.in:8085/ASP/PhdScholarPrint.aspx?ReqData={_general.EncryptWithoutHour(billChequeApprovalResponse!.BillExtra3!.ToString().Replace("RSC", ""))}";
                            billChequeApprovalResponse!.MainPrintOutDetails!.Text = billChequeApprovalResponse?.ForType;
                        }
                    }
                }
                else
                {
                    if (billChequeApprovalResponse?.BillExtra3?.Contains("SEC") == true || billChequeApprovalResponse?.BillExtra3?.Contains("REF") == true)
                    {
                        using DataTable dtUserIdentity = await _advanceRepository.GetBillApprovalAdvanceUserIdentity(billChequeApprovalResponse!.TransID!);
                        long student_id = 0;
                        if (dtUserIdentity.Rows.Count > 0 && Int64.TryParse(dtUserIdentity.Rows[0][0]?.ToString()!, out student_id))
                        {
                            billChequeApprovalResponse!.ItemReturnOnConsumableLinkString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('https://glauniversity.in/studentinformationfee.aspx?Stu_ID={student_id}&User={_general.Encrypt("Security")}',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse!.SequenceID)}</u></a>";

                            billChequeApprovalResponse?.ItemReturnOnConsumableLinkDetails!.Link = $"https://glauniversity.in/studentinformationfee.aspx?Stu_ID={student_id}&User={_general.Encrypt("Security")}";
                            billChequeApprovalResponse?.ItemReturnOnConsumableLinkDetails!.Text = billChequeApprovalResponse!.SequenceID;


                            billChequeApprovalResponse!.StudentId = student_id.ToString();

                            if (billChequeApprovalResponse!.BillExtra3!.Contains("SEC"))
                            {
                                billChequeApprovalResponse!.MainPrintOutString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('https://glauniversity.in/RefundPrint.aspx?Id={_general.Encrypt(billChequeApprovalResponse!.TransID!.ToString())}',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse!.ForType)}</u></a>";

                                billChequeApprovalResponse!.MainPrintOutDetails!.Link = $"https://glauniversity.in/RefundPrint.aspx?Id={_general.Encrypt(billChequeApprovalResponse!.TransID!.ToString())}";
                                billChequeApprovalResponse!.MainPrintOutDetails!.Text = billChequeApprovalResponse!.ForType;
                            }
                        }
                    }
                    else
                    {
                        using DataTable dtUserIdentity = await _advanceRepository.GetBillApprovalAdvanceUserIdentity(billChequeApprovalResponse!.TransID!);
                        if (dtUserIdentity.Rows.Count > 0)
                        {
                            if (dtUserIdentity.Rows[0][0]?.ToString()?.Contains("-ADV") == true)
                            {
                                int referenceNo = 0;
                                if (int.TryParse(dtUserIdentity.Rows[0][0]?.ToString()?.Replace("-ADV", ""), out referenceNo))
                                {
                                    billChequeApprovalResponse!.MainPrintOutString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('Reports/POAdvance.aspx?Id={referenceNo}',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse!.ForType)}</u></a>";

                                    billChequeApprovalResponse!.MainPrintOutDetails!.Link = $"Reports/POAdvance.aspx?Id={referenceNo}";
                                    billChequeApprovalResponse!.MainPrintOutDetails!.Text = billChequeApprovalResponse!.ForType;


                                    DataTable dtOtherApprovalSummary = await _advanceRepository.GetBillApprovalAdvanceExcludeMedBudgetSummary(referenceNo.ToString());
                                    if (dtOtherApprovalSummary.Rows.Count > 0)
                                    {
                                        if (dtOtherApprovalSummary.Rows[0]["MyType"].ToString() == "Against Purchase Approval Advance Form" && dtOtherApprovalSummary.Rows[0]["PExtra4"].ToString() != "No Purchase Approval" && dtOtherApprovalSummary.Rows[0]["PExtra4"]?.ToString()?.Split('#')[0].Length >= 9)
                                        {
                                            dtOtherApprovalSummary = await _advanceRepository.GetBillApprovalAdvanceBudgetSummaryApprovalDetails(dtOtherApprovalSummary.Rows[0]["PExtra4"]?.ToString()?.Split('#')[0]!);
                                        }

                                        billChequeApprovalResponse!.BudgetString = "Total Amount : " + dtOtherApprovalSummary.Rows[0]["TotalAmount"].ToString() + " Rs./-<br/>Total Paid : " + dtOtherApprovalSummary.Rows[0]["TotalPaid"].ToString() + " Rs./-<br/>Total Bal. : " + dtOtherApprovalSummary.Rows[0]["TotalBal"].ToString() + " Rs./-<br/>Status : " + dtOtherApprovalSummary.Rows[0]["Status"].ToString() + "<br/><br/>";

                                        billChequeApprovalResponse?.BudgetPrintOutDetails?.TotalAmount = dtOtherApprovalSummary.Rows[0]["TotalAmount"]?.ToString();
                                        billChequeApprovalResponse?.BudgetPrintOutDetails?.TotalPaid = dtOtherApprovalSummary.Rows[0]["TotalPaid"]?.ToString();
                                        billChequeApprovalResponse?.BudgetPrintOutDetails?.TotalBalance = dtOtherApprovalSummary.Rows[0]["TotalBal"]?.ToString();
                                        billChequeApprovalResponse?.BudgetPrintOutDetails?.Status = dtOtherApprovalSummary.Rows[0]["Status"]?.ToString();


                                        billChequeApprovalResponse!.BudgetStringToolTip = dtOtherApprovalSummary.Rows[0]["PaidComment"]?.ToString()?.Replace("@@", "\n") ?? string.Empty;
                                    }
                                }
                            }
                            if (dtUserIdentity.Rows[0][0]?.ToString()?.Contains("-MOB") == true)
                            {
                                string referenceNo = dtUserIdentity?.Rows[0][0]?.ToString()?.Replace("-MOB", "") ?? string.Empty;
                                billChequeApprovalResponse!.MainPrintOutString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('https://glauniversity.in:8088/ExtraPages/MobileShowBillDetails.aspx?approvedid={_general.Encrypt(billChequeApprovalResponse!.TransID + "#" + referenceNo)}',0,0); return false;\"><u>" + HttpUtility.HtmlDecode(billChequeApprovalResponse!.ForType) + "</u></a>";


                                billChequeApprovalResponse!.MainPrintOutDetails?.Link = $"https://glauniversity.in:8088/ExtraPages/MobileShowBillDetails.aspx?approvedid={_general.Encrypt(billChequeApprovalResponse!.TransID + "#" + referenceNo)}";
                                billChequeApprovalResponse!.MainPrintOutDetails?.Text = billChequeApprovalResponse!.ForType;

                            }
                            if (dtUserIdentity?.Rows[0][0]?.ToString()?.Contains("-SAL") == true)
                            {
                                string referenceNo = dtUserIdentity?.Rows[0][0]?.ToString()?.Replace("-SAL", "") ?? string.Empty;
                                billChequeApprovalResponse!.MainPrintOutString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('https://glauniversity.in:8088/PrintForms/Res_Feedback.aspx?RefNo={_general.Encrypt(referenceNo)}',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse!.ForType)}</u></a>";

                                billChequeApprovalResponse!.MainPrintOutDetails?.Link = $"https://glauniversity.in:8088/PrintForms/Res_Feedback.aspx?RefNo={_general.Encrypt(referenceNo)}";
                                billChequeApprovalResponse!.MainPrintOutDetails?.Text = billChequeApprovalResponse!.ForType;
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(billChequeApprovalResponse?.BillExtra6))
                {
                    billChequeApprovalResponse.WarrentySwitchAllowed = true;
                    if (role == "MANAGEMENT")
                    {
                        billChequeApprovalResponse!.PurposeLinkString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('http://hostel.glauniversity.in:84/warrenty.aspx?approvalid={billChequeApprovalResponse!.BillExtra6}&Switch=Y',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse!.Purpose!)}</u></a>";

                        billChequeApprovalResponse!.PurposePrintOutDetails?.Link = $"http://hostel.glauniversity.in:84/warrenty.aspx?approvalid={billChequeApprovalResponse!.BillExtra6}&Switch=Y";
                        billChequeApprovalResponse!.PurposePrintOutDetails?.Text = billChequeApprovalResponse!.Purpose!;
                    }
                    else
                    {
                        billChequeApprovalResponse!.PurposeLinkString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('http://hostel.glauniversity.in:84/warrenty.aspx?approvalid={billChequeApprovalResponse!.BillExtra6}&Switch=N',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse!.Purpose!)}</u></a>";

                        billChequeApprovalResponse!.PurposePrintOutDetails?.Link = $"http://hostel.glauniversity.in:84/warrenty.aspx?approvalid={billChequeApprovalResponse!.BillExtra6}&Switch=N";
                        billChequeApprovalResponse!.PurposePrintOutDetails?.Text = billChequeApprovalResponse!.Purpose!;
                    }
                }


                string[] splt = billChequeApprovalResponse?.FirmName?.Split('#')!;

                billChequeApprovalResponse!.DepartmentVendorPaidString = $"<b>Department : <a href='#' style='color:black;' onclick=\"openPopUpEnc('Reports/FirmPaidReport.aspx?VId=$$$&SubId=D','{billChequeApprovalResponse.Col1}'); return false;\"><u>{billChequeApprovalResponse.Col1}</u></a></b><br/><a href='#' style='font-weight:bold;' onclick='openPopUp(`Reports/FirmPaidReport.aspx?Id={billChequeApprovalResponse.TransID}`,0,0); return false;'><u>{splt[0]}</u></a><br/><span style='color:blue;'><a href='#' style='font-weight:bold;color:blue;' onclick='openPopUp(`Reports/FirmPaidReport.aspx?VId={billChequeApprovalResponse.BVId}&SubId=&CallBy={_general.Encrypt(billChequeApprovalResponse.BRPID!)}`,0,0); return false;'><u>{splt[1]}</u></a></span>";

                billChequeApprovalResponse!.DepartmentPrintOutDetails!.Department = billChequeApprovalResponse.Col1;
                billChequeApprovalResponse!.DepartmentPrintOutDetails!.DepartmentLink = $"Reports/FirmPaidReport.aspx?VId=$$$&SubId=D','{billChequeApprovalResponse.Col1}";

                billChequeApprovalResponse!.DepartmentPrintOutDetails!.TransactionId = billChequeApprovalResponse.TransID;
                billChequeApprovalResponse!.DepartmentPrintOutDetails!.FirmName = splt[0];
                billChequeApprovalResponse!.DepartmentPrintOutDetails!.FirmLink = $"Reports/FirmPaidReport.aspx?Id={billChequeApprovalResponse.TransID}";

                billChequeApprovalResponse!.DepartmentPrintOutDetails!.RelativePersonName = splt[1];
                billChequeApprovalResponse!.DepartmentPrintOutDetails!.VendorId = billChequeApprovalResponse.BVId;
                billChequeApprovalResponse!.DepartmentPrintOutDetails!.RelativePersonId = billChequeApprovalResponse.BRPID;
                billChequeApprovalResponse!.DepartmentPrintOutDetails!.RelativePersonLink = $"Reports/FirmPaidReport.aspx?VId={billChequeApprovalResponse.BVId}&SubId=&CallBy={_general.Encrypt(billChequeApprovalResponse.BRPID!)}";


                if (billChequeApprovalResponse!.BVId == "827")
                {
                    using DataTable dtImprest = await _advanceRepository.GetBillApprovalAdvanceImprestDetails(billChequeApprovalResponse!.TransID!);
                    if (dtImprest.Rows.Count > 0)
                    {
                        billChequeApprovalResponse!.IsImprestSummary = true;

                        billChequeApprovalResponse!.MainPrintOutString = $"<a href='#' style='font-weight:bold;' onclick=\"openPopUp('Reports/ImprestSummary.aspx?Id={_general.Encrypt(billChequeApprovalResponse!.TransID + "#Yes")}',0,0); return false;\"><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse!.ForType)}</u></a>";

                        billChequeApprovalResponse!.MainPrintOutDetails?.Link = $"Reports/ImprestSummary.aspx?Id={_general.Encrypt(billChequeApprovalResponse!.TransID + "#Yes")}";
                        billChequeApprovalResponse!.MainPrintOutDetails?.Text = billChequeApprovalResponse!.ForType;
                    }
                }

                if (billChequeApprovalResponse!.ForType == "Advance Bill")
                {
                    billChequeApprovalResponse!.ItemReturnOnConsumableLinkString = $"<a href='#' style='font-weight:bold;' onclick='openPopUp(`Reports/FirmPaidReport.aspx?IsAdvance=Y&CallBy={_general.Encrypt(billChequeApprovalResponse!.BRPID!)}`,0,0); return false;'><u>{HttpUtility.HtmlDecode(billChequeApprovalResponse!.SequenceID)}</u></a>";

                    billChequeApprovalResponse?.ItemReturnOnConsumableLinkDetails?.Link = $"Reports/FirmPaidReport.aspx?IsAdvance=Y&CallBy={_general.Encrypt(billChequeApprovalResponse!.BRPID!)}";
                    billChequeApprovalResponse?.ItemReturnOnConsumableLinkDetails?.Text = billChequeApprovalResponse!.SequenceID;
                }


                if (dtAgencyWork.Rows.Count > 0 && dtAgencyWork.Select("BillMappedID=" + billChequeApprovalResponse!.TransID).Length > 0)
                {
                    billChequeApprovalResponse!.ItemReturnOnConsumableLinkString = $"<a href='#' style='font-weight:bold;' onclick='openPopUp(`OtherReports/AbsolutePrint.aspx?TransactionID={_general.Encrypt(billChequeApprovalResponse!.TransID!)}&UName={_general.Encrypt(name)}`,0,0); return false;'><u>{billChequeApprovalResponse!.SequenceID}</u></a>";

                    billChequeApprovalResponse?.ItemReturnOnConsumableLinkDetails?.Link = $"OtherReports/AbsolutePrint.aspx?TransactionID={_general.Encrypt(billChequeApprovalResponse!.TransID!)}&UName={_general.Encrypt(name)}";
                    billChequeApprovalResponse?.ItemReturnOnConsumableLinkDetails?.Text = billChequeApprovalResponse!.SequenceID;
                }



                if (billChequeApprovalResponse!.MyType == "Bills Approval")
                {
                    billChequeApprovalResponse!.ExtraTypeString = billChequeApprovalResponse!.Status;

                    bool mypend = false;

                    if (Array.IndexOf(new string[] { "Waiting For Bills Approval", "Bill Rejected" }, billChequeApprovalResponse!.Status) != -1)
                    {

                        if (billChequeApprovalResponse!.Purpose!.Contains("Exceeded Bill Generated Against"))
                        {
                            billChequeApprovalResponse!.Col5ForeColor = System.Drawing.Color.White;
                            billChequeApprovalResponse!.Col5BackColor = System.Drawing.Color.Green;
                        }


                        using DataTable dtApprovalAuthorities = await _advanceRepository.GetBillApprovalAdvanceBillApprovalAuthorities(billChequeApprovalResponse!.TransID!);
                        if (dtApprovalAuthorities.Rows.Count > 0)
                        {

                            billChequeApprovalResponse!.ApprovalAuthsString = dtApprovalAuthorities.Rows[0][1]?.ToString() ?? string.Empty;
                            if (billChequeApprovalResponse!.ApprovalAuthsString.Contains(employeeId) || (getBillApprovalRequest!.AdditionalEmployeeCode != "" && billChequeApprovalResponse!.ApprovalAuthsString.Contains(getBillApprovalRequest!.AdditionalEmployeeCode!)))
                            {
                                mypend = true;
                            }
                        }

                        billChequeApprovalResponse!.ApprovalAuthsString += $"<span style='color:blue;'>{billChequeApprovalResponse!.BillNameBy}</span><br/>";
                    }

                    if (Array.IndexOf(new string[] { "Bill Rejected" }, billChequeApprovalResponse!.Status) == -1)
                    {
                        using DataTable dtRejections = await _advanceRepository.GetBillApprovalAdvancePreviousRejections(billChequeApprovalResponse!.ForType!, billChequeApprovalResponse!.BillExtra3!);

                        if (dtRejections.Rows.Count > 0 && !string.IsNullOrWhiteSpace(dtRejections.Rows[0][0]?.ToString()))
                        {
                            billChequeApprovalResponse!.RejectionReasonString += $"<br/><font color='red'><font color='blue'><b><u>For Previous Bill Rejected</u></b></font><br/>{dtRejections.Rows[0][0]?.ToString()!.Replace("$", "<br/>")}</font>";
                            billChequeApprovalResponse!.GotRejectedPreviously = true;

                            billChequeApprovalResponse!.RejectionPrintOutDetails!.PreviousBillRejected = dtRejections.Rows[0][0]?.ToString()!;

                        }

                        using DataTable dtExtra7 = await _advanceRepository.GetBillApprovalAdvanceBillBaseExtra7Details(billChequeApprovalResponse!.TransID!);

                        if (dtExtra7.Rows.Count > 0 && !string.IsNullOrWhiteSpace(dtExtra7.Rows[0][0]?.ToString()))
                        {
                            if (billChequeApprovalResponse!.Status == "Waiting For Bills Approval")
                            {
                                billChequeApprovalResponse!.RejectionReasonString += $"<br/><font color='blue'><font color='blue'><b><u>For Bill Approval</u></b></font><br/>{dtExtra7.Rows[0][0]?.ToString()!.Replace("$", "<br/>").Replace("@", "<br/>")}</font>";
                                billChequeApprovalResponse!.RejectionPrintOutDetails!.BillApproval = dtExtra7.Rows[0][0]?.ToString()!;
                            }
                            else
                            {
                                billChequeApprovalResponse!.RejectionReasonString += $"<br/><font color='green'><font color='blue'><b><u>For Bill Approved</u></b></font><br/>{dtExtra7.Rows[0][0]?.ToString()!.Replace("$", "<br/>").Replace("@", "<br/>")}</font>";
                                billChequeApprovalResponse!.RejectionPrintOutDetails!.BillApproved = dtExtra7.Rows[0][0]?.ToString()!;
                            }
                        }


                        using DataTable dtApprovalAuthTimeLimit = await _advanceRepository.GetBillApprovalAdvanceBillApprovalAuthoritiesTimeLimit(billChequeApprovalResponse!.TransID!);
                        if (dtApprovalAuthTimeLimit.Rows.Count > 0)
                        {
                            if (string.IsNullOrWhiteSpace(billChequeApprovalResponse!.MyINICheck))
                            {
                                billChequeApprovalResponse!.Col9BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5555");
                                billChequeApprovalResponse!.Col9ForeColor = System.Drawing.Color.White;

                                billChequeApprovalResponse!.Col6BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5555");
                                billChequeApprovalResponse!.Col6ForeColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                if (billChequeApprovalResponse!.Cond45 != "R")
                                {

                                    if ((Convert.ToDateTime(billChequeApprovalResponse!.Cond45) == DateTime.Now.Date))
                                    {
                                        billChequeApprovalResponse!.DiscountForeColor = System.Drawing.Color.Yellow;
                                    }
                                    else
                                    {
                                        if ((Convert.ToDateTime(billChequeApprovalResponse!.Cond45) < DateTime.Now.Date))
                                        {
                                            billChequeApprovalResponse!.DiscountForeColor = System.Drawing.Color.Red;
                                        }
                                        else
                                        {
                                            billChequeApprovalResponse!.DiscountForeColor = System.Drawing.ColorTranslator.FromHtml("#FF00FF");
                                        }
                                    }
                                }
                                if (Convert.ToDateTime(billChequeApprovalResponse!.MyINICheck).AddDays(Convert.ToInt32(dtApprovalAuthTimeLimit.Rows[0]["Limit"].ToString())) == DateTime.Now.Date)
                                {

                                    billChequeApprovalResponse!.Col9BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                                    billChequeApprovalResponse!.Col6BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                                }

                                if (Convert.ToDateTime(billChequeApprovalResponse!.MyINICheck).AddDays(Convert.ToInt32(dtApprovalAuthTimeLimit.Rows[0]["Limit"].ToString())) < DateTime.Now.Date)
                                {

                                    billChequeApprovalResponse!.Col9BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5555");
                                    billChequeApprovalResponse!.Col9ForeColor = System.Drawing.Color.White;


                                    billChequeApprovalResponse!.Col6BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5555");
                                    billChequeApprovalResponse!.Col6ForeColor = System.Drawing.Color.White;

                                }
                            }
                        }
                        else
                        {
                            billChequeApprovalResponse!.CanApprove = false;
                            billChequeApprovalResponse!.CanReject = false;
                        }

                        if (billChequeApprovalResponse!.Status == "Ready To Issue Amount")
                        {
                            using DataTable dtBillAuthorities = await _advanceRepository.GetBillApprovalAdvanceBillApprovalAuthorities(billChequeApprovalResponse!.TransID!);

                            if (dtBillAuthorities.Rows.Count > 0)
                            {
                                billChequeApprovalResponse!.ReadyToIssueAmountAuthsString += "<br/>" + dtBillAuthorities.Rows[0][1]?.ToString() ?? string.Empty;
                            }

                            billChequeApprovalResponse!.ReadyToIssueAmountAuthsString += $"<br/><span style='color:blue;'>{billChequeApprovalResponse!.BillNameBy}</span><br/>";


                            using DataTable dtAuthoritiesLimit = await _advanceRepository.GetBillApprovalAdvanceApprovalAuthoritiesLimit(billChequeApprovalResponse!.TransID!);

                            if (billChequeApprovalResponse!.Cond45 != "R")
                            {
                                if ((Convert.ToDateTime(billChequeApprovalResponse!.Cond45) == DateTime.Now.Date))
                                {
                                    billChequeApprovalResponse!.DiscountForeColor = System.Drawing.Color.Yellow;
                                }
                                else
                                {
                                    if ((Convert.ToDateTime(billChequeApprovalResponse!.Cond45) < DateTime.Now.Date))
                                    {
                                        billChequeApprovalResponse!.DiscountForeColor = System.Drawing.Color.Red;
                                    }
                                    else
                                    {
                                        billChequeApprovalResponse!.DiscountForeColor = System.Drawing.ColorTranslator.FromHtml("#FF00FF");
                                    }
                                }
                            }
                            if (Convert.ToDateTime(billChequeApprovalResponse!.MyINICheck).AddDays(Convert.ToInt32(dtAuthoritiesLimit.Rows[0]["Limit"].ToString())) == DateTime.Now.Date)
                            {
                                if (Convert.ToInt32(billChequeApprovalResponse!.Paid) != 0 && Convert.ToInt32(billChequeApprovalResponse!.Santioned) > Convert.ToInt32(billChequeApprovalResponse!.Paid))
                                {
                                    billChequeApprovalResponse!.Col9BackColor = System.Drawing.ColorTranslator.FromHtml("#FF8C00");
                                    billChequeApprovalResponse!.Col6BackColor = System.Drawing.ColorTranslator.FromHtml("#FF8C00");
                                }
                                else
                                {
                                    billChequeApprovalResponse!.Col9BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                                    billChequeApprovalResponse!.Col6BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                                }
                            }
                            if (Convert.ToDateTime(billChequeApprovalResponse!.MyINICheck).AddDays(Convert.ToInt32(dtAuthoritiesLimit.Rows[0]["Limit"].ToString())) < DateTime.Now.Date)
                            {
                                if (Convert.ToInt32(billChequeApprovalResponse!.Paid) != 0 && Convert.ToInt32(billChequeApprovalResponse!.Santioned) > Convert.ToInt32(billChequeApprovalResponse!.Paid))
                                {
                                    billChequeApprovalResponse!.Col9BackColor = System.Drawing.ColorTranslator.FromHtml("#FF8C00");
                                    billChequeApprovalResponse!.Col9ForeColor = System.Drawing.Color.White;

                                    billChequeApprovalResponse!.Col6BackColor = System.Drawing.ColorTranslator.FromHtml("#FF8C00");
                                    billChequeApprovalResponse!.Col6ForeColor = System.Drawing.Color.White;
                                }
                                else
                                {
                                    billChequeApprovalResponse!.Col9BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5555");
                                    billChequeApprovalResponse!.Col9ForeColor = System.Drawing.Color.White;

                                    billChequeApprovalResponse!.Col6BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5555");
                                    billChequeApprovalResponse!.Col6ForeColor = System.Drawing.Color.White;
                                }
                            }

                        }

                        if (AllowBillReject && (billChequeApprovalResponse!.Status == "Waiting For Bills Approval" || billChequeApprovalResponse!.Status == "Ready To Issue Amount") && Convert.ToInt32(billChequeApprovalResponse!.Paid) <= 0)
                        {
                            billChequeApprovalResponse!.AllowReject = true;
                        }
                    }
                    else
                    {
                        billChequeApprovalResponse!.CanApprove = false;
                        billChequeApprovalResponse!.CanReject = false;
                    }

                    if (!mypend)
                    {
                        billChequeApprovalResponse!.CanApprove = false;
                        billChequeApprovalResponse!.CanReject = false;
                    }

                    using DataTable dtBillTransactionIssueCol6 = await _advanceRepository.GetBillApprovalAdvanceBillTransactionIssueECol6(billChequeApprovalResponse!.TransID!);
                    if (dtBillTransactionIssueCol6.Rows.Count > 0 && !string.IsNullOrWhiteSpace(dtBillTransactionIssueCol6.Rows[0][0]?.ToString()))
                    {
                        billChequeApprovalResponse!.RejectionReasonString += $"<br/><font color='red'><font color='blue'><b><u>For Cheque Rejected</u></b></font><br/>{dtBillTransactionIssueCol6.Rows[0][0]?.ToString()!.Replace("$", "<br/>")}</font>";
                        billChequeApprovalResponse!.GotRejectedPreviously = true;

                        billChequeApprovalResponse!.RejectionPrintOutDetails!.ChequeRejected = dtBillTransactionIssueCol6.Rows[0][0]?.ToString()!;

                    }
                }
                else
                {
                    using DataTable dtRejections = await _advanceRepository.GetBillApprovalAdvancePreviousRejections(billChequeApprovalResponse!.ForType!, billChequeApprovalResponse!.BillExtra3!);

                    if (dtRejections.Rows.Count > 0)
                    {

                        billChequeApprovalResponse!.RejectionReasonString += $"<br/><font color='red'><font color='blue'><b><u>For Previous Bill Rejected</u></b></font><br/>{dtRejections.Rows[0][0]?.ToString()?.Replace("$", "<br/>")}</font>";

                        billChequeApprovalResponse!.GotRejectedPreviously = true;

                        billChequeApprovalResponse!.RejectionPrintOutDetails!.PreviousBillRejected = dtRejections.Rows[0][0]?.ToString()!;
                    }

                    bool mypend = false;
                    using DataTable dtBillTransactionIssueECol6 = await _advanceRepository.GetBillApprovalAdvanceBillTransactionIssueECol6(billChequeApprovalResponse!.TransID!);
                    if (dtBillTransactionIssueECol6.Rows.Count > 0 && !string.IsNullOrWhiteSpace(dtBillTransactionIssueECol6.Rows[0][0]?.ToString()))
                    {
                        billChequeApprovalResponse!.RejectionReasonString += $"<br/><font color='red'><font color='blue'><b><u>For Cheque Rejected</u></b></font><br/>{dtBillTransactionIssueECol6.Rows[0][0]?.ToString()!.Replace("$", "<br/>")}</font>";
                        billChequeApprovalResponse!.GotRejectedPreviously = true;
                        billChequeApprovalResponse!.RejectionPrintOutDetails!.ChequeRejected = dtBillTransactionIssueECol6.Rows[0][0]?.ToString()!;

                    }

                    if (billChequeApprovalResponse!.Status == "Waiting For Cheque Approval")
                    {
                        using DataTable dtChequeReject = await _advanceRepository.GetBillApprovalAdvanceBillTransactionIssueChequeECol6(billChequeApprovalResponse!.TransID!);
                        if (dtChequeReject.Rows.Count > 0 && !string.IsNullOrWhiteSpace(dtChequeReject.Rows[0][0]?.ToString()))
                        {
                            billChequeApprovalResponse!.RejectionReasonString += $"<br/><font color='green'><font color='blue'><b><u>For Cheque Approved</u></b></font><br/>{dtChequeReject.Rows[0][0]?.ToString()?.Replace("$", "<br/>")?.Replace("@", "<br/>")}</font>";

                            billChequeApprovalResponse!.RejectionPrintOutDetails!.ChequeApproved = dtChequeReject.Rows[0][0]?.ToString()!;
                        }

                        billChequeApprovalResponse!.ExtraTypeString = "Bill Approved";

                        using DataTable dtBillApprovalAuthorities = await _advanceRepository.GetBillApprovalAdvanceBillApprovalAuthorities(billChequeApprovalResponse!.TransID!);
                        if (dtBillApprovalAuthorities.Rows.Count > 0)
                        {
                            billChequeApprovalResponse!.BillApprovedAuthsString += "<br/>" + dtBillApprovalAuthorities.Rows[0][1]?.ToString() ?? string.Empty;
                        }

                        billChequeApprovalResponse!.BillApprovedAuthsString += $"<br/><span style='color:blue;'>{billChequeApprovalResponse!.BillNameBy}</span><br/>";

                        using DataTable dtBillChequeAuthorities = await _advanceRepository.GetBillApprovalAdvanceBillChequeApprovalAuthorities(billChequeApprovalResponse!.TransID!, billChequeApprovalResponse!.SequenceID!);
                        bool rej = false;

                        if (dtBillChequeAuthorities.Rows.Count > 0)
                        {
                            billChequeApprovalResponse!.BillChequeApprovalAuthsString = dtBillChequeAuthorities.Rows[0][1]?.ToString() ?? string.Empty;


                            if (dtBillChequeAuthorities.Rows[0][0].ToString()!.Contains(employeeId) || (getBillApprovalRequest?.AdditionalEmployeeCode != "" && dtBillChequeAuthorities.Rows[0][0]!.ToString()!.Contains(getBillApprovalRequest?.AdditionalEmployeeCode!)))
                            {
                                mypend = true;
                            }

                            if (dtBillChequeAuthorities.Rows[0][2].ToString()!.Replace(",", "").Trim().Length > 0)
                            {
                                rej = true;
                            }
                        }

                        billChequeApprovalResponse!.BillChequeApprovalAuthsString += $"<span style='color:blue;'>{billChequeApprovalResponse!.IssuedName!}</span><br/>";

                        if (!rej)
                        {
                            using DataTable dtApprovalAuthoritiesBillCheckLimit = await _advanceRepository.GetBillApprovalAdvanceBillChequeApprovalAuthoritiesLimit(billChequeApprovalResponse!.TransID!, billChequeApprovalResponse!.SequenceID!);

                            if (dtApprovalAuthoritiesBillCheckLimit.Rows.Count <= 0)
                            {
                                billChequeApprovalResponse!.CanApprove = false;
                                billChequeApprovalResponse!.CanReject = false;
                            }
                            else
                            {
                                if (billChequeApprovalResponse!.Cond45 != "R")
                                {
                                    if ((Convert.ToDateTime(billChequeApprovalResponse!.Cond45) == DateTime.Now.Date))
                                    {
                                        billChequeApprovalResponse!.DiscountForeColor = System.Drawing.Color.Yellow;
                                    }
                                    else
                                    {
                                        if ((Convert.ToDateTime(billChequeApprovalResponse!.Cond45) < DateTime.Now.Date))
                                        {
                                            billChequeApprovalResponse!.DiscountForeColor = System.Drawing.Color.Red;
                                        }
                                        else
                                        {
                                            billChequeApprovalResponse!.DiscountForeColor = System.Drawing.ColorTranslator.FromHtml("#FF00FF");
                                        }
                                    }
                                }
                                if (Convert.ToDateTime(billChequeApprovalResponse!.MyINICheck).AddDays(Convert.ToInt32(dtApprovalAuthoritiesBillCheckLimit.Rows[0]["Limit"].ToString())) == DateTime.Now.Date)
                                {

                                    billChequeApprovalResponse!.Col6BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                                    if (Convert.ToDateTime(billChequeApprovalResponse!.MyEntryCheck).AddDays(Convert.ToInt32(dtApprovalAuthoritiesBillCheckLimit.Rows[0]["Limit"].ToString())) == DateTime.Now.Date)
                                    {
                                        billChequeApprovalResponse!.Col10BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                                    }
                                    else
                                    {
                                        billChequeApprovalResponse!.Col9BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                                    }
                                }
                                if (Convert.ToDateTime(billChequeApprovalResponse!.MyINICheck).AddDays(Convert.ToInt32(dtApprovalAuthoritiesBillCheckLimit.Rows[0]["Limit"].ToString())) < DateTime.Now.Date)
                                {
                                    billChequeApprovalResponse!.Col6BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5555");
                                    billChequeApprovalResponse!.Col6ForeColor = System.Drawing.Color.White;
                                    if (Convert.ToDateTime(billChequeApprovalResponse!.MyINICheck).AddDays(Convert.ToInt32(dtApprovalAuthoritiesBillCheckLimit.Rows[0]["Limit"].ToString())) == DateTime.Now.Date)
                                    {
                                        billChequeApprovalResponse!.Col10BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5555");
                                        billChequeApprovalResponse!.Col10ForeColor = System.Drawing.Color.White;
                                    }
                                    else
                                    {
                                        billChequeApprovalResponse!.Col9BackColor = System.Drawing.ColorTranslator.FromHtml("#FF5555");
                                        billChequeApprovalResponse!.Col9ForeColor = System.Drawing.Color.White;
                                    }
                                }
                            }
                        }
                        else
                        {
                            billChequeApprovalResponse!.ShowRow = false;
                            billChequeApprovalResponse!.CanApprove = false;
                            billChequeApprovalResponse!.CanReject = false;
                        }

                    }

                    if (!mypend)
                    {
                        billChequeApprovalResponse!.CanApprove = false;
                        billChequeApprovalResponse!.CanReject = false;
                    }
                }



                if (billChequeApprovalResponse!.IsSpecial == "True" || billChequeApprovalResponse!.ForType == "Advance Bill")
                {
                    billChequeApprovalResponse!.Col6BackColor = System.Drawing.ColorTranslator.FromHtml("#8CDAFF");
                    billChequeApprovalResponse!.Col6ForeColor = System.Drawing.Color.Black;
                    billChequeApprovalResponse!.Col9BackColor = System.Drawing.ColorTranslator.FromHtml("#8CDAFF");
                    billChequeApprovalResponse!.Col9ForeColor = System.Drawing.Color.Black;
                    billChequeApprovalResponse!.Col10BackColor = System.Drawing.ColorTranslator.FromHtml("#8CDAFF");
                    billChequeApprovalResponse!.Col10ForeColor = System.Drawing.Color.Black;
                }

                using DataTable dtIsBillLate = await _advanceRepository.GetBillApprovalAdvanceIsBillLate(billChequeApprovalResponse!.TransID!);

                if (dtIsBillLate.Rows.Count > 0)
                {
                    billChequeApprovalResponse!.IsBillLate = true;
                }

                if (!string.IsNullOrWhiteSpace(billChequeApprovalResponse!.Col5) && !billChequeApprovalResponse!.Col5!.Contains("---"))
                {
                    billChequeApprovalResponse!.CanOpenFirmPaidReport = true;
                }

                if (billChequeApprovalResponse!.BVId == "262")
                {
                    billChequeApprovalResponse!.CanOpenFirmRejectionReport = true;
                }


                if (!string.IsNullOrWhiteSpace(billChequeApprovalResponse!.ChequeVendor))
                {
                    string NewAddon = "";
                    string[] Splited = billChequeApprovalResponse!.ChequeVendor!.Split('#');

                    if (Splited[3].Trim().ToUpper() != "" && Splited[3].Trim().ToUpper() != "---" && Splited[3].Trim().ToUpper() != billChequeApprovalResponse!.Col5!.ToString().Trim().ToUpper())
                    {
                        NewAddon = NewAddon + "<span style='color:brown; font-weight:bold;'> ( <a href='#'  style='color:brown;' onclick=\"openPopUpEnc('Reports/FirmPaidReport.aspx?VId=$$$&SubId=M','" + Splited[3].Trim().ToUpper() + "" + "'); return false;\"><u>" + Splited[3].Trim().ToUpper() + "</u></a> ) </span><br/>";

                        billChequeApprovalResponse!.ChequeVendorPrintOutDetails!.Link = $"Reports/FirmPaidReport.aspx?VId=$$$&SubId=M','{Splited[3].Trim().ToUpper()}'";
                        billChequeApprovalResponse!.ChequeVendorPrintOutDetails!.Text = Splited[3].Trim().ToUpper();

                    }

                    if (Splited[0] != billChequeApprovalResponse!.BVId)
                    {
                        if (Splited[2].Trim().ToUpper() != "" && !Splited[2].Trim().ToUpper().Contains("---"))
                        {
                            NewAddon = NewAddon + "<b><span><a href='#' style='font-weight:bold;' onclick='openPopUp(`Reports/FirmPaidReport.aspx?VId=" + Splited[0] + "&SubId=" + Splited[2] + "`,0,0); return false;'><u>" + Splited[1] + " (" + Splited[2] + ")</u></a></span></b><br/>";

                            billChequeApprovalResponse!.ChequeVendorPrintOutDetails!.Link = $"Reports/FirmPaidReport.aspx?VId={Splited[0]}&SubId={Splited[2]}";
                            billChequeApprovalResponse!.ChequeVendorPrintOutDetails!.Text = $"{Splited[1]} ({Splited[2]})";

                        }
                        else
                        {
                            NewAddon = NewAddon + "<b><span><a href='#' style='font-weight:bold;' onclick='openPopUp(`Reports/FirmPaidReport.aspx?VId=" + Splited[0] + "&SubId=`,0,0); return false;'><u>" + Splited[1] + "</u></a></span></b><br/>";

                            billChequeApprovalResponse!.ChequeVendorPrintOutDetails!.Link = $"Reports/FirmPaidReport.aspx?VId={Splited[0]}&SubId=";
                            billChequeApprovalResponse!.ChequeVendorPrintOutDetails!.Text = Splited[1];
                        }
                    }

                    if (NewAddon != "")
                    {
                        billChequeApprovalResponse!.ChequeVendorString += $"<hr style='margin: 5px 0px;border: 1px dashed black;border-bottom: 0px;'/><h6 class='no-margin text-bold'>Payment To</h6><hr style='margin: 5px 0px;border: 1px dashed black;border-bottom: 0px;'/>{NewAddon}";
                    }
                }


                billChequeApprovalResponse!.INI = billChequeApprovalResponse!.INI!.Replace("$", "<br/>");

                using DataTable dtAllBillDetails = await _advanceRepository.GetBillApprovalAdvanceAllBillDetails(billChequeApprovalResponse!.TransID!);

                if (dtAllBillDetails.Rows.Count > 0)
                {
                    for (int i = 0; i < dtAllBillDetails.Rows.Count; i++)
                    {
                        BillApprovalBillBaseAllDetails billApprovalBillBaseAllDetails = new BillApprovalBillBaseAllDetails(dtAllBillDetails.Rows[i]);

                        billChequeApprovalResponse!.BillRecords.Add(billApprovalBillBaseAllDetails);
                    }


                    using DataTable dtAllBillDetailsIssue = await _advanceRepository.GetBillApprovalAdvanceAllBillDetailsIssue(billChequeApprovalResponse!.TransID!);

                    foreach (DataRow drPayments in dtAllBillDetailsIssue.Rows)
                    {
                        BillApprovalBillBaseTransactionDetails billApprovalBillBaseTransactionDetails = new BillApprovalBillBaseTransactionDetails(drPayments);

                        using DataTable dtIssueAuthorities = await _advanceRepository.GetBillApprovalAdvanceAllBillDetailsIssueAuthoritiesStatus(billChequeApprovalResponse!.TransID!, billApprovalBillBaseTransactionDetails!.SequenceID!);

                        if (dtIssueAuthorities.Rows.Count <= 0)
                        {
                            if (!string.IsNullOrWhiteSpace(billApprovalBillBaseTransactionDetails.IssuedOn))
                            {
                                billApprovalBillBaseTransactionDetails.Issued = true;
                            }
                            if (!string.IsNullOrWhiteSpace(billApprovalBillBaseTransactionDetails.SignedOn))
                            {
                                billApprovalBillBaseTransactionDetails.Signed = true;
                            }
                            if (!string.IsNullOrWhiteSpace(billApprovalBillBaseTransactionDetails.ReceivedOn))
                            {
                                billApprovalBillBaseTransactionDetails.Recieved = true;
                            }
                            if (!string.IsNullOrWhiteSpace(billApprovalBillBaseTransactionDetails.ClearOn))
                            {
                                billApprovalBillBaseTransactionDetails.Cleared = true;
                            }
                        }
                        else
                        {
                            billApprovalBillBaseTransactionDetails!.RejectString = $"Reject By : {dtIssueAuthorities.Rows[0]["EmployeeDetails"].ToString()}";
                        }

                        billChequeApprovalResponse!.BillRecords[0].TransactionDetails.Add(billApprovalBillBaseTransactionDetails);
                    }


                    using DataTable dtHostelDistribution = await _advanceRepository.GetBillApprovalAdvanceAllBillDetailsIssueHostelDistribution(billChequeApprovalResponse!.TransID!);

                    foreach (DataRow drHostel in dtHostelDistribution.Rows)
                    {
                        BillApprovalIssueHostelWiseDistribution billApprovalIssueHostelWiseDistribution = new BillApprovalIssueHostelWiseDistribution(drHostel);
                        billChequeApprovalResponse!.BillRecords[0].HostelDistributionDetails.Add(billApprovalIssueHostelWiseDistribution);
                    }

                    if (dtHostelDistribution.Rows.Count <= 0)
                    {
                        using DataTable dtVehicleDistribution = await _advanceRepository.GetBillApprovalAdvanceAllBillDetailsIssueVehicleDistribution(billChequeApprovalResponse!.TransID!);

                        foreach (DataRow drVehicle in dtVehicleDistribution.Rows)
                        {
                            BillApprovalIssueVehicleDistribution billApprovalIssueVehicleDistribution = new BillApprovalIssueVehicleDistribution(drVehicle);
                            billChequeApprovalResponse!.BillRecords[0].VehicleDistributionDetails.Add(billApprovalIssueVehicleDistribution);
                        }

                        if (dtVehicleDistribution.Rows.Count > 0)
                        {
                            using DataTable dtVehiclePreviousBills = await _advanceRepository.GetBillApprovalAdvanceAllBillDetailsIssueVehiclePreviousBills(billChequeApprovalResponse!.TransID!);

                            foreach (DataRow drVehicleHistory in dtVehiclePreviousBills.Rows)
                            {
                                BillApprovalVehiclePreviousBills billApprovalVehiclePreviousBills = new BillApprovalVehiclePreviousBills(drVehicleHistory);
                                billChequeApprovalResponse!.BillRecords[0].VehiclePreviousBills.Add(billApprovalVehiclePreviousBills);
                            }
                        }
                    }
                }

                billApprovalResponses.Add(billChequeApprovalResponse!);
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", billApprovalResponses);
        }

        public DateTime convertDatetime(string date)
        {
            return DateTime.ParseExact(date,"MM/dd/yyyy", CultureInfo.InvariantCulture);
        }


        public async Task<ApiResponse> GetChequeAuthority()
        {
            List<TextValues> chequeAuthorityResponses = new List<TextValues>();
            using DataTable dtChequeAuthorities = await _advanceRepository.GetChequeAuth();
            foreach (DataRow dr in dtChequeAuthorities.Rows)
            {
                TextValues chequeAuthorityResponse = new TextValues(dr);
                chequeAuthorityResponses.Add(chequeAuthorityResponse);
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", chequeAuthorityResponses);
        }

        public async Task<ApiResponse> GetPayentDetails()
        {
            using(DataTable dt=await _advanceRepository.PaymentDetails())
            {
                List<TextValues> paymentDetailsResponses = new List<TextValues>();
                foreach(DataRow dr in dt.Rows)
                {
                    TextValues paymentDetailsResponse = new TextValues(dr);
                    paymentDetailsResponses.Add(paymentDetailsResponse);
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", paymentDetailsResponses);
            }
        }


        public async Task<ApiResponse> SaveChequeDetails(string EmpCode, string Type,string EmpName, SaveCheDetailsRequest req)
        {
            int iss = 0;
            if(req.PaymentType=="Full" || req.PaymentType=="Custom")
            {
                int.TryParse(req.IssuedAmount, out iss);
            }
            if(req.PaymentType=="Installment")
            {
                int over = 0;
                int.TryParse(req.IssuedAmount, out iss);
                int.TryParse(req.Remaining, out over);
                if (iss == 0 || over == 0)
                {
                    iss = 0;
                }
                else
                {
                    iss = (over / iss);
                }
            }
            if(req.PaymentType=="Percentage")
            {
                int over = 0;
                int.TryParse(req.IssuedAmount, out iss);
                int.TryParse(req.Remaining, out over);

                if (iss == 0 || over == 0)
                {
                    iss = 0;
                }
                else
                {
                    iss = ((over * iss) / 100);
                }
            }
            req.IssuedAmount= iss.ToString();
            DataTable TransNo=await _advanceRepository.GetTransactionNo(req.TransId??"");

            req.SequenceId= TransNo.Rows[0][0].ToString();
            //update bill base amount
            int updatebillbase = await _advanceRepository.UpdateBillBase(EmpCode,EmpName,iss.ToString(),req.TransId);

            //insert details
            int ins = await _advanceRepository.InsertBillTransactionIssue(EmpCode, EmpName, req);

            //insert into approval authority

            int insauth = await _advanceRepository.UploadChequeApproval(EmpCode, EmpName, req);

            //save file
            string ParentPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Path.Combine(ParentPath, "Upload_Bills")))
            {
                Directory.CreateDirectory(Path.Combine(ParentPath, "Upload_Bills"));
            }
            if (req.PdfFile!=null && req.PdfFile.Length>0)
            {
               if(File.Exists(Path.Combine(ParentPath, "Upload_Bills", req.TransId + "_" + req.SequenceId + ".pdf")))
                {
                    File.Delete(Path.Combine(ParentPath, "Upload_Bills", req.TransId + "_" + req.SequenceId + ".pdf"));
                }
                string davePdf = await _inclusiveService.SaveFile(req.TransId + "_" + req.SequenceId, "Upload_Bills", req.PdfFile, ".pdf");
            }
            if(req.ExcelFile!=null && req.ExcelFile.Length>0)
            {
                if (File.Exists(Path.Combine(ParentPath, "Upload_Bills", req.TransId + "_" + req.SequenceId + ".xlsx")))
                {
                    File.Delete(Path.Combine(ParentPath, "Upload_Bills", req.TransId + "_" + req.SequenceId + ".xlsx"));
                }
                string davePdf = await _inclusiveService.SaveFile(req.TransId + "_" + req.SequenceId, "Upload_Bills", req.ExcelFile, ".xlsx");
            }
            int saveFileRecord = await _advanceRepository.SaveFileCheque(EmpCode,EmpName,req);
            return new ApiResponse(StatusCodes.Status200OK, "Success", "Cheque Upload Successfully");
        }


        public async Task<ApiResponse> GetOtherTransactionDetails(string EmpCode, string AddEmpCode, string TransId, string SeqId)
        {
            //Task<DataTable> GetOtherApproval(string EmpCode,string AddEmpCode,string TransId,string SeqId)
            List<OtherTransactionDetailsResponse> otherTransactionDetailsResponses = new List<OtherTransactionDetailsResponse>();
            using (DataTable dt = await _advanceRepository.GetOtherApproval(EmpCode, AddEmpCode, TransId, SeqId))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OtherTransactionDetailsResponse otherTransactionDetailsResponse = new OtherTransactionDetailsResponse(dr);
                    otherTransactionDetailsResponses.Add(otherTransactionDetailsResponse);
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", otherTransactionDetailsResponses);
            }

        }

        public async Task<ApiResponse> ApprovedTransection(string EmpCode,string EmpAddCode,string EmpName,string TransactionId,string SeqId,string Remark,string Designation)
        {
            if (SeqId == "---")
            {
                int UpdateAppAuth=await _advanceRepository.UpdateApprovalAuthorityWithOutSeqNo(EmpCode,EmpAddCode, EmpName, TransactionId, Designation);
                if(UpdateAppAuth>0)
                {
                    using (DataTable PendingAuth = await _advanceRepository.GetPendingAuthority(TransactionId))
                    {
                        if(PendingAuth.Rows.Count <= 0)
                        {
                            int UpdateStatusWithRemark = await _advanceRepository.UpdateBillStatus( EmpName, Remark, TransactionId);
                        }
                        else
                        {
                            if(!string.IsNullOrEmpty(Remark))
                            {
                                int UpdtaeRemarkOnly = await _advanceRepository.UpdateBillReason(EmpName,Remark,TransactionId);
                            }
                        }
                    }
                }
            }
            else
            {
                int UpdateAuth=await _advanceRepository.ApprovalAuthWithSeqNo(EmpCode,EmpAddCode, EmpName, Designation, TransactionId, SeqId);

                using(DataTable dt=await _advanceRepository.GetCequePendingAuthority(TransactionId,SeqId))
                {
                    if(dt.Rows.Count <= 0)
                    {
                        // int UpdateStatusWithRemark = await _advanceRepository.UpdateBillStatus( EmpName, Remark, TransactionId);

                        string sms = "Waiting To Send";
                        using DataTable SmsDetail = await _advanceRepository.GetSmsDetails(TransactionId);
                        if (SmsDetail.Rows.Count > 0)
                        {
                            if (SmsDetail.Rows[0][0].ToString() == "No")
                            {
                                sms = "SMS Blocked";
                            }
                        }
                        //bill receive
                        int receive = await _advanceRepository.UpdateTransactionIssued(sms, TransactionId, SeqId, Remark, EmpName);

                        //update schedule
                        int scheduled = await _advanceRepository.UpdateScheduled(TransactionId);

                    }
                    else
                    {
                        int updateapp = await _advanceRepository.UpdateBillTransactionIssuedTblAllAuthApproved(EmpName, Remark, TransactionId, SeqId);
                    }

                    //in ready To issues Amount
                    using DataTable PendingReject=await _advanceRepository.GetPendingRejectedRecord(TransactionId);
                    if (PendingReject.Rows.Count <= 0)
                    {
                        int REadyToissueAmount = await _advanceRepository.UpdateREadyToIssueAmount(TransactionId);
                    }
                }

            }
            //approve applicationnew
            int ApproveApplication = await _advanceRepository.ApproveApplication(EmpCode, TransactionId);
            return new ApiResponse(StatusCodes.Status200OK, "Success", "Transaction Approved Successfully");
        }

        public async Task<ApiResponse> BillRejectApproval(string EmpCode,string EmpAddCode,string EmpName, ApprovalRequest req)
        {
            if(req.SeqNo=="---")
            {
                //update bill_transaction_issue
                int UpdateRecord = await _advanceRepository.UpdateRejectRecord(EmpCode,EmpName,EmpAddCode,req.Designation!,req.TransactionId!);
                //update reject billbase
                int RejectBill = await _advanceRepository.UpdateBillBaseReject(req.TransactionId!,req.Remark!,EmpName);

            }
            else
            {
                int UpdateRecord = await _advanceRepository.UpdateAppAuthWithSeq(EmpCode,EmpAddCode,req.Designation,EmpName,req.TransactionId,req.SeqNo);
                //update bill Transa issue
                int Updatedetails = await _advanceRepository.UpdateBillTransactionIssueReject(EmpCode,req.TransactionId,req.SeqNo,req.Remark??"",EmpName);

                //updtae readyto issue
                int upd = await _advanceRepository.UpdateBillRejectStatus(req.TransactionId!);

                //update bill base balance
                int amount_update = await _advanceRepository.BillAmountUpdate(req.TransactionId,req.SeqNo,EmpName);
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", "Record Update Successfully");
        }

        public async Task<ApiResponse> DeleteCheque(string EmpName,string EmpCode,string TransId,string SeqId)
        {
            //update bill base
            int updateBillBase= await _advanceRepository.UpdateAmount(EmpName,TransId,SeqId);

            //add backup
            int insBackup= await _advanceRepository.InsertInBackUpDeleteCheque(EmpName,TransId,SeqId);
            //delete record
            int delete=await _advanceRepository.DeleteChequeTransaction(TransId,SeqId);

            return new ApiResponse(StatusCodes.Status200OK, "Success", "Cheque Deleted Successfully");
        }


        public async Task<ApiResponse> GetTimeLineDetails(string RefNo,string Type)
        {
            GetTimeLineResponse res=new GetTimeLineResponse();
            string BillId="";
            //Purchase
            using (DataTable dt=await _advanceRepository.Purchasetime(RefNo))
            {
                if(dt.Rows.Count>0)
                {
                    int TotalApprovalAuth = 0;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App1DoneOn"].ToString()) && dt.Rows[0]["App1DoneOn"].ToString().Length > 0) TotalApprovalAuth++;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App2DoneOn"].ToString()) && dt.Rows[0]["App2DoneOn"].ToString().Length > 0) TotalApprovalAuth++;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App3DoneOn"].ToString()) && dt.Rows[0]["App3DoneOn"].ToString().Length > 0) TotalApprovalAuth++;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App4DoneOn"].ToString()) && dt.Rows[0]["App4DoneOn"].ToString().Length > 0) TotalApprovalAuth++;

                    int TotalAuth = 0;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App1ID"].ToString()) && dt.Rows[0]["App1ID"].ToString().Length > 0) TotalAuth++;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App2ID"].ToString()) && dt.Rows[0]["App2ID"].ToString().Length > 0) TotalAuth++;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App3ID"].ToString()) && dt.Rows[0]["App3ID"].ToString().Length > 0) TotalAuth++;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App4ID"].ToString()) && dt.Rows[0]["App4ID"].ToString().Length > 0) TotalAuth++;

                    PurchaseDetails purchasedetails = new PurchaseDetails();
                    purchasedetails.Amount= dt.Rows[0]["Amount"].ToString();
                    purchasedetails.Status= dt.Rows[0]["Status"].ToString();
                    purchasedetails.TotalItem= dt.Rows[0]["TotalItem"].ToString();
                    purchasedetails.TotalApproved= TotalApprovalAuth;
                    purchasedetails.BillStatus = dt.Rows[0]["ReferenceBillStatus"].ToString();
                    purchasedetails.ReferenceNo = dt.Rows[0]["ReferenceNo"].ToString();
                    purchasedetails.TotalAuth= TotalAuth;
                    purchasedetails.MyType= dt.Rows[0]["MyType"].ToString();
                    BillId+= dt.Rows[0]["BillId"].ToString();
                    res.purchase = purchasedetails;
                }
            }
            using(DataTable dt= await _advanceRepository.AdvanceSummary(RefNo,Type))
            {
                if (dt.Rows.Count > 0)
                {

                    int TotalApprovalAuth = 0;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App1DoneOn"].ToString()) && dt.Rows[0]["App1DoneOn"].ToString().Length > 0) TotalApprovalAuth++;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App2DoneOn"].ToString()) && dt.Rows[0]["App2DoneOn"].ToString().Length > 0) TotalApprovalAuth++;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App3DoneOn"].ToString()) && dt.Rows[0]["App3DoneOn"].ToString().Length > 0) TotalApprovalAuth++;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App4DoneOn"].ToString()) && dt.Rows[0]["App4DoneOn"].ToString().Length > 0) TotalApprovalAuth++;

                    int TotalAuth = 0;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App1ID"].ToString()) && dt.Rows[0]["App1ID"].ToString().Length > 0) TotalAuth++;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App2ID"].ToString()) && dt.Rows[0]["App2ID"].ToString().Length > 0) TotalAuth++;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App3ID"].ToString()) && dt.Rows[0]["App3ID"].ToString().Length > 0) TotalAuth++;
                    if (!string.IsNullOrEmpty(dt.Rows[0]["App4ID"].ToString()) && dt.Rows[0]["App4ID"].ToString().Length > 0) TotalAuth++;

                    List<PurchaseDetails> purchasedetailslst = new List<PurchaseDetails>();
                    for(int i=0;i<dt.Rows.Count;i++)
                    {
                        PurchaseDetails purchasedetails=new PurchaseDetails();
                        purchasedetails.Amount = dt.Rows[i]["Amount"].ToString();
                        purchasedetails.Status = dt.Rows[i]["Status"].ToString();
                        purchasedetails.ReferenceNo = dt.Rows[i]["ReferenceNo"].ToString();
                        //purchasedetails.TotalApproved = TotalApprovalAuth;
                        //purchasedetails.TotalAuth = TotalAuth;

                        purchasedetails.MyType = dt.Rows[0]["MyType"].ToString();
                        BillId += dt.Rows[0]["BillId"].ToString();
                        purchasedetailslst.Add(purchasedetails);
                    }
                    
                    res.Advance = purchasedetailslst;
                }
            }
            if(Type=="Bill")
            {
                BillId = RefNo;
            }
            using(DataTable dt=await _advanceRepository.BillSummary(BillId))
            {
                if(dt.Rows.Count> 0 && dt.Rows[0]["Amount"].ToString() != "0")
                {
                    List<PurchaseDetails> bild = new List<PurchaseDetails>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        PurchaseDetails purchasedetails = new PurchaseDetails();
                        purchasedetails.Amount = dt.Rows[i]["Amount"].ToString();
                        //purchasedetails.TotalApproved = Convert.ToInt32(dt.Rows[0]["Count"].ToString());
                        purchasedetails.MyType = dt.Rows[i]["TransId"].ToString();
                        purchasedetails.IssuedAmount = dt.Rows[i]["IssuedAmount"].ToString();
                        purchasedetails.Status = dt.Rows[i]["Status"].ToString();
                        bild.Add(purchasedetails);
                    }
                    res.Bill = bild;
                }
            }
            using(DataTable dt=await _advanceRepository.ChequeDetails(BillId))
            {
                if(dt.Rows.Count>0  && dt.Rows[0]["Amount"].ToString()!="0")
                {
                    PurchaseDetails purchasedetails = new PurchaseDetails();
                    purchasedetails.Amount = dt.Rows[0]["Amount"].ToString();
                    purchasedetails.TotalApproved =Convert.ToInt32(dt.Rows[0]["Count"].ToString());
                    purchasedetails.MyType = dt.Rows[0]["TransId"].ToString();
                    res.Cheque = purchasedetails;
                }
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", res);
        }

        public async Task<ApiResponse> BillDetails(string BillId)
        {
            HashSet<BillDetailsResponse1> res=new HashSet<BillDetailsResponse1>();
            using (DataTable dt = await _advanceRepository.BillDetaild(BillId))
            {
                foreach (DataRow row in dt.Rows)
                {
                    BillDetailsResponse1 billdetail = new BillDetailsResponse1(row);
                    List<BillAutority> aut=new List<BillAutority>();
                    DataTable AuthList = await _advanceRepository.GetAuthority(row["TransactionID"].ToString() ?? "");
                    foreach(DataRow auth in AuthList.Rows)
                    {
                        aut.Add(new BillAutority(auth));
                    }
                    billdetail.auth= aut;
                    res.Add(billdetail);
                }

            }
            return new ApiResponse(StatusCodes.Status200OK,"Success",res);
        }

        public async Task<ApiResponse> GetChequeDetails(string BillId)
        {
            HashSet<ChequeResponse> lst = new HashSet<ChequeResponse>();
            using(DataTable dt=await _advanceRepository.GetCheque(BillId))
            {
                foreach (DataRow row in dt.Rows)
                {
                    ChequeResponse res = new ChequeResponse(row);
                    List<BillAutority> aut = new List<BillAutority>();
                    DataTable AuthList = await _advanceRepository.GetChequeAuthority(row["TransactionID"].ToString() ?? "");
                    foreach (DataRow auth in AuthList.Rows)
                    {
                        aut.Add(new BillAutority(auth));
                    }
                    res.auth= aut;
                    lst.Add(res);
                }

                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }
        }



    }
}
