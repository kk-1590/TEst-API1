using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Media
{
    public class EditMediaScheduleRequest
    {
        [Required(ErrorMessage = "Please provide the media schedule id first.")]
        public string? Id { get; set; }

        [Required]
        public string? NewScheduleOn { get; set; }

        [Required]
        public string? NewBillOn { get; set; }

        [Required]
        public string? NewMediaType { get; set; }

        [Required]
        public string? NewMediaTitle { get; set; }

        [Required]
        public string[]? NewAdvertisements { get; set; }

        [Required]
        public string[]? NewEditions { get; set; }

        [Required]
        public string? NewWidth { get; set; }

        [Required]
        public string? NewHeight { get; set; }

        [Required]
        public string? NewRate { get; set; }

        [Required]
        public string? NewAmount { get; set; }

        [Required]
        public string? NewDiscount { get; set; }

        [Required]
        public string? NewTax { get; set; }

        [Required]
        public string? NewFinalAmount { get; set; }

        [Required]
        public string? NewPageNo { get; set; }

        [Required(ErrorMessage = "Please provide the reason for editing the media schedule.")]
        [MinLength(10, ErrorMessage = "The reason must be at least 10 characters long.")]
        public string? Reason { get; set; }

    }
}
