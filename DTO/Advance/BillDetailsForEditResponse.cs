namespace AdvanceAPI.DTO.Advance
{
    public class BillDetailsForEditResponse
    {
        public string? BillTransId { get; set; }
        public string? AdditionalName { get; set; }
        public string? ForOffice { get; set; }
        public string? RefNo { get; set; }
        public string? Department { get; set; }
        public string? FirmName { get; set; }
        public string? FirmContactName { get; set; }
        public string? FirmContactNo { get; set; }
        public string? FirmEmail { get; set; }
        public string? Actualmount { get; set; }
        public string? FirmAlternate { get; set; }
        public string? FirmAddress { get; set; }
        public string? Purpose { get; set; }
        public string? FirmPANNo { get; set; }
        public string? ForEmployee { get; set; }
        public string? Reamining { get; set; }
        public string? AlreadyIssued { get; set; }
        public string? BillNo { get; set; }
        public string? GateDate { get; set; }
        public string? StoreDate { get; set; }
        public string? InitiateOn { get; set; }
        public string? DepartmentDate { get; set; }
        public string? CampusCode { get; set; }
        public string? CampusName { get; set; }
        public string? ExpBillDate { get; set; }
        public string? TestingUpto { get; set; }
        public string? BillOnGate { get; set; }
        public string? BillDate { get; set; }
        public string? AmountDiscount { get; set; }
        public string? Status { get; set; }
        public string? NextBillTill { get; set; }
        public TextValues? App1Auth { get; set; }
        public TextValues? App2Auth { get; set; }
        public TextValues? App3Auth { get; set; }
        public TextValues? App4Auth { get; set; }
        public List<TextValues>? Offices { get; set; }
        public string? ExceFile { get; set; }
        public string? PdfFile { get; set; }
    }
    
}
