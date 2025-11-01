using System.Data;

namespace AdvanceAPI.DTO.Approval
{
    public class ApprovalWarrantyDetails
    {
        public string? Name { get; set; } = string.Empty;
        public string? SerialNo { get; set; } = string.Empty;
        public string? From { get; set; } = string.Empty;
        public string? To { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;

        public ApprovalWarrantyDetails()
        {

        }
        public ApprovalWarrantyDetails(DataRow dr)
        {
            string[]? warrentyDetails = (dr[0]?.ToString() ?? string.Empty).Split('|');
            if (warrentyDetails.Length >= 5)
            {
                Name = warrentyDetails[1];
                SerialNo = warrentyDetails[0];
                From = warrentyDetails[2];
                To = warrentyDetails[3];
                Status = warrentyDetails[4];
            }
        }
    }


}
