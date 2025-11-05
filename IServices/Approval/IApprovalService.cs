using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Approval;

namespace AdvanceAPI.IServices.Approval
{
    public interface IApprovalService
    {
        Task<ApiResponse> AddItemDraft(AddStockItemRequest AddStockItem, string EmpCode);
        Task<ApiResponse> GetDraftedItem(string EmpCode, string AppType, string CampusCode,string RefNo);
        Task<ApiResponse> GetDraftItemSummary(string EmpCode, string AppType, string CampusCode, string RefNo);
        Task<ApiResponse> GetApprovalSessions();
        Task<ApiResponse> GetApprovalFinalAuthorities(GetApprovalFinalAuthoritiesRequest? requestDetails);
        Task<ApiResponse> GetApprovalNumber3Authorities(GetNumber3AuthorityRequest? search);
        Task<ApiResponse> DeleteApprovalDraft(string? emploeeId, DeleteApprovalDraftRequest? deleteRequest);
        Task<ApiResponse> GenerateApproval(string EmpCode, GeneratePurchaseApprovalRequest GeneratePurchaseApproval);
        Task<ApiResponse> DeleteDraftedItem(string ItemId);
        Task<ApiResponse> GetMyApprovals(string? emploeeId, string? type, AprrovalsListRequest? search);
        Task<ApiResponse> DeleteApproval(string? emploeeId, string? referenceNo);
        Task<ApiResponse> GETDRAFt(string emploeeId);
        Task<ApiResponse> GetPOApprovalDetails(string? type, string? referenceNo);
        Task<ApiResponse> UpdateApprovalNote(string? employeeId, string? referenceNo, string? note);
        Task<ApiResponse> GetEditApprovalDetails(string? referenceNo);
        Task<ApiResponse> EditApprovalDetails(string referenceNo, UpdateApprovalEditDetails details, string EmpCode);
        Task<ApiResponse> GetPurchaseApproval(string EmpCode, string EmpCodeAdd, AprrovalsListRequest details);
        Task<ApiResponse> ValidateRepairWarrnty(string CampusCode, string SRNo);
        Task<ApiResponse> PassPurchaseApproval(string? employeeCode, PassApprovalRequest? passRequest);
        Task<ApiResponse> RejectPurchaseApproval(string? employeeCode, RejectACancelpprovalRequest? rejectRequest);
        Task<ApiResponse> CancelPurchaseApproval(string? employeeCode, RejectACancelpprovalRequest? cancelRequest);
    }
}
