using Microsoft.VisualBasic;
using System.Data;
using System.Transactions;

namespace AdvanceAPI.DTO.Advance
{
    public class AdvancePaymentDetails
    {
        public string? PaidAmount { get; set; } = string.Empty;
        public string? TaxAmount { get; set; } = string.Empty;
        public string? IssuedAmount { get; set; } = string.Empty;
        public string? TranactionOn { get; set; } = string.Empty;
        public string? IssuedOn { get; set; } = string.Empty;
        public string? IssuedBy { get; set; } = string.Empty;
        public string? SignedOn { get; set; } = string.Empty;
        public string? RecievedOn { get; set; } = string.Empty;
        public string? Bank { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public bool? ShowFinalImage { get; set; } = false;

        public AdvancePaymentDetails() { }

        public AdvancePaymentDetails(DataRow dr)
        {
            this.PaidAmount = dr["PaidAmount"]?.ToString() ?? string.Empty;
            this.TaxAmount = dr["TaxAmount"]?.ToString() ?? string.Empty;
            this.IssuedAmount = dr["IssuedAmount"]?.ToString() ?? string.Empty;
            this.TranactionOn = dr["TranactionOn"]?.ToString() ?? string.Empty;
            this.IssuedOn = dr["IssuedOn"]?.ToString() ?? string.Empty;
            this.IssuedBy = dr["IssuedBy"]?.ToString() ?? string.Empty;
            this.SignedOn = dr["SignedOn"]?.ToString() ?? string.Empty;
            this.RecievedOn = dr["RecievedOn"]?.ToString() ?? string.Empty;
            this.Bank = dr["Bank"]?.ToString() ?? string.Empty;
            this.Status = dr["Status"]?.ToString() ?? string.Empty;
        }
    }
}
