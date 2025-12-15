using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Media
{
    public class GetCanCreateMediaReleaseOrderDetailsRequest
    {
        [Required(ErrorMessage = "Campus is required.")]
        public string? CampusCode { get; set; }

        [Required(ErrorMessage = "Session is required.")]
        public string? Session { get; set; }

        [Required(ErrorMessage = "Schedule type is required.")]
        public string? ScheduleType { get; set; }

        [Required(ErrorMessage = "Media type is required.")]
        public string? MediaType { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string? Title { get; set; }

    }
}
