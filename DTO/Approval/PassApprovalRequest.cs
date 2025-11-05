namespace AdvanceAPI.DTO.Approval
{
    public class PassApprovalRequest
    {
        public string? ReferenceNo { get; set; } = string.Empty;
        public string? AuthorityNumber { get; set; } = string.Empty;
        public string? AproveRemark { get; set; } = string.Empty;
    }
}
