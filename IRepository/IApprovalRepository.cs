using System.Data;
using AdvanceAPI.DTO.Approval;

namespace AdvanceAPI.IRepository
{
    public interface IApprovalRepository
    {
        Task<DataTable> GetDraftItemRefNo(string EmpCode, string AppType,string RefNo);
        Task<DataTable> GetAutoDraftItemRefNo();
        Task<int> AddDraftItem(string RefNo, AddStockItemRequest AddStock, string EmpCode);
        Task<DataTable> GetDraftedItem(string EmpCode, string AppType, string CampusCode, string RefNo);
        Task<DataTable> GetDraftedSummary(string EmpCode, string AppType, string CampusCode,string RefNo);
        Task<DataTable> GeneratePurchaseApprovalRefNo();

        Task<DataTable> GetApprovalSessions();
        Task<DataTable> GetApprovalFinalAuthority(string? campusCode);
        Task<DataTable> IsNonMathuraNumber3AuthoritiesDefined();
        Task<DataTable> GetApprovalNumber3Authorities(string? search, bool isNonMathuraAuthoritiesRequired);
        Task<DataTable> CheckIsApprovalDraftItemsExists(string? emploeeId, DeleteApprovalDraftRequest? deleteRequest);
        Task<int> DeleteApprovalDraftItems(string? emploeeId, DeleteApprovalDraftRequest? deleteRequest);
        Task<DataTable> GetVenderRegister(string VenderCode);

        Task<int> SubmitPurchaseBill(string EmpCode, GeneratePurchaseApprovalRequest generatePurchaseApprovalRequest,
            string RefNo, string TotalDays);

        Task<DataTable> CheckDeletedDraftedItem(string ItemId);
        Task<int> DeletedDraftedItem(string ItemId);

        Task<DataTable> GetMyApprovals(string? emploeeId, bool OnlySelfApprovals, AprrovalsListRequest? search);
        Task<DataTable> CheckIsApprovalComparisonDefined(string? referenceNo);
        Task<DataTable> CheckIsApprovalExists(string? referenceNo);
        Task<int> DeleteApproval(string? employeeId, string? referenceNo);
        
        Task<DataTable> CheckAlreadyDraftedItem(string EmpCode, string AppType,string ItemCode,string RefNo);
        Task<DataTable> GetDraft(string EmpCode);
        Task<DataTable> GetPOApprovalDetails(string? referenceNo);
        Task<DataTable> CheckIsApprovalComparisonLocked(string? referenceNo);
        Task<DataTable> GetPOApprovalBillExpenditureDetails(string? referenceNo);
        Task<DataTable> GetApprovalPaymentDetails(string? referenceNo);
        Task<DataTable> GetApprovalWarrentyDetails(string? referenceNo);
        Task<DataTable> GetApprovalRepairMaintinanceDetails(string? referenceNo);
        Task<DataTable> GetApprovalItems(string? referenceNo);
        Task<DataTable> GetApprovalItemPreviousPurchaseDetails(string? referenceNo, string? itemCode);
        Task<DataTable> GetApprovalEmployeeDesignationContact(string? employeeId);
        Task<int> UpdateApprovalNote(string? employeeId, string? referenceNo, string? note);
        Task<DataTable> CheckApprovalExistsToUpdate(string? referenceNo);
        Task<DataTable> GetEditApprovalDetails(string? referenceNo);
    }
}
