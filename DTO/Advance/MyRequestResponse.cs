namespace AdvanceAPI.DTO.Advance
{
    public class MyRequestResponse
    {
        public string? Status { get;set; }
        public string? App1Status { get;set; }
        public string? App2Status { get;set; }
        public string? App3Status { get;set; }
        public string? App4Status { get;set; }
        public bool CanDelete { get; set; } = false;
        public string? BudgetAmount { get; set; }
        public string? PreviousTaken { get;  set; }
        public string? MyType { get;  set; }
        public string? CurStatus { get; set; }
        public string? BudgetStatus { get; set; }
        public string? ReferenceNo { get; set; }
        public string? App1Name { get; set; }
        public string? App2Name { get; set; }
        public string? App3Name { get; set; }
        public string? App4Name { get; set; }
        public string? App1On { get;  set; }
        public string? App2On { get; set; }
        public string? App3On { get; set; }
        public string? App4On { get; set; }
        public string? RejectedReason { get; set; }
        public string? IniOn { get;  set; }
        public string? Session { get;  set; }
        public string? Purpose { get; set; }
        public string? TotalAmount { get; set; }
        public string? IniName { get; set; }
        public string? AppDate { get; set; }
        public string? BudgetReferenceNo { get; set; }
        public string? ExtraDetails { get; set; }
        public string? BillUpTo { get; set; }
        public string? App1Photo { get; set; }
        public string? App2Photo { get; set; }
        public string? App3Photo { get; set; }
        public string? App4Photo { get; set; }
        public string? IniByPhoto { get; set; }
        public string? App1ID { get; set; }
        public string? App2ID { get; set; }
        public string? App3ID { get; set; }
        public string? App4ID { get;    set; }
        public string? IniID { get;  set; }
        public string? CampusName { get; internal set; }
    }
}
