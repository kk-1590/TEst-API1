using System.Data;

namespace AdvanceAPI.DTO.Approval
{
    public class ApprovalItemDetails
    {
        public string? ReferenceNo { get; set; } = string.Empty;
        public string? ItemCode { get; set; } = string.Empty;
        public string? ItemName { get; set; } = string.Empty;
        public string? Make { get; set; } = string.Empty;
        public string? Size { get; set; } = string.Empty;
        public string? Unit { get; set; } = string.Empty;
        public string? Balance { get; set; } = string.Empty;
        public string? Quantity { get; set; } = string.Empty;
        public string? PrevRate { get; set; } = string.Empty;
        public string? CurRate { get; set; } = string.Empty;
        public string? ChangeReason { get; set; } = string.Empty;
        public string? WarIn { get; set; } = string.Empty;
        public string? WarType { get; set; } = string.Empty;
        public string? ActualAmount { get; set; } = string.Empty;
        public string? DisPer { get; set; } = string.Empty;
        public string? VatPer { get; set; } = string.Empty;
        public string? TotalAmount { get; set; } = string.Empty;
        public string? IniOn { get; set; } = string.Empty;
        public string? IniId { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public string? R_Total { get; set; } = string.Empty;
        public string? R_Pending { get; set; } = string.Empty;
        public string? R_PingOn { get; set; } = string.Empty;
        public string? R_LastRcvOn { get; set; } = string.Empty;
        public string? R_LastRcvBy { get; set; } = string.Empty;
        public string? R_Status { get; set; } = string.Empty;
        public string? SerialNo { get; set; } = string.Empty;
        public string? Tot { get; set; } = string.Empty;
        public string? Warrenty { get; set; } = string.Empty;
        public ApprovalItemPreviousDetails? PreviosDetails { get; set; } = new ApprovalItemPreviousDetails();


        public ApprovalItemDetails()
        {

        }
        public ApprovalItemDetails(DataRow dr)
        {
            ReferenceNo = dr["ReferenceNo"]?.ToString() ?? String.Empty;
            ItemCode = dr["ItemCode"]?.ToString() ?? String.Empty;
            ItemName = dr["ItemName"]?.ToString() ?? String.Empty;
            Make = dr["Make"]?.ToString() ?? String.Empty;
            Size = dr["Size"]?.ToString() ?? String.Empty;
            Unit = dr["Unit"]?.ToString() ?? String.Empty;
            Balance = dr["Balance"]?.ToString() ?? String.Empty;
            Quantity = dr["Quantity"]?.ToString() ?? String.Empty;
            PrevRate = dr["PrevRate"]?.ToString() ?? String.Empty;
            CurRate = dr["CurRate"]?.ToString() ?? String.Empty;
            ChangeReason = dr["ChangeReason"]?.ToString() ?? String.Empty;
            WarIn = dr["WarIn"]?.ToString() ?? String.Empty;
            WarType = dr["WarType"]?.ToString() ?? String.Empty;
            ActualAmount = dr["ActualAmount"]?.ToString() ?? String.Empty;
            DisPer = dr["DisPer"]?.ToString() ?? String.Empty;
            VatPer = dr["VatPer"]?.ToString() ?? String.Empty;
            TotalAmount = dr["TotalAmount"]?.ToString() ?? String.Empty;
            IniOn = dr["IniOn"]?.ToString() ?? String.Empty;
            IniId = dr["IniId"]?.ToString() ?? String.Empty;
            Status = dr["Status"]?.ToString() ?? String.Empty;
            R_Total = dr["R_Total"]?.ToString() ?? String.Empty;
            R_Pending = dr["R_Pending"]?.ToString() ?? String.Empty;
            R_PingOn = dr["R_PingOn"]?.ToString() ?? String.Empty;
            R_LastRcvOn = dr["R_LastRcvOn"]?.ToString() ?? String.Empty;
            R_LastRcvBy = dr["R_LastRcvBy"]?.ToString() ?? String.Empty;
            R_Status = dr["R_Status"]?.ToString() ?? String.Empty;
            SerialNo = dr["SerialNo"]?.ToString() ?? String.Empty;
            Tot = dr["Tot"]?.ToString() ?? String.Empty;
            Warrenty = dr["Warrenty"]?.ToString() ?? String.Empty;
        }
    }
}
