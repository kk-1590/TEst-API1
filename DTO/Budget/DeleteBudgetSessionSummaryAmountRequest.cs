using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class DeleteBudgetSessionSummaryAmountRequest
    {
        [Required]
        public string? BudgetId { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Reason must be at least 10 characters long.")]
        public string? Reason { get; set; }
    }
}
