using Microsoft.AspNetCore.Mvc;

namespace AdvanceAPI.DTO.Approval
{
    public class AprrovalsListRequest
    {
        public string? CampusCode { get; set; }
        public string? Session { get; set; }

        public string? Status { get; set; }
        public string? Department { get; set; }
        public string? ReferenceNo { get; set; }
        public int? ItemsFrom { get; set; }
        public int? NoOfItems { get; set; }
    }
}
