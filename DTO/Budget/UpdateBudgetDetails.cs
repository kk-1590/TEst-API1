using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class UpdateBudgetDetails
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? RefrenceNo { get; set; }
        [Required]
        public string? BudgetAmount { get; set; }
    }
}
