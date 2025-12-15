using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Advance.BillApproval
{
    public class ApprovalRequest
    {
        [Required]
        public string? TransactionId { get; set; }
        [Required]
        public string? SeqNo { get; set; }
        public string? Remark { get; set; }
        [Required]
        public string? Designation { get; set; }
    }
}
