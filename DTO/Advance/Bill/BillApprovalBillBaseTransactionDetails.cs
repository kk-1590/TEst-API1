using System.Data;

namespace AdvanceAPI.DTO.Advance.Bill
{
    public class BillApprovalBillBaseTransactionDetails
    {
        public string? SequenceID { get; set; } = string.Empty;
        public string? CVName { get; set; } = string.Empty;
        public string? CVSubFirm { get; set; } = string.Empty;
        public string? IssuedType { get; set; } = string.Empty;
        public string? IssuedAmount { get; set; } = string.Empty;
        public string? TaxAmount { get; set; } = string.Empty;
        public string? PaidAmount { get; set; } = string.Empty;
        public string? Mode { get; set; } = string.Empty;
        public string? TransactionNo { get; set; } = string.Empty;
        public string? IssuedByName { get; set; } = string.Empty;
        public string? On { get; set; } = string.Empty;
        public string? Bill { get; set; } = string.Empty;
        public string? OtherCut { get; set; } = string.Empty;
        public string? IssuedOn { get; set; } = string.Empty;
        public string? SignedOn { get; set; } = string.Empty;
        public string? ReceivedOn { get; set; } = string.Empty;
        public string? ClearOn { get; set; } = string.Empty;
        public string? RejectString { get; set; } = string.Empty;
        public bool Issued { get; set; } = false;
        public bool Signed { get; set; } = false;
        public bool Recieved { get; set; } = false;
        public bool Cleared { get; set; } = false;


        public BillApprovalBillBaseTransactionDetails()
        {

        }

        public BillApprovalBillBaseTransactionDetails(DataRow dr)
        {
            SequenceID = dr["SequenceID"]?.ToString() ?? String.Empty;
            CVName = dr["CVName"]?.ToString() ?? String.Empty;
            CVSubFirm = dr["CVSubFirm"]?.ToString() ?? String.Empty;
            IssuedType = dr["IssuedType"]?.ToString() ?? String.Empty;
            IssuedAmount = dr["IssuedAmount"]?.ToString() ?? String.Empty;
            TaxAmount = dr["TaxAmount"]?.ToString() ?? String.Empty;
            PaidAmount = dr["PaidAmount"]?.ToString() ?? String.Empty;
            Mode = dr["Mode"]?.ToString() ?? String.Empty;
            TransactionNo = dr["TransactionNo"]?.ToString() ?? String.Empty;
            IssuedByName = dr["IssuedByName"]?.ToString() ?? String.Empty;
            On = dr["On"]?.ToString() ?? String.Empty;
            Bill = dr["Bill"]?.ToString() ?? String.Empty;
            OtherCut = dr["OtherCut"]?.ToString() ?? String.Empty;
            IssuedOn = dr["IssuedOn"]?.ToString() ?? String.Empty;
            SignedOn = dr["SignedOn"]?.ToString() ?? String.Empty;
            ReceivedOn = dr["ReceivedOn"]?.ToString() ?? String.Empty;
            ClearOn = dr["ClearOn"]?.ToString() ?? String.Empty;
        }
    }
}
