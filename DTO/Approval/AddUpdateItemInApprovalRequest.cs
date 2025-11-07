using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Approval
{
    public class AddUpdateItemInApprovalRequest : IValidatableObject
    {

        [Required]
        public string? ReferenceNo { get; set; }

        [Required]
        public string? ItemCode { get; set; }


        [Required]
        public double? Balance { get; set; }

        [Required]
        public double? Quantity { get; set; }

        [Required]
        public double? PrevRate { get; set; }

        [Required]
        public double? CurRate { get; set; }

        [Required]
        public string? ChangeReason { get; set; }

        [Required]
        public int? Warranty { get; set; }

        [Required]
        [AllowedValues("Day", "Month", "Year", ErrorMessage = "Warranty Type must be either 'Day', 'Month', or 'Year'.")]
        public string? WarrantyType { get; set; }

        [Required]
        public double? ActualAmount { get; set; }

        [Required]
        [Range(0.00, 100.00, ErrorMessage = "Discount Percentage must be between 0 to 100.")]
        public double? DiscountPer { get; set; }

        [Required]
        [Range(0.00, 100.00, ErrorMessage = "GST Percentage must be greater than 0 and up to 100.")]
        public double? GSTPer { get; set; }

        [Required]
        public double? TotalAmount { get; set; }

        public string? SerialNo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Quantity <= 0)
            {
                yield return new ValidationResult(
                    "Quantity must be greater than zero.",
                    new[] { nameof(Quantity) });
            }
            if (CurRate <= 0)
            {
                yield return new ValidationResult(
                    "Current Rate must be greater than zero.",
                    new[] { nameof(CurRate) });
            }
        }
    }
}
