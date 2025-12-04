using System.Data;

namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceApprovalPrintDetailsResponse
    {
        public string? ReferenceNo { get; set; } = string.Empty;
        public string? Session { get; set; } = string.Empty;
        public string? AdvFrom { get; set; } = string.Empty;
        public string? AdvTo { get; set; } = string.Empty;
        public string? AdvFromShow { get; set; } = string.Empty;
        public string? AdvToShow { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public string? RelativePersonID { get; set; } = string.Empty;
        public string? RelativePersonName { get; set; } = string.Empty;
        public string? RelativeDepartment { get; set; } = string.Empty;
        public string? RelativeDesignation { get; set; } = string.Empty;
        public string? ForDepartment { get; set; } = string.Empty;
        public string? FirmAddress { get; set; } = string.Empty;
        public string? MyOrderDate { get; set; } = string.Empty;
        public string? MyType { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;
        public string? Purpose { get; set; } = string.Empty;
        public string? CampusName { get; set; } = string.Empty;
        public string? ExpDate { get; set; } = string.Empty;
        public string? TotalAmount { get; set; } = string.Empty;
        public string? Amount { get; set; } = string.Empty;
        public string? CashPer { get; set; } = string.Empty;
        public string? Other { get; set; } = string.Empty;
        public string? FirmName { get; set; } = string.Empty;
        public string? FirmContactNo { get; set; } = string.Empty;
        public string? IniName { get; set; } = string.Empty;
        public string? IniId { get; set; } = string.Empty;
        public string? OrderDate { get; set; } = string.Empty;
        public string? CreateDate { get; set; } = string.Empty;
        public string? NewExpDate { get; set; } = string.Empty;
        public string? App1Name { get; set; } = string.Empty;
        public string? App2Name { get; set; } = string.Empty;
        public string? App3Name { get; set; } = string.Empty;
        public string? App4Designation { get; set; } = string.Empty;
        public string? App3Designation { get; set; } = string.Empty;
        public string? App2Designation { get; set; } = string.Empty;
        public string? App1Designation { get; set; } = string.Empty;
        public string? App1Status { get; set; } = string.Empty;
        public string? App2Status { get; set; } = string.Empty;
        public string? App3Status { get; set; } = string.Empty;
        public string? App4Status { get; set; } = string.Empty;
        public string? App1On { get; set; } = string.Empty;
        public string? App2On { get; set; } = string.Empty;
        public string? App3On { get; set; } = string.Empty;
        public string? App4On { get; set; } = string.Empty;
        public string? ByPass { get; set; } = string.Empty;
        public string? CancelBy { get; set; } = string.Empty;
        public string? CancelOn { get; set; } = string.Empty;
        public string? PExtra3 { get; set; } = string.Empty;
        public string? PDetails { get; set; } = string.Empty;
        public string? BudgetRequired { get; set; } = string.Empty;
        public string? BudgetAmount { get; set; } = string.Empty;
        public string? PreviousTaken { get; set; } = string.Empty;
        public string? CurStatus { get; set; } = string.Empty;
        public string? BudgetStatus { get; set; } = string.Empty;
        public string? BudgetReferenceNo { get; set; } = string.Empty;




        public List<AdvanceEmployeeLeaveDetails>? LeaveDetails { get; set; } = new List<AdvanceEmployeeLeaveDetails>();
        public List<AdvanceBillDetails>? BillDetails { get; set; } = new List<AdvanceBillDetails>();
        public List<AdvancePaymentDetails>? PaymentDetails { get; set; } = new List<AdvancePaymentDetails>();
        public AdvanceBillAgainstBaseSummaryDetails BillAgainstBaseDetailsSummary { get; set; } = new AdvanceBillAgainstBaseSummaryDetails();
        public AdvanceDistributionVsSubmittedBillDistributionSummary AdvanceDistributionVsSubmittedBill { get; set; } = new AdvanceDistributionVsSubmittedBillDistributionSummary();
        public AdvanceAmountIssuedAgainstBudgetSummary amountIssuedAgainstBudgetSummary { get; set; } = new AdvanceAmountIssuedAgainstBudgetSummary();
        public List<AdvanceBudgetUsageSessionSummaryDetails> advanceBudgetUsageSessionSummaryDetails { get; set; } = new List<AdvanceBudgetUsageSessionSummaryDetails>();
        public long? BillDetailsTotal { get; set; } = 0;
        public bool CanEditNote { get; set; } = false;
        public string PaymentDetailsFinalRemark { get; set; } = string.Empty;
        public string AdvanceAgainstTextRemarkSummary { get; set; } = string.Empty;
        public string AdvanceBudgetTextRemarkSummary { get; set; } = string.Empty;
        public string? RelativePersonContactNo { get; set; } = string.Empty;
        public string? NewExpTextRemarkSummary { get; set; } = string.Empty;
        public bool IsCancelled { get; set; } = false;
        public bool CancelByApp1 { get; set; } = false;
        public bool CancelByApp2 { get; set; } = false;
        public bool CancelByApp3 { get; set; } = false;
        public bool CancelByApp4 { get; set; } = false;
        public string AmountInWords { get; set; } = string.Empty;
        public string ExtraAppName { get; set; } = string.Empty;
        public string AdvanceOn { get; set; } = "----";

        public AdvanceApprovalPrintDetailsResponse()
        {

        }

        public AdvanceApprovalPrintDetailsResponse(DataRow dr)
        {
            this.ReferenceNo = dr["ReferenceNo"]?.ToString() ?? string.Empty;
            this.Session = dr["Session"]?.ToString() ?? string.Empty;
            this.AdvFrom = dr["AdvFrom"]?.ToString() ?? string.Empty;
            this.AdvTo = dr["AdvTo"]?.ToString() ?? string.Empty;
            this.AdvFromShow = dr["AdvFromShow"]?.ToString() ?? string.Empty;
            this.AdvToShow = dr["AdvToShow"]?.ToString() ?? string.Empty;
            this.Status = dr["Status"]?.ToString() ?? string.Empty;
            this.RelativePersonID = dr["RelativePersonID"]?.ToString() ?? string.Empty;
            this.RelativePersonName = dr["RelativePersonName"]?.ToString() ?? string.Empty;
            this.RelativeDepartment = dr["RelativeDepartment"]?.ToString() ?? string.Empty;
            this.RelativeDesignation = dr["RelativeDesignation"]?.ToString() ?? string.Empty;
            this.ForDepartment = dr["ForDepartment"]?.ToString() ?? string.Empty;
            this.FirmAddress = dr["FirmAddress"]?.ToString() ?? string.Empty;
            this.MyOrderDate = dr["MyOrderDate"]?.ToString() ?? string.Empty;
            this.MyType = dr["MyType"]?.ToString() ?? string.Empty;
            this.Note = dr["Note"]?.ToString() ?? string.Empty;
            this.Purpose = dr["Purpose"]?.ToString() ?? string.Empty;
            this.CampusName = dr["CampusName"]?.ToString() ?? string.Empty;
            this.ExpDate = dr["ExpDate"]?.ToString() ?? string.Empty;
            this.TotalAmount = dr["TotalAmount"]?.ToString() ?? string.Empty;
            this.Amount = dr["Amount"]?.ToString() ?? string.Empty;
            this.CashPer = dr["CashPer"]?.ToString() ?? string.Empty;
            this.Other = dr["Other"]?.ToString() ?? string.Empty;
            this.FirmName = dr["FirmName"]?.ToString() ?? string.Empty;
            this.FirmContactNo = dr["FirmContactNo"]?.ToString() ?? string.Empty;
            this.IniName = dr["IniName"]?.ToString() ?? string.Empty;
            this.IniId = dr["IniId"]?.ToString() ?? string.Empty;
            this.OrderDate = dr["OrderDate"]?.ToString() ?? string.Empty;
            this.CreateDate = dr["CreateDate"]?.ToString() ?? string.Empty;
            this.NewExpDate = dr["NewExpDate"]?.ToString() ?? string.Empty;
            this.App1Name = dr["App1Name"]?.ToString() ?? string.Empty;
            this.App2Name = dr["App2Name"]?.ToString() ?? string.Empty;
            this.App3Name = dr["App3Name"]?.ToString() ?? string.Empty;
            this.App4Designation = dr["App4Designation"]?.ToString() ?? string.Empty;
            this.App3Designation = dr["App3Designation"]?.ToString() ?? string.Empty;
            this.App2Designation = dr["App2Designation"]?.ToString() ?? string.Empty;
            this.App1Designation = dr["App1Designation"]?.ToString() ?? string.Empty;
            this.App1Status = dr["App1Status"]?.ToString() ?? string.Empty;
            this.App2Status = dr["App2Status"]?.ToString() ?? string.Empty;
            this.App3Status = dr["App3Status"]?.ToString() ?? string.Empty;
            this.App4Status = dr["App4Status"]?.ToString() ?? string.Empty;
            this.App1On = dr["App1On"]?.ToString() ?? string.Empty;
            this.App2On = dr["App2On"]?.ToString() ?? string.Empty;
            this.App3On = dr["App3On"]?.ToString() ?? string.Empty;
            this.App4On = dr["App4On"]?.ToString() ?? string.Empty;
            this.ByPass = dr["ByPass"]?.ToString() ?? string.Empty;
            this.CancelBy = dr["CancelBy"]?.ToString() ?? string.Empty;
            this.CancelOn = dr["CancelOn"]?.ToString() ?? string.Empty;
            this.PExtra3 = dr["PExtra3"]?.ToString() ?? string.Empty;
            this.PDetails = dr["PDetails"]?.ToString() ?? string.Empty;
            this.BudgetRequired = dr["BudgetRequired"]?.ToString() ?? string.Empty;
            this.BudgetAmount = dr["BudgetAmount"]?.ToString() ?? string.Empty;
            this.PreviousTaken = dr["PreviousTaken"]?.ToString() ?? string.Empty;
            this.CurStatus = dr["CurStatus"]?.ToString() ?? string.Empty;
            this.BudgetStatus = dr["BudgetStatus"]?.ToString() ?? string.Empty;
            this.BudgetReferenceNo = dr["BudgetReferenceNo"]?.ToString() ?? string.Empty;
        }
    }
}
