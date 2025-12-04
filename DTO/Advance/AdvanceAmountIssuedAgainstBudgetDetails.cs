using System.Data;

namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceAmountIssuedAgainstBudgetDetails
    {
        public string? ReferenceNo { get; set; } = string.Empty;
        public string? RelativePersonName { get; set; } = string.Empty;
        public string? AppDate { get; set; } = string.Empty;
        public long? BudgetAmount { get; set; } = 0;
        public long? PreviousTaken { get; set; } = 0;
        public double? Amount { get; set; } = 0;
        public long? Balance { get; set; } = 0;
        public string? Status { get; set; } = string.Empty;
        public long? ExcessUsed { get; set; } = 0;

        public AdvanceAmountIssuedAgainstBudgetDetails()
        {

        }

        public AdvanceAmountIssuedAgainstBudgetDetails(DataRow dr)
        {
            ReferenceNo = dr["ReferenceNo"]?.ToString() ?? string.Empty;
            RelativePersonName = dr["RelativePersonName"]?.ToString() ?? string.Empty;
            AppDate = dr["AppDate"]?.ToString() ?? string.Empty;
            BudgetAmount = dr["BudgetAmount"] != DBNull.Value ? Convert.ToInt64(dr["BudgetAmount"]) : 0;
            PreviousTaken = dr["PreviousTaken"] != DBNull.Value ? Convert.ToInt64(dr["PreviousTaken"]) : 0;
            Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0;
            Balance = dr["Balance"] != DBNull.Value ? Convert.ToInt64(dr["Balance"]) : 0;
            Status = dr["Status"]?.ToString() ?? string.Empty;
            ExcessUsed = dr["ExcessUsed"] != DBNull.Value ? Convert.ToInt64(dr["ExcessUsed"]) : 0;
        }
    }
}
