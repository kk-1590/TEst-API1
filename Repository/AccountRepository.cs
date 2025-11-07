using Microsoft.AspNetCore.Identity.Data;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.Services.Account;
using System.Data;
using AdvanceAPI.SQL.Account;

namespace AdvanceAPI.Repository
{
    public class AccountRepository(ILogger<AccountRepository> logger, IDBOperations dbContext, IHttpContextAccessor httpContextAccessor) : IAccountRepository
    {
        private readonly ILogger<AccountRepository> _logger = logger;
        private readonly IDBOperations _dbContext = dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<DataTable> GetUserStatus(string? userId)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@Employee_Code", userId ?? string.Empty),
                };
                return await _dbContext.SelectAsync(AccountSql.LOGIN_ACCOUNT_STATUS_CHECK, parameters, DBConnections.Salary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in user.");
                throw;
            }
        }

        public async Task<DataTable> GetMainPassword()
        {
            try
            {
                return await _dbContext.SelectAsync(AccountSql.GET_MAIN_PASSWORD, null, DBConnections.Salary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving main password.");
                throw;
            }

        }
        public async Task<DataTable> GetAdvanceAccess(string? userId)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@employee_code", userId ?? string.Empty),
                };
                return await _dbContext.SelectAsync(AccountSql.ADVANCE_ACCESS_CHECK, parameters, DBConnections.Salary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in user.");
                throw;
            }
        }
        public async Task<DataTable> GetAdditionalEmployeeCode(string? userId)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@employee_code", userId ?? string.Empty),
                };
                return await _dbContext.SelectAsync(AccountSql.GET_ADDITIONAL_EMPLOYEE_CODE, parameters, DBConnections.Salary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in user.");
                throw;
            }
        }


        public async Task SaveToken(string? userId, string? name, string? role, string? token, string? tokenExpiresAt, string? refreshToken, string? refreshTokenExpiresAt, string? againstToken)
        {
            try
            {
                var remoteIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                var parameters = new List<SQLParameters>(10)
                {
                    new SQLParameters("@UserId", userId ?? string.Empty),
                    new SQLParameters("@Name", name ?? string.Empty),
                    new SQLParameters("@Role", role ?? string.Empty),
                    new SQLParameters("@Token", token ?? string.Empty),
                    new SQLParameters("@TokenExpiresAt", tokenExpiresAt ?? string.Empty),
                    new SQLParameters("@RefreshToken", refreshToken ?? string.Empty),
                    new SQLParameters("@RefreshTokenExpiresAt", refreshTokenExpiresAt ?? string.Empty),
                    new SQLParameters("@TokenGeneratedFrom", remoteIp),
                    new SQLParameters("@GeneratedAgainstRefreshToken", againstToken ?? string.Empty)
                };

                await _dbContext.DeleteInsertUpdateAsync(AccountSql.ADD_TOKEN, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving token to database.");
                throw;
            }
        }

        public async Task<DataTable> ValidateToken(string? userId, string? token, string? refreshToken)
        {
            try
            {
                var parameters = new List<SQLParameters>(4)
                {
                    new SQLParameters("@UserId", userId ?? string.Empty),
                    new SQLParameters("@Token", token ?? string.Empty),
                    new SQLParameters("@RefreshToken", refreshToken ?? string.Empty)
                };

                return await _dbContext.SelectAsync(AccountSql.VALIDATE_TOKEN, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token in database.");
                throw;
            }
        }

        public async Task UseToken(string? token)
        {
            try
            {
                var remoteIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                var parameters = new List<SQLParameters>(3)
                {
                    new SQLParameters("@TokenId", token ?? string.Empty),
                    new SQLParameters("@RefreshTokenUseFrom", remoteIp)
                };

                await _dbContext.DeleteInsertUpdateAsync(AccountSql.USE_TOKEN, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error using token in database.");
                throw;
            }
        }
        public async Task<DataTable> IsTokenNotLoggedout(string? token)
        {
            try
            {
                var parameters = new List<SQLParameters>(3)
                {
                    new SQLParameters("@Token", token ?? string.Empty),
                };

                return await _dbContext.SelectAsync(AccountSql.TOKEN_IS_NOT_LOGGEDOUT, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error using token in database.");
                throw;
            }
        }

        public async Task LogoutToken(string? token)
        {
            try
            {
                string remoteIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                var parameters = new List<SQLParameters>(2)
                {
                    new SQLParameters("@Token", token ?? string.Empty),
                    new SQLParameters("@LogoutFrom", remoteIp ?? string.Empty)
                };

                await _dbContext.DeleteInsertUpdateAsync(AccountSql.LOGOUT_TOKEN, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging out token in database.");
                throw;
            }
        }

        public async Task<DataTable> GetEmployeeCampusCode(string? employeeId)
        {
            try
            {
                var parameters = new List<SQLParameters>(1)
                {
                    new SQLParameters("@EmployeeId", employeeId ?? string.Empty),
                };

                return await _dbContext.SelectAsync(AccountSql.GET_EMPLOYEE_CAMPUS_CODE, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting GetEmployeeCampusCode.");
                throw;
            }
        }
    }
}