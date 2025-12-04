using System.Data;

namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceBillDistributionDetails
    {
        public string? Title { get; set; } = string.Empty;
        public long? Qty { get; set; } = 0;
        public long? Price { get; set; } = 0;
        public long? Amount { get; set; } = 0;
        public bool? IsHighlighted { get; set; } = false;

        public AdvanceBillDistributionDetails()
        {

        }
        public AdvanceBillDistributionDetails(DataRow dr)
        {
            this.Title = dr["Title"]?.ToString() ?? string.Empty;
            this.Qty = dr["Qty"] != DBNull.Value ? Convert.ToInt64(dr["Qty"]) : 0;
            this.Price = dr["Price"] != DBNull.Value ? Convert.ToInt64(dr["Price"]) : 0;
            this.Amount = dr["Amount"] != DBNull.Value ? Convert.ToInt64(dr["Amount"]) : 0;
        }
    }
}
