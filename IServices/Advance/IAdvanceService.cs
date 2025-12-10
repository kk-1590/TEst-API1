using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Advance;
using AdvanceAPI.DTO.Advance.Bill;

namespace AdvanceAPI.IServices.Advance
{
    public interface IAdvanceService
    {
        Task<ApiResponse> GetApprovals(string RefNo);
        Task<ApiResponse> GetDepartmentHod(string Dept);
        Task<ApiResponse> GenerateAdvance(GenerateAdvancerequest req, string EmpCode, string EmpName, string RefNo);
        Task<ApiResponse> GetFinalAuth(string CampusCode);
        Task<ApiResponse> GetMyAdvanceReq(string Status, string Session, string CampusCode, string RefNo, string Type, string EmpCode, string Department, string itemsFrom, string noOfItems);
        Task<ApiResponse> DeleteAdvance(string RefNo, string EmpCode);
        Task<ApiResponse> GetApprovalDetails(string EmpCode, string EmpCodeAdd, string Type, GetMyAdvanceRequest req);
        Task<ApiResponse> GetAdvanceType();
        Task<ApiResponse> PassAdvanceApproval(string EmpCode, string EmpName, PassApprovalRequest req);
        Task<ApiResponse> RejectApproval(string EmpCode, string EmpName, PassApprovalRequest req);
        Task<ApiResponse> GetBasicDetailsForGenerateBill(string RefNo);
        Task<ApiResponse> GetAuthority(string RefNo);
        Task<ApiResponse> GeneratereNoForUploadBill(string Type, string EmpCode, bool IsThousand, string RefNo = "");
        Task<ApiResponse> GetPurchase(string Type, string Empcode);
        Task<ApiResponse> GetBasicDetailsVisit(string AppType, string Type, string EmpCode);
        Task<ApiResponse> GetartnerVisit(string AppType, string Type, string EmpCode);
        Task<ApiResponse> GetSchoolVisit(string AppType, string Type, string EmpCode);
        Task<ApiResponse> GetSubfirm(int VendorId);
        Task<ApiResponse> LoadOffices(string TypeId, string VendorId);
        Task<ApiResponse> GetBasicDetailsForGenerateAdvance(string Values, string Type);
        Task<ApiResponse> SaveBill(string EmpCode, string EmpName, AddBillGenerateRequest req,string Type);
        Task<ApiResponse> GetDetails(string Cond);
        Task<ApiResponse> GetAdvanceApprovalPrint(string ReferenceNo, string EmployeeId, string EmployeeType);
        Task<ApiResponse> GetLockDetails(string Type);
        Task<ApiResponse> GetBasicDetailsForGenerateBillAgainstAdvance(string RefNo);
        Task<ApiResponse> GeneratereNoForUploadBillAgainstAdvance(string Type, string EmpCode, string RefNo = "");
        Task<ApiResponse> GetAdvancebillAuth(string refNo);
        Task<ApiResponse> GetPurchaseApprovalBill(string EmpCode, string Type, GetApprovalBillRequest req);
        Task<ApiResponse> UpdatePurchaseBillDate(string empCode, UpdatePurchaseBillDateRequest req);
        Task<ApiResponse> UpdateBillDetails(string RefNo);
        Task<ApiResponse> DeleteBill(string EmpCode, string TransId,int Opr=0);
        Task<ApiResponse> UpdateBill(string EmpCode, string EmpName, AddBillGenerateRequest req, string Type);
        Task<ApiResponse> GetBillDetails(string TransId, string EmpName);
        Task<ApiResponse> GetVendorDetails(string VendorId);
        Task<ApiResponse> LoadTransactionDetails(string TransId);
        Task<ApiResponse> getAuthForDirectBill(string CampusCode);
        Task<ApiResponse> GetBillApprovalFilterSessions();
        Task<ApiResponse> GetBillApprovalFilterInitiatedBy();
        Task<ApiResponse> GetBillApprovalFilterChequeBy();
        Task<ApiResponse> GetBillApprovalDetails(GetBillApprovalRequest? getBillApprovalRequest, string employeeId, string role, string name);
        Task<ApiResponse> GetChequeAuthority();
        Task<ApiResponse> GetPayentDetails();
        Task<ApiResponse> SaveChequeDetails(string EmpCode, string Type, string EmpName, SaveCheDetailsRequest req);
    }
}
