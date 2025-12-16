using AdvanceAPI.DTO;
using AdvanceAPI.DTO.FirmPaidDetails;
using AdvanceAPI.DTO.FirmPaidDetails.ApplicationReport;

namespace AdvanceAPI.IServices.FirmPaideport
{

    public interface IFirmPaidServices
    {
        Task<ApiResponse> GetFirmPaidReportById(string EmpName, FirmPaidrequest req);
        Task<ApiResponse> ApplicationReport(string EmpCode, ApplicationReportRequest req);
    }
}
