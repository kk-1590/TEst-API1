using System.Data;

namespace AdvanceAPI.DTO.Media
{
    public record MediaScheduleManagerCreateReleaseOrderAuthoritiesResponse
    {
        public string? EmployeeId { get; init; }
        public string? Name { get; init; }
        public string? Designation { get; init; }

        public MediaScheduleManagerCreateReleaseOrderAuthoritiesResponse()
        {

        }

        public MediaScheduleManagerCreateReleaseOrderAuthoritiesResponse(DataRow dr) : this(dr["Employee_Code"]?.ToString() ?? string.Empty, dr["Name"]?.ToString() ?? string.Empty, dr["Designation"]?.ToString() ?? string.Empty)
        {

        }

        public MediaScheduleManagerCreateReleaseOrderAuthoritiesResponse(string employeeId, string name, string designation)
        {
            EmployeeId = employeeId;
            Name = name;
            Designation = designation;
        }
    }
}
