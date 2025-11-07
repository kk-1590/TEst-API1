using System.Data;

namespace AdvanceAPI.IRepository
{
    public interface IAccountRepository
    {
        Task<DataTable> GetUserStatus(string? userId);
        Task<DataTable> GetMainPassword();
        Task<DataTable> GetAdvanceAccess(string? userId);
        Task<DataTable> GetAdditionalEmployeeCode(string? userId);
        Task SaveToken(string? userId, string? name, string? role, string? token, string? tokenExpiresAt, string? refreshToken, string? refreshTokenExpiresAt, string? againstToken);
        Task<DataTable> ValidateToken(string? userId, string? token, string? refreshToken);
        Task UseToken(string? token);
        Task<DataTable> IsTokenNotLoggedout(string? token);
        Task LogoutToken(string? token);
        Task<DataTable> GetEmployeeCampusCode(string? employeeId);
    }
}
