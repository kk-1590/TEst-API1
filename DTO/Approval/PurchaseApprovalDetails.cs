namespace AdvanceAPI.DTO.Approval;

public class PurchaseApprovalDetails
{
    public bool IsRejectable { get; set; }
    public bool CanApproval { get; set; }
    public bool CanCancel { get; set; }
    public int AuthorityNumber { get; set; }
    public string VendorName { get; set; }
    public string ExcelFileUrl { get; set; }

    public string ApprovalType { get; set; }
    public string Status { get; set; }
    public string OtherDetails { get; set; }
    public string EncRelativePersonID { get; set; }
    public string VendorId { get; set; }
    public string Department { get; set; }
    public string CampusName { get; set; }
    public string BudgetAmount { get; set; }
    public string prevTaken { get; set; }
    public string TotalAmount { get; set; }
    public string BudgetStatus { get; set; }
    public string UploadBy { get; set; }
    public string IniName { get; set; }
    public string RelativePersonName { get; set; }
    public string RelativePersonID { get; set; }
    public string ApprovalBillStatus { get; set; }
    public string RecievingStatus { get; set; }
    public string Note { get; set; }
    public string VendorNameShow { get; set; }
    public string VendorPersonName { get; set; }
    public string VendorContactNo { get; set; }
    public string Maad { get; set; }
    public string LastRecieveOn { get; set; }
    public string ReferenceNo { get; set; }
    public string App1Status { get; set; }
    public string App2Status { get; set; }
    public string App3Status { get; set; }
    public string App4Status { get; set; }

    public string App1Id { get; set; }
    public string App2Id { get; set; }
    public string App3Id { get; set; }
    public string App4Id { get; set; }

    public string App1Name { get; set; }
    public string App2Name { get; set; }
    public string App3Name { get; set; }
    public string App4Name { get; set; }
    public string App1On { get; set; }
    public string App2On { get; set; }
    public string App3On { get; set; }
    public string App4On { get; set; }
    public string FinalStat { get; set; }
    public bool IsExcelFile { get; set; }
    public string PreviousCancelRemark { get; set; }
    public string CancelledOn { get; set; }
    public string CancelledReason { get; set; }
    public string RejectedReason { get; set; }
    public string CloseOn { get; set; }
    public string CloseReason { get; set; }
    public string ByPass { get; set; }
    public string BillId { get; set; }
    public string IniOn { get; internal set; }
    public string ApprovalDate { get; internal set; }
    public string UpTo { get; internal set; }
    public string Purpose { get; internal set; }
    public string TotalItem { get; internal set; }

    public string? InitiatedById { get; set; } = string.Empty;

    public string? InitByPhoto { get; set; } = string.Empty;
    public string? RelativePersonPhoto { get; set; } = string.Empty;
    public string? App1Photo { get; set; } = string.Empty;
    public string? App2Photo { get; set; } = string.Empty;
    public string? App3Photo { get; set; } = string.Empty;
    public string? App4Photo { get; set; } = string.Empty;


}