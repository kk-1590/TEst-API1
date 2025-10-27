using MySql.Data.MySqlClient;
using AdvanceAPI.ENUMS.DB;

namespace AdvanceAPI.IServices.DB
{
    public interface IMySQLConnection
    {
        MySqlConnection GetMySqlConnection(DBConnections connection);
    }
}
