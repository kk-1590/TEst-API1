using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class UpdateBudgetSessionAmountRequest
    {
        [Required]
        public string? BudgetId { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Budget Amount must be a greater than 0.")]
        public long? BudgetAmount { get; set; }

        [Required]
        [MinLengthAttribute(10, ErrorMessage = "Reason must be at least 10 characters long.")]
        public string? Reason { get; set; }

    }
}
