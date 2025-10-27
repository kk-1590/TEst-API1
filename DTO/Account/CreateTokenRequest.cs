namespace AdvanceAPI.DTO.Account
{
    public class CreateTokenRequest
    {
        public string? EmployeeCode { get; set; }
        public string? AdditionalEmployeeCode { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? TypeAlways { get; set; }
        public string? MyRoles { get; set; }
        public string? AgainstRoles { get; set; }
        public string? Application { get; set; }
        public string? RefreshTokenId { get; set; }
    }
}
