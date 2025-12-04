using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Advance
{
    public class GetApprovalBillRequest
    {
        public string? Session { get; set; }
        [Required]
        public string? Type { get; set; }
        public string? ReferenceNo { get; set; }
        [Required]
        public string? Category { get; set; }
        public string? Campus { get; set; }
        public string? VendorId { get; set; }
        [Required]
        public int NoOfItems {  get; set; }
        [Required]
        public int ItemsFrom { get; set; }

    }
}
