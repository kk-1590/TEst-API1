using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Inclusive
{
    public class GetPurchaseItemRequest
    {
        [Required]
        public string? DeptCode { get; set; }
        [Required]
        public string? SearchBy { get; set; }
        public string? ItemName { get; set; }
    }
}
