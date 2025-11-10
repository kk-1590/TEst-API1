using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class AddBudgetDetailsRequest
    {
        [Required]
        public string? ReferenceNo { get; set; }
        [Required]
        public string? Maad { get;set; }
        [Required]
        public string? BudgetAmount { get;set; }

    }
}
