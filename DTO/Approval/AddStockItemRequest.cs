using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Approval
{
    public class AddStockItemRequest
    {
        [Required]
        [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "The discount must have a maximum of two decimal places.")]        public decimal ActualAmount {  get; set; }
        [Required]
        public string? ApprovalType { get;set; }
        [Required]
        public int Balance { get;set; }
        [Required]
        [AllowedValues(101,102,103)]
        public int CampusCode { get;set; }
        [Required]
        public string? ChangeReason { get;set; }
        [Required]
        public string CurrentRate { get;set; }
        [Required]
        [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "The discount must have a maximum of two decimal places.")]
        public decimal DiscountPercent { get;set; }
        [Required]
        public string? ItemCode { get;set; }
        [Required]
        public string? ItemName { get;set; }
        [Required]
        public string? Make { get; set; }
        [Required]
        public string PrevRate { get; set; }
        [Required]
       // [RegularExpression(@"^\d$", ErrorMessage = "integer value Is Required")]
        public int Quantity { get; set; }
        [Required]
        public string? Size { get; set; }
        [Required]
        [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "The discount must have a maximum of two decimal places.")]
        public decimal TotalAmount { get; set; }
        [Required]
        public string? Unit { get; set; }
        [Range(0.00, 100, ErrorMessage = "GST percentage must be greater than 0 and up to 100.")]
        [RegularExpression(@"^\d+\.\d{2}$", ErrorMessage = "The discount must have a maximum of two decimal places.")]
        public decimal? GstPer { get; set; }
        [Required]
       // [RegularExpression(@"^\d$", ErrorMessage = "integer value Is Required")]
        public int Warranty { get; set; }
        [Required]
        [AllowedValues("Day","Month","Year")]
        public string? WarrantyType { get; set; }
        
    }
    
}
