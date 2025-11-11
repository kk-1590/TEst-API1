using System.Data;

namespace AdvanceAPI.DTO.Budget
{
    public class BudgetSessionAmountSummaryResponse
    {

        public string? BudgetId { get; set; } = string.Empty;
        public string? Session { get; set; } = string.Empty;
        public string? CampusName { get; set; } = string.Empty;
        public string? CampusCode { get; set; } = string.Empty;
        public string? BudgetAmount { get; set; } = string.Empty;
        public string? UsedAmount { get; set; } = string.Empty;
        public string? RemainingAmount { get; set; } = string.Empty;
        public string? LockStatus { get; set; } = string.Empty;

        public BudgetSessionAmountSummaryResponse()
        {

        }

        public BudgetSessionAmountSummaryResponse(DataRow dr)
        {
            BudgetId = dr["BudgetId"]?.ToString() ?? string.Empty;
            Session = dr["Session"]?.ToString() ?? string.Empty;
            CampusName = dr["CampusName"]?.ToString() ?? string.Empty;
            CampusCode = dr["CampusCode"]?.ToString() ?? string.Empty;
            BudgetAmount = dr["BudgetAmount"]?.ToString() ?? string.Empty;
            UsedAmount = dr["UsedAmount"]?.ToString() ?? string.Empty;
            RemainingAmount = dr["RemainingAmount"]?.ToString() ?? string.Empty;
            LockStatus = dr["LockStatus"]?.ToString() ?? string.Empty;
        }
    }
}
