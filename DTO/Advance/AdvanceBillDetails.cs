using System.Data;

namespace AdvanceAPI.DTO.Advance
{
    public class AdvanceBillDetails
    {
        public string? ReferenceNo { get; set; } = string.Empty;
        public string? Head { get; set; } = string.Empty;
        public long? Unit { get; set; } = 0;
        public long? Price { get; set; } = 0;
        public long? Amount { get; set; } = 0;
        public string? MonthName { get; set; } = string.Empty;
        public string? Remark { get; set; } = string.Empty;
        public long? PrevAmount { get; set; } = 0;
        public long? AnyDiscount { get; set; } = 0;
        public string? DiscountRemark { get; set; } = string.Empty;
        public bool? IsHighlighted { get; set; } = false;

        public AdvanceBillDetails()
        {

        }

        public AdvanceBillDetails(DataRow dr)
        {
            ReferenceNo = dr["ReferenceNo"]?.ToString() ?? string.Empty;
            Head = dr["Head"]?.ToString() ?? string.Empty;
            Unit = dr["Unit"] != DBNull.Value ? Convert.ToInt64(dr["Unit"]) : 0;
            Price = dr["Price"] != DBNull.Value ? Convert.ToInt64(dr["Price"]) : 0;
            Amount = dr["Amount"] != DBNull.Value ? Convert.ToInt64(dr["Amount"]) : 0;
            MonthName = dr["MonthName"]?.ToString() ?? string.Empty;
            Remark = dr["Remark"]?.ToString() ?? string.Empty;
            PrevAmount = dr["PrevAmount"] != DBNull.Value ? Convert.ToInt64(dr["PrevAmount"]) : 0;
            AnyDiscount = dr["AnyDiscount"] != DBNull.Value ? Convert.ToInt64(dr["AnyDiscount"]) : 0;
            DiscountRemark = dr["DiscountRemark"]?.ToString() ?? string.Empty;
        }
    }
}
