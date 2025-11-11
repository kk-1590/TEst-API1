using System.Data;

namespace AdvanceAPI.DTO.Budget
{
    public class BudgetSessionAmountSummaryResponse
    {

        public string? Session { get; set; } = string.Empty;
        public string? CampusName { get; set; } = string.Empty;
        public string? CampusCode { get; set; } = string.Empty;
        public string? BudgetAmount { get; set; } = string.Empty;
        public string? UsedAmount { get; set; } = string.Empty;
        public string? RemainingAmount { get; set; } = string.Empty;
        public string? EditingStatus { get; set; } = string.Empty;

        public BudgetSessionAmountSummaryResponse()
        {

        }

        public BudgetSessionAmountSummaryResponse(DataRow dr)
        {
            Session = dr["Session"]?.ToString() ?? string.Empty;
            CampusName = dr["CampusName"]?.ToString() ?? string.Empty;
            CampusCode = dr["CampusCode"]?.ToString() ?? string.Empty;
            BudgetAmount = dr["BudgetAmount"]?.ToString() ?? string.Empty;
            UsedAmount = dr["UsedAmount"]?.ToString() ?? string.Empty;
            RemainingAmount = dr["RemainingAmount"]?.ToString() ?? string.Empty;
            EditingStatus = dr["EditingStatus"]?.ToString() ?? string.Empty;
        }
    }
}
