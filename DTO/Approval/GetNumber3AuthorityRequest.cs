using AdvanceAPI.DTO.Inclusive;
using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Approval
{
    public class GetNumber3AuthorityRequest : GetNameFilterRequest
    {
        public string? CampusCode { get; set; }
    }
}
