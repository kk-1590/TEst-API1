using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Account;
using AdvanceAPI.DTO.Inclusive;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Inclusive;
using AdvanceAPI.Services.Account;
using System.Data;

namespace AdvanceAPI.Services.Inclusive
{
    public class InclusiveService : IInclusiveService
    {
        private readonly ILogger<InclusiveService> _logger;
        private readonly IGeneral _general;
        private readonly IIncusiveRepository _inclusive;

        public InclusiveService(ILogger<InclusiveService> logger, IGeneral general, IIncusiveRepository inclusive)
        {
            _logger = logger;
            _general = general;
            _inclusive = inclusive;
        }

        public async Task<ApiResponse> GetCampusList(string? employeeId)
        {
            DataTable campusList = await _inclusive.GetEmployeeCampus(employeeId);

            List<CampusResponse> campusResponses = new List<CampusResponse>();
            foreach (DataRow row in campusList.Rows)
            {
                CampusResponse campus = new CampusResponse
                {
                    CampusCode = row["CampusCode"]?.ToString() ?? string.Empty,
                    CampusName = row["CampusName"]?.ToString() ?? string.Empty
                };
                campusResponses.Add(campus);
            }

            return new ApiResponse(StatusCodes.Status200OK, "Success", campusResponses);
        }
    }
}
