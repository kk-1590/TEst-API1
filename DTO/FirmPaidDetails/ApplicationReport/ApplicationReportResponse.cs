using System.Data;
using System.Drawing;

namespace AdvanceAPI.DTO.FirmPaidDetails.ApplicationReport
{
    public class ApplicationReportResponse
    {
        public HashSet<ApplicationDetaild>? DetailedStatement {  get; set; }
        public HashSet<SummarizedStatement>? Summarizedstmt {  get; set; }
    }
    public class ApplicationDetaild
    {
        public string? Id { get; set; }
        public string? Session { get;set;  }
        public string? ShortDetail { get;set;  }
        public string? Reason { get;set;  }
        public string? Amount { get;set;  }
        public string? BillId { get;set;  }
        public string? IssuedName { get;set;  }
        public string? Punishment { get;set;  }
        public string? PunishmentByName { get;set;  }
        public string? IssuedType { get;set;  }
        public string? VerifiedByName { get;set;  }
        public string? Status { get;set;  }
        public string? PdfFile { get;set; }
        public Color? RowBackColor { get;set; }
        public Color? StatusColor { get; set; } = Color.Transparent;

        public ApplicationDetaild(DataRow dr)
        {
            Id = dr["Id"].ToString();
            Session = dr["Session"].ToString();
            ShortDetail = dr["ShortDetail"].ToString();
            Reason = dr["Reason"].ToString();
            Amount = dr["Amount"].ToString();
            BillId = dr["BillId"].ToString();
            IssuedName = dr["IssuedName"].ToString() ?? string.Empty;
            Punishment = dr["Col1"].ToString();
            PunishmentByName = dr["Col2"].ToString();
            IssuedType = dr["Col3"].ToString();
            VerifiedByName = dr["VerifyBy"].ToString();
            Status = dr["Status"].ToString();
        }
    }
    public class SummarizedStatement
    {
        public string? MyYear { get; set; }
        public string? Month { get; set; }
        public string? Req { get; set; }
        public SummarizedStatement(DataRow dr) 
        {
            MyYear = dr["MyYear"].ToString();
            Month = dr["Month"].ToString();
            Req = dr["Req"].ToString();

        }
    }
}
