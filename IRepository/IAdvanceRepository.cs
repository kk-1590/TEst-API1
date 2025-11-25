using AdvanceAPI.DTO.Advance;
using System.Data;

namespace AdvanceAPI.IRepository
{
    public interface IAdvanceRepository
    {
        Task<DataTable> CheckPreviousPendingBill(string RefNo);
        Task<DataTable> GetVendorDetails(string VendorId);
        Task<DataTable> PurchaseApprovalDetails(string RefNo);
        Task<DataTable> getVendorDetails(string VendorId, string Office);
        Task<DataTable> GetBalanceDetails(string RefNo);
        Task<DataTable> GetPreviousDetails(string RefNo);
        Task<DataTable> GetDepartmentHod(string Department);
        Task<DataTable> GetAdvanceRefNo();
        Task<DataTable> GetBackValue();
        Task<DataTable> GetPurchaseApprovalPValues(string RefNo);
        Task<int> SaveAdvance(GenerateAdvancerequest req, string EmpCode, string EmpName, string RefNo, string FirmName, string FirmPerson, string FirmEmail, string FirmPanNo, string FirmAddress, string FirmContactNo, string FirmAlternateContactNo, string BackValue, string PurchaseApprovalDetails);
        Task<DataTable> FinalAuth(string CampusCode);
        Task<DataTable> GetVendorTextValue(string VendorId);
        Task<DataTable> GetMyAdvanceDetails(string Status, string Session, string CampusCode, string RefNo,string Type,string EmpCode, string Department, string itemsFrom, string noOfItems);
        Task<int> DeleteAdvance(string RefNo, string EmpCode);
        Task<DataTable> GetPassApprovalDetails(string EmpCode, string EmpCodeAdd, string Type, GetMyAdvanceRequest req);
        Task<DataTable> GetApprovalType();
        Task<int> AdvanceApproveDetails(string EmpCode, string EmpName, PassApprovalRequest req);
        Task<DataTable> IsApproveDetails(string EmpCode, string EmpName, PassApprovalRequest req);
        Task<int> ApproveFinalStatus(string EmpCode, string EmpName, PassApprovalRequest req);
        Task<int> InsertBillBase(string EmpCode, string EmpName, PassApprovalRequest req);
        Task<DataTable> BillBaseDetails(string EmpCode, string EmpName, PassApprovalRequest req);
        Task<int> InsertBillAuthority(string BillTransId, string RefNo);
        Task<int> UpdateBillStatus(string BillTransId, string RefNo);
        Task<int> RejectAdvanceApproval(string EmpCode, string EmpName, PassApprovalRequest req);
        Task<DataTable> GetPurchaseBasicForBill(string RefNo);
        Task<DataTable> GetOffices(string VendorId);
        Task<string> CheckWarrentyUploadeOrNot(string RefNo);
        Task<DataTable> GetFinanceAuthority();
        Task<DataTable> Get4Auth();
        Task<DataTable> GetThirdAuth();
        Task<DataTable> LockMenu(string EmpCode);
        Task<DataTable> CanGenerateBill(string Type, string EmpCode, bool IsThousand, string RefNo = "");
        Task<DataTable> LoadApprovalDetails(string Type, string EmpCode);
        Task<DataTable> GetbasicPrintDetails(string Id);
        Task<DataTable> GetContactNo(string EmpCode);
        Task<DataTable> GetVisitBasicDetails(string Type, string EmpCode, string Prefix);
        Task<DataTable> GetPartnerVisit(string Type, string EmpCode, string Prefix);
        Task<DataTable> GetSchoolVisit(string Type, string EmpCode, string Prefix);
        Task<DataTable> LoadSubFirm(int VendorId);
        Task<DataTable> LoadOffices(string RefNo, string VendorId);
        Task<DataTable> GetDepartDetails(string Values, string Type);
    }
}
