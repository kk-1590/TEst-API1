using System.Data;
using AdvanceAPI.DTO.Approval;

namespace AdvanceAPI.IRepository
{
    public interface IApprovalRepository
    {
        Task<DataTable> GetDraftItemRefNo(string EmpCode, string AppType);
        Task<DataTable> GetAutoDraftItemRefNo();
        Task<int> AddDraftItem(string RefNo, AddStockItemRequest AddStock, string EmpCode);
    }
}
