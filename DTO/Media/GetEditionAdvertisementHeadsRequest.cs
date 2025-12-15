using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Media
{
    public class GetEditionAdvertisementHeadsRequest : PaginationSectionRequest
    {
        [Required(ErrorMessage = "Schedule Type is required")]
        public string? ScheduleType { get; set; }

        public string? Type { get; set; }

        public string? Head { get; set; }
    }
}
