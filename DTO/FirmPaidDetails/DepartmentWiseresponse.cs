using System.Data;

namespace AdvanceAPI.DTO.FirmPaidDetails
{
    public class DepartmentWiseresponse
    {
        public string SNO { get; set; }
        public string Name { get; set; }
        public string MyYear { get; set; }
        public string Month { get; set; }
        public string Req { get; set; }
        public string Signed { get; set; }
        public string Cleared { get; set; }

        // Constructor from DataRow
        public DepartmentWiseresponse(DataRow row)
        {
            SNO = row["SNO"].ToString();
            Name = row["Name"].ToString();
            MyYear = row["MyYear"].ToString();
            Month = row["Month"].ToString();

                Req = row["Req"].ToString();

                Signed = row["Signed"].ToString();

                Cleared = row["Cleared"].ToString();
        }

    }
}
