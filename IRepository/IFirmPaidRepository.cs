using System.Data;

namespace AdvanceAPI.IRepository
{

    public interface IFirmPaidRepository
    {
        Task<DataTable> GetVendorIdOffices(string TransId);
        Task<DataTable> GetVendorRegDetails(string VendorId);
        Task<DataTable> DepartMentWise(string Cond);
        Task<DataTable> DepartMentWiseWithOUTAdvance(string Cond);
        Task<DataTable> VenderAdvanceBill(string Cond);
        Task<DataTable> AgainstBillBaseDetails(string TransId, string SeqNo);
        Task<DataTable> BillDistributionAdvanceDetails(string TransId, string SeqNo);
        Task<DataTable> GetYearWiseReport(string Cond);
        Task<DataTable> PersonWiseDetails(string Cond);
        Task<DataTable> HeadWise(string Cond);
        Task<DataTable> DepartmentWiseDetails(string Cond);
        Task<DataTable> sessionWiseReports(string Cond);
        Task<DataTable> VenderWiseDetails(string Cond);
        Task<DataTable> BillApplicationNewDetails(string EmpCode);
        Task<DataTable> SummarizedData(string EmpCode);
        Task<string> GetRelativePerson(string billId);
    }
}
