using System.Data;

namespace AdvanceAPI.IRepository
{
    public interface IApprovalRepository
    {
        Task<DataTable> GetDraftItemRefNo(string EmpCode,string AppType);
        Task<DataTable> GetAutoDraftItemRefNo();
    }
}
