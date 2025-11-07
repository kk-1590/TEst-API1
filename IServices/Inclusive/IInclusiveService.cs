using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Account;
using AdvanceAPI.DTO.Inclusive;
using AdvanceAPI.ENUMS.Inclusive;
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
        Task<bool> IsUserAllowed(string? employeeId, UserRolePermission userRolePermission);
        Task<string> GetEnCryptedKey();
        Task<string> SaveFile(string FileName, string FilePath, IFormFile file,string Ext);
        string CallWebService2(string url, string rno, string campusCode, string host, string mnth, string yr);
        string CallWebService(string url, string rno, string host, string mnth, string yr);
        Task<ApiResponse> GetApprovalCancellationReasons();
        Task<EmployeeDetails> GetEmployeeDetailsByEmployeeCode(string? employeeCode);
        Task<ItemDetails> GetItemDetailsByItemCode(string? itemCode);
    }
}
