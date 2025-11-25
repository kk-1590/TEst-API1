namespace AdvanceAPI.DTO.Advance
{
    public class PurchaseDetailsForGenerateBillREsponse
    {
        public string? FirmName { get;set; }
        public string? Department { get;set; }
        public string? FirmContactName { get;set; }
        public string? FirmEmail { get;set; }
        public string? FirmPANNo { get;set; }
        public string? FirmAddress { get;set; }
        public string? FirmContactNo { get;set; }
        public string? FirmAlternate { get;set; }
        public string? CampusCode { get;set; }
        public string? CampusName { get;set; }
        public string? RelativePerson { get;set; } // ForEmployee
        public string? AmountDiscount { get;set; } 
        public string? AdditionalName { get;set; } 
        public string? Purpose { get;set; } 
        public string? ExpBillDate { get;set; } 
        public string? InitiateOn { get;set; } 
        public string? Actualmount { get;set; } 
        public string? AmountSantioned { get;set; } 
        public string? ApprovalStatus { get;set; }
        public List<TextValues>? Offices { get;set; }
        public string BillExtendDate { get; internal set; }
    }
}
