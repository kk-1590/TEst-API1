using AdvanceAPI.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Media
{
    public class EditMediaScheduleFileRequest
    {
        [Required(ErrorMessage = "Schedule Id is required")]
        public string Id { get; set; }

        [FileValidation(10 * 1024 * 1024, ".pdf")]
        public IFormFile? SupportingDocument { get; set; }
    }
}
