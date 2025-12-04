using System.Data;

namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceDistributionVsSubmittedBillDistribution
    {
        public string? Head { get; set; } = string.Empty;
        public long? Advance { get; set; } = 0;
        public long? Bill { get; set; } = 0;
        public long? Diff { get; set; } = 0;
        public bool IsHighlighted { get; set; } = false;

        public AdvanceDistributionVsSubmittedBillDistribution()
        {

        }
        public AdvanceDistributionVsSubmittedBillDistribution(DataRow dr)
        {
            Head = dr["Head"] != DBNull.Value ? Convert.ToString(dr["Head"]) : string.Empty;
            Advance = dr["Advance"] != DBNull.Value ? Convert.ToInt64(dr["Advance"]) : 0;
            Bill = dr["Bill"] != DBNull.Value ? Convert.ToInt64(dr["Bill"]) : 0;
            Diff = dr["Diff"] != DBNull.Value ? Convert.ToInt64(dr["Diff"]) : 0;
        }
    }
}
