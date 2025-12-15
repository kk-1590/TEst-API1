using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Media
{
    public class GetMediaScheduleManagerCreateReleaseOrderAuthoritiesRequest
    {
        [Required(ErrorMessage = "Authority type is required")]
        [AllowedValues("VERIFY", "APPROVE", "MEMBER", ErrorMessage = "Authority type must be either 'VERIFY', 'APPROVE', or 'MEMBER'")]
        public string? AuthorityType { get; set; }
    }
}
