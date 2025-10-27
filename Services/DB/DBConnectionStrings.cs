using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IServices.DB;

namespace AdvanceAPI.Services.DB
{
    public class DBConnectionStrings : IDBConnectionStrings
    {
        private readonly IConfiguration _configuration;

        private static readonly Dictionary<DBConnections, string?> _connectionStrings = new Dictionary<DBConnections, string?>();

        public DBConnectionStrings(IConfiguration configuration)
        {
            _configuration = configuration;
            SetupAllConnections();
        }

        public string? GetConnectionString(DBConnections connection)
        {
            return _connectionStrings[connection]?.ToString();
        }

        public void SetupAllConnections()
        {
            IConfigurationSection? connectionStrings = _configuration.GetSection("ConnectionStrings");
            _connectionStrings[DBConnections.Advance] = connectionStrings[nameof(DBConnections.Advance)];
            _connectionStrings[DBConnections.Salary] = connectionStrings[nameof(DBConnections.Salary)];
        }
    }
}
