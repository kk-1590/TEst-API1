using AdvanceAPI.DTO;
using AdvanceAPI.DTO.VenderPriceComp;

namespace AdvanceAPI.IServices.VenderPriceComp;

public interface IVenderPriceComparisionServices
{
    Task<ApiResponse> GetApprovalBasicDetails(string RefNo);
    Task<ApiResponse> GetItemDetails(string RefNo);
    Task<ApiResponse> GetVendorDetails(string RefNo);
    Task<ApiResponse> SubmitVendorDetails(string RefNo, string empCode, InsertDetails Details);
    Task<ApiResponse> LockDetails(string RefNo, string empCode);
    Task<string> SaveFile(string FileName, IFormFile file);
}