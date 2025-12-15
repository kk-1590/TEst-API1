namespace AdvanceAPI.DTO.Advance
{
    public sealed class ApiResponseLoadTransactionDetails
    {
        public string? Remaining { get; set; }
        public string? AlreadyIssued { get; set; }
        public List<LoadTransactionDetailsRespomse>? LoadTransactionDetailsRespomses { get; set; }
        public string? AdditionalName { get; internal set; }
        public string? Purpose { get; internal set; }
        public string? FirmName { get; internal set; }
        public string? VendorId { get; internal set; }
        public string? Sub_Firm { get; internal set; }
        public bool EnableBillUpto { get; set; }
        public bool CanMessageSend { get; set; }
    }

    public class LoadTransactionDetailsRespomse
    {
        public string? IssuedOn { get;set; }
        public string? Firm { get;set; }
        public string? Sub_Firm { get;set; }
        public string? Type { get;set; }
        public string? Issued { get;set; }
        public string? Tax { get;set; }
        public string? Other { get;set; }
        public string? Paid { get;set; }
        public string? Mode { get;set; }
        public string? TransNo { get;set; }
        public string? Date { get;set; }
        public string? By { get;set; }
        public string? SeqNo { get; set; }
        public string? FileUrl { get; set; }
        public string? CheckIssuedOn { get; set; }
        public string? ReceivedOn { get;  set; }
        public string? ClearOn { get;  set; }
        public string? Status { get; set; }
        public string? EmpDetails { get; set; }
        public string? SignedOn { get; set; }
        public bool CanDelete { get; set; }
    }
}
