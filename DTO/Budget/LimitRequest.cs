using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class LimitRequest
    {
        public int ItemsFrom { get; set; }
        public int NoOfItems { get; set; }
        [Required]
        [AllowedValues("101","102","103")]
        public string? CampusCode { get; set; }

        [Required]
        public string? Session { get; set; }

       
        public string? BudgetRequired { get; set; }
    }
}
