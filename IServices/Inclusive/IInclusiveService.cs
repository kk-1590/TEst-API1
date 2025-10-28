using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Account;

namespace AdvanceAPI.IServices.Inclusive
{
    public interface IInclusiveService
    {
        Task<ApiResponse> GetCampusList(string? employeeId);
    }
}
