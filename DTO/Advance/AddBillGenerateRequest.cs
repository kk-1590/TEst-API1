using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Advance
{
    public class AddBillGenerateRequest
    {
        [Required]
        public IFormFile? pdf { get;set; }
        [Required]
        public IFormFile? ExcelFile { get;set; }
        [Required]
        public string? VariationReason { get; set; }
        [Required]
        public string? NextBillTill { get; set; }
        [Required]
        public string? ForTypeOf { get; set; }
        [Required]
        public string? RefNo { get; set; }
        [Required]
        public string? StoreDate { get; set; }
        [Required]
        public string? GateDate { get; set; }
        [Required]
        public string? DepartmentDate { get; set; }
        [Required]
        public string? BillDate { get; set; }
        [Required]
        public string? BillOnGate { get; set; }
        [Required]
        public string? ExpBillDate { get; set; }
        [Required]
        public string? TestingUpto { get; set; }
        [Required]
        public string? AdditionalName { get; set; }



    }
}
