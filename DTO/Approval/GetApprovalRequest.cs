using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Approval;

public class GetApprovalRequest
{
    [Required]
    [AllowedValues(101,102,103)]
    public int CampusCode { get; set; }
    
    public string? Department { get; set; }
    [Required]
    public string Session { get; set; }
    [Required]
    [AllowedValues("Pending My","Pending All","Approved","Cancelled","Rejected")]
    public string Status { get; set; }
    [Required]
    public int Limit{ get; set; }
    [Required]
    public int Page { get; set; }
    
}