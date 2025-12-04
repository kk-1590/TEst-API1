namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceAmountIssuedAgainstBudgetSummary
    {
        public List<AdvanceAmountIssuedAgainstBudgetDetails> AdvanceAmountIssuedAgainstBudgetDetails { get; set; } = new List<AdvanceAmountIssuedAgainstBudgetDetails>();
        public string SummaryText { get; set; } = string.Empty;
    }
}
