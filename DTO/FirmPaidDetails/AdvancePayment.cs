using System.Data;
using System.Drawing;

namespace AdvanceAPI.DTO.FirmPaidDetails
{
    public class AdvancePayment
    {
        public string SNO { get; set; }
        public string Person { get; set; }
        public string Purpose { get; set; }
        public int IssuedAmount { get; set; }
        public string HighlightReceivedAmount { get; set; }
        public int Balance { get; set; }
        public int ReceivedAmount { get; set; }
        public string Tran { get; set; }
        public string Ini { get; set; }
        public string Sign { get; set; }
        public string Clr { get; set; }
        public Color BackColor { get;set; }
        public Color ForeColor { get; set; }
        public int TransactionID { get; set; }
        public int SequenceID { get; set; }
        public string BU { get; set; }
        public string Concat { get; set; }
        public AdvancePayment()
        {

        }

        // Constructor that takes a DataRow
        public AdvancePayment(DataRow row)
        {
            SNO = row["SNO"].ToString();
            Person = row["Person"].ToString();
            Purpose = row["Purpose"].ToString();

            if (int.TryParse(row["IssuedAmount"].ToString(), out int amt))
                IssuedAmount = amt;

            Tran = row["Tran"].ToString();
            Ini = row["Ini"].ToString();
            Sign = row["Sign"].ToString();
            Clr = row["Clr"].ToString();

            if (int.TryParse(row["TransactionID"].ToString(), out int tid))
                TransactionID = tid;

            if (int.TryParse(row["SequenceID"].ToString(), out int sid))
                SequenceID = sid;

            BU = row["BU"].ToString();
            Concat = row["CONCAT"].ToString();
        }

    }
}
