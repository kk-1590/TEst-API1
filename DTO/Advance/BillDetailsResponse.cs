namespace AdvanceAPI.DTO.Advance
{
    public class BillDetailsResponse
    {
        // Basic
        public string? TransactionId { get; set; }
        public string? Session { get; set; }
        public string? CampusName { get; set; }
        public string? ForType { get; set; }
        // Employee  ?
        public string? RelativePersonId { get; set; }
        public string? RelativePersonName { get; set; }
        public string? RelativeDesignation { get; set; }
        public string? RelativeContactNo { get; set; }
        // Firm / Ven?dor
        public string? VendorId { get; set; }
        public string? FirmName { get; set; }
        public string? FirmAddress { get; set; }
        public string? FirmContactNo { get; set; }
        public string? AdditionalName { get; set; }
        // Department? / purpose
        public string? Department { get; set; }
        public string? Purpose { get; set; }
        // Dates (for?matted strings, exactly jaise report me dikh rahe hain)
        public string? ReportDate { get; set; }        // current date: dd.MM.yyyy
        public string? ExpBillDate { get; set; }
        public string? GateDate { get; set; }
        public string? IniDate { get; set; }
        public string? DeptDate { get; set; }
        public string? BillDate { get; set; }
        public string? TestBillDate { get; set; }
        public string? UploadOn { get; set; }
        public string? UploadBy { get; set; }
        // Amounts   ?
        public decimal AmountRequired { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal AmountRemaining { get; set; }
        public string? Discount { get; set; }
        // Status / l?inks
        public string? BillStatus { get; set; }
        public string? ScanCopyLinkHtml { get; set; }
        public string? ApprovalLinkHtml { get; set; }
        public string? PageName { get; set; }
        public string? ApprovedBySummary { get; set; }
                     
        // Bill field?s
        public string? BillNo { get; set; }
                   
        public string? ReportBy { get; set; }          // Guest / username
        public List<BillIssueRowDto> Issues { get; set; } = new();
    }
    public sealed class BillIssueRowDto
    {
        public int RowNo { get; set; }
        public string? CvName { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal IssuedAmount { get; set; }
        public string? Tran { get; set; }
        public string? IssuedOn { get; set; }
        public string? By { get; set; }
        public string? SignedOn { get; set; }
        public string? ReceivedOn { get; set; }
        public string? BankOn { get; set; }
        public string? Status { get; set; }
        public string? PdfUrl { get; set; }
        public string? ExcelUrl { get; set; }
    }
}
