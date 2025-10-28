using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.Services.Account;
using AdvanceAPI.SQL.Account;
using AdvanceAPI.SQLConstants.Inclusive;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace AdvanceAPI.Repository
{
    public class IncusiveRepository(ILogger<IncusiveRepository> logger, IDBOperations dbContext, IHttpContextAccessor httpContextAccessor) : IIncusiveRepository
    {
        private readonly ILogger<IncusiveRepository> _logger = logger;
        private readonly IDBOperations _dbContext = dbContext;

        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        public async Task<DataTable> GetEmployeeCampus(string? userId)
        {
            try
            {
                var parameters = new List<SQLParameters>()
                {
                    new SQLParameters("@EmployeeCode", userId ?? string.Empty),
                };
                return await _dbContext.SelectAsync(InclusiveSql.GET_ALLOWED_CAMPUS, parameters, DBConnections.Salary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetEmployeeCampus.");
                throw;
            }

        }
    }
}
