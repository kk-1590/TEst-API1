using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class UpdateMaadBudegtRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Maad { get; set; }
        [Required]
        public int CampusCode { get; set; }
        [Required]
        public string? Session { get; set; }
        [Required]
        public int Status {  get; set; }
        [Required]
        public int BudgetRequired { get;set; }
        [Required]
        public string? Remark { get;set; }

    }
}
