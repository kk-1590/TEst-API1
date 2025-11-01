using System.Security.Cryptography.Xml;

namespace AdvanceAPI.DTO.Approval
{
    public class DraftResponse
    {
        //AppType,DraftName,CampusCode,COUNT(ReferenceNo) 'ItemCount',SUM(Balance) 'TotalBalance'
        public string? AppType { get; set; }
        public string? DraftName { get;set; }
        public string? CampusCode { get; set; }
        public int ItemCount { get; set; }
        public string? Balance { get; set; }
        public string? ReferenceNo { get; set; }
        public string? CampusName { get; set; }
    }
}
