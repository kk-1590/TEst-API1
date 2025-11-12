using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class CreateDepartmentBudgetSummaryV2Request
    {
        [Required]
        [AllowedValues("101", "102", "103", ErrorMessage = "Sorry!! Invalid Campus Found...")]
        public string? CampusCode { get; set; }

        [Required]
        public string? Session { get; set; }

        [Required]
        public string? Department { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Budget Name must be at least 10 characters long.")]
        public string? BudgetName { get; set; }
    }
}
