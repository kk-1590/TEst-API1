namespace AdvanceAPI.DTO.Approval
{
    public class FinalAuthoritiesResponse
    {
        public string? Name { get; set; }
        public string? EmployeeCode { get; set; }
        public string? Designation { get; set; }
        public string? ApprovalCategory { get; set; }
        public long? LimitFrom { get; set; }
        public long? LimitTo { get; set; }
    }
}
