using System.Data;

namespace AdvanceAPI.DTO.Advance.Bill
{
    public class BillChequeApprovalResponse
    {
        public string? INI { get; set; } = string.Empty;
        public string? IssuedName { get; set; } = string.Empty;
        public string? BillNameBy { get; set; } = string.Empty;
        public string? ForType { get; set; } = string.Empty;
        public string? TransID { get; set; } = string.Empty;
        public string? MyType { get; set; } = string.Empty;
        public string? SequenceID { get; set; } = string.Empty;
        public string? InitiatedBy { get; set; } = string.Empty;
        public string? FirmName { get; set; } = string.Empty;
        public string? Purpose { get; set; } = string.Empty;
        public string? Santioned { get; set; } = string.Empty;
        public string? Paid { get; set; } = string.Empty;
        public string? On { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public string? Till { get; set; } = string.Empty;
        public string? MyIssue { get; set; } = string.Empty;
        public string? TransactionID { get; set; } = string.Empty;
        public string? MyINICheck { get; set; } = string.Empty;
        public string? MyEntryCheck { get; set; } = string.Empty;
        public string? IsSpecial { get; set; } = string.Empty;
        public string? CashDiscount { get; set; } = string.Empty;
        public string? Bal { get; set; } = string.Empty;
        public string? Cond45 { get; set; } = string.Empty;
        public string? CheqAmt { get; set; } = string.Empty;
        public string? TotBill { get; set; } = string.Empty;
        public string? BillExtra3 { get; set; } = string.Empty;
        public string? BillExtra6 { get; set; } = string.Empty;
        public string? ShowMeNow { get; set; } = string.Empty;
        public string? BillExtra4 { get; set; } = string.Empty;
        public string? Col5 { get; set; } = string.Empty;
        public string? Col1 { get; set; } = string.Empty;
        public string? ChequeVendor { get; set; } = string.Empty;
        public string? BVId { get; set; } = string.Empty;
        public string? BRPID { get; set; } = string.Empty;

        public bool? BillApprovedPdfExists { get; set; } = false;
        public bool? BillPdfExists { get; set; } = false;
        public bool? BillExcelExists { get; set; } = false;
        public bool? SequenceBillPdfExists { get; set; } = false;
        public bool? SequenceBillExcelExists { get; set; } = false;
        public bool? WarrentySwitchAllowed { get; set; } = false;
        public bool? IsImprestSummary { get; set; } = false;
        public bool? GotRejectedPreviously { get; set; } = false;
        public bool? CanApprove { get; set; } = true;
        public bool? CanReject { get; set; } = true;
        public bool? AllowReject { get; set; } = false;
        public bool? ShowRow { get; set; } = true;
        public bool? IsBillLate { get; set; } = true;
        public bool? CanOpenFirmPaidReport { get; set; } = true;
        public bool? CanOpenFirmRejectionReport { get; set; } = true;


        public string? BudgetString { get; set; } = string.Empty;
        public string? BudgetStringToolTip { get; set; } = string.Empty;
        public string? StudentId { get; set; } = string.Empty;
        public string? DepartmentVendorPaidString { get; set; } = string.Empty;
        public string? ApprovalAuthsString { get; set; } = string.Empty;
        public string? ExtraTypeString { get; set; } = string.Empty;
        public string? BillChequeApprovalAuthsString { get; set; } = string.Empty;
        public string? ReadyToIssueAmountAuthsString { get; set; } = string.Empty;
        public string? BillApprovedAuthsString { get; set; } = string.Empty;
        public string? RejectionReasonString { get; set; } = string.Empty;
        public string? ChequeVendorString { get; set; } = string.Empty;
        public string? MainPrintOutString { get; set; } = string.Empty;
        public string? MainGateRecievingLinkString { get; set; } = string.Empty;
        public string? ItemReturnOnConsumableLinkString { get; set; } = string.Empty;
        public string? PurposeLinkString { get; set; } = string.Empty;
        public System.Drawing.Color? Col9BackColor { get; set; }
        public System.Drawing.Color? Col10BackColor { get; set; }
        public System.Drawing.Color? Col6BackColor { get; set; }
        public System.Drawing.Color? Col5BackColor { get; set; }
        public System.Drawing.Color? Col9ForeColor { get; set; }
        public System.Drawing.Color? Col6ForeColor { get; set; }
        public System.Drawing.Color? Col10ForeColor { get; set; }
        public System.Drawing.Color? Col5ForeColor { get; set; }
        public System.Drawing.Color? DiscountForeColor { get; set; }
        public List<BillApprovalBillBaseAllDetails> BillRecords { get; set; } = new List<BillApprovalBillBaseAllDetails>();

        public BillChequeApprovalResponse()
        {

        }

        public BillChequeApprovalResponse(DataRow dr)
        {
            INI = dr["INI"]?.ToString() ?? String.Empty;
            IssuedName = dr["IssuedName"]?.ToString() ?? String.Empty;
            BillNameBy = dr["BillNameBy"]?.ToString() ?? String.Empty;
            ForType = dr["ForType"]?.ToString() ?? String.Empty;
            TransID = dr["TransID"]?.ToString() ?? String.Empty;
            MyType = dr["MyType"]?.ToString() ?? String.Empty;
            SequenceID = dr["SequenceID"]?.ToString() ?? String.Empty;
            InitiatedBy = dr["Initiated By"]?.ToString() ?? String.Empty;
            FirmName = dr["Firm Name"]?.ToString() ?? String.Empty;
            Purpose = dr["Purpose"]?.ToString() ?? String.Empty;
            Santioned = dr["Santioned"]?.ToString() ?? String.Empty;
            Paid = dr["Paid"]?.ToString() ?? String.Empty;
            On = dr["On"]?.ToString() ?? String.Empty;
            Status = dr["Status"]?.ToString() ?? String.Empty;
            Till = dr["Till"]?.ToString() ?? String.Empty;
            MyIssue = dr["MyIssue"]?.ToString() ?? String.Empty;
            TransactionID = dr["TransactionID"]?.ToString() ?? String.Empty;
            MyINICheck = dr["MyINICheck"]?.ToString() ?? String.Empty;
            MyEntryCheck = dr["MyEntryCheck"]?.ToString() ?? String.Empty;
            IsSpecial = dr["IsSpecial"]?.ToString() ?? String.Empty;
            CashDiscount = dr["CashDiscount"]?.ToString() ?? String.Empty;
            Bal = dr["Bal"]?.ToString() ?? String.Empty;
            Cond45 = dr["Cond45"]?.ToString() ?? String.Empty;
            CheqAmt = dr["CheqAmt"]?.ToString() ?? String.Empty;
            TotBill = dr["TotBill"]?.ToString() ?? String.Empty;
            BillExtra3 = dr["BillExtra3"]?.ToString() ?? String.Empty;
            BillExtra6 = dr["BillExtra6"]?.ToString() ?? String.Empty;
            ShowMeNow = dr["ShowMeNow"]?.ToString() ?? String.Empty;
            BillExtra4 = dr["BillExtra4"]?.ToString() ?? String.Empty;
            Col5 = dr["Col5"]?.ToString() ?? String.Empty;
            Col1 = dr["Col1"]?.ToString() ?? String.Empty;
            ChequeVendor = dr["ChequeVendor"]?.ToString() ?? String.Empty;
            BVId = dr["BVId"]?.ToString() ?? String.Empty;
            BRPID = dr["BRPID"]?.ToString() ?? String.Empty;

        }

    }
}
