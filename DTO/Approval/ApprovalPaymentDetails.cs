using System.Data;

namespace AdvanceAPI.DTO.Approval
{


    public class ApprovalPaymentDetails
    {
        public double? TotalAmount { get; set; } = 0;
        public double? TotalIssueAmount { get; set; } = 0;
        public string? PaymentDetailsComment { get; set; } = string.Empty;
        public List<ApprovalPaymentDetailsList>? PaymentList { get; set; } = new List<ApprovalPaymentDetailsList>();

        public ApprovalPaymentDetails()
        {
        }
    }


}
