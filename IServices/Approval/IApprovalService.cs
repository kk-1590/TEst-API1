using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Approval;

namespace AdvanceAPI.IServices.Approval
{
    public interface IApprovalService
    {
        Task<ApiResponse> AddItemDraft(AddStockItemRequest AddStockItem, string EmpCode);
    }
}
