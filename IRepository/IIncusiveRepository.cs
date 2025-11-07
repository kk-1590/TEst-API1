using AdvanceAPI.DTO.Inclusive;
using AdvanceAPI.ENUMS.Inclusive;
using System.Data;

namespace AdvanceAPI.IRepository
{
    public interface IIncusiveRepository
    {
        Task<DataTable> GetEmployeeCampus(string? userId);
        Task<DataTable> GetApprovalType();
        Task<DataTable> GetPurchaseDepartment();
        Task<DataTable> GetItems(GetPurchaseItemRequest getPurchaseItemRequest);
        Task<DataTable> GetBaseUrlFromTable();
        Task<DataTable> GetItemDetails(string ItemCode);
        Task<DataTable> GetItemMakeCode(string ItemName, string Size, string Unit);
        Task<DataTable> GetItemsDetails(string ItemName, string Make);
        Task<DataTable> GetEmployeeCampusCodes(string? userId);
        Task<DataTable> GetAllMaad();
        Task<DataTable> GetAllDepartments(List<string>? campusCodes);
        Task<DataTable> GetVendors(string? search);
        Task<DataTable> GetVendorSubFirms(int? vendorId);
        Task<DataTable> GetAllEmployees(string? search);
        Task<DataTable> GetIsVendorBudgetEnable(string? vendorId);
        Task<DataTable> IsFirmBudgetExist(GetFirmBudgetRequest? firm);
        Task<DataTable> IsFirmBudgetPending(GetFirmBudgetRequest? firm, string? fromDate, string? toDate, ApprovalTypes approvalTypes);
        Task<DataTable> GetVendorBudgetDetails(GetFirmBudgetRequest? firm, string? fromDate, string? toDate);
        Task<DataTable> CheckUserRole(string? employeeCode, UserRolePermission userRolePermission);
        Task<DataTable> GetFileKey();
        Task<DataTable> GetApprovalCancellationReasons();
        Task<DataTable> GetEmployeeDetails(string? employeeCode);
        Task<DataTable> GetItemDetailsByItemCode(string? itemCode);
    }
}
