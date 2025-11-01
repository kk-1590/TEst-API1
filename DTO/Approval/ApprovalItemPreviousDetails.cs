using System.Data;

namespace AdvanceAPI.DTO.Approval
{
    public class ApprovalItemPreviousDetails
    {
        public string? ReferenceNo { get; set; } = string.Empty;
        public string? Quantity { get; set; } = string.Empty;
        public string? IniOn { get; set; } = string.Empty;
        public string? RecieveOn { get; set; } = string.Empty;

        public ApprovalItemPreviousDetails() { }

        public ApprovalItemPreviousDetails(DataRow dr)
        {
            ReferenceNo = dr["ReferenceNo"]?.ToString() ?? String.Empty;
            Quantity = dr["Quantity"]?.ToString() ?? String.Empty;
            IniOn = dr["IniOn"]?.ToString() ?? String.Empty;
            RecieveOn = dr["RecieveOn"]?.ToString() ?? String.Empty;
        }
    }
}
