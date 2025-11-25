using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.Advance
{
    public class GenerateAdvancerequest
    {
        [Required]
        public string? ApprovalType { get;set; }
        [Required]
        public string? Office { get;set; }
        [Required]
        public string? Session { get;set; }
        [Required]
        public string? Note { get; set; }
        [Required]
        public string? Purpose { get;set; }
        [Required]
        public string? BillUptoValue { get;set; }
        [Required]
        public string? BillUptoType { get;set; }
        [Required]
        public string? ForDepartment { get;set; }
        [Required]
        public string? Amount { get;set; }
        [Required]
        public string? TotalAmount { get;set; }
        [Required]
        public string? App1ID { get;set; }
        [Required]
        public string? App1Name { get;set; }
        [Required]
        public string? App1Designation { get;set; }
        [Required]
        public string? App1Status { get;set; }
        [Required]
        public string? App2ID { get;set; }
        [Required]
        public string? App2Name { get;set; }
        [Required]
        public string? App2Designation { get;set; }
        [Required]
        public string? App2Status { get;set; }
        [Required]
        public string? App3ID { get;set; }
        [Required]
        public string? App3Name { get;set; }
        [Required]
        public string? App3Designation { get;set; }
        [Required]
        public string? App3Status { get;set; }
        [Required]
        public string? App4ID { get;set; }
        [Required]
        public string? App4Name { get;set; }
        [Required]
        public string? App4Designation { get;set; }
        [Required]
        public string? App4Status { get;set; }
        [Required]
        public string? Category { get; set; }
        [Required]
        public string? MessageSend { get; set; }
        [Required]
        public string? VendorID { get; set; }
        [Required]
        public string? RelativePersonID { get; set; }
        [Required]
        public string? RelativePersonName { get; set; }
        [Required]
        public string? RelativeDesignation { get; set; }
        [Required]
        public string? RelativeDepartment { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "DateTill must be in YYYY-MM-DD format.")]
        public string? AppDate { get; set; }
        [Required]
        public string? ReferenceBillStatus { get; set; }
        [Required]
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "DateTill must be in YYYY-MM-DD format.")]
        public string? BillTill { get; set; }
        [Required]
        public string? ExtendedBillDate { get; set; }
        [Required]
        public string? R_Pending { get; set; }
        [Required]
        public string? R_Status { get; set; }
        [Required]
        public string? PExtra3 { get; set; }
        [Required]
        public string? PExtra2 { get; set; }
        [Required]
        public string? PExtra4 { get; set; }
        [Required]
        public string? BudgetRequired { get; set; }
        [Required]
        public string? BudgetAmount { get; set; }
        [Required]
        public string? PreviousTaken { get; set; }
        [Required]
        public string? BudgetStatus { get; set; }
        [Required]
        public string? CurStatus { get; set; }
        [Required]
        public string? BudgetReferenceNo { get; set; }
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "DateTill must be in YYYY-MM-DD format.")]
        public string? DistFrom { get; set; }
        [RegularExpression(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "DateTill must be in YYYY-MM-DD format.")]
        public string? DistTo { get; set; }
        [Required]
        [AllowedValues("101","102","103")]
        public string? CampusCode { get; set; }
        public string? CampusName { get; set; }




    }
}
