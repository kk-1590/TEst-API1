using System.Data;

namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceBudgetUsageSessionSummaryDetails
    {
        public string? Session { get; set; } = string.Empty;
        public string? ReferenceNo { get; set; } = string.Empty;
        public string? FromDate { get; set; } = string.Empty;
        public string? ToDate { get; set; } = string.Empty;
        public double? TotalAmount { get; set; } = 0;
        public long? Used { get; set; } = 0;

        private List<AdvanceBudgetUsageMonthSummaryDetails> _usageMonthSummaryDetails = new();
        public List<AdvanceBudgetUsageMonthSummaryDetails> UsageMonthSummaryDetails
        {
            get => _usageMonthSummaryDetails;
            set => _usageMonthSummaryDetails = value;
        }

        public long? JanTotal { get; set; } = 0;
        public long? FebTotal { get; set; } = 0;
        public long? MarTotal { get; set; } = 0;
        public long? AprTotal { get; set; } = 0;
        public long? MayTotal { get; set; } = 0;
        public long? JunTotal { get; set; } = 0;
        public long? JulTotal { get; set; } = 0;
        public long? AugTotal { get; set; } = 0;
        public long? SepTotal { get; set; } = 0;
        public long? OctTotal { get; set; } = 0;
        public long? NovTotal { get; set; } = 0;
        public long? DecTotal { get; set; } = 0;
        public long? TotTotal { get; set; } = 0;


        public AdvanceBudgetUsageSessionSummaryDetails()
        {

        }

        public AdvanceBudgetUsageSessionSummaryDetails(DataRow dr)
        {
            Session = dr["Session"]?.ToString() ?? string.Empty;
            ReferenceNo = dr["ReferenceNo"]?.ToString() ?? string.Empty;
            FromDate = dr["FromDate"]?.ToString() ?? string.Empty;
            ToDate = dr["ToDate"]?.ToString() ?? string.Empty;
            TotalAmount = Convert.ToDouble(dr["TotalAmount"]?.ToString() ?? "0");
            Used = Convert.ToInt64(dr["Used"]?.ToString() ?? "0");
        }
    }
}
