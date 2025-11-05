
using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Approval;

public class serviceWaranRequest
{
    [Required]
    public string CampusCode { get; set; }
    [Required]
    public string SRNo { get; set; }
}