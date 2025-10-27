using AdvanceAPI.ENUMS.DB;

namespace AdvanceAPI.IServices.DB
{
    public interface IDBConnectionStrings
    {
        void SetupAllConnections();
        string? GetConnectionString(DBConnections connection);
    }
}
