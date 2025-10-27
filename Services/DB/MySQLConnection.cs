using MySql.Data.MySqlClient;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IServices.DB;

namespace AdvanceAPI.Services.DB
{
    public class MySQLConnection : IMySQLConnection
    {
        private readonly IDBConnectionStrings _myConnections;
        public MySQLConnection(IDBConnectionStrings dBConnectionStrings)
        {
            _myConnections = dBConnectionStrings;
        }
        public MySqlConnection GetMySqlConnection(DBConnections connection)
        {
            return new MySqlConnection(_myConnections.GetConnectionString(connection));
        }
    }
}
