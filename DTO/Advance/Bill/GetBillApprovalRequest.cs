using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Advance.Bill
{
    public class GetBillApprovalRequest : PaginationSectionRequest
    {

        public string? CampusCode { get; set; }

        [Required]
        public string? Session { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public string? Category { get; set; }

        public string? ChequeBy { get; set; }
        public string? Department { get; set; }
        public string? InitiatedBy { get; set; }
        public string? ReferenceNo { get; set; }
        public string? SequenceId { get; set; }
        public string? AdditionalEmployeeCode { get; set; }
    }
}
