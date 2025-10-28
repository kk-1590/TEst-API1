using System.Data;

namespace AdvanceAPI.IRepository
{
    public interface IIncusiveRepository
    {
        Task<DataTable> GetEmployeeCampus(string? userId);
    }
}
