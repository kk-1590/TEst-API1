using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AdvanceAPI.DTO.Advance
{
    public class purchaseApprovalDetailsResponse
    {
        public string? Department { get;set; }
        public string? Purpose { get;set; }
        public string? Note { get;set; }
        public string? BalanceAmount { get;set; }
        public List<TextValues>? Vendorlst { get;set; }
        public List<TextValues>? VendorOffice { get;set; }
        
    }
    public class TextValues
    {
        [Required]
        public string? Value { get; set; }
        [Required]
        public string? Text { get;set; }
        public string? EmpCode { get;set; }
        public TextValues() { }
        public TextValues(DataRow dr)
        {
            Value = dr["Value"].ToString();
            Text = dr["Text"].ToString();
        }
    }
}
