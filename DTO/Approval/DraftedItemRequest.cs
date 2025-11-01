using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Approval;

public class DraftedItemRequest
{
    [Required]
    public string AppType { get; set; }
    [Required]
    public string? RefNo { get; set; }
    [Required]
    [AllowedValues(101,102,103)]
    public int CampusCode { get; set; }
}