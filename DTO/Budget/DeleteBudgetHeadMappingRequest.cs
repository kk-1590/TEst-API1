using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class DeleteBudgetHeadMappingRequest
    {

        [Required]
        public string? HeadMappingId { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Reason should be at least 10 characters long")]
        public string? Reason { get; set; }
    }
}
