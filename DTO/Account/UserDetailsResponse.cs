using System.Data;

namespace AdvanceAPI.DTO.Account
{
    public class UserDetailsResponse
    {
        public string? UserId { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? FatherName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string? Role { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? ContactNo { get; set; } = string.Empty;

        public UserDetailsResponse()
        {

        }

        public UserDetailsResponse(DataRow dr)
        {
            if (dr != null)
            {
                UserId = dr["UserId"]?.ToString() ?? string.Empty;
                Name = dr["Name"]?.ToString() ?? string.Empty;
                FatherName = dr["FatherName"]?.ToString() ?? string.Empty;
                Gender = dr["Gender"]?.ToString() ?? string.Empty;
                Role = dr["Role"]?.ToString() ?? string.Empty;
                Status = dr["Status"]?.ToString() ?? string.Empty;
                Email = dr["Email"]?.ToString() ?? string.Empty;
                ContactNo = dr["ContactNo"]?.ToString() ?? string.Empty;
            }
        }
    }
}
