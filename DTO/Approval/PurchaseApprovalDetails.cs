namespace AdvanceAPI.DTO.Approval;

public class PurchaseApprovalDetails
{
    public bool IsRejectable{get;set;}
    public string ApprovalType{get;set;}
    public string Status {get;set;}
    public string OtherDetails {get;set;}
    public string EncRelativePersonID {get;set;}
    public string VendorId {get;set;}
    public string Department {get;set;}
    public string CampusName {get;set;}
    public string BudgetAmount {get;set;}
    public string prevTaken {get;set;}
    public string TotalAmount {get;set;}
    public string BudgetStatus {get;set;}
    public string UploadBy {get;set;}
    
    
}