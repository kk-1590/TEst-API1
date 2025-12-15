using AdvanceAPI.ENUMS.Media;
using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Media
{
    public class GetMediaSchedulesRequest : PaginationSectionRequest
    {
        [Required(ErrorMessage = "Campus is required")]
        public string? CampusCode { get; set; }

        [Required(ErrorMessage = "Session is required")]
        public string? Session { get; set; }

        [Required(ErrorMessage = "Schedule Type is required")]
        public string? ScheduleType { get; set; }


        public string? MediaType { get; set; }

        public string? Title { get; set; }



        [Required(ErrorMessage = "SortBy is required")]
        [AllowedValues("DateWiseTypeWise", "TypeWiseDateWise", ErrorMessage = "SortBy must be either 'DateWiseTypeWise' or 'TypeWiseDateWise'")]
        public string? SortBy { get; set; }
    }
}
