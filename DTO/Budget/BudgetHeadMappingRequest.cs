using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class BudgetHeadMappingRequest : PaginationSectionRequest
    {
        [Required]
        [AllowedValues("101", "102", "103", ErrorMessage = "Sorry!! Invalid Campus Details Found...")]
        public string? CampusCode { get; set; }

        [Required]
        public string? Session { get; set; }

        public string? BudgetType { get; set; }

    }
}
