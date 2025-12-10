using System.Data;

namespace AdvanceAPI.DTO.Advance.Bill
{
    public class BillApprovalBillBaseAllDetails
    {
        public string? TransactionID { get; set; } = string.Empty;
        public string? Session { get; set; } = string.Empty;
        public string? ForType { get; set; } = string.Empty;
        public string? RelativePersonID { get; set; } = string.Empty;
        public string? RelativePersonName { get; set; } = string.Empty;
        public string? RelativeDepartment { get; set; } = string.Empty;
        public string? RelativeDesignation { get; set; } = string.Empty;
        public string? FirmName { get; set; } = string.Empty;
        public string? FirmPerson { get; set; } = string.Empty;
        public string? FirmEmail { get; set; } = string.Empty;
        public string? FirmPanNo { get; set; } = string.Empty;
        public string? FirmAddress { get; set; } = string.Empty;
        public string? FirmContactNo { get; set; } = string.Empty;
        public string? FirmAlternateContactNo { get; set; } = string.Empty;
        public string? AmountRequired { get; set; } = string.Empty;
        public string? AmountPaid { get; set; } = string.Empty;
        public string? AmountRemaining { get; set; } = string.Empty;
        public string? IssuedBy { get; set; } = string.Empty;
        public string? IssuedName { get; set; } = string.Empty;
        public string? IssuedOn { get; set; } = string.Empty;
        public string? IssuedFrom { get; set; } = string.Empty;
        public string? LastUpdatedOn { get; set; } = string.Empty;
        public string? LastUpdatedBy { get; set; } = string.Empty;
        public string? Col1 { get; set; } = string.Empty;
        public string? Col2 { get; set; } = string.Empty;
        public string? Col3 { get; set; } = string.Empty;
        public string? Col4 { get; set; } = string.Empty;
        public string? Col5 { get; set; } = string.Empty;
        public string? Remark { get; set; } = string.Empty;
        public string? GateEntryOn { get; set; } = string.Empty;
        public string? DepartmentApprovalDate { get; set; } = string.Empty;
        public string? StoreDate { get; set; } = string.Empty;
        public string? BillDate { get; set; } = string.Empty;
        public string? BillReceivedOnGate { get; set; } = string.Empty;
        public string? BillNo { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public string? VendorID { get; set; } = string.Empty;
        public string? CashDiscount { get; set; } = string.Empty;
        public string? BillExtra1 { get; set; } = string.Empty;
        public string? BillExtra2 { get; set; } = string.Empty;
        public string? BillExtra3 { get; set; } = string.Empty;
        public string? ApprovedOn { get; set; } = string.Empty;
        public string? BillExtra4 { get; set; } = string.Empty;
        public string? BillExtra5 { get; set; } = string.Empty;
        public string? BillExtra6 { get; set; } = string.Empty;
        public string? BillExtra7 { get; set; } = string.Empty;
        public string? BillExtra8 { get; set; } = string.Empty;
        public string? MyDistributedOn { get; set; } = string.Empty;
        public string? MyDistributedBy { get; set; } = string.Empty;
        public string? MyDistributedFrom { get; set; } = string.Empty;
        public string? CampusCode { get; set; } = string.Empty;
        public string? CampusName { get; set; } = string.Empty;
        public string? PdfUploaded { get; set; } = string.Empty;
        public string? ExcelUploaded { get; set; } = string.Empty;
        public string? MyFirmName { get; set; } = string.Empty;
        public string? G { get; set; } = string.Empty;
        public string? D { get; set; } = string.Empty;
        public string? S { get; set; } = string.Empty;
        public string? GT { get; set; } = string.Empty;
        public string? BL { get; set; } = string.Empty;
        public string? INI { get; set; } = string.Empty;
        public string? MyEmail { get; set; } = string.Empty;
        public string? MyContact { get; set; } = string.Empty;
        public string? Sch { get; set; } = string.Empty;
        public string? Act { get; set; } = string.Empty;
        public List<BillApprovalBillBaseTransactionDetails> TransactionDetails { get; set; } = new List<BillApprovalBillBaseTransactionDetails>();
        public List<BillApprovalIssueHostelWiseDistribution> HostelDistributionDetails { get; set; } = new List<BillApprovalIssueHostelWiseDistribution>();
        public List<BillApprovalIssueVehicleDistribution> VehicleDistributionDetails { get; set; } = new List<BillApprovalIssueVehicleDistribution>();
        public List<BillApprovalVehiclePreviousBills> VehiclePreviousBills { get; set; } = new List<BillApprovalVehiclePreviousBills>();


        public BillApprovalBillBaseAllDetails()
        {

        }
        public BillApprovalBillBaseAllDetails(DataRow dr)
        {
            TransactionID = dr["TransactionID"]?.ToString() ?? String.Empty;
            Session = dr["Session"]?.ToString() ?? String.Empty;
            ForType = dr["ForType"]?.ToString() ?? String.Empty;
            RelativePersonID = dr["RelativePersonID"]?.ToString() ?? String.Empty;
            RelativePersonName = dr["RelativePersonName"]?.ToString() ?? String.Empty;
            RelativeDepartment = dr["RelativeDepartment"]?.ToString() ?? String.Empty;
            RelativeDesignation = dr["RelativeDesignation"]?.ToString() ?? String.Empty;
            FirmName = dr["FirmName"]?.ToString() ?? String.Empty;
            FirmPerson = dr["FirmPerson"]?.ToString() ?? String.Empty;
            FirmEmail = dr["FirmEmail"]?.ToString() ?? String.Empty;
            FirmPanNo = dr["FirmPanNo"]?.ToString() ?? String.Empty;
            FirmAddress = dr["FirmAddress"]?.ToString() ?? String.Empty;
            FirmContactNo = dr["FirmContactNo"]?.ToString() ?? String.Empty;
            FirmAlternateContactNo = dr["FirmAlternateContactNo"]?.ToString() ?? String.Empty;
            AmountRequired = dr["AmountRequired"]?.ToString() ?? String.Empty;
            AmountPaid = dr["AmountPaid"]?.ToString() ?? String.Empty;
            AmountRemaining = dr["AmountRemaining"]?.ToString() ?? String.Empty;
            IssuedBy = dr["IssuedBy"]?.ToString() ?? String.Empty;
            IssuedName = dr["IssuedName"]?.ToString() ?? String.Empty;
            IssuedOn = dr["IssuedOn"]?.ToString() ?? String.Empty;
            IssuedFrom = dr["IssuedFrom"]?.ToString() ?? String.Empty;
            LastUpdatedOn = dr["LastUpdatedOn"]?.ToString() ?? String.Empty;
            LastUpdatedBy = dr["LastUpdatedBy"]?.ToString() ?? String.Empty;
            Col1 = dr["Col1"]?.ToString() ?? String.Empty;
            Col2 = dr["Col2"]?.ToString() ?? String.Empty;
            Col3 = dr["Col3"]?.ToString() ?? String.Empty;
            Col4 = dr["Col4"]?.ToString() ?? String.Empty;
            Col5 = dr["Col5"]?.ToString() ?? String.Empty;
            Remark = dr["Remark"]?.ToString() ?? String.Empty;
            GateEntryOn = dr["GateEntryOn"]?.ToString() ?? String.Empty;
            DepartmentApprovalDate = dr["DepartmentApprovalDate"]?.ToString() ?? String.Empty;
            StoreDate = dr["StoreDate"]?.ToString() ?? String.Empty;
            BillDate = dr["BillDate"]?.ToString() ?? String.Empty;
            BillReceivedOnGate = dr["BillReceivedOnGate"]?.ToString() ?? String.Empty;
            BillNo = dr["BillNo"]?.ToString() ?? String.Empty;
            Status = dr["Status"]?.ToString() ?? String.Empty;
            VendorID = dr["VendorID"]?.ToString() ?? String.Empty;
            CashDiscount = dr["CashDiscount"]?.ToString() ?? String.Empty;
            BillExtra1 = dr["BillExtra1"]?.ToString() ?? String.Empty;
            BillExtra2 = dr["BillExtra2"]?.ToString() ?? String.Empty;
            BillExtra3 = dr["BillExtra3"]?.ToString() ?? String.Empty;
            ApprovedOn = dr["ApprovedOn"]?.ToString() ?? String.Empty;
            BillExtra4 = dr["BillExtra4"]?.ToString() ?? String.Empty;
            BillExtra5 = dr["BillExtra5"]?.ToString() ?? String.Empty;
            BillExtra6 = dr["BillExtra6"]?.ToString() ?? String.Empty;
            BillExtra7 = dr["BillExtra7"]?.ToString() ?? String.Empty;
            BillExtra8 = dr["BillExtra8"]?.ToString() ?? String.Empty;
            MyDistributedOn = dr["MyDistributedOn"]?.ToString() ?? String.Empty;
            MyDistributedBy = dr["MyDistributedBy"]?.ToString() ?? String.Empty;
            MyDistributedFrom = dr["MyDistributedFrom"]?.ToString() ?? String.Empty;
            CampusCode = dr["CampusCode"]?.ToString() ?? String.Empty;
            CampusName = dr["CampusName"]?.ToString() ?? String.Empty;
            PdfUploaded = dr["PdfUploaded"]?.ToString() ?? String.Empty;
            ExcelUploaded = dr["ExcelUploaded"]?.ToString() ?? String.Empty;
            MyFirmName = dr["MyFirmName"]?.ToString() ?? String.Empty;
            G = dr["G"]?.ToString() ?? String.Empty;
            D = dr["D"]?.ToString() ?? String.Empty;
            S = dr["S"]?.ToString() ?? String.Empty;
            GT = dr["GT"]?.ToString() ?? String.Empty;
            BL = dr["BL"]?.ToString() ?? String.Empty;
            INI = dr["INI"]?.ToString() ?? String.Empty;
            MyEmail = dr["MyEmail"]?.ToString() ?? String.Empty;
            MyContact = dr["MyContact"]?.ToString() ?? String.Empty;
            Sch = dr["Sch"]?.ToString() ?? String.Empty;
            Act = dr["Act"]?.ToString() ?? String.Empty;
        }
    }

}
