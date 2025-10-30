using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Approval;

public class GeneratePurchaseApprovalRequest
{
    [Required(ErrorMessage = "Amount in words is required.")]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Amount in words must contain only alphabets.")]
    public string AmountInWords { get; set; }
    [Required(ErrorMessage = "Amount in Digit is required.")]
    public int AmountInDigit { get; set; }
    
    [Required(ErrorMessage = "Amount in words is required.")]
    public string Approval1 { get; set; }
    public string? Approval2 { get; set; }
    public string? Approval3 { get; set; }
    public string FinalApprovalAuth { get; set; }
    [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "Date must be in YYYY-MM-DD format.")]
    [Required]
    public string ApprovalDate { get; set; }
    [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "DateTill must be in YYYY-MM-DD format.")]
    [Required]
    public string ApprovalTillDate { get; set; }
    
    [Required]
    public string ApprovalDepartment { get; set; }
    [Required]
    public int ApprovalMessers { get; set; }
    [Required]
    public string ApprovalNote { get; set; }
    [Required]
    public string ApprovalPurpose { get; set; }
    [Required]
    public string ApprovalType { get; set; }
    [Required]
    public string ApprovalCategory { get; set; }
    [Required]
    public string BudgetAmount { get; set; }
    [Required]
    public string BudgetBalanceAmount { get; set; }
    [Required]
    public string BudgetDet { get; set; }
    [Required]
    public string BudgetRefNo { get; set; }
    [Required]
    public string BudgetReleaseAmount { get; set; }
    [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "The discount must have a maximum of two decimal places.")]
    [Required]
    public string OverAllDiscount { get; set; }
    [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "The discount must have a maximum of two decimal places.")]
    [Required]
    public string OverAllGST { get; set; }
    
    [Required]
    public string InitBy { get; set; }
    
    [Required]
    public string Maad { get; set; } 
    [Required]
    public int OtherCharges { get; set; } 
    [Required]
    public string SubFirm { get; set; } 
    [Required]
    public int TotalAmount { get; set; }
    [Required]
    [AllowedValues(101,102,103)]
    public int campus { get; set; }
    [Required]
    public List<DraftedItemResponse> DraftedItems { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TotalAmount >= 1000)
        {
            if (string.IsNullOrEmpty(Approval2) || string.IsNullOrEmpty(Approval3))
            {
                yield return new ValidationResult(
                    "Sorry! If Amount Is Greater Then or Equal 1000 Please Select Approval Auth. 2 and 3.",
                    new[] { nameof(Approval2), nameof(Approval3) });
            }
        }
    }
}
    

