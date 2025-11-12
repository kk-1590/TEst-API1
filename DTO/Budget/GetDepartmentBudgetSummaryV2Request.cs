using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class GetDepartmentBudgetSummaryV2Request : PaginationSectionRequest
    {

        [Required]
        public string? CampusCode { get; set; }

        [Required]
        public string? Session { get; set; }

        public string? Department { get; set; }

        public string? ReferenceNo { get; set; }

    }
}
