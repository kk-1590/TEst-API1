using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Media
{
    public class GetMediaBudgetDetailsRequest : PaginationSectionRequest
    {

        [Required(ErrorMessage = "Campus is required")]
        [AllowedValues("101", "102", "103", ErrorMessage = "Sorry!! Invalid Campus Found.")]
        public string? CampusCode { get; set; }

        [Required(ErrorMessage = "Session is required")]
        public string? Session { get; set; }

        [Required(ErrorMessage = "Schedule Type is required")]
        [AllowedValues("Advertisement", ErrorMessage = "Sorry!! Invalid Schedule Type Found.")]
        public string? ScheduleType { get; set; }

        public string? MediaType { get; set; }

        public string? Head { get; set; }

    }
}
