using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Approval;

public class UpdateApprovalEditDetails
{
    public IFormFile File { get; set; }
    [Required]
    public string? Maad { get; set; }
    [Required]
    public string? DepartMent { get; set; }
    
    [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "DateTill must be in YYYY-MM-DD format.")]
    [Required]
    public string? AppDate { get; set; }
    [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "DateTill must be in YYYY-MM-DD format.")]
    [Required]
    public string? BillTill { get; set; }
    [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "DateTill must be in YYYY-MM-DD format.")]
    [Required]
    public string? ExtendedBillDate { get; set; }
    [Required]
    public string? Purpose { get; set; }
    [Required]
    public string? Note { get; set; }
    [Required]
    public int VendorId { get; set; }
    [Required]
    public string? MyType { get; set; }
    
}