using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Advance;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.DTO.Inclusive;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Advance;
using AdvanceAPI.IServices.Inclusive;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Ocsp;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace AdvanceAPI.Services.Advance
{
    public class AdvanceService : IAdvanceService
    {
        private IAdvanceRepository _advanceRepository;
        private IGeneral _general;
        private readonly IInclusiveService _inclusiveService;
        public AdvanceService(IAdvanceRepository advanceRepository, IGeneral general, IInclusiveService inclusiveService) {
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



            req.ApprovalType = req.ApprovalType+" Advance Form";
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
            int ins = await _advanceRepository.SaveAdvance(req, EmpCode, EmpName, RefrenceNo, fname, fcontcatname, femail, "", fadd, fcno, falter, PurChaseBackValue, PurchaseApprovalDetails,RefNo);

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
                    using (DataTable ds= await _advanceRepository.GetrefnoForGenerateBillAdvance(Type, EmpCode, dr["ReferenceNo"].ToString()))
                    {
                        MyReq.CanUploadBill=ds.Rows.Count > 0;
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
           
          
            return new ApiResponse(StatusCodes.Status200OK,"Success",details);
        }

        public async Task<ApiResponse> SaveBill(string EmpCode,string EmpName, AddBillGenerateRequest req,string Type)
        {
            if(req.ForTypeOf=="Advance Bill")
            {
                DataTable cann = await _advanceRepository.GetrefnoForGenerateBillAdvance(Type,EmpCode,req.RefNo!);
                if(cann.Rows.Count<=0)
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
                DataTable dt = await _advanceRepository.CanGenerateBill(Type, EmpCode, Convert.ToInt32(req.Amount) > 1000 ? true : false, req.RefNo!);
                if (dt.Rows.Count <= 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Invalid Access");
                }
            }


                string BillRefNo = "";
            using (DataTable GetBillBaseRefNo = await _advanceRepository.GetBillBaseRefNo()) 
            {
                if(GetBillBaseRefNo.Rows.Count>0)
                {
                    string myid = GetBillBaseRefNo.Rows[0][0].ToString()??string.Empty;
                    string NewId = GetBillBaseRefNo.Rows[0][0].ToString()??string.Empty;
                     BillRefNo = GetBillBaseRefNo.Rows[0][0].ToString()??string.Empty;
                }
                //Task<int> SaveBillAuth(string EmpCode, string EmpName, AddBillGenerateRequest req, string BillBaseREfNo)
                //Task<int> SaveBillSave(string EmpCode,string EmpName, AddBillGenerateRequest req,string BillBaseREfNo)
                int saverecord = await _advanceRepository.SaveBillSave(EmpCode,EmpName,req,BillRefNo);
                int SaveAuth = await _advanceRepository.SaveBillAuth(EmpCode,EmpName,req,BillRefNo);



                //FileName = _general.EncryptWithKey(FileName, await _inclusiveService.GetEnCryptedKey());
                //string str = await _inclusiveService.SaveFile(FileName, FilePath, file, ".pdf");
                //check pdf file and delete
                string FilePath = Directory.GetCurrentDirectory();
                FilePath = Path.Combine(FilePath, "Upload_Bills");
                string FileName = BillRefNo + ".pdf";
                
                if(!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }
                if (req.pdf!=null && req.pdf.Length > 0)
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
            DateLockResponse lst=new DateLockResponse();
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
            using (DataTable dt = await _advanceRepository. GetPurchaseBasicForBillAgainstAdvance(RefNo))
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
                lst.ExtraValues= dt.Rows[0]["PExtra3"].ToString() ?? ""; ;

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

            if(CampusCode!="101")
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

                return new ApiResponse(StatusCodes.Status200OK, "Success", new {app3List= lst1, app4List= lst2, app1List= lst3, app2List= lst4 });
        }
        public async Task<ApiResponse> GetPurchaseApprovalBill(string EmpCode, string Type, GetApprovalBillRequest req)
        {
            //Task<DataTable> GetApprovalBill(string EmpCode,string Type, GetApprovalBillRequest req)
            List< BillApprovalResponse > lst=new List<BillApprovalResponse> ();
            using (DataTable dt = await _advanceRepository.GetApprovalBill(EmpCode,Type,req))
            {
                DataTable mytab = new DataTable();
                mytab=dt.Clone();
                mytab.Rows.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BillApprovalResponse ls=new BillApprovalResponse();
                    if (Convert.ToInt32(dt.Rows[i]["AmountRemaining"].ToString()) <= 0)
                    {
                        DataTable dts = await _advanceRepository.GetIssuedAmount(dt.Rows[i]["TransactionID"].ToString()??"");

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
                    if (mytab.Rows[i]["BillExtra6"].ToString()!.Length>0)
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
                            ls.CanEditDate=true;
                        }
                        DataTable AuthDetails = await _advanceRepository.GetApprovalAuthority(mytab.Rows[i]["Trans. ID"].ToString());
                        List<Authorities> authorities = new List<Authorities>();
                        foreach(DataRow dr in AuthDetails.Rows)
                        {
                            authorities
                                .Add(new Authorities
                                {
                                    Name = dr["EmployeeDetails"].ToString(),
                                    On = dr["DB"].ToString(),
                                    Status = dr["Status"].ToString(),
                                    EmployeeId = dr["EmployeeID"].ToString(),
                                    Img =await _inclusiveService.GetEmployeePhotoUrl(dr["EmployeeID"].ToString()??"")
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
                        using (DataTable auth=await _advanceRepository.GetIssuedAmounREadyApproval(dt.Rows[i]["TransactionID"].ToString() ?? ""))
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
            if(Convert.ToDateTime(req.NewAppDate)!=Convert.ToDateTime(req.OldAppDate))
            {
               store+= "Ini. Dt. : " + req.OldAppDate.Replace("-",".") + "->" + req.NewAppDate.Replace("-",".") + ", ";
               Update += " Col2=@NewAppDate,";
                param.Add(new SQLParameters("@NewAppDate", req.NewAppDate));
            }
            if(Convert.ToDateTime(req.NewBillDate)!= Convert.ToDateTime(req.OldBillDate))
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
            if(store.Length>0)
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
               int ins=await _advanceRepository.UpdateRemark(req.NewPurpose ?? "", req.TransId ?? "");
            }
            return new ApiResponse(StatusCodes.Status200OK, "Success", "Record Update Successfully");

        }

    }

    
}
