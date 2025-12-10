using System.Data;

namespace AdvanceAPI.DTO.Advance.Bill
{
    public class BillApprovalIssueHostelWiseDistribution
    {
        public string? HostelName { get; set; } = string.Empty;
        public string? From { get; set; } = string.Empty;
        public string? Upto { get; set; } = string.Empty;
        public string? Amount { get; set; } = string.Empty;


        public BillApprovalIssueHostelWiseDistribution()
        {

        }
        public BillApprovalIssueHostelWiseDistribution(DataRow dr)
        {
            HostelName = dr["HostelName"]?.ToString() ?? String.Empty;
            From = dr["From"]?.ToString() ?? String.Empty;
            Upto = dr["Upto"]?.ToString() ?? String.Empty;
            Amount = dr["Amount"]?.ToString() ?? String.Empty;
        }
    }
}
