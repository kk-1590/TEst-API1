using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Account;
using AdvanceAPI.DTO.Inclusive;
using System.Data;

namespace AdvanceAPI.IServices.Inclusive
{
    public interface IInclusiveService
    {
        Task<ApiResponse> GetCampusList(string? employeeId);
        Task<ApiResponse> GetApprovalType();
        Task<ApiResponse> GetPurchaseDepartment();
        Task<ApiResponse> GetItems(GetPurchaseItemRequest getPurchaseItemRequest);
        Task<ApiResponse> GetStock(string itemCode, string CampusCode);
        Task<ApiResponse> GetAllMaad();
        Task<ApiResponse> GetAllDepartments(string? employeeId);
        Task<ApiResponse> GetVendors(string? search);
        Task<ApiResponse> GetVendorSubFirms(int? vendorId);
        Task<ApiResponse> GetAllEmployees(string? search);
        Task<ApiResponse> GetBudget(GetFirmBudgetRequest? firm);
    }
}
