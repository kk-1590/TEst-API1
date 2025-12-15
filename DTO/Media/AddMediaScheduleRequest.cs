using AdvanceAPI.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Media
{
    public class AddMediaScheduleRequest
    {
        [Required(ErrorMessage = "Campus is required.")]
        [AllowedValues("101", "102", "103", ErrorMessage = "Sorry!! Invalid campus found.")]
        public string? CampusCode { get; set; }

        [Required(ErrorMessage = "Session is required.")]
        public string? Session { get; set; }

        [Required(ErrorMessage = "Schedule type is required.")]
        public string? ScheduleType { get; set; }

        [Required(ErrorMessage = "Media type is required.")]
        public string? MediaType { get; set; }

        [Required(ErrorMessage = "Media title is required.")]
        public string? MediaTitle { get; set; }

        [Required(ErrorMessage = "Schedule date is required.")]
        public string? ScheduleDate { get; set; }

        [Required(ErrorMessage = "Bill up to date is required.")]
        public string? BillUpTo { get; set; }

        [Required(ErrorMessage = "Advertisement type is required.")]
        public string? AdvertisementTypes { get; set; }

        [Required(ErrorMessage = "Edition is required.")]
        public string? Editions { get; set; }

        [Required(ErrorMessage = "Size-W is required.")]
        [Range(0, long.MaxValue, ErrorMessage = "Size-W must be greater than or equal to 0.")]
        public long? SizeW { get; set; }

        [Required(ErrorMessage = "Size-H is required.")]
        [Range(0, long.MaxValue, ErrorMessage = "Size-H must be greater than or equal to 0.")]
        public long? SizeH { get; set; }

        [Required(ErrorMessage = "Rate is required.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Rate must be greater than or equal to 0.")]
        public double? Rate { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0.")]
        public double? Amount { get; set; }

        [Required(ErrorMessage = "Discount is required.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Discount must be greater than or equal to 0.")]
        public double? Discount { get; set; }

        [Required(ErrorMessage = "Tax is required.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Tax must be greater than or equal to 0.")]
        public double? Tax { get; set; }

        [Required(ErrorMessage = "Final amount is required.")]
        [Range(0, long.MaxValue, ErrorMessage = "Final amount must be greater than or equal to 0.")]
        public long? FinalAmount { get; set; }


        [Required(ErrorMessage = "Page No is required.")]
        [Range(0, long.MaxValue, ErrorMessage = "Page No must be greater than or equal to 0.")]
        public long? PageNo { get; set; }


        [FileValidation(10 * 1024 * 1024, ".pdf")]
        public IFormFile? SupportingDocument { get; set; }
    }
}
