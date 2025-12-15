using System.Data;

namespace AdvanceAPI.DTO.FirmPaidDetails
{
    public class BillModel
    {
        public string SNO { get; set; }
        public string Person { get; set; }
        public string Purpose { get; set; }
        public decimal IssuedAmount { get; set; }
        public string Tran { get; set; }
        public string Ini { get; set; }
        public string Sign { get; set; }
        public string Clr { get; set; }
        public string Concat { get; set; }
        public string FName { get; set; }
        public string MadName { get; set; }

        public BillModel()
        {

        }

        // Constructor that takes a DataRow
        public BillModel(DataRow row)
        {
            SNO = row["SNO"].ToString()??string.Empty;
            Person = row["Person"].ToString()??string.Empty;
            Purpose = row["Purpose"].ToString()?? string.Empty;

            // Safe conversion for IssuedAmount
            if (decimal.TryParse(row["IssuedAmount"].ToString(), out decimal amt))
                IssuedAmount = amt;

            Tran = row["Tran"].ToString() ?? string.Empty;
            Ini = row["Ini"].ToString() ?? string.Empty;
            Sign = row["Sign"].ToString() ?? string.Empty;
            Clr = row["Clr"].ToString() ?? string.Empty;
            Concat = row["CONCAT"].ToString() ?? string.Empty;
            FName = row["FName"].ToString() ?? string.Empty ;
            MadName = row["MadName"].ToString() ?? string.Empty;
        }

    }
}
