using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Approval;

namespace AdvanceAPI.IServices.Approval
{
    public interface IApprovalService
    {
        Task<ApiResponse> AddItemDraft(AddStockItemRequest AddStockItem, string EmpCode);
        Task<ApiResponse> GetDraftedItem(string EmpCode, string AppType, string CampusCode);
        Task<ApiResponse> GetDraftItemSummary(string EmpCode, string AppType, string CampusCode);
        Task<ApiResponse> GetApprovalSessions();
        Task<ApiResponse> GetApprovalFinalAuthorities(GetApprovalFinalAuthoritiesRequest? requestDetails);
        Task<ApiResponse> GetApprovalNumber3Authorities(GetNumber3AuthorityRequest? search);
        Task<ApiResponse> DeleteApprovalDraft(string? emploeeId, DeleteApprovalDraftRequest? deleteRequest);
        Task<ApiResponse> GenerateApproval(string EmpCode, GeneratePurchaseApprovalRequest GeneratePurchaseApproval);
        Task<ApiResponse> DeleteDraftedItem(string ItemId);
        Task<ApiResponse> GetMyApprovals(string? emploeeId, string? type, AprrovalsListRequest? search);
        Task<ApiResponse> DeleteApproval(string? emploeeId, string? referenceNo);
    }
}
