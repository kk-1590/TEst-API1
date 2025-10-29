using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Inclusive
{
    public class GetFirmBudgetRequest
    {
        [Required]
        public string? VendorId { get; set; }

        [Required]
        public string? SubFirm { get; set; }
    }
}
