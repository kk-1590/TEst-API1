namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceDistributionVsSubmittedBillDistributionSummary
    {
        public long? TotalAdvance { get; set; } = 0;
        public long? TotalBill { get; set; } = 0;
        public long? TotalDiff { get; set; } = 0;
        public List<AdvanceDistributionVsSubmittedBillDistribution> DistributionDetails { get; set; } = new List<AdvanceDistributionVsSubmittedBillDistribution>();
    }
}
