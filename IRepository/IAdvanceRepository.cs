using AdvanceAPI.DTO.Advance;
using AdvanceAPI.DTO.DB;
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
        Task<DataTable> GetBackValue(string Type);
        Task<DataTable> GetPurchaseApprovalPValues(string RefNo);
        Task<int> SaveAdvance(GenerateAdvancerequest req, string EmpCode, string EmpName, string RefNo, string FirmName, string FirmPerson, string FirmEmail, string FirmPanNo, string FirmAddress, string FirmContactNo, string FirmAlternateContactNo, string BackValue, string PurchaseApprovalDetails,string PrevRefNo);
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
        Task<DataTable> GetPurchaseBasicForBillAgainstAdvance(string RefNo);
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
        Task<DataTable> GetBillBaseRefNo();
        Task<DataTable> getPurchaseApprovalDetails(string RefNo);
        Task<int> SaveBillSave(string EmpCode, string EmpName, AddBillGenerateRequest req, string BillBaseREfNo);
        Task<int> SaveBillAuth(string EmpCode, string EmpName, AddBillGenerateRequest req, string BillBaseREfNo);
        Task<int> UpdatePdf(string RefNo);
        Task<int> UpdateExcel(string RefNo);
        Task<DataTable> GetFirm(string cond);
        Task<DataTable> GetDateLock(string Type);

        Task<DataTable> GetMissionAdmissionReturnTypes();
        Task<DataTable> GetAdvancePrintApprovalDetails(string ReferenceNo);
        Task<DataTable> GetAdvanceOfficeMappingEmployeeCodes(string OfficeName, string Session);
        Task<DataTable> GetAdvanceEmployeeLeaveDetails(string EmployeeIds, string AdvanceFrom, string AdvanceTo);
        Task<DataTable> GetAdvanceBillSummaryDetails(string ReferenceNo);
        Task<DataTable> GetAdvancePaymentDetailsByPaymentGroupId(string ReferenceNo);
        Task<DataTable> GetAdvancePaymentDetailsByReferenceNo(string ReferenceNo);
        Task<DataTable> GetAdvancePaymentTransactionIdsByReferenceNo(string ReferenceNo);
        Task<DataTable> GetAdvanceBillDetailsByTransactionIds(string TransactionIds);
        Task<DataTable> GetAdvanceBillBillDistributionDetailsById(string BillId);
        Task<DataTable> GetAdvanceBillAgainstBaseDistributionDetailsByTransactionIds(string TransactionIds);
        Task<DataTable> GetAdvanceOtherSummaryDistributionDetailsByReferenceNo(string ReferenceNo);
        Task<DataTable> GetAdvanceAmountIssuedAgainstBudgetByReferenceNo(string ReferenceNo);
        Task<DataTable> GetAdvanceBudgetUsageSessionWiseByReferenceNo(string ReferenceNo);
        Task<DataTable> GetAdvanceBudgetUsageMonthWiseByReferenceNo(string ReferenceNo);
        Task<DataTable> GetrefnoForGenerateBillAdvance(string Type, string EmpCode,string RefNo);
        Task<DataTable> Auth1AdvanceBill(string refNo);
        Task<DataTable> Auth2AdvanceBill(string refNo);
        Task<DataTable> Auth3AdvanceBill(string refNo);
        Task<DataTable> Auth4AdvanceBill(string refNo);
        Task<DataTable> GetAuthAdvanceBillAuth4(string Campus);
        Task<DataTable> GetAuthAdvanceBillAuth1(string Campus);
        Task<DataTable> GetAuthAdvanceBillAuth2(string Campus);
        Task<DataTable> GetApprovalBill(string EmpCode, string Type, GetApprovalBillRequest req);
        Task<DataTable> GetIssuedAmount(string TransId);
        Task<DataTable> GetApprovalAuthority(string Tid);
        Task<DataTable> GetIssuedAmounREadyApproval(string TransId);
        Task<int> UpdateBillBase(string Cond, string TransId, string WarId, List<SQLParameters> lst);
        Task<int> UpdateRemark(string Remark, string TransId);

    }
}
