namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceBillAgainstBaseSummaryDetails
    {
        public string? TotalAdvance { get; set; } = string.Empty;
        public string? BillAmount { get; set; } = string.Empty;
        public string? Difference { get; set; } = string.Empty;
        public string? FinalStatus { get; set; } = string.Empty;
        public string? ExcessBill { get; set; } = string.Empty;
        public string? TotalBills { get; set; } = string.Empty;
        public List<AdvanceApprovalBillDetailsPrintOut>? BillAgainstBaseDetails { get; set; } = new List<AdvanceApprovalBillDetailsPrintOut>();
    }
}
