using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class CreateNewBudgetSessionAmountSummaryRequest
    {

        [Required]
        [RegularExpression(@"^\d{4}-\d{2}$", ErrorMessage = "Session must be in the format 'YYYY-YY', e.g., '2023-24'.")]
        public string? Session { get; set; }

        [Required]
        [AllowedValues("101", "102", "103", ErrorMessage = "Sorry!! Invalid Campus Details Found...")]
        public string? CampusCode { get; set; }


        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Budget Amount must be a greater than 0.")]
        public long? BudgetAmount { get; set; }


    }
}
