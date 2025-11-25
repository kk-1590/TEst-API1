using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Advance
{
    public class PassApprovalRequest
    {
        [Required]
        public int SeqNo { get; set; }
        
        public string? Remark { get; set; }
        [Required]
        public string? RefNo { get; set; }
        
        public string? Bypass { get;set; }
        [Required]
        public string? Designation { get;set; }
        
    }
}
