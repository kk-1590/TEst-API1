using System.Data;

namespace AdvanceAPI.DTO.Media
{
    public record AdvertisementVendorsResponse
    {
        public string? VendorName { get; init; }
        public string? VendorId { get; init; }

        public AdvertisementVendorsResponse()
        {

        }

        public AdvertisementVendorsResponse(DataRow dr) : this(dr["VendorName"]?.ToString() ?? string.Empty, dr["VendorID"]?.ToString() ?? string.Empty)
        {

        }

        public AdvertisementVendorsResponse(string vendorName, string vendorID)
        {
            VendorName = vendorName;
            VendorId = vendorID;
        }
    }
}
