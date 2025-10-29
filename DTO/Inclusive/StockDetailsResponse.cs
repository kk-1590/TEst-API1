namespace AdvanceAPI.DTO.Inclusive
{
    public class StockDetailsResponse
    {
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? Make { get; set; }   
        public string? Size { get; set; }
        public string? PrevRate { get; set; }
        public string? PrevPurchase { get; set; }
        public string? Unit { get; set; }

        public string? Stock { get;set; }
    }
}
