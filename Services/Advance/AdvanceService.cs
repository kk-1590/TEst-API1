using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Advance;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Advance;
using AdvanceAPI.IServices.Inclusive;
using MySqlX.XDevAPI;
using Org.BouncyCastle.Ocsp;
using System.Collections.Generic;
using System.Data;

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



            req.ApprovalType = "Against Purchase Approval Advance Form";
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
            using (DataTable dt = await _advanceRepository.GetBackValue())
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

            //Task<int> SaveAdvance(GenerateAdvancerequest req, string EmpCode, string EmpName, string RefNo,string FirmName,string FirmPerson,string FirmEmail,string FirmPanNo,string FirmAddress,string FirmContactNo,string FirmAlternateContactNo,string BackValue,string PurchaseApprovalDetails)
            int ins = await _advanceRepository.SaveAdvance(req, EmpCode, EmpName, RefrenceNo, fname, fcontcatname, femail, "", fadd, fcno, falter, PurChaseBackValue, PurchaseApprovalDetails);

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
                        Value = dt.Rows[0]["App3ID"].ToString()
                    });
                    App1List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App2Name"].ToString() + " - " + dt.Rows[0]["App2Designation"].ToString(),
                        Value = dt.Rows[0]["App2ID"].ToString()
                    });
                    App1List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App1Name"].ToString() + " - " + dt.Rows[0]["App1Designation"].ToString(),
                        Value = dt.Rows[0]["App1ID"].ToString()
                    });
                    App2List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App3Name"].ToString() + " - " + dt.Rows[0]["App3Designation"].ToString(),
                        Value = dt.Rows[0]["App3ID"].ToString()
                    });
                    App2List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App2Name"].ToString() + " - " + dt.Rows[0]["App2Designation"].ToString(),
                        Value = dt.Rows[0]["App2ID"].ToString()
                    });
                    App2List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App1Name"].ToString() + " - " + dt.Rows[0]["App1Designation"].ToString(),
                        Value = dt.Rows[0]["App1ID"].ToString()
                    });

                }
                if (dt.Rows[0]["App2ID"].ToString()!.Length > 0)
                {
                    App1List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App2Name"].ToString() + " - " + dt.Rows[0]["App2Designation"].ToString(),
                        Value = dt.Rows[0]["App2ID"].ToString()
                    });
                    App1List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App1Name"].ToString() + " - " + dt.Rows[0]["App1Designation"].ToString(),
                        Value = dt.Rows[0]["App1ID"].ToString()
                    });
                    App2List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App2Name"].ToString() + " - " + dt.Rows[0]["App2Designation"].ToString(),
                        Value = dt.Rows[0]["App2ID"].ToString()
                    });
                    App2List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App1Name"].ToString() + " - " + dt.Rows[0]["App1Designation"].ToString(),
                        Value = dt.Rows[0]["App1ID"].ToString()
                    });
                }
                else
                {
                    App1List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App1Name"].ToString() + " - " + dt.Rows[0]["App1Designation"].ToString(),
                        Value = dt.Rows[0]["App1ID"].ToString()
                    });
                    App2List.Add(new TextValues
                    {
                        Text = dt.Rows[0]["App1Name"].ToString() + " - " + dt.Rows[0]["App1Designation"].ToString(),
                        Value = dt.Rows[0]["App1ID"].ToString()
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
            string[] splt = Values.Split('#');
            using (DataTable dt = await _advanceRepository.CheckPreviousPendingBill(splt[0]))
            {
                if (dt.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status200OK, "Warning", "Sorry! Previous Advance Approval Is Still Pending To Approve For Same Purchase Approval Initiated By " + dt.Rows[0]["IniName"].ToString() + " on " + dt.Rows[0]["AppDate"].ToString() + " of Amount : " + dt.Rows[0]["Amount"].ToString() + " Rs/-");
                }
                DataTable d = await _advanceRepository.GetVendorTextValue(splt[1]);
                List<TextValues> VendorDetails = new List<TextValues>();
                foreach (DataRow row in d.Rows)
                {
                    VendorDetails.Add(new TextValues(row));
                }
                DataTable dts = await _advanceRepository.GetDepartDetails(Values, Type);
            }

            return new ApiResponse(StatusCodes.Status200OK,"Success","");
        }
    }

    
}
