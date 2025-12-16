using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.SQLConstants.FirmPaidReport;
using System.Data;

namespace AdvanceAPI.Repository
{
    public class FirmPaidRepository : IFirmPaidRepository
    {
        private ILogger<FirmPaidRepository> _logger;
        private readonly IGeneral _general;
        private readonly IDBOperations _dbOperations;
        public FirmPaidRepository(IDBOperations dBOperations,IGeneral general, ILogger<FirmPaidRepository> logger)
        {
            _dbOperations = dBOperations;
            _general= general;
            _logger = logger;
        }

        public async Task<DataTable> GetVendorIdOffices(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransactionID", TransId));
                return await _dbOperations.SelectAsync(FirmReportSql.GET_VENDER_ID_OFFICES, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {

                _logger.LogError("Error During GetVendorIdOffices..", ex);
                throw;
            }
        }
        public async Task<DataTable> GetVendorRegDetails(string VendorId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@VendorID", VendorId));
                return await _dbOperations.SelectAsync(FirmReportSql.VENDOR_REG_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetVendorRegDetails..", ex);
                throw;
            }
        }
        public async Task<DataTable> DepartMentWise(string Cond)
        {
            try
            {
                return await _dbOperations.SelectAsync(FirmReportSql.DEPARTMENT_WISE_DETAILS.Replace("@Condition", Cond), null, DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error During DepartMentWise..", ex);
                throw;
            }
        }
        public async Task<DataTable> DepartMentWiseWithOUTAdvance(string Cond)
        {
            try
            {
                return await _dbOperations.SelectAsync(FirmReportSql.DEPARTMENT_WISE_DETAILS_WITHOUT_ADVANCE.Replace("@Condition", Cond), null, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During DepartMentWiseWithAdvance..", ex);
                throw;
            }
        }
        public async Task<DataTable> VenderAdvanceBill(string Cond)
        {
            try
            {
                return await _dbOperations.SelectAsync(FirmReportSql.VENDER_ADVANCE_BILL.Replace("@Condition", Cond), null, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During VenderAdvanceBill..", ex);
                throw;
            }
        }
        public async Task<DataTable> AgainstBillBaseDetails(string TransId,string SeqNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                param.Add(new SQLParameters("@SeqNo", SeqNo));
                return await _dbOperations.SelectAsync(FirmReportSql.BILL_AGAINST_BASE_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During AgainstBillBaseDetails..", ex);
                throw;
            }
        }
        public async Task<DataTable> BillDistributionAdvanceDetails(string TransId, string SeqNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                param.Add(new SQLParameters("@SeqNo", SeqNo));
                return await _dbOperations.SelectAsync(FirmReportSql.BILL_DISTRIBUTION_ADVANCE_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During BillDistributionAdvanceDetails..", ex);
                throw;
            }
        }
        public async Task<DataTable> GetYearWiseReport(string Cond)
        {
            try
            {
                return await _dbOperations.SelectAsync(FirmReportSql.YEAR_WISE_RECORD.Replace("@Condition", Cond),DBConnections.Advance);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error During GetYearWiseReport..", ex);
                throw;
            }
        }
        public async Task<DataTable> PersonWiseDetails(string Cond)
        {
            try
            {
                return await _dbOperations.SelectAsync(FirmReportSql.PERSON_WISE_REPORT.Replace("@Condition", Cond), DBConnections.Advance);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error During PersonWiseDetails..", ex);
                throw;
            }
        }
        public async Task<DataTable> HeadWise(string Cond)
        {
            try
            {
                return await _dbOperations.SelectAsync(FirmReportSql.HEAD_WISE_DETAILS.Replace("@Condition", Cond), DBConnections.Advance);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error During HeadWise..", ex);
                throw;
            }
        }
        public async Task<DataTable> DepartmentWiseDetails(string Cond)
        {
            try
            {
                return await _dbOperations.SelectAsync(FirmReportSql.DEPARMENT_WISE.Replace("@Condition", Cond),DBConnections.Advance);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error During DepartmentWiseDetails..", ex);
                throw;
            }
        }
        public async Task<DataTable> sessionWiseReports(string Cond)
        {
            try
            {
                return await _dbOperations.SelectAsync(FirmReportSql.SESSION_WISE_REPORT.Replace("@Condition", Cond),DBConnections.Advance);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error During DepartmentWiseDetails..", ex);
                throw;
            }
        }
        public async Task<DataTable> VenderWiseDetails(string Cond)
        {
            try
            {
                return await _dbOperations.SelectAsync(FirmReportSql.VENDER_WISE.Replace("@Condition", Cond), DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During VenderWiseDetails..", ex);
                throw;
            }
        }
        public async Task<DataTable> BillApplicationNewDetails(string EmpCode)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                return await _dbOperations.SelectAsync(FirmReportSql.BILL_APPLICATION_NEW_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During BillApplicationNewDetails..", ex);
                throw;
            }
        }
        public async Task<DataTable> SummarizedData(string EmpCode)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode",EmpCode));
                return await _dbOperations.SelectAsync(FirmReportSql.Summarized_STATEMENT, param, DBConnections.Advance); ;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error During SummarizedData..", ex);
                throw;
            }
        }

        public async Task<string> GetRelativePerson(string billId)
        {
            DataTable D =await _dbOperations.SelectAsync ("Select IssuedTo from advances.bill_applications_new where BillId=" + billId+"",DBConnections.Advance);
            if (D.Rows.Count > 0)
            {
                return D.Rows[0][0].ToString()??"";
            }
            else
            {
                D = await _dbOperations.SelectAsync("Select RelativePersonID from advances.bill_base where TransactionID=" + billId+"",DBConnections.Advance);
                if (D.Rows.Count > 0)
                {
                    return D.Rows[0][0].ToString() ?? "";
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
