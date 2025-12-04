using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Advance
{
    public class UpdatePurchaseBillDateRequest
    {
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date must be in YYYY-MM-DD format.")]
        public string NewAppDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date must be in YYYY-MM-DD format.")]
        public string OldAppDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date must be in YYYY-MM-DD format.")]
        public string NewBillDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date must be in YYYY-MM-DD format.")]
        public string OldBillDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date must be in YYYY-MM-DD format.")]
        public string NewGateDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date must be in YYYY-MM-DD format.")]
        public string OldGateDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date must be in YYYY-MM-DD format.")]
        public string NewExpDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date must be in YYYY-MM-DD format.")]
        public string OldExpDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date must be in YYYY-MM-DD format.")]
        public string NewTestDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date must be in YYYY-MM-DD format.")]
        public string OldTestDate { get; set; }
        [Required]
        public string? NewPurpose { get; set; }
        [Required]
        public string? OldPurpose { get; set; }
        [Required]
        public string? NewAdditionalName { get; set; }
        [Required]
        public string? OldAdditionalName { get; set; }
        public string? NewWarrentyID { get; set; }
        public string? TransId { get; set; }

    }
}
