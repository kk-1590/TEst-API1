using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class GetDepartmentBudgetSummaryRequest
    {
        [Required]
        public string? CampusCode { get; set; }

        [Required]
        public string? Session { get; set; }

        public string? Department { get; set; }

        [Required]
        public int? RecordFrom { get; set; }

        [Required]
        public int? NoOfRecords { get; set; }
    }
}
