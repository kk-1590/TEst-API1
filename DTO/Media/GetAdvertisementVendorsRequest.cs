namespace AdvanceAPI.DTO.Media
{
    public class GetAdvertisementVendorsRequest : PaginationSectionRequest
    {
        public string? VendorName { get; set; }
    }
}
