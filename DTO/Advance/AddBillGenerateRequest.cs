using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Advance
{
    public class AddBillGenerateRequest
    {
        public string? BillId { get; set; }
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
        
        public string? RefNo { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "StoreDate must be in YYYY-MM-DD format.")]
        public string? StoreDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "InitiateOn must be in YYYY-MM-DD format.")]
        public string? InitiateOn { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "GateDate must be in YYYY-MM-DD format.")]
        public string? GateDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "DepartmentDate must be in YYYY-MM-DD format.")]
        public string? DepartmentDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "BillDate must be in YYYY-MM-DD format.")]
        public string? BillDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "BillOnGate must be in YYYY-MM-DD format.")]
        public string? BillOnGate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "ExpBillDate must be in YYYY-MM-DD format.")]
        public string? ExpBillDate { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "TestingUpto must be in YYYY-MM-DD format.")]
        public string? TestingUpto { get; set; }
        [Required]
        public string? AdditionalName { get; set; }
        [Required]
        public string? Amount { get; set; }
        [Required]
        public string Purpose { get;  set; }
        [Required]
        public string BillNo { get;  set; }
        [Required]
        public string Department { get; set; }
        [Required]
        public string? Office { get;    set; }
        [Required]
        public int? Discount { get; set; }
        public string? CampusCode { get;  set; }
        public string? CampusName { get;  set; }
        [Required]
        [AllowedValues("Open","Close")]
        public string? BillStatus { get; set; }
        public string? Auth1Id { get; set; }
        public string? Auth2Id { get; set; }
        public string? Auth3Id { get; set; }
        public string? Auth4Id { get; set; }
        public string? Auth1Name { get; set; }
        public string? Auth2Name { get; set; }
        public string? Auth3Name { get; set; }
        public string? Auth4Name { get; set; }
        public string? RelativePersonId { get; set; }
        public string? RelativePersonName { get; set; }
        public string? RelativeDepartment { get; set; }
        public string? RelativeDesignation { get; set; }
        public string? FirmName { get; set; }
        public string? FirmPerson { get; set; }
        public string? FirmEmail { get; set; }
        public string? FirmPanNo { get; set; }
        public string? FirmAddress { get; set; }
        public string? FirmContactNo { get; set; }
        public string? FirmAlternateContactNo { get; set; }
        public string? VendorID { get; set; }
       
    }
}
