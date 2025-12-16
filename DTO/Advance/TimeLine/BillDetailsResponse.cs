using System.Data;

namespace AdvanceAPI.DTO.Advance.TimeLine
{
    public class BillDetailsResponse1
    {
        public string? AmountPaid { get; set; }
        public string? AmountRemaining { get; set; }
        public string? AmountRequired { get; set; }
        public string? Status { get; set; }
        public string? BillNo { get; set; }
        public List<BillAutority> auth {  get; set; }
        public BillDetailsResponse1()
        {

        }

        public BillDetailsResponse1(DataRow dr)
        {
            AmountPaid = dr["AmountPaid"].ToString();
            AmountRemaining = dr["AmountRemaining"].ToString();
            AmountRequired = dr["AmountRequired"].ToString();
            Status = dr["Status"].ToString();
            BillNo = dr["BillNo"].ToString() ?? string.Empty;
        }

    }
    public class BillAutority
    {
        public string? Name { get; set; }
        public string? EmpCode { get; set; }
        public string? On {  get; set; }
        public string? Status { get; set; }
        public BillAutority(DataRow dr) 
        {
            Name= dr["EmployeeDetails"].ToString();
            EmpCode = dr["EmployeeID"].ToString();
            On=dr["On"].ToString();
            Status = dr["Status"].ToString();
        }
    }
}
