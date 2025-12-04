using System.Data;

namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceEmployeeLeaveDetails
    {
        public string? Name { get; set; } = string.Empty;
        public string? LeaveType { get; set; } = string.Empty;
        public string? LeaveFrom { get; set; } = string.Empty;
        public string? LeaveTo { get; set; } = string.Empty;
        public AdvanceEmployeeLeaveDetails()
        {

        }

        public AdvanceEmployeeLeaveDetails(DataRow dr)
        {
            Name = dr["Name"]?.ToString() ?? string.Empty;
            LeaveType = dr["LeaveType"]?.ToString() ?? string.Empty;
            LeaveFrom = dr["LeaveFrom"]?.ToString() ?? string.Empty;
            LeaveTo = dr["LeaveTo"]?.ToString() ?? string.Empty;
        }
    }
}
