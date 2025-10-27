using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using System.Data;

namespace AdvanceAPI.IServices.DB
{
    public interface IDBOperations
    {

        Task<DataTable> SelectAsync(string query, List<SQLParameters>? parameters, DBConnections connection);

        Task<int> DeleteInsertUpdateAsync(string query, List<SQLParameters>? parameters, DBConnections connection);
        Task<DataTable> SelectProcedureAsync(string procedureName, List<SQLParameters>? parameters, DBConnections connection);
        Task<List<DataTable>> SelectMultipleProcedureAsync(string procedureName, List<SQLParameters>? parameters, DBConnections connection);
        Task ExecuteProcedureAsync(string procedureName, List<SQLParameters>? parameters, DBConnections connection);
    }
}
