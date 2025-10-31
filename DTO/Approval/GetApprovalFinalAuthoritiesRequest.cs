namespace AdvanceAPI.DTO.Approval
{
    public class GetApprovalFinalAuthoritiesRequest
    {
        public string? CampusCode { get; set; }
        public string? ApprovalType { get; set; }
        public int? Amount { get; set; }

    }
}
