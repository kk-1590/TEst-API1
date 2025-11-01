namespace AdvanceAPI.DTO.Approval
{
    public class DeleteApprovalDraftRequest
    {
        public string? CampusCode { get; set; }
        public string? ApprovalType { get; set; }
        public string? ReferenceNo { get; set; }
    }
}
