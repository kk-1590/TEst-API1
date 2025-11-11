using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class AddBudgetTypeHeadRequest
    {
        [Required]
        public string? Session { get; set; }

        [Required]
        [AllowedValues("101", "102", "103", ErrorMessage = "Sorry!! Invalid Session Details Found...")]
        public string? CampusCode { get; set; }

        [Required]
        [AllowedValues("Recurring", "Non-Recurring", ErrorMessage = "Sorry!! Invalid Type Found...")]
        public string? BudgetType { get; set; }

        [Required]
        public string? BudgetHead { get; set; }
    }
}
