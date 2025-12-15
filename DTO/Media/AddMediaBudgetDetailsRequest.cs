using AdvanceAPI.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Media
{
    public class AddMediaBudgetDetailsRequest
    {
        [Required(ErrorMessage = "Campus is required.")]
        [AllowedValues("101", "102", "103", ErrorMessage = "Invalid campus found.")]
        public string? CampusCode { get; set; }

        [Required(ErrorMessage = "Session is required.")]
        public string? Session { get; set; }

        [Required(ErrorMessage = "Schedule Type is required.")]
        [AllowedValues("Advertisement", ErrorMessage = "Invalid schedule type found.")]
        public string? ScheduleType { get; set; }

        [Required(ErrorMessage = "Media Type is required.")]
        public string? MediaType { get; set; }

        [Required(ErrorMessage = "Head is required.")]
        public string? Head { get; set; }

        [Required(ErrorMessage = "Media Type Id is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "Media Type Id must be valid.")]
        public long? MediaTypeId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public long? Amount { get; set; }

        [FileValidation(10 * 1024 * 1024, ".pdf")]
        public IFormFile? SupportingDocument { get; set; }
    }
}
