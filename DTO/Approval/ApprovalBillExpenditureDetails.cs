using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace AdvanceAPI.DTO.Approval
{
    public class ApprovalBillExpenditureDetails
    {
        public string? Title { get; set; } = string.Empty;
        public string? Amount { get; set; } = string.Empty;

        public ApprovalBillExpenditureDetails()
        {

        }

        public ApprovalBillExpenditureDetails(DataRow dr)
        {
            Title = dr["Title"]?.ToString() ?? string.Empty;
            Amount = dr["Amount"]?.ToString() ?? string.Empty;
        }
    }

}
