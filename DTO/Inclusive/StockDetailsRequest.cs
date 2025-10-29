using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Inclusive
{
    public class StockDetailsRequest
    {
        [Required]
        public string? ItemCode { get; set; }
        [Required]
        public int CampusCode { get; set; }
    }
}
