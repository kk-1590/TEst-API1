using System.ComponentModel.DataAnnotations;

namespace AdvanceAPI.DTO.VenderPriceComp;

public class InsertDetails
{
    [Required]
    public string ItemCode { get; set; }
    [Required]
    public string ItemName { get; set; }
    [Required]
    public string Price { get; set; }
    [Required]
    public string Remark {get; set;}
    [Required]
    public string VendorNo { get; set; }
    [Required]
    public string VendorName { get; set; }
    [Required]
    public string VendorId { get; set; }
}