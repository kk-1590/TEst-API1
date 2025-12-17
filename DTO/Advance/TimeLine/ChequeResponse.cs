using System.Data;

namespace AdvanceAPI.DTO.Advance.TimeLine
{
    public class ChequeResponse
    {
        public string? TransactionID { get; set; }
        public string? SequenceID { get;set;  }
        public string? TaxAmount { get;set;  }
        public string? PaidAmount { get;set;  }
        public string? IssuedAmount { get;set;  }
        public string? Mode { get;set;  }
        public string? TransactionNo { get;set;  }
        public string? IssuedOn { get;set;  }
        public string? SignedOn { get;set;  }
        public string? ReceivedOn { get;set;  }
        public string? DistributionOn { get;set;  }
        public string? DistributedBy { get;set;  }
        public string? ReceivedBy { get;set;  }
        public string? Remark { get;set;  }
        public List<BillAutority> auth {  get;set; }
        public ChequeResponse() { }
        public ChequeResponse(DataRow dr)
        {
            string? c(string name) =>
                dr.Table != null && dr.Table.Columns.Contains(name) && dr[name] != DBNull.Value
                    ? dr[name].ToString()
                    : null;

            TransactionID = c("TransactionID");
            SequenceID = c("SequenceID");
            TaxAmount = c("TaxAmount");
            PaidAmount = c("PaidAmount");
            IssuedAmount = c("IssuedAmount");
            Mode = c("Mode");
            TransactionNo = c("TransactionNo");
            IssuedOn = c("IssuedOn");
            SignedOn = c("SignedOn");
            ReceivedOn = c("ReceivedOn");
            DistributionOn = c("DistributionOn");
            DistributedBy = c("DistributedBy");
            ReceivedBy = c("ReceivedBy");
            Remark = c("Remark");
        }
    }
}
