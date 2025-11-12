using System.Data;

namespace AdvanceAPI.DTO.Budget
{
    public class DepartmentBudgetSummaryResponse
    {

        public string? ReferenceNo { get; set; } = string.Empty;
        public string? Session { get; set; } = string.Empty;
        public string? CampusName { get; set; } = string.Empty;
        public string? CampusCode { get; set; } = string.Empty;
        public string? Department { get; set; } = string.Empty;
        public string? BudgetName { get; set; } = string.Empty;
        public string? BudgetAmount { get; set; } = string.Empty;
        public string? RecurringBudgetAmount { get; set; } = string.Empty;
        public string? NonRecurringBudgetAmount { get; set; } = string.Empty;
        public string? BudgetAmountUsed { get; set; } = string.Empty;
        public string? RecurringBudgetAmountUsed { get; set; } = string.Empty;
        public string? NonRecurringBudgetAmountUsed { get; set; } = string.Empty;
        public string? BudgetAmountRemaining { get; set; } = string.Empty;
        public string? RecurringBudgetAmountRemaining { get; set; } = string.Empty;
        public string? NonRecurringBudgetAmountRemaining { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public string? BudgetStatus { get; set; } = string.Empty;

        public DepartmentBudgetSummaryResponse()
        {

        }

        public DepartmentBudgetSummaryResponse(DataRow dr)
        {
            ReferenceNo = dr["ReferenceNo"]?.ToString() ?? string.Empty;
            Session = dr["Session"]?.ToString() ?? string.Empty;
            CampusName = dr["CampusName"]?.ToString() ?? string.Empty;
            CampusCode = dr["CampusCode"]?.ToString() ?? string.Empty;
            Department = dr["Department"]?.ToString() ?? string.Empty;
            BudgetName = dr["BudgetName"]?.ToString() ?? string.Empty;
            BudgetAmount = dr["BudgetAmount"]?.ToString() ?? string.Empty;
            RecurringBudgetAmount = dr["RecurringBudgetAmount"]?.ToString() ?? string.Empty;
            NonRecurringBudgetAmount = dr["NonRecurringBudgetAmount"]?.ToString() ?? string.Empty;
            BudgetAmountUsed = dr["BudgetAmountUsed"]?.ToString() ?? string.Empty;
            RecurringBudgetAmountUsed = dr["RecurringBudgetAmountUsed"]?.ToString() ?? string.Empty;
            NonRecurringBudgetAmountUsed = dr["NonRecurringBudgetAmountUsed"]?.ToString() ?? string.Empty;
            BudgetAmountRemaining = dr["BudgetAmountRemaining"]?.ToString() ?? string.Empty;
            RecurringBudgetAmountRemaining = dr["RecurringBudgetAmountRemaining"]?.ToString() ?? string.Empty;
            NonRecurringBudgetAmountRemaining = dr["NonRecurringBudgetAmountRemaining"]?.ToString() ?? string.Empty;
            Status = dr["Status"]?.ToString() ?? string.Empty;
            BudgetStatus = dr["BudgetStatus"]?.ToString() ?? string.Empty;
        }
    }
}
