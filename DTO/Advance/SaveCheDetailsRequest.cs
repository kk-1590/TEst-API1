using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Advance
{
    public class SaveCheDetailsRequest
    {
        [Required]
        public string? TransId { get; set; }
        [Required]
        public string? IssuedAmount { get; set; }
        [Required]
        public string? TaxAmount { get; set; }
       
        public string? OtherAmount { get; set; }
    
        public string? OtherType { get; set; }
        [Required]
        public string? PaidAmount { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "InitiateOn must be in YYYY-MM-DD format.")]
        public string? IssuedDate { get; set; }
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "InitiateOn must be in YYYY-MM-DD format.")]
        public string? BillUpto { get; set; }
        [Required]
        public string? ChequeNo { get; set; }
        [Required]
        public string? ApprovalAuthText { get; set; }
        [Required]
        public string? ApprovalAuthValue { get; set; }
        [Required]
        public string? Firm { get; set; }
        [Required]
        public string? SubFirm { get; set; }
        [Required]
        public string? AdditionalName { get; set; }
        [Required]
        public string? PaymentAccount { get; set; }
        [Required]
        public string? PaymentType { get; set; }
        [Required]
        public string? PaymentMode { get; set; }
        [Required]
        public string? PaymentBank { get; set; }
        [Required]
        public string? Purpose { get; set; }
        
        [Required]
        public string? Message { get; set; }
        //Repeatation Day After Expiration
       
        public string? RepeatDay { get; set; }
       
        public string? MessageTo { get; set; }
        [Required]
        public string? Remaining { get; set; }

        public IFormFile? PdfFile { get; set; }
        public IFormFile? ExcelFile { get; set; }
        [Required]
        public string? PaymentAsset { get;  set; }
        [Required]
        public string? VendorId { get;  set; }
        [Required]
        public string? CampusCode { get;  set; }
        [Required]
        public string? CampusName { get; set; }
       
        public string? SequenceId { get; set; }
    }
}
