using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class CreateDepartmentBudgetSummaryRequest
    {
        [Required]
        [AllowedValues("101", "102", "103", ErrorMessage = "Sorry!! Invalid Campus Found...")]
        public string? CampusCode { get; set; }

        [Required]
        public string? Session { get; set; }

        [Required]
        public string? Department { get; set; }

        [Required]
        public string? BudgetName { get; set; }

        [Required]
        public long? BudgetAmount { get; set; }

    }
}
