using System.Data;

namespace AdvanceAPI.DTO.Media
{
    public record MediaTypeHeadsResponse
    {
        public string Id { get; init; } = string.Empty;
        public string Media { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;

        public MediaTypeHeadsResponse() { }

        public MediaTypeHeadsResponse(DataRow dr) : this(dr["Id"]?.ToString() ?? string.Empty, dr["Media"]?.ToString() ?? string.Empty, dr["Title"]?.ToString() ?? string.Empty)
        {

        }

        public MediaTypeHeadsResponse(string id, string media, string title)
        {
            Id = id;
            Media = media;
            Title = title;
        }
    }
}
