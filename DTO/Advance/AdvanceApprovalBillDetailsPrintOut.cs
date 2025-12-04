using System.Data;

namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceApprovalBillDetailsPrintOut
    {

        public string? SequenceID { get; set; } = string.Empty;
        public string? AmountRequired { get; set; } = string.Empty;
        public string? LastUpdatedOn { get; set; } = string.Empty;
        public string? LastUpdatedBy { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public List<AdvanceBillDistributionDetails>? DistributionDetails { get; set; } = new List<AdvanceBillDistributionDetails>();
        public long? DistributionDetailsTotal { get; set; } = 0;

        public AdvanceApprovalBillDetailsPrintOut()
        {

        }
        public AdvanceApprovalBillDetailsPrintOut(DataRow dr)
        {
            SequenceID = dr["SequenceID"]?.ToString() ?? string.Empty;
            AmountRequired = dr["AmountRequired"]?.ToString() ?? string.Empty;
            LastUpdatedOn = dr["LastUpdatedOn"]?.ToString() ?? string.Empty;
            LastUpdatedBy = dr["LastUpdatedBy"]?.ToString() ?? string.Empty;
            Status = dr["Status"]?.ToString() ?? string.Empty;
        }
    }
}
