using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Media
{
    public class AddNewMediaTypeHeadRequest
    {
        [Required(ErrorMessage = "Schedule Type is required.")]
        public string? ScheduleType { get; set; }

        [Required(ErrorMessage = "Media Type is required.")]
        public string? MediaType { get; set; }

        [Required(ErrorMessage = "Head is required.")]
        public string? Head { get; set; }
    }
}
