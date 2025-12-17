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
        public string? FirmPerson {  get; set; }
        public string? FirmContactNo {  get; set; }
        public string? FirmAddress {  get; set; }
        public string? Remark {  get; set; }
        public string? BillDate {  get; set; }
        public string? OnGate {  get; set; }
        public string? StoreDate {  get; set; }
        public string? ApprovedOn {  get; set; }
        public string? Department {  get; set; }
        public string? Maad {  get; set; }
        public string? PdfLink { get; set; }
        public string? ExcelLink { get; set; }
        public string? TestDate { get; set; }
        public string? ExpDate { get; set; }
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
            FirmPerson = dr["FirmPerson"].ToString() ?? string.Empty;
            FirmContactNo = dr["FirmContactNo"].ToString() ?? string.Empty;
            FirmAddress = dr["FirmAddress"].ToString() ?? string.Empty;
            Remark = dr["Remark"].ToString();
            BillDate = dr["BillDate"].ToString();
            OnGate = dr["OnGate"].ToString();
            StoreDate = dr["StoreDate"].ToString();
            ApprovedOn = dr["ApprovedOn"].ToString();
            Department = dr["Department"].ToString();
            Maad = dr["Maad"].ToString();
            TestDate = dr["TestDate"].ToString();
            ExpDate = dr["ExpDate"].ToString();
            string ParentPath = Directory.GetCurrentDirectory();
            if (File.Exists(Path.Combine(ParentPath, "Upload_Bills", dr["TransactionID"].ToString()??""+".pdf")))
            {
                PdfLink = Path.Combine("Upload_Bills", dr["TransactionID"].ToString() ?? "" + ".pdf");
            }
            if (File.Exists(Path.Combine(ParentPath, "Upload_Bills", dr["TransactionID"].ToString()??""+".xlsx")))
            {
                PdfLink = Path.Combine("Upload_Bills", dr["TransactionID"].ToString() ?? "" + ".xlsx");
            }
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
