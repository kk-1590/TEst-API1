using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Budget
{
    public class MapNewMaad
    {
        [AllowedValues(101,102,103)]
        [Required]
        public int CampusCode {  get; set; }
        [Required]
        public string? Maad { get; set; }
        [Required]
        [AllowedValues(0,1)]
        public int BusgetRequired { get; set; }
    }
}
