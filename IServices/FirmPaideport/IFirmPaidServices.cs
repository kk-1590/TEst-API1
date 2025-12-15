using AdvanceAPI.DTO;
using AdvanceAPI.DTO.FirmPaidDetails;

namespace AdvanceAPI.IServices.FirmPaideport
{

    public interface IFirmPaidServices
    {
        Task<ApiResponse> GetFirmPaidReportById(string EmpName, FirmPaidrequest req);
    }
}
