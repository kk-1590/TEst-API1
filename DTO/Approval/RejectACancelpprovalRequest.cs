namespace AdvanceAPI.DTO.Approval
{
    public class RejectACancelpprovalRequest
    {
        public string? ReferenceNo { get; set; } = string.Empty;
        public string? AuthorityNumber { get; set; } = string.Empty;
        public string? Reason { get; set; } = string.Empty;
    }
}
