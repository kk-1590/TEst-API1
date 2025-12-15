using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Media
{
    public class GetMediaTypeHeadsRequest : PaginationSectionRequest
    {
        [Required(ErrorMessage = "Schedule Type is required")]
        public string? ScheduleType { get; set; }

        public string? MediaType { get; set; }

        public string? Head { get; set; }
    }
}
