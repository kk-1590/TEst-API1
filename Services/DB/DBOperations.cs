using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IServices.DB;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading;

namespace AdvanceAPI.Services.DB
{
    public class DBOperations : IDBOperations
    {
        private const int DefaultTimeout = 1200;
        private readonly IMySQLConnection _sqlConnection;
        private readonly ILogger<DBOperations> _logger;

        public DBOperations(IMySQLConnection sqlConnection, ILogger<DBOperations> logger)
        {
            _sqlConnection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<DataTable> SelectAsync(string query, List<SQLParameters>? parameters, DBConnections connection)
        {
            return await ExecuteQueryAsync(query, parameters, connection, DefaultTimeout).ConfigureAwait(false);
        }
        public async Task<DataTable> SelectAsync(string query,  DBConnections connection)
        {
            return await ExecuteQueryAsync(query, new List<SQLParameters>(), connection, DefaultTimeout).ConfigureAwait(false);
        }

        public async Task<int> DeleteInsertUpdateAsync(string query, List<SQLParameters>? parameters, DBConnections connection)
        {
            return await ExecuteNonQueryAsync(query, parameters, connection).ConfigureAwait(false);
        }

        private async Task<DataTable> ExecuteQueryAsync(string query, List<SQLParameters>? parameters, DBConnections connection, int commandTimeout)
        {
            ValidateQuery(query);

            try
            {
                await using var sqlConnection = _sqlConnection.GetMySqlConnection(connection);
                await sqlConnection.OpenAsync(CancellationToken.None).ConfigureAwait(false);

               await using var sqlCommand = CreateSqlCommand(query, parameters ?? new List<SQLParameters>(), sqlConnection, commandTimeout);

                await using var sqlDataReader = await sqlCommand.ExecuteReaderAsync(CancellationToken.None).ConfigureAwait(false);
                var dataTable = new DataTable();
                dataTable.Load(sqlDataReader);
                return dataTable;
            }
            catch (Exception ex)
            {
                LogError(ex, query, parameters ?? new List<SQLParameters>(), connection);
                throw new DataAccessException("An error occurred while executing the query.", ex);
            }
        }

        private async Task<int> ExecuteNonQueryAsync(string query, List<SQLParameters>? parameters, DBConnections connection)
        {
            ValidateQuery(query);

            try
            {
                using var sqlConnection = _sqlConnection.GetMySqlConnection(connection);
                await sqlConnection.OpenAsync(CancellationToken.None).ConfigureAwait(false);



                using var sqlCommand = CreateSqlCommand(query, parameters ?? new List<SQLParameters>(), sqlConnection);

                int result = await sqlCommand.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                LogError(ex, query, parameters ?? new List<SQLParameters>(), connection);
                throw new DataAccessException("An error occurred while executing the query.", ex);
            }
        }

        public async Task<DataTable> SelectProcedureAsync(string procedureName, List<SQLParameters>? parameters, DBConnections connection)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException("Procedure name cannot be null or empty.", nameof(procedureName));
            }

            try
            {
                using var sqlConnection = _sqlConnection.GetMySqlConnection(connection);
                await sqlConnection.OpenAsync(CancellationToken.None).ConfigureAwait(false);

                using var sqlCommand = new MySqlCommand(procedureName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (parameters is { Count: > 0 })
                {
                    foreach (var parameter in parameters)
                    {
                        sqlCommand.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);
                    }
                }

                using var sqlDataReader = await sqlCommand.ExecuteReaderAsync(CancellationToken.None).ConfigureAwait(false);

                var dataTable = new DataTable();
                dataTable.Load(sqlDataReader);
                return dataTable;
            }
            catch (Exception ex)
            {
                LogError(ex, procedureName, parameters ?? new List<SQLParameters>(), connection);
                throw new DataAccessException("An error occurred while executing the stored procedure.", ex);
            }
        }
        public async Task<List<DataTable>> SelectMultipleProcedureAsync(string procedureName, List<SQLParameters>? parameters, DBConnections connection)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException("Procedure name cannot be null or empty.", nameof(procedureName));
            }

            try
            {
                using var sqlConnection = _sqlConnection.GetMySqlConnection(connection);
                await sqlConnection.OpenAsync(CancellationToken.None).ConfigureAwait(false);

                using var sqlCommand = new MySqlCommand(procedureName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (parameters is { Count: > 0 })
                {
                    foreach (var parameter in parameters)
                    {
                        sqlCommand.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);
                    }
                }

                var dataTables = new List<DataTable>();
                using var sqlDataReader = await sqlCommand.ExecuteReaderAsync(CancellationToken.None).ConfigureAwait(false);

                while (!sqlDataReader.IsClosed)
                {
                    var dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    dataTables.Add(dataTable);
                }

                return dataTables;
            }
            catch (Exception ex)
            {
                LogError(ex, procedureName, parameters ?? new List<SQLParameters>(), connection);
                throw new DataAccessException("An error occurred while executing the stored procedure.", ex);
            }
        }

        public async Task ExecuteProcedureAsync(string procedureName, List<SQLParameters>? parameters, DBConnections connection)
        {
            if (string.IsNullOrWhiteSpace(procedureName))
            {
                throw new ArgumentException("Procedure name cannot be null or empty.", nameof(procedureName));
            }

            try
            {
                using var sqlConnection = _sqlConnection.GetMySqlConnection(connection);
                await sqlConnection.OpenAsync(CancellationToken.None).ConfigureAwait(false);

                using var sqlCommand = new MySqlCommand(procedureName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                if (parameters is { Count: > 0 })
                {
                    foreach (var parameter in parameters)
                    {
                        sqlCommand.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);
                    }
                }
                await sqlCommand.ExecuteNonQueryAsync(CancellationToken.None).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LogError(ex, procedureName, parameters ?? new List<SQLParameters>(), connection);
                throw new DataAccessException("An error occurred while executing the stored procedure.", ex);
            }
        }

        private static void ValidateQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Query cannot be null or empty.", nameof(query));
            }
        }

        private MySqlCommand CreateSqlCommand(string query, List<SQLParameters>? parameters, MySqlConnection sqlConnection, int commandTimeout = DefaultTimeout, MySqlTransaction? transaction = null)
        {
            var sqlCommand = new MySqlCommand(query, sqlConnection, transaction)
            {
                CommandTimeout = commandTimeout
            };

            if (parameters?.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    sqlCommand.Parameters.AddWithValue(parameter.Name, parameter.Value ?? DBNull.Value);
                }
            }

            return sqlCommand;
        }

        private void LogError(Exception ex, string queryOrProcedure, List<SQLParameters> parameters, DBConnections connection)
        {
            _logger.LogError(ex, "An error occurred while executing: {QueryOrProcedure}\nParameters: {@Parameters}\nConnection: {Connection}", queryOrProcedure, parameters, connection);
        }

        public void Dispose()
        {
            
        }
    }
}
