using System.Data;

namespace AdvanceAPI.DTO.Approval
{
    public class ApprovalRepairMaintinanceDetails
    {
        public string? Name { get; set; } = string.Empty;
        public string? SerialNo { get; set; } = string.Empty;
        public string? ApprovalDate { get; set; } = string.Empty;
        public string? Amount { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;

        public ApprovalRepairMaintinanceDetails()
        {

        }
        public ApprovalRepairMaintinanceDetails(DataRow dr)
        {
            string[]? repairDetails = (dr[0]?.ToString() ?? string.Empty).Split('|');
            if (repairDetails.Length >= 2)
            {
                Name = repairDetails[1];
                SerialNo = repairDetails[0];
                ApprovalDate = dr["App"]?.ToString() ?? string.Empty;
                Amount = dr["Amount"]?.ToString() ?? string.Empty;
                Status = dr["Status"]?.ToString() ?? string.Empty;
            }
        }
    }
}
