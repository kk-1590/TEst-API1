using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.FirmPaidDetails
{
    public class FirmPaidrequest
    {
        [AllowedValues("Y")]
        public string? IsAdvance { get; set; }
        public string? Id { get;set; }
        public string? VId { get;set; }
        public string? SubId { get;set; }
        public string? CallBy { get;set; }
    }
}
