using System.Data;

namespace AdvanceAPI.DTO.Inclusive
{
    public class EmployeeDetails
    {
        public string? Name { get; set; }
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public string? ContactNo { get; set; }

        public EmployeeDetails() { }

        public EmployeeDetails(DataRow dr)
        {
            Name = dr["first_name"]?.ToString() ?? string.Empty;
            Department = dr["santioneddeptt"]?.ToString() ?? string.Empty;
            Designation = dr["deisgnation"]?.ToString() ?? string.Empty;
            ContactNo = dr["contactno"]?.ToString() ?? string.Empty;
        }
    }
}
