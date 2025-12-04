using System.Data;

namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceBudgetUsageMonthSummaryDetails
    {
        public string? Head { get; set; } = string.Empty;
        public long? Jan { get; set; } = 0;
        public long? Feb { get; set; } = 0;
        public long? Mar { get; set; } = 0;
        public long? Apr { get; set; } = 0;
        public long? May { get; set; } = 0;
        public long? Jun { get; set; } = 0;
        public long? Jul { get; set; } = 0;
        public long? Aug { get; set; } = 0;
        public long? Sep { get; set; } = 0;
        public long? Oct { get; set; } = 0;
        public long? Nov { get; set; } = 0;
        public long? Dec { get; set; } = 0;
        public long? Tot { get; set; } = 0;

        public string? JanRemark { get; set; } = string.Empty;
        public string? FebRemark { get; set; } = string.Empty;
        public string? MarRemark { get; set; } = string.Empty;
        public string? AprRemark { get; set; } = string.Empty;
        public string? MayRemark { get; set; } = string.Empty;
        public string? JunRemark { get; set; } = string.Empty;
        public string? JulRemark { get; set; } = string.Empty;
        public string? AugRemark { get; set; } = string.Empty;
        public string? SepRemark { get; set; } = string.Empty;
        public string? OctRemark { get; set; } = string.Empty;
        public string? NovRemark { get; set; } = string.Empty;
        public string? DecRemark { get; set; } = string.Empty;

        public bool IsHighlighted { get; set; } = false;


        public AdvanceBudgetUsageMonthSummaryDetails()
        {

        }

        public AdvanceBudgetUsageMonthSummaryDetails(DataRow dr)
        {
            Head = dr["Head"]?.ToString() ?? string.Empty;
            Jan = Convert.ToInt64(dr["Jan"]?.ToString() ?? "0");
            Feb = Convert.ToInt64(dr["Feb"]?.ToString() ?? "0");
            Mar = Convert.ToInt64(dr["Mar"]?.ToString() ?? "0");
            Apr = Convert.ToInt64(dr["Apr"]?.ToString() ?? "0");
            May = Convert.ToInt64(dr["May"]?.ToString() ?? "0");
            Jun = Convert.ToInt64(dr["Jun"]?.ToString() ?? "0");
            Jul = Convert.ToInt64(dr["Jul"]?.ToString() ?? "0");
            Aug = Convert.ToInt64(dr["Aug"]?.ToString() ?? "0");
            Sep = Convert.ToInt64(dr["Sep"]?.ToString() ?? "0");
            Oct = Convert.ToInt64(dr["Oct"]?.ToString() ?? "0");
            Nov = Convert.ToInt64(dr["Nov"]?.ToString() ?? "0");
            Dec = Convert.ToInt64(dr["Dec"]?.ToString() ?? "0");
            Tot = Convert.ToInt64(dr["Tot"]?.ToString() ?? "0");

            JanRemark = dr["JanRemark"]?.ToString() ?? string.Empty;
            FebRemark = dr["FebRemark"]?.ToString() ?? string.Empty;
            MarRemark = dr["MarRemark"]?.ToString() ?? string.Empty;
            AprRemark = dr["AprRemark"]?.ToString() ?? string.Empty;
            MayRemark = dr["MayRemark"]?.ToString() ?? string.Empty;
            JunRemark = dr["JunRemark"]?.ToString() ?? string.Empty;
            JulRemark = dr["JulRemark"]?.ToString() ?? string.Empty;
            AugRemark = dr["AugRemark"]?.ToString() ?? string.Empty;
            SepRemark = dr["SepRemark"]?.ToString() ?? string.Empty;
            OctRemark = dr["OctRemark"]?.ToString() ?? string.Empty;
            NovRemark = dr["NovRemark"]?.ToString() ?? string.Empty;
            DecRemark = dr["DecRemark"]?.ToString() ?? string.Empty;
        }
    }
}
