using System.Data;

namespace AdvanceAPI.DTO.Advance.Bill
{
    public class BillApprovalVehiclePreviousBills
    {
        public string? VehicleNo { get; set; } = string.Empty;
        public string? Particular { get; set; } = string.Empty;
        public string? Firm { get; set; } = string.Empty;
        public string? Amount { get; set; } = string.Empty;
        public string? PassOn { get; set; } = string.Empty;

        public BillApprovalVehiclePreviousBills()
        {

        }
        public BillApprovalVehiclePreviousBills(DataRow dr)
        {
            VehicleNo = dr["VehicleNo"]?.ToString() ?? String.Empty;
            Particular = dr["Particular"]?.ToString() ?? String.Empty;
            Firm = dr["Firm"]?.ToString() ?? String.Empty;
            Amount = dr["Amount"]?.ToString() ?? String.Empty;
            PassOn = dr["PassOn"]?.ToString() ?? String.Empty;
        }
    }
}
