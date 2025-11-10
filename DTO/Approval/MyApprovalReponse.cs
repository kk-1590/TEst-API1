using System.Data;

namespace AdvanceAPI.DTO.Approval
{
    public class MyApprovalReponse
    {
        public string? PreviousCancelRemark { get; set; } = string.Empty;
        public string? IsReGenerated { get; set; } = string.Empty;
        public string? RejectBy { get; set; } = string.Empty;
        public string? RejectReason { get; set; } = string.Empty;
        public string? RelativePersonID { get; set; } = string.Empty;
        public string? RelativePersonName { get; set; } = string.Empty;
        public string? ReferenceNo { get; set; } = string.Empty;
        public string? Session { get; set; } = string.Empty;
        public string? CampusName { get; set; } = string.Empty;
        public string? Purpose { get; set; } = string.Empty;
        public string? TotalItem { get; set; } = string.Empty;
        public string? BillDateExtendedShow1 { get; set; } = string.Empty;
        public string? TotalAmount { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public string? InitiatedByName { get; set; } = string.Empty;
        public string? AppDate { get; set; } = string.Empty;
        public string? InitiatedOn { get; set; } = string.Empty;
        public string? App1Name { get; set; } = string.Empty;
        public string? App2Name { get; set; } = string.Empty;
        public string? App3Name { get; set; } = string.Empty;
        public string? App4Name { get; set; } = string.Empty;
        public string? App1Status { get; set; } = string.Empty;
        public string? App2Status { get; set; } = string.Empty;
        public string? App3Status { get; set; } = string.Empty;
        public string? App4Status { get; set; } = string.Empty;
        public string? App1On { get; set; } = string.Empty;
        public string? App2On { get; set; } = string.Empty;
        public string? App3On { get; set; } = string.Empty;
        public string? App4On { get; set; } = string.Empty;
        public string? CancelledReason { get; set; } = string.Empty;
        public string? CancelledOn { get; set; } = string.Empty;
        public string? CancelledBy { get; set; } = string.Empty;
        public string? CloseReason { get; set; } = string.Empty;
        public string? CloseOn { get; set; } = string.Empty;
        public string? CloseBy { get; set; } = string.Empty;
        public string? FinalStatus { get; set; } = string.Empty;
        public string? BudgetRequired { get; set; } = string.Empty;
        public string? BudgetAmount { get; set; } = string.Empty;
        public string? PreviousTaken { get; set; } = string.Empty;
        public string? CurStatus { get; set; } = string.Empty;
        public string? BudgetStatus { get; set; } = string.Empty;
        public string? BudgetReferenceNo { get; set; } = string.Empty;
        public string? BillId { get; set; } = string.Empty;
        public string? BillRequired { get; set; } = string.Empty;
        public string? VendorId { get; set; } = string.Empty;
        public string? Department { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public string? DBBillExtendedDate { get; set; } = string.Empty;
        public string? AdditionalName { get; set; } = string.Empty;
        public string? ApprovalCategory { get; set; } = string.Empty;
        public string? Authorities { get; set; } = string.Empty;
        public string? ApprovalDate { get; set; } = string.Empty;
        public string? BillDateExtendedShow2 { get; set; } = string.Empty;

        public string? VendorName { get; set; } = string.Empty;
        public string? VendorPersonaName { get; set; } = string.Empty;
        public string? VendorPersonaContactNo { get; set; } = string.Empty;
        public string? ApprovalBillStatus { get; set; } = string.Empty;
        public string? ReceivingStatus { get; set; } = string.Empty;
        public string? LastReceiveOn { get; set; } = string.Empty;
        public string? CampusCode { get; set; } = string.Empty;
        public string? Maad { get; set; } = string.Empty;



        public bool? CanDeleteApproval { get; set; } = false;
        public bool? CanRegenerateApproval { get; set; } = false;
        public bool? CanEditApproval { get; set; } = false;
        public bool? OpenComparisionChart { get; set; } = false;
        public bool? CanLockBillStatus { get; set; } = false;
        public bool? CanOpenExcel { get; set; } = false;

        public bool? CanEditItems { get; set; } = false;

        public MyApprovalReponse()
        {

        }

        public MyApprovalReponse(DataRow dr)
        {
            PreviousCancelRemark = dr["PreviousCancelRemark"]?.ToString() ?? String.Empty;
            IsReGenerated = dr["ReGenerated"]?.ToString() ?? String.Empty;
            RejectBy = dr["RejectBy"]?.ToString() ?? String.Empty;
            RejectReason = dr["RejectReason"]?.ToString() ?? String.Empty;
            RelativePersonID = dr["RelativePersonID"]?.ToString() ?? String.Empty;
            RelativePersonName = dr["RelativePersonName"]?.ToString() ?? String.Empty;
            ReferenceNo = dr["ReferenceNo"]?.ToString() ?? String.Empty;
            Session = dr["Session"]?.ToString() ?? String.Empty;
            CampusName = dr["CampusName"]?.ToString() ?? String.Empty;
            Purpose = dr["Purpose"]?.ToString() ?? String.Empty;
            TotalItem = dr["TotalItem"]?.ToString() ?? String.Empty;
            BillDateExtendedShow1 = dr["ExeOn"]?.ToString() ?? String.Empty;
            TotalAmount = dr["TotalAmount"]?.ToString() ?? String.Empty;
            Status = dr["Status"]?.ToString() ?? String.Empty;
            InitiatedByName = dr["IniName"]?.ToString() ?? String.Empty;
            AppDate = dr["AppDate"]?.ToString() ?? String.Empty;
            InitiatedOn = dr["IniOn"]?.ToString() ?? String.Empty;
            App1Name = dr["App1Name"]?.ToString() ?? String.Empty;
            App2Name = dr["App2Name"]?.ToString() ?? String.Empty;
            App3Name = dr["App3Name"]?.ToString() ?? String.Empty;
            App4Name = dr["App4Name"]?.ToString() ?? String.Empty;
            App1Status = dr["App1Status"]?.ToString() ?? String.Empty;
            App2Status = dr["App2Status"]?.ToString() ?? String.Empty;
            App3Status = dr["App3Status"]?.ToString() ?? String.Empty;
            App4Status = dr["App4Status"]?.ToString() ?? String.Empty;
            App1On = dr["App1On"]?.ToString() ?? String.Empty;
            App2On = dr["App2On"]?.ToString() ?? String.Empty;
            App3On = dr["App3On"]?.ToString() ?? String.Empty;
            App4On = dr["App4On"]?.ToString() ?? String.Empty;
            CancelledReason = dr["CancelledReason"]?.ToString() ?? String.Empty;
            CancelledOn = dr["CancelledOn"]?.ToString() ?? String.Empty;
            CancelledBy = dr["CancelledBy"]?.ToString() ?? String.Empty;
            CloseReason = dr["CloseReason"]?.ToString() ?? String.Empty;
            CloseOn = dr["CloseOn"]?.ToString() ?? String.Empty;
            CloseBy = dr["CloseBy"]?.ToString() ?? String.Empty;
            FinalStatus = dr["FinalStat"]?.ToString() ?? String.Empty;
            BudgetRequired = dr["BudgetRequired"]?.ToString() ?? String.Empty;
            BudgetAmount = dr["BudgetAmount"]?.ToString() ?? String.Empty;
            PreviousTaken = dr["PreviousTaken"]?.ToString() ?? String.Empty;
            CurStatus = dr["CurStatus"]?.ToString() ?? String.Empty;
            BudgetStatus = dr["BudgetStatus"]?.ToString() ?? String.Empty;
            BudgetReferenceNo = dr["BudgetReferenceNo"]?.ToString() ?? String.Empty;
            BillId = dr["BillId"]?.ToString() ?? String.Empty;
            BillRequired = dr["BillRequired"]?.ToString() ?? String.Empty;
            VendorId = dr["VendorID"]?.ToString() ?? String.Empty;
            Department = dr["ForDepartment"]?.ToString() ?? String.Empty;
            Note = dr["Note"]?.ToString() ?? String.Empty;
            Type = dr["MyType"]?.ToString() ?? String.Empty;
            DBBillExtendedDate = dr["ExtendedBillDate"]?.ToString() ?? String.Empty;
            AdditionalName = dr["AdditionalName"]?.ToString() ?? String.Empty;
            ApprovalCategory = dr["AppCat"]?.ToString() ?? String.Empty;
            Authorities = dr["Auth"]?.ToString() ?? String.Empty;
            ApprovalDate = dr["AD"]?.ToString() ?? String.Empty;
            BillDateExtendedShow2 = dr["EBD"]?.ToString() ?? String.Empty;
            VendorName = dr["FirmName"]?.ToString() ?? String.Empty;
            VendorPersonaName = dr["FirmPerson"]?.ToString() ?? String.Empty;
            VendorPersonaContactNo = dr["FirmContactNo"]?.ToString() ?? String.Empty;
            ApprovalBillStatus = dr["ReferenceBillStatus"]?.ToString() ?? String.Empty;
            ReceivingStatus = dr["RecievingStatus"]?.ToString() ?? String.Empty;
            LastReceiveOn = dr["LastRecieveOn"]?.ToString() ?? String.Empty;
            CampusCode = dr["CampusCode"]?.ToString() ?? String.Empty;
            Maad = dr["Maad"]?.ToString() ?? String.Empty;
            CanDeleteApproval = this.Status == "Pending" && this.App1Status == "Pending" && this.App2Status == "Pending" && this.App3Status == "Pending" && this.App4Status == "Pending" && this.Status == "Pending";
            CanEditApproval = this.CanDeleteApproval == true;
            CanEditItems = this.CanEditApproval == true;
            CanRegenerateApproval = this.Status == "Rejected" && this.IsReGenerated == "0";
        }
    }
}
