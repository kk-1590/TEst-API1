namespace AdvanceAPI.DTO.Media
{
    public class CanCreateMediaReleaseOrderDetailsResponse
    {
        public bool CanUploadReleaseOrder { get; set; } = false;
        public bool IsOverBudget { get; set; } = false;
        public string? Instruction { get; set; } = string.Empty;
        public string? HeaderString { get; set; } = string.Empty;
        public HashSet<MediaBudgetSummaryBudgetScheduledRO>? SummaryList { get; set; } = new HashSet<MediaBudgetSummaryBudgetScheduledRO>();
    }
}
