using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class AddDepartmentSummaryUpdateRequest: AddDepartmentSummaryRequest
    {
        [Required]
        public int Id { get; set; } 
    }
}
