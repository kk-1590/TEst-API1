using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class BudgetSessionAmountSummaryRequest : PaginationSectionRequest
    {
        public string? CampusCode { get; set; }
        public string? Session { get; set; }



    }
}
