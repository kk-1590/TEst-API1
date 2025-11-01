using System.Data;
using AdvanceAPI.DTO.VenderPriceComp;

namespace AdvanceAPI.IRepository;

public interface IVenderPriceCompRepository
{
    Task<DataTable> GetBasicPurchaseApprovalDetails(string referenceNo);
    Task<DataTable> GetBasiclockDetails(string referenceNo);
    Task<DataTable> GetVenderDetails(string referenceNo, string ItemCode);
    Task<DataTable> GetVendorDetails(string referenceNo);
    Task<int> InsertVenderInChart(string referenceNo);
    Task<DataTable> CheckLockComparisionChart(string referenceNo);
    Task<DataTable> GetAllVendor(string referenceNo);
    Task<int> InsertVendoorPrice(string EmpCode, string RefNo, InsertDetails Details);
    Task<DataTable> Checkvendorinchart(string ItemCode, string VendorNo, string RefNo);
    Task<int> LockDetails(string EmpCode, string RefNo);
}