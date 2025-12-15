using System.Data;

namespace AdvanceAPI.DTO.Advance.BillApproval
{
    public class OtherTransactionDetailsResponse
    {
        public string? TransactionId { get;set; }
        public string? SequenceID { get;set; }
        public string? TransactionNo { get;set; }
        public string? FirmName { get;set; }
        public string? IniOn { get;set; }
        public string? Chq { get;set; }
        public string? IssuedByName { get;set; }
        public string? Remark { get;set; }
        public string? IssuedAmount { get;set; }
        public OtherTransactionDetailsResponse(DataRow dr)
        {
            TransactionId = dr["TransactionId"] == DBNull.Value ? null : dr["TransactionId"].ToString();
            SequenceID = dr["SequenceID"] == DBNull.Value ? null : dr["SequenceID"].ToString();
            TransactionNo = dr["TransactionNo"] == DBNull.Value ? null : dr["TransactionNo"].ToString();
            FirmName = dr["FirmName"] == DBNull.Value ? null : dr["FirmName"].ToString();
            IniOn = dr["IniOn"] == DBNull.Value ? null : dr["IniOn"].ToString();
            Chq = dr["Chq"] == DBNull.Value ? null : dr["Chq"].ToString();
            IssuedByName = dr["IssuedByName"] == DBNull.Value ? null : dr["IssuedByName"].ToString();
            Remark = dr["Remark"] == DBNull.Value ? null : dr["Remark"].ToString();
            IssuedAmount = dr["IssuedAmount"] == DBNull.Value ? null : dr["IssuedAmount"].ToString();
        }
    }
}
