namespace AdvanceAPI.DTO.Approval
{
    public class DeleteApprovalItemRequest
    {
        public string? ReferenceNo { get; set; }
        public string? ItemCode { get; set; }
        public string? Reason { get; set; }
    }
}
