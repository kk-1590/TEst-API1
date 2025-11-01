using System.Data;

namespace AdvanceAPI.DTO.Approval
{
    public class ApprovalPaymentDetailsList
    {
        public string? PaidAmount { get; set; } = string.Empty;
        public string? TaxAmount { get; set; } = string.Empty;
        public string? IssuedAmount { get; set; } = string.Empty;
        public string? TransactionNo { get; set; } = string.Empty;
        public string? IssuedOn { get; set; } = string.Empty;
        public string? IssuedByName { get; set; } = string.Empty;
        public string? SignedOn { get; set; } = string.Empty;
        public string? ReceivedOn { get; set; } = string.Empty;
        public string? Bank { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;

        public ApprovalPaymentDetailsList()
        {

        }

        public ApprovalPaymentDetailsList(DataRow dr)
        {
            PaidAmount = dr["PaidAmount"]?.ToString() ?? string.Empty;
            TaxAmount = dr["TaxAmount"]?.ToString() ?? string.Empty;
            IssuedAmount = dr["IssuedAmount"]?.ToString() ?? string.Empty;
            TransactionNo = dr["TransactionNo"]?.ToString() ?? string.Empty;
            IssuedOn = dr["IssuedOn"]?.ToString() ?? string.Empty;
            IssuedByName = dr["IssuedByName"]?.ToString() ?? string.Empty;
            SignedOn = dr["SignedOn"]?.ToString() ?? string.Empty;
            ReceivedOn = dr["ReceivedOn"]?.ToString() ?? string.Empty;
            Bank = dr["Bank"]?.ToString() ?? string.Empty;
            Status = dr["Status"]?.ToString() ?? string.Empty;
        }
    }
}
