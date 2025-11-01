using System.Data;

namespace AdvanceAPI.DTO.Approval
{
    public class EditGetApprovalDetailsResponse
    {
        public string? ReferenceNo { get; set; } = string.Empty;
        public string? MyType { get; set; } = string.Empty;
        public string? CampusName { get; set; } = string.Empty;
        public string? TotalAmount { get; set; } = string.Empty;
        public string? ApprovalCategory { get; set; } = string.Empty;
        public string? Authorities { get; set; } = string.Empty;
        public string? Maad { get; set; } = string.Empty;
        public string? Department { get; set; } = string.Empty;
        public string? VendorID { get; set; } = string.Empty;
        public string? FirmName { get; set; } = string.Empty;
        public string? FirmAddress { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;
        public string? Purpose { get; set; } = string.Empty;
        public string? AppDateShow { get; set; } = string.Empty;
        public string? BillTillShow { get; set; } = string.Empty;
        public string? AppDateDB { get; set; } = string.Empty;
        public string? BillTillDB { get; set; } = string.Empty;
        public bool? isExcelFileUploaded { get; set; } = false;

        public EditGetApprovalDetailsResponse()
        {

        }


        public EditGetApprovalDetailsResponse(DataRow dr)
        {
            ReferenceNo = dr["ReferenceNo"]?.ToString() ?? String.Empty;
            MyType = dr["MyType"]?.ToString() ?? String.Empty;
            CampusName = dr["CampusName"]?.ToString() ?? String.Empty;
            TotalAmount = dr["TotalAmount"]?.ToString() ?? String.Empty;
            ApprovalCategory = dr["ApprovalCategory"]?.ToString() ?? String.Empty;
            Authorities = dr["Authorities"]?.ToString() ?? String.Empty;
            Maad = dr["Maad"]?.ToString() ?? String.Empty;
            Department = dr["Department"]?.ToString() ?? String.Empty;
            VendorID = dr["VendorID"]?.ToString() ?? String.Empty;
            FirmName = dr["FirmName"]?.ToString() ?? String.Empty;
            FirmAddress = dr["FirmAddress"]?.ToString() ?? String.Empty;
            Note = dr["Note"]?.ToString() ?? String.Empty;
            Purpose = dr["Purpose"]?.ToString() ?? String.Empty;
            AppDateShow = dr["AppDateShow"]?.ToString() ?? String.Empty;
            BillTillShow = dr["BillTillShow"]?.ToString() ?? String.Empty;
            AppDateDB = dr["AppDateDB"]?.ToString() ?? String.Empty;
            BillTillDB = dr["BillTillDB"]?.ToString() ?? String.Empty;
        }

    }
}
