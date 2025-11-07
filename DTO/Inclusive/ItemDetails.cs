using System.Data;

namespace AdvanceAPI.DTO.Inclusive
{
    public class ItemDetails
    {
        public string? ItemCode { get; set; } = string.Empty;
        public string? ItemName { get; set; } = string.Empty;
        public string? Make { get; set; } = string.Empty;
        public string? Size { get; set; } = string.Empty;
        public string? Unit { get; set; } = string.Empty;

        public ItemDetails() { }

        public ItemDetails(DataRow dr)
        {
            ItemCode = dr["ItemCode"]?.ToString() ?? string.Empty;
            ItemName = dr["ItemName"]?.ToString() ?? string.Empty;
            Make = dr["Make"]?.ToString() ?? string.Empty;
            Size = dr["Size"]?.ToString() ?? string.Empty;
            Unit = dr["Unit"]?.ToString() ?? string.Empty;
        }

    }
}
