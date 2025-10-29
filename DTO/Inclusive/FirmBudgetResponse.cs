namespace AdvanceAPI.DTO.Inclusive
{
    public class FirmBudgetResponse
    {
        public bool ApprovalAuthorized { get; set; } = true;
        public string BudgetReferenceNo { get; set; } = "Not Required";
        public string BudgetRequestStatus { get; set; } = "Not Required";
        public string BudgetAmont { get; set; } = "N/A";
        public string ReleasedAmount { get; set; } = "N/A";
    }
}
