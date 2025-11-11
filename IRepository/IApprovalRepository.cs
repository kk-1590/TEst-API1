using System.Data;
using AdvanceAPI.DTO.Approval;
using AdvanceAPI.DTO.Inclusive;

namespace AdvanceAPI.IRepository
{
    public interface IApprovalRepository
    {
        Task<DataTable> GetDraftItemRefNo(string EmpCode, string AppType, string RefNo);
        Task<DataTable> GetAutoDraftItemRefNo();
        Task<int> AddDraftItem(string RefNo, AddStockItemRequest AddStock, string EmpCode);
        Task<DataTable> GetDraftedItem(string EmpCode, string AppType, string CampusCode, string RefNo);
        Task<DataTable> GetDraftedSummary(string EmpCode, string AppType, string CampusCode, string RefNo);
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

        Task<DataTable> CheckAlreadyDraftedItem(string EmpCode, string AppType, string ItemCode, string RefNo);
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
        Task<DataTable> GetMyApprovalsCount(string? emploeeId, bool OnlySelfApprovals, AprrovalsListRequest? search);
        Task<int> EditApprovalDetails(string? referenceNo, UpdateApprovalEditDetails details, string EmpCode);
        Task<DataTable> GetApprovalDetails(string EmpCode, string EmpCodeAdd, AprrovalsListRequest details);
        Task<DataTable> GetBaseUrl();

        Task<int> ApproveApprovalRequest(string? employeeId, string? employeeName, string? designation, PassApprovalRequest? passRequest);
        Task<DataTable> CheckPassApprovalValidExists(string? employeeId, string? referenceNo, string? authorityNo);
        Task<DataTable> GetStatusApprovalForFinalStatus(string? referenceNo);
        Task<int> UpdateApprovalFinalApproved(string? referenceNo);
        Task<DataTable> CheckIsVivekSirApprovedApproval(string? referenceNo);
        Task<int> RejectApprovalRequest(string? employeeId, string? employeeName, string? designation, RejectACancelpprovalRequest? rejectRequest);
        Task<DataTable> CheckCanCancelApproval(string? employeeId, string? referenceNo, string? authorityNo);
        Task<int> CancelApprovalRequest(string? employeeId, string? employeeName, RejectACancelpprovalRequest? cancelRequest);
        Task<DataTable> GetApprovalSummaryAmountDetails(string? referenceNo);
        Task<DataTable> GetApprovalTotalItemsAndAmount(string? referenceNo);
        Task<int> UpdateApprovalSummaryItemsCountAmount(string? referenceNo, int? totalItems, double? itemsAmount, int? payableAmount);
        Task<DataTable> GetApprovalIsCompletePending(string? referenceNo);

        Task<DataTable> GetApprovalHasItemCode(string? referenceNo, string? itemCode);
        Task<DataTable> GetApprovalHasOtherItems(string? referenceNo, string? itemCode);

        Task<int> DeleteItemFromApproval(string? employeeId, DeleteApprovalItemRequest? deleteRequest);
        Task<int> AddItemInCreatedApproval(string? employeeId, AddUpdateItemInApprovalRequest? addRequest, ItemDetails itemDetails);
        Task<int> UpdateItemInCreatedApproval(string? employeeId, AddUpdateItemInApprovalRequest? updateRequest);
    }
}
