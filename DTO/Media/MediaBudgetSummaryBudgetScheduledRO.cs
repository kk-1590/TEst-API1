using System.Data;

namespace AdvanceAPI.DTO.Media
{
    public record MediaBudgetSummaryBudgetScheduledRO
    {
        public string? SNO { get; init; }
        public string? Type { get; init; }
        public string? Amount { get; init; }
        public string? Remark { get; init; }


        public MediaBudgetSummaryBudgetScheduledRO()
        {

        }

        public MediaBudgetSummaryBudgetScheduledRO(DataRow dr) : this(dr["SNO"]?.ToString() ?? string.Empty, dr["Type"]?.ToString() ?? string.Empty, dr["Amount"]?.ToString() ?? string.Empty, dr["Remark"]?.ToString() ?? string.Empty)
        {

        }

        public MediaBudgetSummaryBudgetScheduledRO(string? sno, string? type, string? amount, string? remark)
        {
            SNO = sno;
            Type = type;
            Amount = amount;
            Remark = remark;
        }

    }
}
