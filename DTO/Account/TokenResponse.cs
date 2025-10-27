namespace AdvanceAPI.DTO.Account
{
    public class TokenResponse
    {
        public string? EmployeeCode { get; set; }
        public string? AdditionalEmployeeCode { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? TypeAlways { get; set; }
        public string? MyRoles { get; set; }
        public string? AgainstRoles { get; set; }
        public string? Application { get; set; }


        public string? Token { get; set; }
        public long? Expires { get; set; }

        public string? RefreshToken { get; set; }
        public long? RefreshExpires { get; set; }
    }
}
