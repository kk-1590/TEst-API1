using System.Data;

namespace AdvanceAPI.DTO.Advance.Bill
{
    public class BillApprovalIssueVehicleDistribution
    {
        public string? VehicleNo { get; set; } = string.Empty;
        public string? For { get; set; } = string.Empty;
        public string? From { get; set; } = string.Empty;
        public string? Upto { get; set; } = string.Empty;
        public string? Amount { get; set; } = string.Empty;


        public BillApprovalIssueVehicleDistribution()
        {

        }

        public BillApprovalIssueVehicleDistribution(DataRow dr)
        {
            VehicleNo = dr["VehicleNo"]?.ToString() ?? String.Empty;
            For = dr["For"]?.ToString() ?? String.Empty;
            From = dr["From"]?.ToString() ?? String.Empty;
            Upto = dr["Upto"]?.ToString() ?? String.Empty;
            Amount = dr["Amount"]?.ToString() ?? String.Empty;
        }
    }
}
