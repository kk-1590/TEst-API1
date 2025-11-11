using System.Data;

namespace AdvanceAPI.DTO.Budget
{
    public record BudgetHeadMappingResponse
    {
        public string? HeadMapId { get; set; }
        public string? Session { get; set; }
        public string? CampusCode { get; set; }
        public string? CampusName { get; set; }
        public string? BudgetType { get; set; }
        public string? BudgetHead { get; set; }

        public BudgetHeadMappingResponse()
        {

        }

        public BudgetHeadMappingResponse(DataRow dr)
        {
            HeadMapId = dr["Id"]?.ToString() ?? string.Empty;
            Session = dr["Session"]?.ToString() ?? string.Empty;
            CampusCode = dr["CampusCode"]?.ToString() ?? string.Empty;
            CampusName = dr["CampusName"]?.ToString() ?? string.Empty;
            BudgetType = dr["BudgetType"]?.ToString() ?? string.Empty;
            BudgetHead = dr["BudgetHead"]?.ToString() ?? string.Empty;
        }
    }
}
