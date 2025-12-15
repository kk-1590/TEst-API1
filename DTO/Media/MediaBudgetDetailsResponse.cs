using System.Data;

namespace AdvanceAPI.DTO.Media
{
    public record MediaBudgetDetailsResponse
    {
        public string Id { get; init; } = string.Empty;
        public string Session { get; init; } = string.Empty;
        public string ScheduleType { get; init; } = string.Empty;
        public string MediaType { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Amount { get; init; } = string.Empty;
        public string MediaTypeId { get; init; } = string.Empty;
        public bool IsPdfExists { get; set; } = false;


        public MediaBudgetDetailsResponse()
        {

        }

        public MediaBudgetDetailsResponse(DataRow dr) : this(dr["Id"]?.ToString() ?? string.Empty,
                                                    dr["Session"]?.ToString() ?? string.Empty,
                                                    dr["ScheduleType"]?.ToString() ?? string.Empty,
                                                    dr["MediaType"]?.ToString() ?? string.Empty,
                                                    dr["Title"]?.ToString() ?? string.Empty,
                                                    dr["Amount"]?.ToString() ?? string.Empty,
                                                    dr["MediaTypeId"]?.ToString() ?? string.Empty)
        {

        }

        public MediaBudgetDetailsResponse(string id, string session, string scheduleType, string mediaType, string title, string amount, string mediaTypeId)
        {
            Id = id;
            Session = session;
            ScheduleType = scheduleType;
            MediaType = mediaType;
            Title = title;
            Amount = amount;
            MediaTypeId = mediaTypeId;
        }
    }
}
