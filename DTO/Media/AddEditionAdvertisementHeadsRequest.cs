using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Media
{
    public class AddEditionAdvertisementHeadsRequest
    {
        [Required(ErrorMessage = "Schedule Type is required")]
        public string? ScheduleType { get; set; }


        [Required(ErrorMessage = "Type is required")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "Head is required")]
        public string? Head { get; set; }
    }
}
