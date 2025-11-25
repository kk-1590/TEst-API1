namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceApprovalResponse
    {
        public string? MyType { get;set; }
        public string? Status { get; set; }
        public string? ByPass { get; set; }
        public string? Test { get;set; }
        public string? BudgetAmount { get;set; }
        public string? TotalAmount { get;set; }
        public string? PreviousTaken { get;set; }
        public string? BudgetStatus { get;set; }
        public string? ReferenceNo { get;set; }
        public string? App1Status { get;set; }
        public string? App2Status { get;set; }
        public string? App3Status { get;set; }
        public string? App4Status { get;set; }
        public string? App1Name { get;set; }
        public string? App2Name { get;set; }
        public string? App3Name { get;set; }
        public string? App4Name { get;set; }
        public string? App1ID { get;set; }
        public string? App2ID { get;set; }
        public string? App3ID { get;set; }
        public string? App4ID { get;set; }
        public string? App1On { get;set; }
        public string? App2On { get;set; }
        public string? App3On { get;set; }
        public string? App4On { get;set; }
        public string? IniBy { get;set; }
        public string? IniOn { get;set; }
        public string? IniById { get;set; }
        public string? FinalStat { get;set; }
        public string? RejectedReason { get;set; }
        public string? CancelledReason { get;set; }
        public string? CancelledBy { get;set; }
        public string? CancelledOn { get;set; }
        public string? BillId { get;set; }
        public bool CanReject { get; set; } = false;
        public bool CanCancel { get; set; } = false;
        public bool CanApprove { get; set; }=false;
        public string? RelativePerson { get; set; }
        public string? CampusName { get; set; }
        public string? DepartMent { get;  set; }
        public string? RelativePersonID { get; set; }
        public string? RelativeDepartment { get;  set; }
        public string? FirmName { get;  set; }
        public string? VendorId { get;  set; }
        public string? BillUpTo { get; internal set; }
        public string? Note { get; internal set; }
        public string? Purpose { get; internal set; }
        public string? Extra { get; internal set; }
        public string App1Photo { get; internal set; }
        public string App2Photo { get; internal set; }
        public string App3Photo { get; internal set; }
        public string App4Photo { get; internal set; }
        public string IniByPhoto { get; internal set; }
        public string? AppDate { get; internal set; }
        public int ApprovalNo { get; internal set; }
    }
}
