using System.Data;

namespace AdvanceAPI.DTO.Media
{
    public record EditionAdvertisementHeadsResponse
    {
        public string Id { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;

        public EditionAdvertisementHeadsResponse() { }

        public EditionAdvertisementHeadsResponse(DataRow dr) : this(dr["Id"]?.ToString() ?? string.Empty, dr["Type"]?.ToString() ?? string.Empty, dr["Value"]?.ToString() ?? string.Empty)
        {

        }

        public EditionAdvertisementHeadsResponse(string id, string type, string value)
        {
            Id = id;
            Type = type;
            Value = value;
        }
    }
}
