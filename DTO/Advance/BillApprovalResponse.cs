namespace AdvanceAPI.DTO.Advance
{
    public class BillApprovalResponse
    {
        public string? TransactionId { get;set; }
        public string? InitOn { get;set; }
        public string? InitBy { get;set; }
        public string? FirmName { get;set; }
        public string? Purpose { get;set; }
        public string? SenAmount { get;set; }
        public string? Paid { get;set; }
        public string? PaidOn { get;set; }
        public string? Status { get;set; }
        public string? AmountRemaining { get;set; }
        public string? IssuidName { get;set; }
        public string? IsSpacial { get;set; }
        public string? CashDiscount { get;set; }
        public string? ExtraBill { get;set; }
        public string? Col5 { get;set; }
        public string? CampusName { get;set; }
        public string? PurposeLink {  get;set; }
        public string? NewIni {  get;set; }
        public string? NewExp {  get;set; }
        public string? NewBill {  get;set; }
        public string? NewGate {  get;set; }
        public string? BillExtra6 {  get;set; }
        public string? NewTest {  get;set; }
        public bool CanEditDate {  get;set; }
        public string? MyINICheck {  get;set; }
        public string? Limit {  get;set; }
        public List<Authorities>? AuthList {  get;set; }
        public string IssImg { get; internal set; }
        public string? IssuedOn { get; internal set; }
        public string? IssuedBy { get; internal set; }
    }
    public class Authorities
    {
        public string? Name { get;set; }
        public string? On { get;set; }
        public string? Status { get;set; }
        public string? EmployeeId { get;set; }
        public string? Img { get;set; }
    }
    
}
