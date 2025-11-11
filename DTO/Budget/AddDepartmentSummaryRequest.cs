using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class AddDepartmentSummaryRequest
    {
        [Required]
        public string? ReferenceNo { get; set; }
        [Required]
        [AllowedValues("Recurring", "Non-Recurring")]
        public string? BudgetType { get;set; }
        [Required]
        public string? BudgetHead { get;set; }
        [Required]
        public string? BudgetMaad { get;set; }
        [Required]
        public int BudgetAmount {  get;set; }
        [Required]
        public int AllowOverBudgetApproval { get;set; }


    }
}
