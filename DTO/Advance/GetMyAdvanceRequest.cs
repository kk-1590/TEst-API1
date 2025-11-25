namespace AdvanceAPI.DTO.Advance
{
    public class GetMyAdvanceRequest
    {
        public string? CampusCode { get;set; }
        public string? Status { get;set; }
        public string? ReferenceNo { get;set; }
        public string? Session { get;set; }
        public string? Department { get;set; }
        public int NoOfItems { get; set; } = 0;
        public int ItemsFrom { get; set; } = 0;

    }
}
