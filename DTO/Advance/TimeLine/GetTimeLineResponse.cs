namespace AdvanceAPI.DTO.Advance.TimeLine
{
    public class GetTimeLineResponse
    {
        public PurchaseDetails? purchase { get; set; }
        public List<PurchaseDetails>? Advance { get; set; }
        public List<PurchaseDetails>? Bill { get; set; }
        public PurchaseDetails? Cheque { get; set; }
    }
    public class PurchaseDetails
    {
        public string? MyType { get; set; }
        public string? IssuedAmount { get; set; }
        public int TotalAuth { get; set; }
        public int TotalApproved { get; set; }
        public string? BillStatus { get; set; }
        public string? ReferenceNo { get; set; }
        public string? Amount { get; set; }
        public string? Status { get; set; }
        public string? TotalItem { get; set; }
    }
    
}
