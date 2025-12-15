using System.Data;

namespace AdvanceAPI.DTO.FirmPaidDetails
{
    public class HeadWise
    {
        
            public string SNO { get; set; }
            public string Name { get; set; }
            public string MyYear { get; set; }
            public string Month { get; set; }
            public string Req { get; set; }
            public string Signed { get; set; }
            public string Cleared { get; set; }

            // Constructor from DataRow
            public HeadWise()
            {

            }

            public HeadWise(DataRow row)
            {
                SNO = row["SNO"].ToString()??string.Empty;
                Name = row["Name"].ToString()??string.Empty;
                MyYear = row["MyYear"].ToString() ?? string.Empty;
                Month = row["Month"].ToString() ?? string.Empty;
                Req = row["Req"].ToString() ?? string.Empty;
                Signed = row["Signed"].ToString() ?? string.Empty;
                Cleared = row["Cleared"].ToString() ?? string.Empty;
            }
        
    }
}
