using AdvanceAPI.DTO.Advance;
using AdvanceAPI.DTO.Advance.Bill;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.SQLConstants.Advance;
using AdvanceAPI.SQLConstants.Approval;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Mysqlx.Crud;
using MySqlX.XDevAPI;
using NuGet.Protocol.Plugins;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Utilities.Zlib;
using System.Data;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace AdvanceAPI.Repository
{
    public class AdvanceRepository : IAdvanceRepository
    {

        private ILogger<AdvanceRepository> _logger;
        private readonly IGeneral _general;
        private readonly IDBOperations _dbOperations;
        public AdvanceRepository(ILogger<AdvanceRepository> logger, IGeneral general, IDBOperations dBOperations)
        {
            _logger = logger;
            _general = general;
            _dbOperations = dBOperations;
        }

        public async Task<DataTable> CheckPreviousPendingBill(string RefNo)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.SelectAsync(AdvanceSql.CHECK_PREVIOUS_BILL_PENDING, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During CheckPreviousPendingBill..");
                throw;
            }
        }
        public async Task<DataTable> GetVendorDetails(string VendorId)
        {
            try
            {
                List<SQLParameters> sqlParametersList = new List<SQLParameters>();
                sqlParametersList.Add(new SQLParameters("@VenderID", VendorId));
                return await _dbOperations.SelectAsync(ApprovalSql.GET_VENDER_REGISTER, sqlParametersList, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetVendorDetails..");
                throw;
            }
        }
        public async Task<DataTable> GetVendorTextValue(string VendorId)
        {
            try
            {
                List<SQLParameters> sqlParametersList = new List<SQLParameters>();
                sqlParametersList.Add(new SQLParameters("@VendorId", VendorId));
                return await _dbOperations.SelectAsync(AdvanceSql.VENDOR_REG, sqlParametersList, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetVendorDetails..");
                throw;
            }
        }
        public async Task<DataTable> PurchaseApprovalDetails(string RefNo)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PURCHASE_APPROVAL_DETAILS, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During PurchaseApprovalDetails..");
                throw;
            }
        }
        public async Task<DataTable> getVendorDetails(string VendorId, string Office)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@VendorId", VendorId));
                sqlParameters.Add(new SQLParameters("@Office", Office));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_VENDOR_OFFICES, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During getVendorDetails..");
                throw;
            }
        }
        public async Task<DataTable> GetBalanceDetails(string RefNo)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@RefNo", RefNo));

                return await _dbOperations.SelectAsync(AdvanceSql.GET_AMOUNT_DETAILS, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetBalanceDetails...");
                throw;
            }
        }

        public async Task<DataTable> GetPreviousDetails(string RefNo)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@RefNo", RefNo));

                return await _dbOperations.SelectAsync(AdvanceSql.GET_PREVIOUS_ADVANCE_DETAILS, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetPreviousDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetDepartmentHod(string Department)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("@Dept", Department));

                return await _dbOperations.SelectAsync(AdvanceSql.GET_DEPARTMENT_HOD, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetDepartmentHod...");
                throw;
            }
        }


        public async Task<DataTable> GetAdvanceRefNo()
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_APPROVAL_REFNO, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetAdvanceRefNo...");
                throw;
            }
        }

        public async Task<DataTable> GetBackValue(string Type)
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PURCHASE_APPROVAL_BACK_VALUE.Replace("@Value", Type.Replace("Advance Form", "").Trim() + "%"), DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetBackValue...");
                throw;
            }
        }

        public async Task<DataTable> GetPurchaseApprovalPValues(string RefNo)
        {
            try
            {
                List<SQLParameters> sqlParameters = new List<SQLParameters>();
                sqlParameters.Add(new SQLParameters("RefNo", RefNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PURCHASE_APPROVAL_DETAILS_PREVIOUS_VALUE, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetPurchaseApprovalPValues...");
                throw;
            }
        }
        public async Task<int> SaveAdvance(GenerateAdvancerequest req, string EmpCode, string EmpName, string RefNo, string FirmName, string FirmPerson, string FirmEmail, string FirmPanNo, string FirmAddress, string FirmContactNo, string FirmAlternateContactNo, string BackValue, string PurchaseApprovalDetails, string PrevRefNo)
        {
            //            INSERT INTO otherapprovalsummary(
            //  ReferenceNo, Session, MyType, Note, Purpose, BillUptoValue, BillUptoType, IniId, IniName, IniOn, IniFrom, ForDepartment,
            //  FirmName, FirmPerson, FirmEmail, FirmPanNo, FirmAddress, FirmContactNo, FirmAlternateContactNo, Amount, TotalAmount,
            //  Status, App1ID, App1Name, App1Designation, App1Status, App1DoneOn, App2ID, App2Name, App2Designation, App2Status,
            //  App2DoneOn, App4ID, App4Name, App4Designation, App4Status, App4DoneOn, AppCat, MessageSend, VendorID, RelativePersonID,
            //  RelativePersonName, RelativeDesignation, RelativeDepartment, AppDate, ReferenceBillStatus, BillTill, ExtendedBillDate,
            //  R_Pending, R_Status, PExtra3, PExtra2, PExtra4, BudgetRequired, BudgetAmount, PreviousTaken, CurStatus, BudgetStatus,
            //  BudgetReferenceNo, App3ID, App3Name, App3Designation, App3Status, AdvFrom, AdvTo, CampusCode, CampusName
            //) VALUES(
            //  @ReferenceNo, @Session, @MyType, @Note, @Purpose, @BillUptoValue, @BillUptoType, @IniId, @IniName,now(), @IniFrom,
            //  @ForDepartment, @FirmName, @FirmPerson, @FirmEmail, @FirmPanNo, @FirmAddress, @FirmContactNo, @FirmAlternateContactNo,
            //  @Amount, @TotalAmount, @Status, @App1ID, @App1Name, @App1Designation, @App1Status, @App1DoneOn, @App2ID, @App2Name,
            //  @App2Designation, @App2Status, @App2DoneOn, @App4ID, @App4Name, @App4Designation, @App4Status, @App4DoneOn, @AppCat,
            //  @MessageSend, @VendorID, @RelativePersonID, @RelativePersonName, @RelativeDesignation, @RelativeDepartment, @AppDate,
            //  @ReferenceBillStatus, @BillTill, @ExtendedBillDate, @R_Pending, @R_Status, @PExtra3, @PExtra2, @PExtra4,
            //  @BudgetRequired, @BudgetAmount, @PreviousTaken, @CurStatus, @BudgetStatus, @BudgetReferenceNo, @App3ID, @App3Name,
            //  @App3Designation, @App3Status, @AdvFrom, @AdvTo, @CampusCode, @CampusName
            //);

            List<SQLParameters> param = new List<SQLParameters>();
            param.Add(new SQLParameters("@ReferenceNo", RefNo));
            param.Add(new SQLParameters("@ForDepartment", req.ForDepartment ?? string.Empty));
            param.Add(new SQLParameters("@Session", req.Session ?? string.Empty));
            param.Add(new SQLParameters("@MyType", req.ApprovalType ?? string.Empty));
            param.Add(new SQLParameters("@Note", req.Note ?? string.Empty));
            param.Add(new SQLParameters("@Purpose", req.Purpose ?? string.Empty));
            param.Add(new SQLParameters("@BillUptoValue", req.BillUptoValue ?? string.Empty));
            param.Add(new SQLParameters("@BillUptoType", "Day"));
            param.Add(new SQLParameters("@IniId", EmpCode));
            param.Add(new SQLParameters("@IniName", EmpName));
            param.Add(new SQLParameters("@IniFrom", _general.GetIpAddress()));
            param.Add(new SQLParameters("@FirmName", FirmName));
            param.Add(new SQLParameters("@FirmPerson", FirmPerson));
            param.Add(new SQLParameters("@FirmEmail", FirmEmail));
            param.Add(new SQLParameters("@FirmPanNo", FirmPanNo));
            param.Add(new SQLParameters("@FirmAddress", FirmAddress));
            param.Add(new SQLParameters("@FirmContactNo", FirmContactNo));
            param.Add(new SQLParameters("@FirmAlternateContactNo", FirmAlternateContactNo));
            param.Add(new SQLParameters("@Amount", req.Amount!));
            param.Add(new SQLParameters("@TotalAmount", req.TotalAmount ?? string.Empty));
            param.Add(new SQLParameters("@Status", "Pending"));
            param.Add(new SQLParameters("@App1ID", req.App1ID ?? string.Empty));
            param.Add(new SQLParameters("@App1Name", req.App1Name ?? string.Empty));
            param.Add(new SQLParameters("@App1Designation", req.App1Designation ?? string.Empty));
            param.Add(new SQLParameters("@App1Status", "Pending"));
            param.Add(new SQLParameters("@App1DoneOn", null));
            param.Add(new SQLParameters("@App2ID", req.App2ID ?? string.Empty));
            param.Add(new SQLParameters("@App2Name", req.App2Name ?? string.Empty));
            param.Add(new SQLParameters("@App2Designation", req.App2Designation ?? string.Empty));
            param.Add(new SQLParameters("@App2Status", "Pending"));
            param.Add(new SQLParameters("@App2DoneOn", null));
            param.Add(new SQLParameters("@App3ID", req.App3ID ?? string.Empty));
            param.Add(new SQLParameters("@App3Name", req.App3Name ?? string.Empty));
            param.Add(new SQLParameters("@App3Designation", req.App3Designation ?? string.Empty));
            param.Add(new SQLParameters("@App3Status", "Pending"));
            param.Add(new SQLParameters("@App3DoneOn", null));
            param.Add(new SQLParameters("@App4ID", req.App4ID ?? string.Empty));
            param.Add(new SQLParameters("@App4Name", req.App4Name ?? string.Empty));
            param.Add(new SQLParameters("@App4Designation", req.App4Designation ?? string.Empty));
            param.Add(new SQLParameters("@App4Status", "Pending"));
            param.Add(new SQLParameters("@App4DoneOn", null));
            param.Add(new SQLParameters("@AppCat", req.Category ?? string.Empty));
            param.Add(new SQLParameters("@MessageSend", "Pending"));
            param.Add(new SQLParameters("@VendorID", req.VendorID ?? string.Empty));
            param.Add(new SQLParameters("@RelativePersonName", req.RelativePersonName ?? string.Empty));
            param.Add(new SQLParameters("@RelativePersonID", req.RelativePersonID ?? string.Empty));
            param.Add(new SQLParameters("@RelativeDesignation", req.RelativeDesignation ?? string.Empty));
            param.Add(new SQLParameters("@RelativeDepartment", req.RelativeDepartment ?? string.Empty));
            param.Add(new SQLParameters("@AppDate", req.AppDate ?? string.Empty));
            param.Add(new SQLParameters("@ReferenceBillStatus", "Open"));
            param.Add(new SQLParameters("@BillTill", req.BillTill ?? string.Empty));
            param.Add(new SQLParameters("@ExtendedBillDate", req.ExtendedBillDate ?? string.Empty));
            param.Add(new SQLParameters("@R_Pending", req.Amount ?? string.Empty));
            param.Add(new SQLParameters("@R_Status", "Pending"));
            param.Add(new SQLParameters("@PExtra3", req.Office ?? string.Empty));
            param.Add(new SQLParameters("@PExtra2", BackValue.Split("#")[0]));
            param.Add(new SQLParameters("@PExtra4", PurchaseApprovalDetails));
            param.Add(new SQLParameters("@BudgetRequired", req.BudgetRequired ?? string.Empty));
            param.Add(new SQLParameters("@BudgetAmount", req.BudgetAmount ?? string.Empty));
            param.Add(new SQLParameters("@PreviousTaken", req.PreviousTaken ?? string.Empty));
            param.Add(new SQLParameters("@CurStatus", req.CurStatus ?? string.Empty));
            param.Add(new SQLParameters("@BudgetStatus", req.BudgetStatus ?? string.Empty));
            param.Add(new SQLParameters("@BudgetReferenceNo", req.BudgetReferenceNo ?? string.Empty));
            param.Add(new SQLParameters("@AdvFrom", req.DistFrom ?? string.Empty));
            param.Add(new SQLParameters("@AdvTo", req.DistTo ?? string.Empty));
            param.Add(new SQLParameters("@CampusCode", req.CampusCode ?? string.Empty));
            param.Add(new SQLParameters("@CampusName", _general.CampusNameByCode(req.CampusCode!)));
            try
            {
                int ins = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.INSERT_ADVANCE_DETAILS, param, DBConnections.Advance);

                if (req.ApprovalType.Contains("Mission Admission"))
                {
                    //DataTable dtFlat = new DataTable();
                    //dtFlat.Columns.AddRange(new DataColumn[6] { new DataColumn("Head"), new DataColumn("Unit"), new DataColumn("Price"), new DataColumn("Amount"), new DataColumn("Month"), new DataColumn("Remark") });

                }

                if (req.ApprovalType.Contains("Corporate Visit"))
                {
                    param.Clear();
                    param.Add(new SQLParameters("@AdvRefNo", RefNo));
                    param.Add(new SQLParameters("@RefNo", PrevRefNo.Replace("PLC", "")));
                    int inss = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_PLACEMEMT_SUMMARY, param, DBConnections.Advance);
                }
                if (req.ApprovalType.Contains("Partner Visit"))
                {
                    param.Clear();
                    param.Add(new SQLParameters("@AdvRefNo", RefNo));
                    param.Add(new SQLParameters("@RefNo", PrevRefNo.Replace("PLC", "")));
                    int inss = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_VISIT_SUMMARY, param, DBConnections.Advance);
                }
                if (req.ApprovalType.Contains("School Visit"))
                {
                    param.Clear();
                    param.Add(new SQLParameters("@AdvRefNo", RefNo));
                    param.Add(new SQLParameters("@RefNo", PrevRefNo.Replace("PLC", "")));
                    int inss = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_SCHOOL_SUMMARY, param, DBConnections.Advance);
                }


                //GET_UPDATE_ADVANCETOKEN
                param.Clear();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@VendorId", req.VendorID ?? string.Empty));
                param.Add(new SQLParameters("@IniBy", req.App1ID ?? string.Empty));
                param.Add(new SQLParameters("@VendorIdOffice", req.VendorID + "#" + req.Office ?? string.Empty));
                int iin = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.GET_UPDATE_ADVANCETOKEN, param, DBConnections.Advance);
                return ins;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During SaveAdvance...");
                throw;
            }
        }





        public async Task<DataTable> FinalAuth(string CampusCode)
        {
            if (CampusCode == "101")
            {
                string Query = AdvanceSql.GET_FINAL_APPROVAL_AUTH + " and CampusCode=@CampusCode ORDER BY SubType,MyOrder";
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@CampusCode", CampusCode));
                return await _dbOperations.SelectAsync(Query, param, DBConnections.Advance);
            }
            else
            {
                string Query = AdvanceSql.GET_FINAL_APPROVAL_AUTH + " AND Employee_Code=(SELECT `Value` FROM `othervalues` WHERE Type='Online And Noida Campus Member 4 Authority') GROUP BY Employee_Code  ORDER BY SubType,MyOrder";
                return await _dbOperations.SelectAsync(Query, DBConnections.Advance);
            }
        }

        public async Task<DataTable> GetMyAdvanceDetails(string Status, string Session, string CampusCode, string RefNo, string Type, string EmpCode, string Department, string itemsFrom, string noOfItems)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();



                string Con = "";
                if (!string.IsNullOrEmpty(RefNo))
                {
                    Con += " And ReferenceNo=@ReferenceNo";
                    parameters.Add(new SQLParameters("@ReferenceNo", RefNo));
                }
                else
                {
                    if (!string.IsNullOrEmpty(Session))
                    {
                        Con += " And `Session`=@Session";
                        parameters.Add(new SQLParameters("@Session", Session));
                    }
                    if (!string.IsNullOrEmpty(Status))
                    {
                        Con += " And `Status`=@Status";
                        parameters.Add(new SQLParameters("@Status", Status));
                    }
                    if (!string.IsNullOrEmpty(CampusCode))
                    {
                        Con += " And `CampusCode`=@CampusCode";
                        parameters.Add(new SQLParameters("@CampusCode", CampusCode));
                    }
                    if (!string.IsNullOrEmpty(Department))
                    {
                        Con += " And ForDepartment=@Department";
                        parameters.Add(new SQLParameters("@Department", Department));

                    }
                }

                if (Type.ToUpper() == "STORE")
                {
                    Con += "  And (IniId = '" + EmpCode + "' or RelativePersonID='" + EmpCode + "')";
                }

                return await _dbOperations.SelectAsync(AdvanceSql.GET_MY_ADVANCE_DETAILS.Replace("@Condition", Con).Replace("@limit", noOfItems).Replace("@offset", itemsFrom), parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetMyAdvanceDetails...");
                throw;
            }
            // return new DataTable();
        }

        public async Task<int> DeleteAdvance(string RefNo, string EmpCode)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@RefNo", RefNo));
                parameters.Add(new SQLParameters("@EmpCode", EmpCode));
                parameters.Add(new SQLParameters("@Ip", _general.GetIpAddress()));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.DELETE_ADVANCE, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetMyAdvanceDetails...");
                throw;
            }
        }

        public async Task<DataTable> GetPassApprovalDetails(string EmpCode, string EmpCodeAdd, string Type, GetMyAdvanceRequest req)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                string Cond = "  And `Session`=@Session And ( App1ID=@EmpCode || App2ID=@EmpCode  || App4ID=@EmpCode  || App1ID=@EmpCodeAdd || App2ID=@EmpCodeAdd  || App4ID=@EmpCodeAdd   || (App3ID is not NULL && (App3ID=@EmpCodeAdd  || App3ID=@EmpCodeAdd)) )";
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpCodeAdd", EmpCodeAdd));
                param.Add(new SQLParameters("@Session", req.Session!));

                if (!string.IsNullOrEmpty(req.ReferenceNo))
                {
                    Cond += "  And ReferenceNo=@RefNo";
                    param.Add(new SQLParameters("@RefNo", req.ReferenceNo));
                }
                else
                {
                    //if(!string.IsNullOrEmpty(req.Session))
                    //{
                    //    Cond += " And `Session`=@Session";
                    //    param.Add(new SQLParameters("@Session", req.Session));
                    //}
                    if (!string.IsNullOrEmpty(req.CampusCode))
                    {
                        Cond += " AND CampusCode=@CampusCode";
                        param.Add(new SQLParameters("@CampusCode", req.CampusCode));
                    }
                    if (!string.IsNullOrEmpty(req.Department))
                    {
                        Cond += " And ForDepartment=@Dept";
                        param.Add(new SQLParameters("@Dept", req.Department));
                    }
                    switch (req.Status)
                    {
                        case "Pending My":
                            Cond += "  And `Status`='Pending' And ( (App1ID=@EmpCode And App1Status='Pending') || (App2ID=@EmpCode And App2Status='Pending')  || (App3ID=@EmpCode And App3Status='Pending')  || (App4ID=@EmpCode And App4Status='Pending') || (App1ID=@EmpCodeAdd And App1Status='Pending') || (App2ID=@EmpCodeAdd And App2Status='Pending')  || (App3ID=@EmpCodeAdd And App3Status='Pending')  || (App4ID=@EmpCodeAdd And App4Status='Pending') )";

                            break;
                        case "Pending All":
                            Cond += "  And `Status`='Pending'";

                            break;
                        default:
                            Cond += " And `Status`=@Status";
                            param.Add(new SQLParameters("@Status", req.Status ?? string.Empty));
                            break;
                    }
                }

                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_APPROVAL_DETAILS.Replace("@Condition", Cond).Replace("@limit", req.NoOfItems.ToString()).Replace("@offset", req.ItemsFrom.ToString()), param, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetPassApprovalDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetApprovalType()
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_APPROVAL_LIST, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetApprovalType...");
                throw;
            }
        }
        public async Task<int> AdvanceApproveDetails(string EmpCode, string EmpName, PassApprovalRequest req)
        {
            try
            {
                string UpdateRecord = "";
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpName", EmpName));
                param.Add(new SQLParameters("@Designation", req.Designation!));
                param.Add(new SQLParameters("@RefNo", req.RefNo!));
                if (req.SeqNo == 1)
                {
                    UpdateRecord = " App1Status='Approved',App1DoneOn=now(),App1ID=@EmpCode,App1Name=@EmpName,App1Designation=@Designation";

                }
                if (req.SeqNo == 2)
                {
                    UpdateRecord = "  App2Status='Approved',App2DoneOn=now(),App2ID=@EmpCode,App2Name=@EmpName,App2Designation=@Designation";
                }
                if (req.SeqNo == 3)
                {
                    UpdateRecord = "  App3Status='Approved',App3DoneOn=now(),App3ID=@EmpCode,App3Name=@EmpName,App3Designation=@Designation";
                }
                if (req.SeqNo == 4)
                {
                    UpdateRecord = "PExtra1=@Remark,App4Status='Approved',App4DoneOn=now(),App4ID=@EmpCode,App4Name=@EmpName,App4Designation=@Designation";
                    param.Add(new SQLParameters("@Remark", req.Remark!.Replace("'", "")));
                }
                int ins = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.PASS_ADVANCE_APPROVAL.Replace("@Condition", UpdateRecord), param, DBConnections.Advance);

                return ins;

                //DataTable Status = await _dbOperations.SelectAsync(AdvanceSql.GET_APPROVE_QUERY, param, DBConnections.Advance);
                //if (Status.Rows.Count > 0 && Status.Rows[0][0].ToString() == "Approved")
                //{
                //    int UpdateStatus = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_ADVANCE_APPROVAL, param, DBConnections.Advance);

                //}


            }
            catch (Exception ex)
            {
                _logger.LogError("Error During AdvanceApproveDetails...");
                throw;
            }
        }

        public async Task<DataTable> IsApproveDetails(string EmpCode, string EmpName, PassApprovalRequest req)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpName", EmpName));
                param.Add(new SQLParameters("@Designation", req.Designation!));
                param.Add(new SQLParameters("@RefNo", req.RefNo!));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_APPROVE_QUERY, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During IsApproveDetails...");
                throw;
            }
        }

        public async Task<int> ApproveFinalStatus(string EmpCode, string EmpName, PassApprovalRequest req)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpName", EmpName));
                param.Add(new SQLParameters("@Designation", req.Designation!));
                param.Add(new SQLParameters("@RefNo", req.RefNo!));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_ADVANCE_APPROVAL, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During ApproveFinalStatus...");
                throw;
            }
        }
        public async Task<int> InsertBillBase(string EmpCode, string EmpName, PassApprovalRequest req)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpName", EmpName));
                param.Add(new SQLParameters("@Designation", req.Designation!));
                param.Add(new SQLParameters("@RefNo", req.RefNo!));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.INSERT_IN_BILL_BASE, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During ApproveFinalStatus...");
                throw;
            }
        }
        public async Task<DataTable> BillBaseDetails(string EmpCode, string EmpName, PassApprovalRequest req)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpName", EmpName));
                param.Add(new SQLParameters("@Designation", req.Designation!));
                param.Add(new SQLParameters("@RefNo", req.RefNo!));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_DETAILS_FROM_BILL_BASE, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During ApproveFinalStatus...");
                throw;
            }
        }
        public async Task<int> InsertBillAuthority(string BillTransId, string RefNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@BillTransectionNo", BillTransId));
                parameters.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.INSERT_AUTHORITY, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During InsertBillAuthority...");
                throw;
            }
        }
        public async Task<int> UpdateBillStatus(string BillTransId, string RefNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@BillTransId", BillTransId));
                parameters.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.CLOSE_BILL_OTHER_APPROVAL, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During UpdateBillStatus...");
                throw;
            }
        }

        public async Task<int> RejectAdvanceApproval(string EmpCode, string EmpName, PassApprovalRequest req)
        {
            try
            {
                string uptype = "";
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpName", EmpName));
                param.Add(new SQLParameters("@Designation", req.Designation!));
                param.Add(new SQLParameters("@Remark", req.Remark!));
                param.Add(new SQLParameters("@RefNo", req.RefNo!));
                if (req.SeqNo == 1)
                {
                    uptype = "App1Status='Rejected',App1DoneOn=now(),App1ID=@EmpCode,App1Name=@EmpName,App1Designation=@Designation";
                }
                if (req.SeqNo == 2)
                {
                    uptype = "App2Status='Rejected',App2DoneOn=now(),App2ID=@EmpCode,App2Name=@EmpName,App2Designation=@Designation";
                }
                if (req.SeqNo == 3)
                {
                    uptype = "App3Status='Rejected',App3DoneOn=now(),App3ID=@EmpCode,App3Name=@EmpName,App3Designation=@Designation";
                }
                if (req.SeqNo == 4)
                {
                    uptype = "App4Status='Rejected',App4DoneOn=now(),App4ID=@EmpCode,App4Name=@EmpName,App4Designation=@Designation";
                }
                int ins = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.REJECT_QUERY.Replace("@Condition", uptype), param, DBConnections.Advance);
                return ins;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During RejectAdvanceApproval...");
                throw;
            }
        }
        public async Task<DataTable> GetPurchaseBasicForBill(string RefNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PURCHASE_DETAILS_FOR_GENERATE_BILL, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetPurchaseBasicForBill...");
                throw;
            }
        }
        public async Task<DataTable> GetOffices(string VendorId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@VendorId", VendorId));
                return await _dbOperations.SelectAsync(AdvanceSql.VENDOR_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetOffices...");
                throw;
            }
        }
        public async Task<string> CheckWarrentyUploadeOrNot(string RefNo)
        {
            List<SQLParameters> param = new List<SQLParameters>();
            param.Add(new SQLParameters("@RefNo", RefNo));
            DataTable dtn = await _dbOperations.SelectAsync(AdvanceSql.CHECK_WAR_IN_QUERY, param, DBConnections.Advance);
            if (dtn.Rows.Count <= 0)
            {
                return "";
            }
            //else
            {
                dtn = await _dbOperations.SelectAsync(AdvanceSql.CHECK_WAR_STATUS, param, DBConnections.Advance);
                if (dtn.Rows.Count <= 0)
                {
                    return "";
                }
                else
                {
                    if (dtn.Rows[0][0].ToString() == "0")
                    {
                        return "Please Update Warrenty In Stock For Purchased Item (With Warrenty) In This Approval Before Uploading Bills";
                    }
                    else
                    {
                        return "";
                    }
                }
            }



        }
        public async Task<DataTable> GetFinanceAuthority()
        {
            try
            {

                return await _dbOperations.SelectAsync(AdvanceSql.GET_FINANCE_AUTHORITY, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetFinanceAuthority...");
                throw;
            }
        }
        public async Task<DataTable> GetThirdAuth()
        {
            try
            {

                return await _dbOperations.SelectAsync(AdvanceSql.GET_THIRD_AUTH, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetThirdAuth...");
                throw;
            }
        }
        public async Task<DataTable> Get4Auth()
        {
            try
            {

                return await _dbOperations.SelectAsync(AdvanceSql.GET_FINANCE_AUTHORITY + "  AND employee_code=(SELECT `Value` FROM advances.`othervalues` WHERE Type='Online And Noida Campus Member 4 Authority') GROUP BY Employee_Code", DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During Get4Auth...");
                throw;
            }
        }

        public async Task<DataTable> CanGenerateBillAdvance(string Type, string EmpCode, string RefNo = "")
        {
            try
            {
                string Cond = "";
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                if (Type.ToUpper() == "STORE")
                {
                    Cond += " And (IniId = @EmpCode Or RelativePersonID=@EmpCode)";
                }


                DataTable LockMenuDetails = await LockMenu(EmpCode);
                string ids = "";
                if (RefNo != "")
                {
                    ids = ids == "" ? RefNo : (ids + "," + RefNo);
                }
                if (ids.Length > 0)
                {
                    Cond += " And ReferenceNo in (" + ids + ")";
                }

                return await _dbOperations.SelectAsync(AdvanceSql.GET_DETAILS_FOR_CAN_BILL_UPLOAD.Replace("@Condition", Cond), param, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error During CanGenerateBill...", ex);
                throw;
            }
        }
        public async Task<DataTable> CanGenerateBill(string Type, string EmpCode, bool IsThousand, string RefNo = "")
        {
            try
            {
                string Cond = "";
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                if (Type.ToUpper() == "STORE")
                {
                    Cond += " And (IniId = @EmpCode Or RelativePersonID=@EmpCode)";
                }
                if (IsThousand)
                {
                    Cond += "  And BillRequired='No' And ReferenceBillStatus ='Closed' And BillId is null";
                }
                else
                {
                    Cond += " And ReferenceBillStatus ='Open'";
                }

                DataTable LockMenuDetails = await LockMenu(EmpCode);
                string ids = "";
                if (RefNo != "")
                {
                    ids = ids == "" ? RefNo : (ids + "," + RefNo);
                }
                if (ids.Length > 0)
                {
                    string[] spl = ids.Split(',');

                    // Wrap each element in single quotes
                    string inClause = string.Join(",", spl.Select(x => $"'{x}'"));

                    Cond += " And ReferenceNo in (" + inClause + ")";

                }

                return await _dbOperations.SelectAsync(AdvanceSql.GET_DETAILS_FOR_CAN_BILL_UPLOAD.Replace("@Condition", Cond), param, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error During CanGenerateBill...", ex);
                throw;
            }
        }
        public async Task<DataTable> LockMenu(string EmpCode)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@EmpCode", EmpCode));
                return await _dbOperations.SelectAsync(AdvanceSql.GETLOCKMENU, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During LockMenu...", ex);
                throw;
            }

        }

        public async Task<DataTable> LoadApprovalDetails(string Type, string EmpCode)
        {
            try
            {
                string Cond = "";
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                if (Type.ToUpper() == "STORE")
                {
                    Cond += "  And (IniId = @EmpCode Or RelativePersonID=@EmpCode)";
                }
                return await _dbOperations.SelectAsync(AdvanceSql.GET_APPROVAL_REFNO.Replace("@Condition", Cond), param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During LoadApprovalDetails...", ex);
                throw;
            }
        }
        public async Task<DataTable> GetbasicPrintDetails(string Id)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", Id));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BASIC_DETAILS_ADVANCE, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During LoadApprovalDetails...", ex);
                throw;
            }
        }
        public async Task<DataTable> GetContactNo(string EmpCode)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_CONTACT_NO, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetContactNo...", ex);
                throw;
            }
        }

        public async Task<DataTable> GetVisitBasicDetails(string Type, string EmpCode, string Prefix)
        {
            try
            {
                string str = "";
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                if (Type.ToUpper() == "STORE")
                {
                    str += "  And (IniId = @EmpCode Or RelativePersonID=@EmpCode)";
                }
                return await _dbOperations.SelectAsync(AdvanceSql.LOAD_VISIT.Replace("@Condition", str).Replace("@Prefix", "'" + Prefix + "'"), param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetVisitBasicDetails...", ex);
                throw;
            }
        }
        public async Task<DataTable> GetPartnerVisit(string Type, string EmpCode, string Prefix)
        {
            try
            {
                string str = "";
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                if (Type.ToUpper() == "STORE")
                {
                    str += "  And (IniId = @EmpCode Or RelativePersonID=@EmpCode)";
                }
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PARTNER_VISIT.Replace("@Condition", str).Replace("@Prefix", "'" + Prefix + "'"), param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetVisitBasicDetails...", ex);
                throw;
            }
        }
        public async Task<DataTable> GetSchoolVisit(string Type, string EmpCode, string Prefix)
        {
            try
            {
                string str = "";
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                if (Type.ToUpper() == "STORE")
                {
                    str += "  And (IniId = @EmpCode Or RelativePersonID=@EmpCode)";
                }
                return await _dbOperations.SelectAsync(AdvanceSql.GET_SCHOOL_VISIT.Replace("@Condition", str).Replace("@Prefix", "'" + Prefix + "'"), param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetVisitBasicDetails...", ex);
                throw;
            }
        }
        public async Task<DataTable> LoadSubFirm(int VendorId)
        {
            try
            {
                string str = "";
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@VendorId", VendorId));

                return await _dbOperations.SelectAsync(AdvanceSql.GET_FIRM_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetVisitBasicDetails...", ex);
                throw;
            }
        }
        public async Task<DataTable> LoadOffices(string RefNo, string VendorId)
        {
            try
            {

                string Cond = "";
                List<SQLParameters> param = new List<SQLParameters>();

                param.Add(new SQLParameters("@VendorId", VendorId));
                if (RefNo.Contains("#"))
                {
                    string[] splt = RefNo.Split('#');
                    param.Add(new SQLParameters("@RefNo", splt[0]));
                    DataTable dt = await _dbOperations.SelectAsync(AdvanceSql.GET_PURCHASE_OFFICE, param, DBConnections.Advance);
                    if (dt.Rows.Count > 0)
                    {
                        Cond = " And Offices='" + dt.Rows[0]["PExtra3"].ToString() + "'";
                    }
                }

                return await _dbOperations.SelectAsync(AdvanceSql.LOAD_VENDOR_OFFICE.Replace("@Condition", Cond), param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetVisitBasicDetails...", ex);
                throw;
            }
        }

        public async Task<DataTable> GetDepartDetails(string Values, string Type)
        {
            try
            {
                string[] splt = Values.Split("#");
                if (Type.Equals("Against Purchase Approval"))
                {
                    List<SQLParameters> param = new List<SQLParameters>();
                    param.Add(new SQLParameters("@RefNo", splt[0]));
                    return await _dbOperations.SelectAsync(AdvanceSql.AGAINST_PURCHASE_APPROVAL, param, DBConnections.Advance);
                }
                if (Type.Equals("Corporate Visit"))
                {
                    List<SQLParameters> param = new List<SQLParameters>();
                    param.Add(new SQLParameters("@RefNo", splt[0].Replace("PLC", "")));
                    return await _dbOperations.SelectAsync(AdvanceSql.AGAINST_PURCHASE_APPROVAL, param, DBConnections.Advance);

                }
                if (Type.Equals("School Visit"))
                {
                    List<SQLParameters> param = new List<SQLParameters>();
                    param.Add(new SQLParameters("@RefNo", splt[0].Replace("ADM", "")));
                    return await _dbOperations.SelectAsync(AdvanceSql.AGAINST_PURCHASE_APPROVAL, param, DBConnections.Advance);
                }
                else
                {
                    return new DataTable();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetDepartDetails...", ex);
                throw;
            }
        }

        public async Task<DataTable> GetBillBaseRefNo()
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_BASE_REF_NO, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetBillBaseRefNo...", ex);
                throw;
            }
        }

        public async Task<DataTable> getPurchaseApprovalDetails(string RefNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PURCHASE_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During getPurchaseApprovalDetails...", ex);
                throw;
            }
        }

        public async Task<int> SaveBillSave(string EmpCode, string EmpName, AddBillGenerateRequest req, string BillBaseREfNo)
        {
            // "insert into bill_base (TransactionID,Session,ForType,RelativePersonID,RelativePersonName,RelativeDesignation,RelativeDepartment,FirmName,FirmPerson,FirmEmail,FirmPanNo,FirmAddress,FirmContactNo,\r\nFirmAlternateContactNo,AmountRequired,AmountPaid,AmountRemaining,IssuedBy,IssuedName,IssuedOn,IssuedFrom,LastUpdatedOn,LastUpdatedBy,Remark,GateEntryOn,DepartmentApprovalDate,StoreDate,BillDate,\r\nBillReceivedOnGate,BillNo,Status,VendorID,Col1,Col2,Col3,Col4,Col5,CashDiscount,BillExtra1,BillExtra3,BillExtra4,BillExtra5,CampusCode,CampusName) Values\r\n(@TransactionID,@Session,@ForType,@RelativePersonID,@RelativePersonName,@RelativeDesignation,@RelativeDepartment,@FirmName,@FirmPerson,@FirmEmail,@FirmPanNo,@FirmAddress,@FirmContactNo,\r\n@FirmAlternateContactNo,@AmountRequired,@AmountPaid,@AmountRemaining,@IssuedBy,@IssuedName,NOW(),@Ip,NOW(),@LastUpdatedBy,@Purpose,@GateEntryOn,@DepartmentApprovalDate,@StoreDate,@BillDate,@BillReceivedOnGate,@BillNo,'Waiting For Bills Approval',@VendorID,@Department,@MeInitiate,@SelectedOffice,@Col4,@Col5,\r\n@CashDiscount,@Initiate,@RefNo,@Initiate,@nextbill,@CampusCode,@CampusName);";
            try
            {


                DataTable Details = new DataTable();
                if (!string.IsNullOrEmpty(req.RefNo))
                {
                    if (req.ForTypeOf == "Advance Bill")
                    {
                        Details = await getAdvanceApprovalDetails(req.RefNo);
                    }
                    else
                    {
                        Details = await getPurchaseApprovalDetails(req.RefNo);
                    }
                }
                //  DataTable VenderDetails = await GetVendorDetails(Details.Rows[0]["VendorId"].ToString() ?? "");
                if (string.IsNullOrEmpty(req.RefNo))
                {
                    req.RefNo = "";
                }
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransactionID", BillBaseREfNo));
                param.Add(new SQLParameters("@Session", _general.GetFinancialSession(DateTime.Now)));
                param.Add(new SQLParameters("@ForType", req.ForTypeOf));
                param.Add(new SQLParameters("@RelativePersonID", req.RefNo == "" ? req.RelativePersonId ?? "" : Details.Rows[0]["RelativePersonID"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@RelativePersonName", req.RefNo == "" ? req.RelativePersonName ?? "" : Details.Rows[0]["RelativePersonName"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@RelativeDesignation", req.RefNo == "" ? req.RelativeDesignation ?? "" : Details.Rows[0]["RelativeDesignation"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@RelativeDepartment", req.RefNo == "" ? req.RelativeDepartment ?? "" : Details.Rows[0]["RelativeDepartment"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@FirmName", req.RefNo == "" ? req.FirmName ?? "" : Details.Rows[0]["FirmName"].ToString() ?? ""));
                param.Add(new SQLParameters("@FirmPerson", req.RefNo == "" ? req.FirmPerson ?? "" : Details.Rows[0]["FirmPerson"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@FirmEmail", req.RefNo == "" ? req.FirmEmail ?? "" : Details.Rows[0]["FirmEmail"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@FirmPanNo", req.RefNo == "" ? req.FirmPanNo ?? "" : Details.Rows[0]["FirmPanNo"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@FirmAddress", req.RefNo == "" ? req.FirmAddress ?? "" : Details.Rows[0]["FirmAddress"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@FirmContactNo", req.RefNo == "" ? req.FirmContactNo ?? "" : Details.Rows[0]["FirmContactNo"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@FirmAlternateContactNo", req.RefNo == "" ? req.FirmAlternateContactNo ?? "" : Details.Rows[0]["FirmAlternateContactNo"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@AmountRequired", req.Amount ?? string.Empty));
                param.Add(new SQLParameters("@AmountPaid", 0));
                param.Add(new SQLParameters("@AmountRemaining", req.Amount ?? string.Empty));
                param.Add(new SQLParameters("@IssuedBy", EmpCode));
                param.Add(new SQLParameters("@IssuedName", EmpName));
                param.Add(new SQLParameters("@Ip", _general.GetIpAddress()));
                param.Add(new SQLParameters("@LastUpdatedBy", EmpName));
                param.Add(new SQLParameters("@Purpose", req.Purpose));
                param.Add(new SQLParameters("@BillNo", req.BillNo));
                param.Add(new SQLParameters("@VendorID", req.RefNo == "" ? req.VendorID ?? "" : Details.Rows[0]["VendorID"].ToString() ?? ""));
                param.Add(new SQLParameters("@Department", req.Department));
                param.Add(new SQLParameters("@MeInitiate", req.InitiateOn ?? string.Empty));
                param.Add(new SQLParameters("@SelectedOffice", req.Office ?? string.Empty));
                param.Add(new SQLParameters("@Col5", req.AdditionalName ?? string.Empty));
                param.Add(new SQLParameters("@Col4", "---"));
                param.Add(new SQLParameters("@CashDiscount", req.Discount!));
                param.Add(new SQLParameters("@Initiate", req.ExpBillDate!));
                param.Add(new SQLParameters("@RefNo", req.RefNo!));
                param.Add(new SQLParameters("@nextbill", req.NextBillTill!));
                param.Add(new SQLParameters("@CampusCode", req.CampusCode!));
                param.Add(new SQLParameters("@CampusName", req.CampusName!));



                param.Add(new SQLParameters("@TestUpto", req.TestingUpto!));
                if (req.ForTypeOf == "Advance Bill")
                {
                    param.Add(new SQLParameters("@BillReceivedOnGate", null));
                    param.Add(new SQLParameters("@StoreDate", null));
                    param.Add(new SQLParameters("@BillDate", null));
                    param.Add(new SQLParameters("@GateEntryOn", null));
                    param.Add(new SQLParameters("@DepartmentApprovalDate", null));
                }
                else
                {
                    param.Add(new SQLParameters("@BillReceivedOnGate", req.BillOnGate!));
                    param.Add(new SQLParameters("@StoreDate", req.StoreDate!));
                    param.Add(new SQLParameters("@BillDate", req.BillDate!));
                    param.Add(new SQLParameters("@GateEntryOn", req.GateDate!));
                    param.Add(new SQLParameters("@DepartmentApprovalDate", req.DepartmentDate!));
                }

                int ins = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.INSERRT_INTO_BILL_BASE, param, DBConnections.Advance);

                string newnext = "";
                if (!string.IsNullOrEmpty(req.NextBillTill))
                {
                    newnext = ",ExtendedBillDate='" + req.NextBillTill + "'";
                }
                if (req.RefNo != "")
                {
                    if (req.ForTypeOf == "Advance Bill")
                    {
                        string UpdateQ = "update otherapprovalsummary set BillId=if(BillId is NULL,'," + BillBaseREfNo + ",',CONCAT(BillId,'" + BillBaseREfNo + ",')),BillVariationRemark='" + req.VariationReason?.Trim().ToUpper() + "',ReferenceBillStatus='" + req.BillStatus + "'" + newnext + " where ReferenceNo='" + req.RefNo + "'";

                        int upDate = await _dbOperations.DeleteInsertUpdateAsync(UpdateQ, new List<SQLParameters>(), DBConnections.Advance);

                        param.Clear();
                        param.Add(new SQLParameters("@EmpCode", EmpCode));
                        param.Add(new SQLParameters("@VendorId", Details.Rows[0]["VendorID"].ToString() ?? string.Empty));
                        param.Add(new SQLParameters("@IniBy", EmpCode ?? string.Empty));
                        param.Add(new SQLParameters("@VendorIdOffice", Details.Rows[0]["VendorID"].ToString() + "#" + req.Office ?? string.Empty));
                        int iin = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.GET_UPDATE_ADVANCETOKEN, param, DBConnections.Advance);
                    }
                    else
                    {
                        string UpdateQ = "update purchaseapprovalsummary set BillId=if(BillId is NULL,'," + BillBaseREfNo + ",',CONCAT(BillId,'" + BillBaseREfNo + ",')),BillVariationRemark='" + req.VariationReason?.Trim().ToUpper() + "',ReferenceBillStatus='" + req.BillStatus + "' " + newnext + " where ReferenceNo=" + req.RefNo + "";
                        int upDate = await _dbOperations.DeleteInsertUpdateAsync(UpdateQ, new List<SQLParameters>(), DBConnections.Advance);
                    }
                }

                return ins;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During SaveBillSave...", ex);
                throw;
            }
        }

        public async Task<int> SaveBillAuth(string EmpCode, string EmpName, AddBillGenerateRequest req, string BillBaseREfNo)
        {
            List<SQLParameters> param = new List<SQLParameters>();
            int cnt = 1;
            if (req.Auth1Id!.Length > 0)
            {
                param.Clear();
                param.Add(new SQLParameters("@AuthName", req.Auth1Name ?? string.Empty));
                param.Add(new SQLParameters("@AuthId", req.Auth1Id));
                param.Add(new SQLParameters("@Count", cnt));
                param.Add(new SQLParameters("@TransId", BillBaseREfNo));
                cnt++;
                int ins = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.SaveAuth, param, DBConnections.Advance);
            }
            if (req.Auth2Id!.Length > 0)
            {
                param.Clear();
                param.Add(new SQLParameters("@AuthName", req.Auth2Name ?? string.Empty));
                param.Add(new SQLParameters("@AuthId", req.Auth2Id));
                param.Add(new SQLParameters("@Count", cnt));
                param.Add(new SQLParameters("@TransId", BillBaseREfNo));
                cnt++;
                int ins = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.SaveAuth, param, DBConnections.Advance);
            }
            if (req.Auth3Id != null && req.Auth3Id!.Length > 0)
            {
                param.Clear();
                param.Add(new SQLParameters("@AuthName", req.Auth3Name ?? string.Empty));
                param.Add(new SQLParameters("@AuthId", req.Auth3Id));
                param.Add(new SQLParameters("@Count", cnt));
                param.Add(new SQLParameters("@TransId", BillBaseREfNo));
                cnt++;
                int ins = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.SaveAuth, param, DBConnections.Advance);
            }
            if (req.Auth4Id != null && req.Auth4Id!.Length > 0)
            {
                param.Clear();
                param.Add(new SQLParameters("@AuthName", req.Auth4Name ?? string.Empty));
                param.Add(new SQLParameters("@AuthId", req.Auth4Id));
                param.Add(new SQLParameters("@Count", cnt));
                param.Add(new SQLParameters("@TransId", BillBaseREfNo));
                cnt++;
                int ins = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.SaveAuth, param, DBConnections.Advance);
            }
            return cnt;
        }
        public async Task<int> UpdatePdf(string RefNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_PDF_UPLOAD, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During UpdatePdf...", ex);
                throw;
            }
        }
        public async Task<int> UpdateExcel(string RefNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_EXCEL_UPLOAD, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During UpdatePdf...", ex);
                throw;
            }
        }
        public async Task<DataTable> GetFirm(string cond)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();

                return await _dbOperations.SelectAsync(AdvanceSql.GETFIRMDETAILS.Replace("@VendorId", cond), param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetFirm...", ex);
                throw;
            }
        }

        public async Task<DataTable> GetDateLock(string Type)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@Type", Type));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_DATE_ADVANCE_LOCK, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetDateLock...", ex);
                throw;
            }
        }

        public async Task<DataTable> getAdvanceApprovalDetails(string RefNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During getAdvanceApprovalDetails...", ex);
                throw;
            }
        }


        public async Task<DataTable> GetMissionAdmissionReturnTypes()
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_MISSION_ADMISSION_RETURN_TYPES, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetMissionAdmissionReturnTypes...");
                throw;
            }
        }
        public async Task<DataTable> GetAdvancePrintApprovalDetails(string ReferenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@ReferenceNo", ReferenceNo));

                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_APPROVAL_PRINT_DETAILS, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvancePrintApprovalDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetAdvanceOfficeMappingEmployeeCodes(string OfficeName, string Session)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@OfficeName", OfficeName));
                parameters.Add(new SQLParameters("@Session", Session));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_OFFICE_MAPPING_EMPLOYEE_DETAILS, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvanceOfficeMappingEmployeeCodes...");
                throw;
            }
        }
        public async Task<DataTable> GetAdvanceEmployeeLeaveDetails(string EmployeeIds, string AdvanceFrom, string AdvanceTo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@ForCodes", EmployeeIds));
                parameters.Add(new SQLParameters("@AdvanceFrom", AdvanceFrom));
                parameters.Add(new SQLParameters("@AdvanceTo", AdvanceTo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_EEMPLOYEE_LEAVE_DETAILS, parameters, DBConnections.Salary);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvanceEmployeeLeaveDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetAdvanceBillSummaryDetails(string ReferenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@ReferenceNo", ReferenceNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_BILL_SUMMARY, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvanceBillSummaryDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetAdvancePaymentDetailsByPaymentGroupId(string ReferenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@ReferenceNo", ReferenceNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_PAYMENT_DETAILS_WITH_PAYMENT_GROUP_REFERENCE_NO, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvancePaymentDetailsByPaymentGroupId...");
                throw;
            }
        }

        public async Task<DataTable> GetAdvancePaymentDetailsByReferenceNo(string ReferenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@ReferenceNo", ReferenceNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_PAYMENT_DETAILS_WITH_REFERENCE_NO, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvancePaymentDetailsByReferenceNo...");
                throw;
            }
        }
        public async Task<DataTable> GetAdvancePaymentTransactionIdsByReferenceNo(string ReferenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@ReferenceNo", ReferenceNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_PAYMENT_TRANSACTION_IDS, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvancePaymentTransactionIdsByReferenceNo...");
                throw;
            }
        }

        public async Task<DataTable> GetAdvanceBillDetailsByTransactionIds(string TransactionIds)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@TransactionIds", TransactionIds));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_PAYMENT_TRANSACTION_IDS_BILL_DETAILS, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvanceBillDetailsByTransactionIds...");
                throw;
            }
        }
        public async Task<DataTable> GetAdvanceBillBillDistributionDetailsById(string BillId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@BillId", BillId));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_PAYMENT_BILL_DISTRIBUTION_DETAILS, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvanceBillBillDistributionDetailsById...");
                throw;
            }
        }

        public async Task<DataTable> GetAdvanceBillAgainstBaseDistributionDetailsByTransactionIds(string TransactionIds)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@TransactionIds", TransactionIds));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_PAYMENT_BILL_AGINST_BASE_DISTRIBUTION_DETAILS, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvanceBillAgainstBaseDistributionDetailsByTransactionIds...");
                throw;
            }
        }
        public async Task<DataTable> GetAdvanceOtherSummaryDistributionDetailsByReferenceNo(string ReferenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@ReferenceNo", ReferenceNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_OTHER_SUMMARY_DISTRIBUTION_DETAILS, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvanceOtherSummaryDistributionDetailsByReferenceNo...");
                throw;
            }
        }

        public async Task<DataTable> GetAdvanceAmountIssuedAgainstBudgetByReferenceNo(string ReferenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@ReferenceNo", ReferenceNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_AMOUNT_ISSUED_AGINST_BUDGET, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvanceAmountIssuedAgainstBudgetByReferenceNo...");
                throw;
            }
        }
        public async Task<DataTable> GetAdvanceBudgetUsageSessionWiseByReferenceNo(string ReferenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@ReferenceNo", ReferenceNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_BUDGET_SESSION_WISE_USAGE, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvanceBudgetUsageSessionWiseByReferenceNo...");
                throw;
            }
        }
        public async Task<DataTable> GetAdvanceBudgetUsageMonthWiseByReferenceNo(string ReferenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@ReferenceNo", ReferenceNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_BUDGET_MONTH_WISE_USAGE, parameters, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetAdvanceBudgetUsageMonthWiseByReferenceNo...");
                throw;
            }
        }
        public async Task<DataTable> GetPurchaseBasicForBillAgainstAdvance(string RefNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PURCHASE_DETAILS_FOR_GENERATE_BILL_AGAINST_ADVANCE, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetPurchaseBasicForBillAgainstAdvance...");
                throw;
            }
        }
        public async Task<DataTable> GetrefnoForGenerateBillAdvance(string Type, string EmpCode, string RefNo = "")
        {
            try
            {
                string Cond = "";
                List<SQLParameters> param = new List<SQLParameters>();
                if (Type.ToUpper() == "STORE")
                {
                    Cond += "  And (IniId = @EmpCode Or RelativePersonID=@EmpCode)";
                    param.Add(new SQLParameters("@EmpCode", EmpCode));
                }
                if (!string.IsNullOrEmpty(RefNo))
                {
                    Cond += " And ReferenceNo=" + RefNo + "";
                }
                return await _dbOperations.SelectAsync(AdvanceSql.GET_REF_NO_FOR_UPLOAD_BILL_IN_ADVANCE.Replace("@Condition", Cond), param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetrefnoForGenerateBillAdvance...");
                throw;
            }
        }
        public async Task<DataTable> Auth1AdvanceBill(string refNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", refNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GETADVANCEBILLAUTH1, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During Auth1AdvanceBill...");
                throw;
            }
        }
        public async Task<DataTable> Auth2AdvanceBill(string refNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", refNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GETADVANCEBILLAUTH2, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During Auth2AdvanceBill...");
                throw;
            }
        }
        public async Task<DataTable> Auth3AdvanceBill(string refNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", refNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GETADVANCEBILLAUTH3, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During Auth3AdvanceBill...");
                throw;
            }
        }
        public async Task<DataTable> Auth4AdvanceBill(string refNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", refNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GETADVANCEBILLAUTH4, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During Auth4AdvanceBill...");
                throw;
            }
        }

        public async Task<DataTable> GetAuthAdvanceBillAuth2(string Campus)
        {
            try
            {

                {
                    string query = AdvanceSql.gtAuth + "  AND employee_code=(SELECT `Value` FROM advances.`othervalues` WHERE Type='Online And Noida Campus Member 4 Authority') GROUP BY Employee_Code";
                    return await _dbOperations.SelectAsync(query, DBConnections.Advance);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetAuthAdvanceBill...");
                throw;
            }
        }
        public async Task<DataTable> GetAuthAdvanceBillAuth1(string Campus)
        {
            try
            {

                {
                    string query = AdvanceSql.GetAuthForOutMathura;
                    return await _dbOperations.SelectAsync(query, DBConnections.Advance);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetAuthAdvanceBill...");
                throw;
            }
        }
        public async Task<DataTable> GetAuthAdvanceBillAuth4(string Campus)
        {
            try
            {


                return await _dbOperations.SelectAsync(AdvanceSql.gtAuth + "  and FIND_IN_SET('" + Campus + "',B.CampusCodes)  ORDER BY MyOrder", DBConnections.Advance);



            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetAuthAdvanceBill...");
                throw;
            }
        }

        public async Task<DataTable> GetApprovalBill(string EmpCode, string Type, GetApprovalBillRequest req)
        {
            try
            {
                string Cond = "";
                List<SQLParameters> Parameters = new List<SQLParameters>();

                Cond += "  And (BillExtra3 is not NULL and BillExtra3 not like '%-%' and BillExtra3 not like '' and BillExtra3 not like 'S%' and BillExtra3 not like 'WRK%' and  BillExtra3 not like 'P%')  And Status=@Status";
                Parameters.Add(new SQLParameters("@Status", req.Category ?? ""));
                if (Type == "STORE")
                {
                    Cond += "  And IssuedBy = @EmpCode ";
                    Parameters.Add(new SQLParameters("@EmpCode", EmpCode));
                }
                if (!string.IsNullOrEmpty(req.ReferenceNo))
                {
                    Cond += " And M.TransactionID=@TransId";
                    Parameters.Add(new SQLParameters("@TransId", req.ReferenceNo));
                }
                else
                {
                    if (req.Category == "Ready To Issue Amount")
                    {
                        Cond += "  And M.AmountRemaining>0";
                    }
                    if (!string.IsNullOrEmpty(req.VendorId))
                    {
                        Cond += "  And M.VendorID=@VendorId";
                        Parameters.Add(new SQLParameters("@VendorId", req.VendorId));
                    }
                    if (!string.IsNullOrEmpty(req.Campus))
                    {
                        Cond += "  AND M.CampusCode=@CampusCode";
                        Parameters.Add(new SQLParameters("CampusCode", req.Campus));
                    }
                }
                Parameters.Add(new SQLParameters("@Session", req.Session ?? string.Empty));
                Parameters.Add(new SQLParameters("@Limit", req.NoOfItems));
                Parameters.Add(new SQLParameters("@OffSet", req.ItemsFrom));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_APPROVAL_BILL.Replace("@Condition", Cond).Replace("@ForType", req.Type), Parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetApprovalBill...");
                throw;
            }
        }
        public async Task<DataTable> GetIssuedAmount(string TransId)
        {
            try
            {
                List<SQLParameters> Parameters = new List<SQLParameters>();
                Parameters.Add(new SQLParameters("@TransactionId", TransId));

                return await _dbOperations.SelectAsync(AdvanceSql.GET_ISSUED_AMOUNT, Parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetIssuedAmount...");
                throw;
            }
        }
        public async Task<DataTable> GetIssuedAmounREadyApproval(string TransId)
        {
            try
            {
                List<SQLParameters> Parameters = new List<SQLParameters>();
                Parameters.Add(new SQLParameters("@TransId", TransId));

                return await _dbOperations.SelectAsync(AdvanceSql.GET_APPROVAL_AUTHORITY_READY_TO_ISSUE, Parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetIssuedAmounREadyApproval...");
                throw;
            }
        }
        public async Task<DataTable> GetApprovalAuthority(string Tid)
        {
            try
            {
                List<SQLParameters> Parameters = new List<SQLParameters>();
                Parameters.Add(new SQLParameters("@TransId", Tid));

                return await _dbOperations.SelectAsync(AdvanceSql.GET_AUTHORITY, Parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetIssuedAmount...");
                throw;
            }
        }
        public async Task<int> UpdateBillBase(string Cond, string TransId, string WarId, List<SQLParameters> lst)
        {
            try
            {
                lst.Add(new SQLParameters("@NewWarId", WarId));
                string Query = AdvanceSql.Update_BillBase.Replace("@Cond", Cond).Replace("@TransId", TransId);
                return await _dbOperations.DeleteInsertUpdateAsync(Query, lst, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During UpdateBillBase...");
                throw;
            }
        }
        public async Task<int> UpdateRemark(string Remark, string TransId)
        {
            try
            {
                List<SQLParameters> lst = new List<SQLParameters>();
                lst.Add(new SQLParameters("@Purpose", Remark));
                lst.Add(new SQLParameters("@TransId", TransId));


                string Query = AdvanceSql.UPDATE_REMARK;
                return await _dbOperations.DeleteInsertUpdateAsync(Query, lst, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During UpdateBillBase...");
                throw;
            }
        }
        public async Task<DataTable> GetEditBillDetails(string RefNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", RefNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_DETAILS_FOR_EDIT, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetEditBillDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetRefAuth(string refNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", refNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_APPROVAL_AUTHO, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetRefAuth...");
                throw;
            }
        }


        public async Task<int> UpdateDeleteStatus(string BillId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@BillId", BillId));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.DELETE_UPDATE_BILL_SUMMARY, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During UpdateDeleteStatus...");
                throw;
            }
        }
        public async Task<int> InsertDeleteLog(string BillId, string EmpCode)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@BillId", BillId));
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@reason", "Delete"));
                param.Add(new SQLParameters("@IpAddress", _general.GetIpAddress()));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.DELETE_UPDATE_BILL_SUMMARY, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During InsertDeleteLog...");
                throw;
            }
        }
        public async Task<int> DeleteBillBase(string BillId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", BillId));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.BILL_BASE, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During DeleteBillBase...");
                throw;
            }
        }
        public async Task<int> DeleteBillAuth(string BillId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", BillId));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.DELETE_AUTH, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During DeleteBillAuth...");
                throw;
            }
        }
        public async Task<int> UpdateBill(string EmpCode, string EmpName, AddBillGenerateRequest req, string BillBaseREfNo)
        {
            try
            {
                DataTable Details = new DataTable();
                if (req.ForTypeOf == "Advance Bill")
                {
                    Details = await getAdvanceApprovalDetails(req.RefNo);
                }
                else
                {
                    Details = await getPurchaseApprovalDetails(req.RefNo);
                }
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransactionID", BillBaseREfNo));
                param.Add(new SQLParameters("@Session", _general.GetFinancialSession(DateTime.Now)));
                param.Add(new SQLParameters("@ForType", req.ForTypeOf));
                param.Add(new SQLParameters("@RelativePersonID", Details.Rows[0]["RelativePersonID"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@RelativePersonName", Details.Rows[0]["RelativePersonName"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@RelativeDesignation", Details.Rows[0]["RelativeDesignation"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@RelativeDepartment", Details.Rows[0]["RelativeDepartment"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@FirmName", Details.Rows[0]["FirmName"].ToString() ?? ""));
                param.Add(new SQLParameters("@FirmPerson", Details.Rows[0]["FirmPerson"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@FirmEmail", Details.Rows[0]["FirmEmail"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@FirmPanNo", Details.Rows[0]["FirmPanNo"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@FirmAddress", Details.Rows[0]["FirmAddress"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@FirmContactNo", Details.Rows[0]["FirmContactNo"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@FirmAlternateContactNo", Details.Rows[0]["FirmAlternateContactNo"].ToString() ?? string.Empty));
                param.Add(new SQLParameters("@AmountRequired", req.Amount ?? string.Empty));
                param.Add(new SQLParameters("@AmountPaid", 0));
                param.Add(new SQLParameters("@AmountRemaining", req.Amount ?? string.Empty));
                param.Add(new SQLParameters("@IssuedBy", EmpCode));
                param.Add(new SQLParameters("@IssuedName", EmpName));
                param.Add(new SQLParameters("@Ip", _general.GetIpAddress()));
                param.Add(new SQLParameters("@LastUpdatedBy", EmpName));
                param.Add(new SQLParameters("@Purpose", req.Purpose));
                param.Add(new SQLParameters("@BillNo", req.BillNo));
                param.Add(new SQLParameters("@VendorID", Details.Rows[0]["VendorID"].ToString() ?? ""));
                param.Add(new SQLParameters("@Department", req.Department));
                param.Add(new SQLParameters("@MeInitiate", req.InitiateOn ?? string.Empty));
                param.Add(new SQLParameters("@SelectedOffice", req.Office ?? string.Empty));
                param.Add(new SQLParameters("@Col5", req.AdditionalName ?? string.Empty));
                param.Add(new SQLParameters("@Col4", "---"));
                param.Add(new SQLParameters("@CashDiscount", req.Discount!));
                param.Add(new SQLParameters("@Initiate", req.ExpBillDate!));
                param.Add(new SQLParameters("@RefNo", req.RefNo!));
                param.Add(new SQLParameters("@nextbill", req.NextBillTill!));
                param.Add(new SQLParameters("@CampusCode", req.CampusCode!));
                param.Add(new SQLParameters("@CampusName", req.CampusName!));



                param.Add(new SQLParameters("@TestUpto", req.TestingUpto!));
                if (req.ForTypeOf == "Advance Bill")
                {
                    param.Add(new SQLParameters("@BillReceivedOnGate", null));
                    param.Add(new SQLParameters("@StoreDate", null));
                    param.Add(new SQLParameters("@BillDate", null));
                    param.Add(new SQLParameters("@GateEntryOn", null));
                    param.Add(new SQLParameters("@DepartmentApprovalDate", null));
                }
                else
                {
                    param.Add(new SQLParameters("@BillReceivedOnGate", req.BillOnGate!));
                    param.Add(new SQLParameters("@StoreDate", req.StoreDate!));
                    param.Add(new SQLParameters("@BillDate", req.BillDate!));
                    param.Add(new SQLParameters("@GateEntryOn", req.GateDate!));
                    param.Add(new SQLParameters("@DepartmentApprovalDate", req.DepartmentDate!));
                }

                int ins = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_BILL_BASE, param, DBConnections.Advance);

                string newnext = "";
                if (!string.IsNullOrEmpty(req.NextBillTill))
                {
                    newnext = ",ExtendedBillDate='" + req.NextBillTill + "'";
                }
                if (req.ForTypeOf == "Advance Bill")
                {
                    string UpdateQ = "update otherapprovalsummary set BillId=BillId,BillVariationRemark='" + req.VariationReason?.Trim().ToUpper() + "',ReferenceBillStatus='" + req.BillStatus + "'" + newnext + " where ReferenceNo='" + req.BillId + "'";

                    int upDate = await _dbOperations.DeleteInsertUpdateAsync(UpdateQ, new List<SQLParameters>(), DBConnections.Advance);

                    param.Clear();
                    param.Add(new SQLParameters("@EmpCode", EmpCode));
                    param.Add(new SQLParameters("@VendorId", Details.Rows[0]["VendorID"].ToString() ?? string.Empty));
                    param.Add(new SQLParameters("@IniBy", EmpCode ?? string.Empty));
                    param.Add(new SQLParameters("@VendorIdOffice", Details.Rows[0]["VendorID"].ToString() + "#" + req.Office ?? string.Empty));
                    int iin = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.GET_UPDATE_ADVANCETOKEN, param, DBConnections.Advance);
                }
                else
                {
                    string UpdateQ = "update purchaseapprovalsummary set BillId=BillId,BillVariationRemark='" + req.VariationReason + "',ReferenceBillStatus='" + req.BillStatus + "'" + newnext + " where ReferenceNo='" + req.BillId + "'";
                    int upDate = await _dbOperations.DeleteInsertUpdateAsync(UpdateQ, new List<SQLParameters>(), DBConnections.Advance);
                }

                return ins;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During UpdateBill...");
                throw;
            }
        }

        public async Task<DataTable> GetBillDetails(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_PRINT_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetBillDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetImprestByBillTransactionId(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_IMPREST_BILL_TRANS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetBillDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetWorkshopBillDetails(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.SelectAsync(AdvanceSql.WORK_SHOP_BILLL_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetBillDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetApprovalsAuthSummary(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_APPROVAL_AUTH_SUMMARY, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetBillDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetBillIssues(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_ISSUED_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetBillDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetRelativeContactNo(string EmpCode)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_EMPBASIC_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetRelativeContactNo...");
                throw;
            }
        }
        public async Task<DataTable> RowColor(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.SelectAsync(AdvanceSql.LimitColor, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During RowColor...");
                throw;
            }
        }
        public async Task<DataTable> RowColorReadyAmount(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.SelectAsync(AdvanceSql.READY_CHECK_LIMIT, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During RowColorReadyAmount...");
                throw;
            }
        }
        public async Task<DataTable> GetAuthStatusBill(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_AUTH_STATUS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetAuthStatusBill...");
                throw;
            }
        }
        public async Task<DataTable> GetVenderRegisterDetails(string VendorId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@VenderId", VendorId));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_VENDER_REGISTER, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetAuthStatusBill...");
                throw;
            }
        }
        public async Task<DataTable> GetTransDetails(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_TRANSACTION_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetTransDetails...");
                throw;
            }
        }
        public async Task<DataTable> BillBaseDetails(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_BASE_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During BillBaseDetails...");
                throw;
            }
        }
        public async Task<DataTable> CheucqAuth(string TransId, string SeqNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                param.Add(new SQLParameters("@SeqNo", SeqNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_CHEQUE_AUTH, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During BillBaseDetails...");
                throw;
            }
        }
        public async Task<DataTable> GEtSpecialVendor(string VendorId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@VendorId", VendorId));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_SPECIAL_VENDOR, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GEtSpecialVendor...");
                throw;

            }
        }
        public async Task<DataTable> GetPerson(string campusCode)
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PERSON_BILL_PERSON.Replace("@Condition", " and CampusCode in (" + (campusCode == "103" ? "101" : campusCode) + ") ORDER BY first_name,deisgnation desc"), DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetPerson...");
                throw;
            }
        }
        public async Task<DataTable> GetBillauth(string campusCode)
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_AUTHORITY_FOR_BILL.Replace("@Condition", " and FIND_IN_SET('" + campusCode + "',B.CampusCodes) ORDER BY MyOrder"), DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetBillauth...");
                throw;
            }
        }
        public async Task<DataTable> GetFirstSecond(string campusCode)
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_FIRST_SECOND_AUTH.Replace("@Condition", " and CampusCode in (" + (campusCode == "103" ? "101" : campusCode) + ") ORDER BY first_name"), DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetBillauth...");
                throw;
            }
        }

        public async Task<DataTable> GetBillApprovalFilterSessions()
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_FILTER_SESSION, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalFilterSessions...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalFilterInitiatedBy()
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_INITIATED_BY, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalFilterInitiatedBy...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalFilterChequeBy()
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_BILL_CHEQUE_BY, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalFilterChequeBy...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAgencyBillIds()
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_AGENCY_BILL_IDS, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAgencyBillIds...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalDetails(GetBillApprovalRequest? getBillApprovalRequest, string employeeId, string role)
        {

            try
            {
                string query = "";
                string mychqcond = "";
                string mybillcond = "";
                string showcond = "";
                List<SQLParameters> parameters = new List<SQLParameters>();
                if (!string.IsNullOrWhiteSpace(getBillApprovalRequest?.CampusCode))
                {
                    showcond += " and A.CampusCode=@CampusCode";
                    parameters.Add(new SQLParameters("@CampusCode", getBillApprovalRequest.CampusCode));
                }

                if (!string.IsNullOrWhiteSpace(getBillApprovalRequest?.ChequeBy))
                {
                    mychqcond = " And C.IssuedBy=@IssuedBy";
                    parameters.Add(new SQLParameters("@IssuedBy", getBillApprovalRequest.ChequeBy));
                }

                if ((role.ToUpper() == "GUEST" || role.ToUpper() == "STORE") || (Array.IndexOf(new string[] { "Pending Bill", "Pending Cheque", "Pending All" }, getBillApprovalRequest?.Category) != -1))
                {
                    if ((role.ToUpper() == "GUEST" || role.ToUpper() == "STORE"))
                    {
                        showcond = " And B.EmployeeID= @EmployeeId";

                        parameters.Add(new SQLParameters("@EmployeeId", employeeId));


                        if (!string.IsNullOrWhiteSpace(getBillApprovalRequest?.AdditionalEmployeeCode))
                        {
                            showcond = " And (B.EmployeeID = @AdditionalEmployeeID or B.EmployeeID = @AdditionalEmployeeID)";
                            parameters.Add(new SQLParameters("@AdditionalEmployeeID", getBillApprovalRequest.AdditionalEmployeeCode));
                        }
                    }

                    if (Array.IndexOf(new string[] { "Pending Bill", "Pending Cheque", "Pending All" }, getBillApprovalRequest?.Category) != -1)
                    {
                        showcond = showcond + " And B.Status='Pending'";
                    }
                }

                if (!string.IsNullOrWhiteSpace(getBillApprovalRequest?.Department))
                {
                    showcond = showcond + " And A.Col1=@Department";
                    parameters.Add(new SQLParameters("@Department", getBillApprovalRequest.Department));
                }

                if (!string.IsNullOrWhiteSpace(getBillApprovalRequest?.InitiatedBy))
                {
                    showcond = showcond + " And A.IssuedBy=@InitiatedBy";
                    parameters.Add(new SQLParameters("@InitiatedBy", getBillApprovalRequest.InitiatedBy));
                }

                if (!string.IsNullOrWhiteSpace(getBillApprovalRequest?.ReferenceNo))
                {
                    showcond = showcond + " And A.TransactionID=@TransactionID";
                    parameters.Add(new SQLParameters("@TransactionID", getBillApprovalRequest.ReferenceNo));
                }

                parameters.Add(new SQLParameters("@LimitItems", getBillApprovalRequest!.NoOfRecords ?? 0));
                parameters.Add(new SQLParameters("@OffSetItems", getBillApprovalRequest!.RecordFrom ?? 0));

                if (getBillApprovalRequest?.Category == "Bill & Chq.")
                {
                    if (mychqcond == "")
                    {
                        query = $"select * from ((select DISTINCT CONCAT(IF(DATE_FORMAT(A.Col2,'%d %b, %y') is Not NULL And A.VendorID!=827,CONCAT('App Dt :',DATE_FORMAT(A.Col2,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra4,'%d %b, %y') is Not NULL  And A.BillDate is not null  And A.BillExtra4!=A.BillDate  And A.VendorID!=827,CONCAT('Test Dt :',DATE_FORMAT(A.BillExtra4,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra1,'%d %b, %y') is Not NULL,CONCAT('Exp Dt :',DATE_FORMAT(A.BillExtra1,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillDate,'%d %b, %y') is Not NULL,CONCAT('Bill Dt :',DATE_FORMAT(A.BillDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.IssuedOn,'%d %b, %y') is Not NULL,CONCAT('Upl Dt :',DATE_FORMAT(A.IssuedOn,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y') is Not NULL,CONCAT('Dept Dt :',DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.ApprovedOn,'%d %b, %y') is Not NULL,CONCAT('Pass Dt :',DATE_FORMAT(A.ApprovedOn,'%d %b, %y'),'$'),''),IF(A.Col4 is Not NULL And A.Col4!='---'  And WEEKDAY(A.Col4) is not null,CONCAT('Pub. Dt :',DATE_FORMAT(A.Col4,'%d %b, %y'),'$'),'')) 'INI',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'IssuedName',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'BillNameBy',ForType,A.TransactionID 'TransID','Bills Approval' as 'MyType',CAST('---' as CHAR) as 'SequenceID',RelativePersonName 'Initiated By',if(A.Col3 is NULL or A.Col3='----',CONCAT(FirmName,'#',RelativePersonName),CONCAT(FirmName,'( ',A.Col3,' )','#',RelativePersonName)) 'Firm Name',Remark 'Purpose',AmountRequired 'Santioned',AmountPaid 'Paid',DATE_FORMAT(IssuedOn,'%b %d,%Y') 'On',A.Status,DATE_FORMAT(ADDDATE(AssignedOn,INTERVAL `Limit` DAY),'%m-%d-%Y') 'Till',A.Col2 as 'MyIssue',A.TransactionID,IFNULL(DATE_FORMAT(if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,IFNULL(A.BillDate,A.Col2) ),'%m-%d-%Y'),'') 'MyINICheck',IFNULL(DATE_FORMAT(A.IssuedOn,'%m-%d-%Y'),'') 'MyEntryCheck',(Select DISTINCT IsSpecial from specialvendors M where M.VendorID=A.VendorID) 'IsSpecial',CashDiscount,(A.AmountRequired-A.AmountPaid) 'Bal',if(A.BillDate is not null ,if(A.CashDiscount=0,DATE_FORMAT(ADDDATE(A.BillDate,INTERVAL 45 DAY) ,'%Y-%m-%d'),'R'),'R') 'Cond45','' as 'CheqAmt',(A.AmountRequired+CashDiscount) 'TotBill',A.BillExtra3,A.BillExtra6,REPLACE(A.BillExtra2,'$','\n') 'ShowMeNow',if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,A.Col2 ) as 'BillExtra4', A.Col5,A.Col1,'' as 'ChequeVendor',A.VendorID as 'BVId',A.RelativePersonID as 'BRPID' from bill_base A, approvals_authority B where A.TransactionID=B.TransactionID And B.Type='Bills Approval' {mybillcond} {showcond} And `Session`=@Session And (A.`Status` ='Waiting For Bills Approval' OR IF(A.AmountRemaining>0 And A.Status='Ready To Issue Amount',TRUE,false) ) AND FIND_IN_SET(ForType,@ForTypes)  order by A.BillExtra4) UNION (select DISTINCT  CONCAT(IF(DATE_FORMAT(A.Col2,'%d %b, %y') is Not NULL And A.VendorID!=827,CONCAT('App Dt :',DATE_FORMAT(A.Col2,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra4,'%d %b, %y') is Not NULL  And A.BillDate is not null  And A.BillExtra4!=A.BillDate  And A.VendorID!=827,CONCAT('Test Dt :',DATE_FORMAT(A.BillExtra4,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra1,'%d %b, %y') is Not NULL,CONCAT('Exp Dt :',DATE_FORMAT(A.BillExtra1,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillDate,'%d %b, %y') is Not NULL,CONCAT('Bill Dt :',DATE_FORMAT(A.BillDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.IssuedOn,'%d %b, %y') is Not NULL,CONCAT('Upl Dt :',DATE_FORMAT(A.IssuedOn,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y') is Not NULL,CONCAT('Dept Dt :',DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.ApprovedOn,'%d %b, %y') is Not NULL,CONCAT('Pass Dt :',DATE_FORMAT(A.ApprovedOn,'%d %b, %y'),'$'),''),IF(A.Col4 is Not NULL And A.Col4!='---'  And WEEKDAY(A.Col4) is not null,CONCAT('Pub. Dt :',DATE_FORMAT(A.Col4,'%d %b, %y'),'$'),'')) 'INI',CONCAT(C.IssuedByName ,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'IssuedName',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'BillNameBy',ForType,A.TransactionID 'TransID',Type as 'MyType',CAST(B.SequenceNo as CHAR) as 'SequenceID',RelativePersonName 'Initiated By',if(A.Col3 is NULL or A.Col3='----',CONCAT(FirmName,'#',RelativePersonName),CONCAT(FirmName,'( ',A.Col3,' )','#',RelativePersonName)) 'Firm Name',C.Remark 'Purpose',AmountRequired 'Santioned',AmountPaid 'Paid',DATE_FORMAT(C.IssuedOn,'%b %d,%Y') 'On','Waiting For Cheque Approval' as  'Status',DATE_FORMAT(ADDDATE(AssignedOn,INTERVAL `Limit` DAY),'%m-%d-%Y') 'Till',A.Col2 as 'MyIssue',A.TransactionID,IFNULL(DATE_FORMAT(if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,IFNULL(A.BillDate,A.Col2) ),'%m-%d-%Y'),'') 'MyINICheck',IFNULL(DATE_FORMAT(A.IssuedOn,'%m-%d-%Y'),'') 'MyEntryCheck',(Select DISTINCT IsSpecial from specialvendors M where M.VendorID=A.VendorID) 'IsSpecial',CashDiscount,(A.AmountRequired-A.AmountPaid) 'Bal',if(A.BillDate is not null ,if(A.CashDiscount=0,DATE_FORMAT(ADDDATE(A.BillDate,INTERVAL 45 DAY) ,'%m-%d-%Y'),'R'),'R') 'Cond45',CAST(CONCAT('Chq:',IssuedAmount) as CHAR) as 'CheqAmt',(A.AmountRequired+CashDiscount) 'TotBill',A.BillExtra3,A.BillExtra6,REPLACE(A.BillExtra2,'$','\n') 'ShowMeNow',if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,A.Col2 ) as 'BillExtra4', A.Col5,A.Col1,CONCAT(CAST(CVId as CHAR),'#',CVName,'#',CVSubFirm,'#',CVAddName) as 'ChequeVendor',A.VendorID as 'BVId',A.RelativePersonID as 'BRPID'   from bill_base A, approvals_authority B,bill_transaction_issue C where B.TransactionID=C.TransactionID And B.SequenceNo=C.SequenceID And A.TransactionID=B.TransactionID {mychqcond} {showcond} And B.Type ='Bills Approval -> Cheque Approval' And `Session`=@Session And C.SignedOn is NULL AND FIND_IN_SET(ForType,@ForTypes) order by A.BillExtra4)) A1 ORDER BY A1.BillExtra4,A1.TransactionID LIMIT @LimitItems OFFSET @OffSetItems";

                        parameters.Add(new SQLParameters("@Session", getBillApprovalRequest?.Session ?? string.Empty));
                        parameters.Add(new SQLParameters("@ForTypes", getBillApprovalRequest?.Type ?? string.Empty));
                    }
                    else
                    {
                        query = $"select * from ((select DISTINCT  CONCAT(IF(DATE_FORMAT(A.Col2,'%d %b, %y') is Not NULL And A.VendorID!=827,CONCAT('App Dt :',DATE_FORMAT(A.Col2,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra4,'%d %b, %y') is Not NULL  And A.BillDate is not null  And A.BillExtra4!=A.BillDate  And A.VendorID!=827,CONCAT('Test Dt :',DATE_FORMAT(A.BillExtra4,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra1,'%d %b, %y') is Not NULL,CONCAT('Exp Dt :',DATE_FORMAT(A.BillExtra1,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillDate,'%d %b, %y') is Not NULL,CONCAT('Bill Dt :',DATE_FORMAT(A.BillDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.IssuedOn,'%d %b, %y') is Not NULL,CONCAT('Upl Dt :',DATE_FORMAT(A.IssuedOn,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y') is Not NULL,CONCAT('Dept Dt :',DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.ApprovedOn,'%d %b, %y') is Not NULL,CONCAT('Pass Dt :',DATE_FORMAT(A.ApprovedOn,'%d %b, %y'),'$'),''),IF(A.Col4 is Not NULL And A.Col4!='---'  And WEEKDAY(A.Col4) is not null,CONCAT('Pub. Dt :',DATE_FORMAT(A.Col4,'%d %b, %y'),'$'),'')) 'INI',CONCAT(C.IssuedByName ,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'IssuedName',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'BillNameBy',ForType,A.TransactionID 'TransID',Type as 'MyType',CAST(B.SequenceNo as CHAR) as 'SequenceID',RelativePersonName 'Initiated By',if(A.Col3 is NULL or A.Col3='----',CONCAT(FirmName,'#',RelativePersonName),CONCAT(FirmName,'( ',A.Col3,' )','#',RelativePersonName)) 'Firm Name',C.Remark 'Purpose',IssuedAmount 'Santioned',PaidAmount 'Paid',DATE_FORMAT(C.IssuedOn,'%b %d,%Y') 'On','Waiting For Cheque Approval' as  'Status',DATE_FORMAT(ADDDATE(AssignedOn,INTERVAL `Limit` DAY),'%m/%d/%Y') 'Till',A.Col2 as 'MyIssue',A.TransactionID,IFNULL(DATE_FORMAT(if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,IFNULL(A.BillDate,A.Col2) ),'%m/%d/%Y'),'') 'MyINICheck',IFNULL(DATE_FORMAT(A.IssuedOn,'%m/%d/%Y'),'') 'MyEntryCheck',(Select DISTINCT IsSpecial from specialvendors M where M.VendorID=A.VendorID) 'IsSpecial',CashDiscount,(A.AmountRequired-A.AmountPaid) 'Bal',if(A.BillDate is not null ,if(A.CashDiscount=0,DATE_FORMAT(ADDDATE(A.BillDate,INTERVAL 45 DAY) ,'%Y-%m-%d'),'R'),'R') 'Cond45','' as 'CheqAmt',(A.AmountRequired+CashDiscount) 'TotBill',A.BillExtra3,A.BillExtra6,REPLACE(A.BillExtra2,'$','\n') 'ShowMeNow',if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,A.Col2 ) as 'BillExtra4', A.Col5,A.Col1,CONCAT(CAST(CVId as CHAR),'#',CVName,'#',CVSubFirm,'#',CVAddName) as 'ChequeVendor',A.VendorID as 'BVId',A.RelativePersonID as 'BRPID'   from bill_base A, approvals_authority B,bill_transaction_issue C where B.TransactionID=C.TransactionID And B.SequenceNo=C.SequenceID And A.TransactionID=B.TransactionID {mychqcond} {showcond} And B.Type = 'Bills Approval -> Cheque Approval' And `Session`=@Session And C.SignedOn is NULL AND FIND_IN_SET(ForType,@ForTypes)  order by A.BillExtra4)) A1 ORDER BY A1.BillExtra4,A1.TransactionID LIMIT @LimitItems OFFSET @OffSetItems";

                        parameters.Add(new SQLParameters("@Session", getBillApprovalRequest?.Session ?? string.Empty));
                        parameters.Add(new SQLParameters("@ForTypes", getBillApprovalRequest?.Type ?? string.Empty));

                    }
                }

                if (getBillApprovalRequest?.Category == "Pending Bill")
                {
                    string trcond = "";

                    if (!string.IsNullOrWhiteSpace(getBillApprovalRequest?.ReferenceNo))
                    {
                        trcond = " And A.TransactionID=@ReferenceNo";
                        parameters.Add(new SQLParameters("@ReferenceNo", getBillApprovalRequest.ReferenceNo));
                    }

                    query = $"select distinct CONCAT(IF(DATE_FORMAT(A.Col2,'%d %b, %y') is Not NULL And A.VendorID!=827,CONCAT('App Dt :',DATE_FORMAT(A.Col2,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra4,'%d %b, %y') is Not NULL  And A.BillDate is not null  And A.BillExtra4!=A.BillDate  And A.VendorID!=827,CONCAT('Test Dt :',DATE_FORMAT(A.BillExtra4,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra1,'%d %b, %y') is Not NULL,CONCAT('Exp Dt :',DATE_FORMAT(A.BillExtra1,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillDate,'%d %b, %y') is Not NULL,CONCAT('Bill Dt :',DATE_FORMAT(A.BillDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.IssuedOn,'%d %b, %y') is Not NULL,CONCAT('Upl Dt :',DATE_FORMAT(A.IssuedOn,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y') is Not NULL,CONCAT('Dept Dt :',DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.ApprovedOn,'%d %b, %y') is Not NULL,CONCAT('Pass Dt :',DATE_FORMAT(A.ApprovedOn,'%d %b, %y'),'$'),''),IF(A.Col4 is Not NULL And A.Col4!='---'  And WEEKDAY(A.Col4) is not null,CONCAT('Pub. Dt :',DATE_FORMAT(A.Col4,'%d %b, %y'),'$'),'')) 'INI',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'IssuedName',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'BillNameBy',ForType,A.TransactionID 'TransID','Bills Approval' as 'MyType',CAST('---' as CHAR) as 'SequenceID',RelativePersonName 'Initiated By',if(A.Col3 is NULL or A.Col3='----',CONCAT(FirmName,'#',RelativePersonName),CONCAT(FirmName,'( ',A.Col3,' )','#',RelativePersonName)) 'Firm Name',Remark 'Purpose',AmountRequired 'Santioned',AmountPaid 'Paid',DATE_FORMAT(IssuedOn,'%b %d,%Y') 'On',A.Status,DATE_FORMAT(ADDDATE(AssignedOn,INTERVAL `Limit` DAY),'%m/%d/%Y') 'Till',A.Col2 as 'MyIssue',A.TransactionID,IFNULL(DATE_FORMAT(if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,IFNULL(A.BillDate,A.Col2) ),'%m/%d/%Y'),'') 'MyINICheck',IFNULL(DATE_FORMAT(A.IssuedOn,'%m/%d/%Y'),'') 'MyEntryCheck',(Select DISTINCT IsSpecial from specialvendors M where M.VendorID=A.VendorID) 'IsSpecial',CashDiscount,(A.AmountRequired-A.AmountPaid) 'Bal',if(A.BillDate is not null ,if(A.CashDiscount=0,DATE_FORMAT(ADDDATE(A.BillDate,INTERVAL 45 DAY) ,'%Y-%m-%d'),'R'),'R') 'Cond45','' as 'CheqAmt',(A.AmountRequired+CashDiscount) 'TotBill',A.BillExtra3,A.BillExtra6,REPLACE(A.BillExtra2,'$','\n') 'ShowMeNow',if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,A.Col2 ) as 'BillExtra4', A.Col5,A.Col1,'' as 'ChequeVendor',A.VendorID as 'BVId',A.RelativePersonID as 'BRPID' from bill_base A, approvals_authority B where A.TransactionID=B.TransactionID {trcond} And B.Type='Bills Approval' {mybillcond} {showcond} And `Session`=@Session And A.`Status` ='Waiting For Bills Approval' AND FIND_IN_SET(ForType,@ForTypes) order by A.BillExtra4,A.TransactionID LIMIT @LimitItems OFFSET @OffSetItems";

                    parameters.Add(new SQLParameters("@Session", getBillApprovalRequest?.Session ?? string.Empty));
                    parameters.Add(new SQLParameters("@ForTypes", getBillApprovalRequest?.Type ?? string.Empty));
                }

                if (getBillApprovalRequest?.Category == "Pending Cheque")
                {

                    string trcond = "";

                    if (!string.IsNullOrWhiteSpace(getBillApprovalRequest?.ReferenceNo) && !string.IsNullOrWhiteSpace(getBillApprovalRequest?.SequenceId))
                    {
                        trcond = " And C.TransactionID=@ReferenceNo And C.SequenceID=@SequenceId ";

                        parameters.Add(new SQLParameters("@ReferenceNo", getBillApprovalRequest.ReferenceNo));
                        parameters.Add(new SQLParameters("@SequenceId", getBillApprovalRequest.SequenceId));
                    }


                    if (mychqcond == "")
                    {
                        query = $"select   distinct CONCAT(IF(DATE_FORMAT(A.Col2,'%d %b, %y') is Not NULL And A.VendorID!=827,CONCAT('App Dt :',DATE_FORMAT(A.Col2,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra4,'%d %b, %y') is Not NULL And A.BillDate is not null And A.BillExtra4!=A.BillDate  And A.VendorID!=827,CONCAT('Test Dt :',DATE_FORMAT(A.BillExtra4,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra1,'%d %b, %y') is Not NULL,CONCAT('Exp Dt :',DATE_FORMAT(A.BillExtra1,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillDate,'%d %b, %y') is Not NULL,CONCAT('Bill Dt :',DATE_FORMAT(A.BillDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.IssuedOn,'%d %b, %y') is Not NULL,CONCAT('Upl Dt :',DATE_FORMAT(A.IssuedOn,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.ApprovedOn,'%d %b, %y') is Not NULL,CONCAT('Pass Dt :',DATE_FORMAT(A.ApprovedOn,'%d %b, %y'),'$'),''),IF(A.Col4 is Not NULL And A.Col4!='---'  And WEEKDAY(A.Col4) is not null,CONCAT('Pub. Dt :',DATE_FORMAT(A.Col4,'%d %b, %y'),'$'),'')) 'INI',CONCAT(C.IssuedByName ,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'IssuedName',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'BillNameBy',ForType,A.TransactionID 'TransID',Type as 'MyType',CAST(B.SequenceNo as CHAR) as 'SequenceID',RelativePersonName 'Initiated By',if(A.Col3 is NULL or A.Col3='----',CONCAT(FirmName,'#',RelativePersonName),CONCAT(FirmName,'( ',A.Col3,' )','#',RelativePersonName)) 'Firm Name',C.Remark 'Purpose',AmountRequired 'Santioned',AmountPaid 'Paid',DATE_FORMAT(C.IssuedOn,'%b %d,%Y') 'On','Waiting For Cheque Approval' as  'Status',DATE_FORMAT(ADDDATE(AssignedOn,INTERVAL `Limit` DAY),'%m/%d/%Y') 'Till',A.Col2 as 'MyIssue',A.TransactionID,IFNULL(DATE_FORMAT(if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,IFNULL(A.BillDate,A.Col2) ),'%m/%d/%Y'),'') 'MyINICheck',IFNULL(DATE_FORMAT(A.IssuedOn,'%m/%d/%Y'),'') 'MyEntryCheck',(Select DISTINCT IsSpecial from specialvendors M where M.VendorID=A.VendorID) 'IsSpecial',CashDiscount,(A.AmountRequired-A.AmountPaid) 'Bal',if(A.BillDate is not null ,if(A.CashDiscount=0,DATE_FORMAT(ADDDATE(A.BillDate,INTERVAL 45 DAY) ,'%Y-%m-%d'),'R'),'R') 'Cond45',CAST(CONCAT('Chq:',IssuedAmount) as CHAR) as 'CheqAmt',(A.AmountRequired+CashDiscount) 'TotBill',A.BillExtra3,A.BillExtra6,REPLACE(A.BillExtra2,'$','\n') 'ShowMeNow',if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,A.Col2 ) as 'BillExtra4', A.Col5,A.Col1,CONCAT(CAST(CVId as CHAR),'#',CVName,'#',CVSubFirm,'#',CVAddName) as 'ChequeVendor',A.VendorID as 'BVId',A.RelativePersonID as 'BRPID'   from bill_base A, approvals_authority B,bill_transaction_issue C where B.TransactionID=C.TransactionID {trcond} And B.SequenceNo=C.SequenceID And A.TransactionID=B.TransactionID {mychqcond} {showcond} And B.Type ='Bills Approval -> Cheque Approval' And `Session`=@Session And C.SignedOn is NULL AND FIND_IN_SET(ForType,@ForTypes) order by A.BillExtra4,A.TransactionID LIMIT @LimitItems OFFSET @OffSetItems";

                        parameters.Add(new SQLParameters("@Session", getBillApprovalRequest?.Session ?? string.Empty));
                        parameters.Add(new SQLParameters("@ForTypes", getBillApprovalRequest?.Type ?? string.Empty));
                    }
                    else
                    {
                        query = $"select  distinct  CONCAT(IF(DATE_FORMAT(A.Col2,'%d %b, %y') is Not NULL And A.VendorID!=827,CONCAT('App Dt :',DATE_FORMAT(A.Col2,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra4,'%d %b, %y') is Not NULL  And A.BillDate is not null  And A.BillExtra4!=A.BillDate  And A.VendorID!=827,CONCAT('Test Dt :',DATE_FORMAT(A.BillExtra4,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra1,'%d %b, %y') is Not NULL,CONCAT('Exp Dt :',DATE_FORMAT(A.BillExtra1,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillDate,'%d %b, %y') is Not NULL,CONCAT('Bill Dt :',DATE_FORMAT(A.BillDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.IssuedOn,'%d %b, %y') is Not NULL,CONCAT('Upl Dt :',DATE_FORMAT(A.IssuedOn,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y') is Not NULL,CONCAT('Dept Dt :',DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.ApprovedOn,'%d %b, %y') is Not NULL,CONCAT('Pass Dt :',DATE_FORMAT(A.ApprovedOn,'%d %b, %y'),'$'),''),IF(A.Col4 is Not NULL And A.Col4!='---'  And WEEKDAY(A.Col4) is not null,CONCAT('Pub. Dt :',DATE_FORMAT(A.Col4,'%d %b, %y'),'$'),'')) 'INI',CONCAT(C.IssuedByName ,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'IssuedName',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'BillNameBy',ForType,A.TransactionID 'TransID',Type as 'MyType',CAST(B.SequenceNo as CHAR) as 'SequenceID',RelativePersonName 'Initiated By',if(A.Col3 is NULL or A.Col3='----',CONCAT(FirmName,'#',RelativePersonName),CONCAT(FirmName,'( ',A.Col3,' )','#',RelativePersonName)) 'Firm Name',C.Remark 'Purpose',IssuedAmount 'Santioned',PaidAmount 'Paid',DATE_FORMAT(C.IssuedOn,'%b %d,%Y') 'On','Waiting For Cheque Approval' as  'Status',DATE_FORMAT(ADDDATE(AssignedOn,INTERVAL `Limit` DAY),'%m/%d/%Y') 'Till',A.Col2 as 'MyIssue',A.TransactionID,IFNULL(DATE_FORMAT(if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,IFNULL(A.BillDate,A.Col2) ),'%m/%d/%Y'),'') 'MyINICheck',IFNULL(DATE_FORMAT(A.IssuedOn,'%m/%d/%Y'),'') 'MyEntryCheck',(Select DISTINCT IsSpecial from specialvendors M where M.VendorID=A.VendorID) 'IsSpecial',CashDiscount,(A.AmountRequired-A.AmountPaid) 'Bal',if(A.BillDate is not null ,if(A.CashDiscount=0,DATE_FORMAT(ADDDATE(A.BillDate,INTERVAL 45 DAY) ,'%Y-%m-%d'),'R'),'R') 'Cond45','' as 'CheqAmt',(A.AmountRequired+CashDiscount) 'TotBill',A.BillExtra3,A.BillExtra6,REPLACE(A.BillExtra2,'$','\n') 'ShowMeNow',if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,A.Col2 ) as 'BillExtra4', A.Col5,A.Col1,CONCAT(CAST(CVId as CHAR),'#',CVName,'#',CVSubFirm,'#',CVAddName) as 'ChequeVendor',A.VendorID as 'BVId',A.RelativePersonID as 'BRPID'   from bill_base A, approvals_authority B,bill_transaction_issue C where B.TransactionID=C.TransactionID {trcond} And B.SequenceNo=C.SequenceID And A.TransactionID=B.TransactionID {mychqcond} {showcond} And B.Type ='Bills Approval -> Cheque Approval' And `Session`=@Session And C.SignedOn is NULL AND FIND_IN_SET(ForType,@ForTypes)  order by A.BillExtra4,A.TransactionID LIMIT @LimitItems OFFSET @OffSetItems";

                        parameters.Add(new SQLParameters("@Session", getBillApprovalRequest?.Session ?? string.Empty));
                        parameters.Add(new SQLParameters("@ForTypes", getBillApprovalRequest?.Type ?? string.Empty));
                    }
                }

                if (getBillApprovalRequest?.Category == "Pending All")
                {
                    string trcond = "";

                    if (mychqcond == "")
                    {
                        query = $"Select * from ((select distinct CONCAT(IF(DATE_FORMAT(A.Col2,'%d %b, %y') is Not NULL And A.VendorID!=827,CONCAT('App Dt :',DATE_FORMAT(A.Col2,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra4,'%d %b, %y') is Not NULL  And A.BillDate is not null  And A.BillExtra4!=A.BillDate  And A.VendorID!=827,CONCAT('Test Dt :',DATE_FORMAT(A.BillExtra4,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra1,'%d %b, %y') is Not NULL,CONCAT('Exp Dt :',DATE_FORMAT(A.BillExtra1,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillDate,'%d %b, %y') is Not NULL,CONCAT('Bill Dt :',DATE_FORMAT(A.BillDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.IssuedOn,'%d %b, %y') is Not NULL,CONCAT('Upl Dt :',DATE_FORMAT(A.IssuedOn,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y') is Not NULL,CONCAT('Dept Dt :',DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.ApprovedOn,'%d %b, %y') is Not NULL,CONCAT('Pass Dt :',DATE_FORMAT(A.ApprovedOn,'%d %b, %y'),'$'),''),IF(A.Col4 is Not NULL And A.Col4!='---'  And WEEKDAY(A.Col4) is not null,CONCAT('Pub. Dt :',DATE_FORMAT(A.Col4,'%d %b, %y'),'$'),'')) 'INI',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'IssuedName',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'BillNameBy',ForType,A.TransactionID 'TransID','Bills Approval' as 'MyType',CAST('---' as CHAR) as 'SequenceID',RelativePersonName 'Initiated By',if(A.Col3 is NULL or A.Col3='----',CONCAT(FirmName,'#',RelativePersonName),CONCAT(FirmName,'( ',A.Col3,' )','#',RelativePersonName)) 'Firm Name',Remark 'Purpose',AmountRequired 'Santioned',AmountPaid 'Paid',DATE_FORMAT(IssuedOn,'%b %d,%Y') 'On',A.Status,DATE_FORMAT(ADDDATE(AssignedOn,INTERVAL `Limit` DAY),'%m/%d/%Y') 'Till',A.Col2 as 'MyIssue',A.TransactionID,IFNULL(DATE_FORMAT(if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,IFNULL(A.BillDate,A.Col2) ),'%m/%d/%Y'),'') 'MyINICheck',IFNULL(DATE_FORMAT(A.IssuedOn,'%m/%d/%Y'),'') 'MyEntryCheck',(Select DISTINCT IsSpecial from specialvendors M where M.VendorID=A.VendorID) 'IsSpecial',CashDiscount,(A.AmountRequired-A.AmountPaid) 'Bal',if(A.BillDate is not null ,if(A.CashDiscount=0,DATE_FORMAT(ADDDATE(A.BillDate,INTERVAL 45 DAY) ,'%Y-%m-%d'),'R'),'R') 'Cond45','' as 'CheqAmt',(A.AmountRequired+CashDiscount) 'TotBill',A.BillExtra3,A.BillExtra6,REPLACE(A.BillExtra2,'$','\n') 'ShowMeNow',if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,A.Col2 ) as 'BillExtra4', A.Col5,A.Col1,'' as 'ChequeVendor',A.VendorID as 'BVId',A.RelativePersonID as 'BRPID' from bill_base A, approvals_authority B where A.TransactionID=B.TransactionID {trcond} And B.Type='Bills Approval' {mybillcond} {showcond} And `Session`=@Session And A.`Status` ='Waiting For Bills Approval' AND FIND_IN_SET(ForType,@ForTypes) order by A.BillExtra4) UNION (select   distinct CONCAT(IF(DATE_FORMAT(A.Col2,'%d %b, %y') is Not NULL And A.VendorID!=827,CONCAT('App Dt :',DATE_FORMAT(A.Col2,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra4,'%d %b, %y') is Not NULL  And A.BillDate is not null  And A.BillExtra4!=A.BillDate  And A.VendorID!=827,CONCAT('Test Dt :',DATE_FORMAT(A.BillExtra4,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra1,'%d %b, %y') is Not NULL,CONCAT('Exp Dt :',DATE_FORMAT(A.BillExtra1,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillDate,'%d %b, %y') is Not NULL,CONCAT('Bill Dt :',DATE_FORMAT(A.BillDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.IssuedOn,'%d %b, %y') is Not NULL,CONCAT('Upl Dt :',DATE_FORMAT(A.IssuedOn,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y') is Not NULL,CONCAT('Dept Dt :',DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.ApprovedOn,'%d %b, %y') is Not NULL,CONCAT('Pass Dt :',DATE_FORMAT(A.ApprovedOn,'%d %b, %y'),'$'),''),IF(A.Col4 is Not NULL And A.Col4!='---'  And WEEKDAY(A.Col4) is not null,CONCAT('Pub. Dt :',DATE_FORMAT(A.Col4,'%d %b, %y'),'$'),'')) 'INI',CONCAT(C.IssuedByName ,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'IssuedName',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'BillNameBy',ForType,A.TransactionID 'TransID',Type as 'MyType',CAST(B.SequenceNo as CHAR) as 'SequenceID',RelativePersonName 'Initiated By',if(A.Col3 is NULL or A.Col3='----',CONCAT(FirmName,'#',RelativePersonName),CONCAT(FirmName,'( ',A.Col3,' )','#',RelativePersonName)) 'Firm Name',C.Remark 'Purpose',AmountRequired 'Santioned',AmountPaid 'Paid',DATE_FORMAT(C.IssuedOn,'%b %d,%Y') 'On','Waiting For Cheque Approval' as  'Status',DATE_FORMAT(ADDDATE(AssignedOn,INTERVAL `Limit` DAY),'%m/%d/%Y') 'Till',A.Col2 as 'MyIssue',A.TransactionID,IFNULL(DATE_FORMAT(if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,IFNULL(A.BillDate,A.Col2) ),'%m/%d/%Y'),'') 'MyINICheck',IFNULL(DATE_FORMAT(A.IssuedOn,'%m/%d/%Y'),'') 'MyEntryCheck',(Select DISTINCT IsSpecial from specialvendors M where M.VendorID=A.VendorID) 'IsSpecial',CashDiscount,(A.AmountRequired-A.AmountPaid) 'Bal',if(A.BillDate is not null ,if(A.CashDiscount=0,DATE_FORMAT(ADDDATE(A.BillDate,INTERVAL 45 DAY) ,'%Y-%m-%d'),'R'),'R') 'Cond45',CAST(CONCAT('Chq:',IssuedAmount) as CHAR) as 'CheqAmt',(A.AmountRequired+CashDiscount) 'TotBill',A.BillExtra3,A.BillExtra6,REPLACE(A.BillExtra2,'$','\n') 'ShowMeNow',if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,A.Col2 ) as 'BillExtra4', A.Col5,A.Col1,CONCAT(CAST(CVId as CHAR),'#',CVName,'#',CVSubFirm,'#',CVAddName) as 'ChequeVendor',A.VendorID as 'BVId',A.RelativePersonID as 'BRPID'   from bill_base A, approvals_authority B,bill_transaction_issue C where B.TransactionID=C.TransactionID {trcond} And B.SequenceNo=C.SequenceID And A.TransactionID=B.TransactionID {mychqcond} {showcond} And B.Type ='Bills Approval -> Cheque Approval' And `Session`=@Session And C.SignedOn is NULL AND FIND_IN_SET(ForType,@ForTypes) order by A.BillExtra4)) A order by A.BillExtra4,A.TransactionID LIMIT @LimitItems OFFSET @OffSetItems";

                        parameters.Add(new SQLParameters("@Session", getBillApprovalRequest?.Session ?? string.Empty));
                        parameters.Add(new SQLParameters("@ForTypes", getBillApprovalRequest?.Type ?? string.Empty));
                    }
                    else
                    {
                        query = $"select  distinct  CONCAT(IF(DATE_FORMAT(A.Col2,'%d %b, %y') is Not NULL And A.VendorID!=827,CONCAT('App Dt :',DATE_FORMAT(A.Col2,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra4,'%d %b, %y') is Not NULL  And A.BillDate is not null  And A.BillExtra4!=A.BillDate  And A.VendorID!=827,CONCAT('Test Dt :',DATE_FORMAT(A.BillExtra4,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra1,'%d %b, %y') is Not NULL,CONCAT('Exp Dt :',DATE_FORMAT(A.BillExtra1,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillDate,'%d %b, %y') is Not NULL,CONCAT('Bill Dt :',DATE_FORMAT(A.BillDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.IssuedOn,'%d %b, %y') is Not NULL,CONCAT('Upl Dt :',DATE_FORMAT(A.IssuedOn,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y') is Not NULL,CONCAT('Dept Dt :',DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.ApprovedOn,'%d %b, %y') is Not NULL,CONCAT('Pass Dt :',DATE_FORMAT(A.ApprovedOn,'%d %b, %y'),'$'),''),IF(A.Col4 is Not NULL And A.Col4!='---'  And WEEKDAY(A.Col4) is not null,CONCAT('Pub. Dt :',DATE_FORMAT(A.Col4,'%d %b, %y'),'$'),'')) 'INI',CONCAT(C.IssuedByName ,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'IssuedName',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'BillNameBy',ForType,A.TransactionID 'TransID',Type as 'MyType',CAST(B.SequenceNo as CHAR) as 'SequenceID',RelativePersonName 'Initiated By',if(A.Col3 is NULL or A.Col3='----',CONCAT(FirmName,'#',RelativePersonName),CONCAT(FirmName,'( ',A.Col3,' )','#',RelativePersonName)) 'Firm Name',C.Remark 'Purpose',IssuedAmount 'Santioned',PaidAmount 'Paid',DATE_FORMAT(C.IssuedOn,'%b %d,%Y') 'On','Waiting For Cheque Approval' as  'Status',DATE_FORMAT(ADDDATE(AssignedOn,INTERVAL `Limit` DAY),'%m/%d/%Y') 'Till',A.Col2 as 'MyIssue',A.TransactionID,IFNULL(DATE_FORMAT(if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,IFNULL(A.BillDate,A.Col2) ),'%m/%d/%Y'),'') 'MyINICheck',IFNULL(DATE_FORMAT(A.IssuedOn,'%m/%d/%Y'),'') 'MyEntryCheck',(Select DISTINCT IsSpecial from specialvendors M where M.VendorID=A.VendorID) 'IsSpecial',CashDiscount,(A.AmountRequired-A.AmountPaid) 'Bal',if(A.BillDate is not null ,if(A.CashDiscount=0,DATE_FORMAT(ADDDATE(A.BillDate,INTERVAL 45 DAY) ,'%Y-%m-%d'),'R'),'R') 'Cond45','' as 'CheqAmt',(A.AmountRequired+CashDiscount) 'TotBill',A.BillExtra3,A.BillExtra6,REPLACE(A.BillExtra2,'$','\n') 'ShowMeNow',if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,A.Col2 ) as 'BillExtra4', A.Col5,A.Col1,CONCAT(CAST(CVId as CHAR),'#',CVName,'#',CVSubFirm,'#',CVAddName) as 'ChequeVendor',A.VendorID as 'BVId',A.RelativePersonID as 'BRPID'   from bill_base A, approvals_authority B,bill_transaction_issue C where B.TransactionID=C.TransactionID {trcond} And B.SequenceNo=C.SequenceID And A.TransactionID=B.TransactionID {mychqcond}  {showcond} And B.Type ='Bills Approval -> Cheque Approval' And `Session`=@Session And C.SignedOn is NULL AND FIND_IN_SET(ForType,@ForTypes)  order by A.BillExtra4,A.TransactionID LIMIT @LimitItems OFFSET @OffSetItems";

                        parameters.Add(new SQLParameters("@Session", getBillApprovalRequest?.Session ?? string.Empty));
                        parameters.Add(new SQLParameters("@ForTypes", getBillApprovalRequest?.Type ?? string.Empty));
                    }
                }

                if (getBillApprovalRequest?.Category == "Ready To Issue")
                {
                    string trcond = "";


                    if (!string.IsNullOrWhiteSpace(getBillApprovalRequest?.ReferenceNo))
                    {
                        trcond = " And A.TransactionID=@ReferenceNo";
                        parameters.Add(new SQLParameters("@ReferenceNo", getBillApprovalRequest.ReferenceNo));
                    }

                    query = $"select distinct  CONCAT(IF(DATE_FORMAT(A.Col2,'%d %b, %y') is Not NULL And A.VendorID!=827,CONCAT('App Dt :',DATE_FORMAT(A.Col2,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra4,'%d %b, %y') is Not NULL  And A.BillDate is not null  And A.BillExtra4!=A.BillDate  And A.VendorID!=827,CONCAT('Test Dt :',DATE_FORMAT(A.BillExtra4,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillExtra1,'%d %b, %y') is Not NULL,CONCAT('Exp Dt :',DATE_FORMAT(A.BillExtra1,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.BillDate,'%d %b, %y') is Not NULL,CONCAT('Bill Dt :',DATE_FORMAT(A.BillDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.IssuedOn,'%d %b, %y') is Not NULL,CONCAT('Upl Dt :',DATE_FORMAT(A.IssuedOn,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y') is Not NULL,CONCAT('Dept Dt :',DATE_FORMAT(A.DepartmentApprovalDate,'%d %b, %y'),'$'),''),IF(DATE_FORMAT(A.ApprovedOn,'%d %b, %y') is Not NULL,CONCAT('Pass Dt :',DATE_FORMAT(A.ApprovedOn,'%d %b, %y'),'$'),''),IF(A.Col4 is Not NULL And A.Col4!='---'  And WEEKDAY(A.Col4) is not null,CONCAT('Pub. Dt :',DATE_FORMAT(A.Col4,'%d %b, %y'),'$'),'')) 'INI',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'IssuedName',CONCAT(A.IssuedName,' (',CAST(DATE_FORMAT(A.IssuedOn,'%d %b, %y') as CHAR),')') 'BillNameBy',ForType,A.TransactionID 'TransID','Bills Approval' as 'MyType',CAST('---' as CHAR) as 'SequenceID',RelativePersonName 'Initiated By',if(A.Col3 is NULL or A.Col3='----',CONCAT(FirmName,'#',RelativePersonName),CONCAT(FirmName,'( ',A.Col3,' )','#',RelativePersonName)) 'Firm Name',Remark 'Purpose',AmountRequired 'Santioned',AmountPaid 'Paid',DATE_FORMAT(IssuedOn,'%b %d,%Y') 'On',A.Status,DATE_FORMAT(ADDDATE(AssignedOn,INTERVAL `Limit` DAY),'%m/%d/%Y') 'Till',A.Col2 as 'MyIssue',A.TransactionID,IFNULL(DATE_FORMAT(if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,IFNULL(A.BillDate,A.Col2) ),'%m/%d/%Y'),'') 'MyINICheck',IFNULL(DATE_FORMAT(A.IssuedOn,'%m/%d/%Y'),'') 'MyEntryCheck',(Select DISTINCT IsSpecial from specialvendors M where M.VendorID=A.VendorID) 'IsSpecial',CashDiscount,(A.AmountRequired-A.AmountPaid) 'Bal',if(A.BillDate is not null ,if(A.CashDiscount=0,DATE_FORMAT(ADDDATE(A.BillDate,INTERVAL 45 DAY) ,'%Y-%m-%d'),'R'),'R') 'Cond45','' as 'CheqAmt',(A.AmountRequired+CashDiscount) 'TotBill',A.BillExtra3,A.BillExtra6,REPLACE(A.BillExtra2,'$','\n') 'ShowMeNow',if(A.BillDate is not NULL And A.BillExtra4 is not NULL And A.BillDate!=A.BillExtra4,A.BillExtra4,A.Col2 ) as 'BillExtra4', A.Col5,A.Col1,'' as 'ChequeVendor',A.VendorID as 'BVId',A.RelativePersonID as 'BRPID' from bill_base A, approvals_authority B where A.TransactionID=B.TransactionID {trcond} And B.Type='Bills Approval' {mybillcond}  {showcond} And `Session`=@Session And A.AmountRemaining>0 And A.Status='Ready To Issue Amount' AND FIND_IN_SET(ForType,@ForTypes) order by A.BillExtra4,A.TransactionID LIMIT @LimitItems OFFSET @OffSetItems";

                    parameters.Add(new SQLParameters("@Session", getBillApprovalRequest?.Session ?? string.Empty));
                    parameters.Add(new SQLParameters("@ForTypes", getBillApprovalRequest?.Type ?? string.Empty));
                }

                return await _dbOperations.SelectAsync(query, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalDetails...");
                throw;
            }

        }
        public async Task<DataTable> GetBillApprovalAdvanceBudgetSummary(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_CASE_BUDGET_SUMMARY, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceBudgetSummary...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceExcludeMedBudgetSummary(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_CASE_BUDGET_SUMMARY_EXCLUDE_MED_CASE, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceExcludeMedBudgetSummary...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceBudgetSummaryApprovalDetails(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_CASE_BUDGET_SUMMARY_APPROVAL_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceExcludeMedBudgetSummaryApprovalDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceMedReleaseOrder(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_CASE_BUDGET_SUMMARY_RELEASE_ORDER, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceMedReleaseOrder...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceUserIdentity(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_USER_IDENTITY, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceUserIdentity...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceImprestDetails(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_IMPREST_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceImprestDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceBillApprovalAuthorities(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_APPROVAL_AUTHORITIES, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceBillApprovalAuthorities...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvancePreviousRejections(string type, string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ForType", type),
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_GET_PREVIOUS_REJECTION, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvancePreviousRejections...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceBillBaseExtra7Details(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_GET_BILLBASE_EXTRA7, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceBiliBaseExtra7Details...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceBillApprovalAuthoritiesTimeLimit(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_APPROVAL_AUTHORITIES_TIME_LIMIT, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceBillApprovalAuthoritiesTimeLimit...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceBillTransactionIssueECol6(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_TRANSACTION_ISSUE_ECOL6, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceBillTransactionIssueECol6...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceApprovalAuthoritiesLimit(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_APPROVAL_AUTHORITIES_LIMIT, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceApprovalAuthoritiesLimit...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceBillTransactionIssueChequeECol6(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_TRANSACTION_ISSUE_CHEQUE_ECOL6, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceBillTransactionIssueChequeECol6...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceBillChequeApprovalAuthorities(string referenceNo, string sequenceId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo),
                    new SQLParameters("@SequenceId", sequenceId),
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_CHEQUE_APPROVAL_AUTHORITIES, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceBillChequeApprovalAuthorities...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceBillChequeApprovalAuthoritiesLimit(string referenceNo, string sequenceId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo),
                    new SQLParameters("@SequenceId", sequenceId),
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_CHEQUE_APPROVAL_AUTHORITIES_LIMIT, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceBillChequeApprovalAuthoritiesLimit...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceIsBillLate(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_IS_BILL_LATE_TRANSACTION_ID, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceIsBillLate...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceAllBillDetails(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_ALL_BILL_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceAllBillDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceAllBillDetailsIssue(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_ALL_BILL_DETAILS_ISSUE, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceAllBillDetailsIssue...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceAllBillDetailsIssueAuthoritiesStatus(string referenceNo, string sequenceId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo),
                    new SQLParameters("@SequenceId", sequenceId),

                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_ALL_BILL_DETAILS_ISSUE_AUTHORITIES_STATUS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceAllBillDetailsIssueAuthoritiesStatus...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceAllBillDetailsIssueHostelDistribution(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_ALL_BILL_DETAILS_ISSUE_HOSTEL_DISTRIBUTION, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceAllBillDetailsIssueHostelDistribution...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceAllBillDetailsIssueVehicleDistribution(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_ALL_BILL_DETAILS_ISSUE_VEHICLE_DISTRIBUTION, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceAllBillDetailsIssueVehicleDistribution...");
                throw;
            }
        }
        public async Task<DataTable> GetBillApprovalAdvanceAllBillDetailsIssueVehiclePreviousBills(string referenceNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@ReferenceNo", referenceNo)
                };

                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_APPROVAL_ADVANCE_BILL_ALL_BILL_DETAILS_ISSUE_VEHICLE_PREVIOUS_BILLS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceAllBillDetailsIssueVehiclePreviousBills...");
                throw;
            }
        }


        public async Task<DataTable> GetChequeAuth()
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_CHEQUE_AUTHORITY, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetChequeAuth...");
                throw;
            }
        }
        public async Task<DataTable> PaymentDetails()
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_GET_PAYMENT_DETAILS, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During PaymentDetails...");
                throw;
            }
        }
        public async Task<DataTable> GetTransactionNo(string TransId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@TransId", TransId)
                };
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_TRANSACTION_NO, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetTransactionNo...");
                throw;
            }
        }
        public async Task<int> UpdateBillBase(string EmpCode, string EmpName, string PaidAmount, string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpName", EmpName));
                param.Add(new SQLParameters("@PaidAmount", Convert.ToInt32(PaidAmount)));
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_BILL_IN_BILL_BASE, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateBillBase...");
                throw;
            }
        }
        public async Task<int> InsertBillTransactionIssue(string EmpCode, string EmpName, SaveCheDetailsRequest req)
        {
            try
            {
                //insert into bill_transaction_issue (TransactionID,SequenceID,TaxType,TaxAmount,PaidAmount,IssuedAmount,IssuedType,Mode,TransactionNo,IssuedOn,IssuedBy,IssuedByName,Remark,StoredOn,StoredBy,BillUpto,ExtraCol1,ExtraCol2,ExtraCol3,ExtraCol4,ExtraCol5,ExtraCol8,ExtraCol9,ExtraCol10,ExtraCol7,CVId,CVName,CVSubFirm,CVAddName,CampusCode,CampusName) Values (@TransactionID,@SeqId,'Amount',@TaxAmount,@PaidAmount,@IssuedAmount,@IssuedAmountType,@PaymentMode,@TransactionNo,@IssuedOn,@EmpCode,@EmpName,@Purpose,now(),@EmpName,@myupto,@Other,@TaxOther,@MessageRequired,@MessageDay,@MessageTo,@PaymentAsset,@Col9,@Col10,@PaymentBank,@VendorId,@FirmName,@SubFirm,@AdditionalName,@CampusCode,@CampusName)

                string[] sslt = req.PaymentAccount.Split("#");
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpName", EmpName));
                param.Add(new SQLParameters("@TransactionID", req.TransId ?? ""));
                param.Add(new SQLParameters("@SeqId", req.SequenceId ?? ""));
                param.Add(new SQLParameters("@TaxAmount", req.TaxAmount ?? ""));
                param.Add(new SQLParameters("@PaidAmount", req.PaidAmount ?? ""));
                param.Add(new SQLParameters("@IssuedAmount", req.IssuedAmount ?? ""));
                param.Add(new SQLParameters("@IssuedAmountType", req.PaymentType ?? ""));
                param.Add(new SQLParameters("@PaymentMode", req.PaymentMode ?? ""));
                param.Add(new SQLParameters("@TransactionNo", req.ChequeNo ?? ""));
                param.Add(new SQLParameters("@IssuedOn", req.IssuedDate ?? ""));
                param.Add(new SQLParameters("@IssuedBy", EmpCode));
                param.Add(new SQLParameters("@myupto", req.BillUpto == "" ? null : req.BillUpto));
                param.Add(new SQLParameters("@Other", req.OtherType));
                param.Add(new SQLParameters("@TaxOther", req.OtherAmount ?? ""));
                param.Add(new SQLParameters("@MessageRequired", req.Message ?? ""));
                param.Add(new SQLParameters("@MessageDay", req.RepeatDay ?? ""));
                param.Add(new SQLParameters("@MessageTo", req.MessageTo ?? ""));

                param.Add(new SQLParameters("@PaymentAsset", req.PaymentAsset ?? ""));
                param.Add(new SQLParameters("@Col9", sslt[0]));
                param.Add(new SQLParameters("@Col10", sslt[1]));
                param.Add(new SQLParameters("@PaymentBank", req.PaymentBank ?? ""));
                param.Add(new SQLParameters("@VendorId", req.VendorId ?? ""));
                param.Add(new SQLParameters("@FirmName", req.Firm.Split('[')[0].Trim().Replace("'", "") ?? ""));
                param.Add(new SQLParameters("@SubFirm", req.SubFirm ?? ""));
                param.Add(new SQLParameters("@AdditionalName", req.AdditionalName ?? ""));
                param.Add(new SQLParameters("@CampusCode", req.CampusCode ?? ""));
                param.Add(new SQLParameters("@CampusName", req.CampusName ?? ""));
                //param.Add(new SQLParameters("@IssuedByName", EmpName));
                param.Add(new SQLParameters("@Purpose", req.Purpose ?? ""));
                param.Add(new SQLParameters("@StoredBy", EmpName));

                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.INSERT_TRANSACTION_DETAILS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During InsertBillTransactionIssue...");
                throw;
            }
        }
        public async Task<int> UploadChequeApproval(string EmpCode, string EmpName, SaveCheDetailsRequest req)
        {
            try
            {
                ///insert into approvals_authority (Type,TransactionID,SequenceNo,ApprovalNo,EmployeeID,EmployeeDetails,AssignedOn,`Limit`,`Status`) Values ('Bills Approval -> Cheque Approval',@TransId,@SeqNo,@Count,@AppId,@AppText,now(),(Select `Limit` from mylimits where LimitFor='Bills Approval -> Cheque Approval' And StepNo=1),'Pending')
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpName", EmpName));
                string[] AppIds = req.ApprovalAuthValue?.Split(",");
                string[] AppTexts = req.ApprovalAuthText?.Split(",");
                int count = 1;

                for (int i = 0; i < AppIds.Length; i++)
                {
                    param.Add(new SQLParameters("@TransId", req.TransId ?? ""));
                    param.Add(new SQLParameters("@SeqNo", req.SequenceId ?? ""));
                    param.Add(new SQLParameters("@Count", count));
                    param.Add(new SQLParameters("@AppId", AppIds[i]));
                    param.Add(new SQLParameters("@AppText", AppTexts[i] ?? ""));

                    int ins = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.INS_CHEQUE_AUTH, param, DBConnections.Advance);
                    count++;
                    param.Clear();
                }

                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UploadChequeApproval...");
                throw;
            }
        }
        public async Task<int> SaveFileCheque(string EmpCode, string EmpName, SaveCheDetailsRequest req)
        {
            try
            {
                //insert into bill_transaction_issue_file(TransactionID,SequenceID,FileType,UploadedOn,UploadedBy,UploadedFrom) Values(@TransId,@SeqNo,@DcType,NOW(),@EmpCode,@IpAddress)
                if (req.PdfFile != null && req.PdfFile.Length > 0)
                {
                    List<SQLParameters> param = new List<SQLParameters>();
                    param.Add(new SQLParameters("@TransId", req.TransId ?? ""));
                    param.Add(new SQLParameters("@SeqNo", req.SequenceId ?? ""));
                    param.Add(new SQLParameters("@DcType", "PDF"));
                    param.Add(new SQLParameters("@EmpCode", EmpCode));
                    param.Add(new SQLParameters("@IpAddress", _general.GetIpAddress()));
                    await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.INSERT_CHEQUE_BILL_ISSUE_FILE, param, DBConnections.Advance);
                }
                if (req.ExcelFile != null && req.ExcelFile.Length > 0)
                {
                    List<SQLParameters> param = new List<SQLParameters>();
                    param.Add(new SQLParameters("@TransId", req.TransId ?? ""));
                    param.Add(new SQLParameters("@SeqNo", req.SequenceId ?? ""));
                    param.Add(new SQLParameters("@DcType", "EXCEL"));
                    param.Add(new SQLParameters("@EmpCode", EmpCode));
                    param.Add(new SQLParameters("@IpAddress", _general.GetIpAddress()));
                    await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.INSERT_CHEQUE_BILL_ISSUE_FILE, param, DBConnections.Advance);
                }

                return 1;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During SaveFileCheque...");
                throw;
            }
        }
        public async Task<DataTable> GetOtherApproval(string EmpCode, string AddEmpCode, string TransId, string SeqId)
        {
            try
            {

                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@EmpCode", EmpCode),
                    new SQLParameters("@AddEmpCode", AddEmpCode),
                    new SQLParameters("@TransId", TransId),
                    new SQLParameters("@SeqId", SeqId),
                };
                return await _dbOperations.SelectAsync(AdvanceSql.GET_OTHER_TRANSACTION_FOR_APPROVAL, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetOtherApproval...");
                throw;
            }
        }

        public async Task<int> UpdateApprovalAuthorityWithOutSeqNo(string EmpCode, string EmpAddCode, string EmpName, string TransId, string Designation)
        {
            try
            {
                //update approvals_authority set `Status`='Approved', DoneOn=now(), `Comment`='Approved & Proceed',EmployeeID=@EmpCode,EmployeeDetails=@EmpName,Col2=@Designation where TransactionID=@TransId And Type='Bills Approval' And (EmployeeID=@EmpCode or EmployeeID=@EmpAddCode)
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpName", EmpName));
                param.Add(new SQLParameters("@Designation", Designation));
                param.Add(new SQLParameters("@EmpAddCode", EmpAddCode));
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPADATE_AUTHORITY_WITHOUT_SEQ, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateApprovalAuthorityWithOutSeqNo...");
                throw;
            }
        }
        public async Task<DataTable> GetPendingAuthority(string TransId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@TransId",TransId)
                };
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PENDING_AUTHORITY_DETAILS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetPendingAuthority...");
                throw;
            }
        }
        public async Task<int> UpdateBillStatus(string EmpName, string Reason, string TransId)
        {
            string Remark = "";
            if (!string.IsNullOrEmpty(Reason))
            {
                Remark = " ,BillExtra7=CONCAT(IF(BillExtra7 is NULL,'',CONCAT(BillExtra7,'@')),'Approved By : " + EmpName + "$Reason : " + Reason.Trim().ToUpper() + "')";
            }
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_BILL_STATUS.Replace("@Condition", Remark), param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateBillStatus...");
                throw;
            }
        }
        public async Task<int> UpdateBillReason(string EmpName, string Reason, string TransId)
        {
            string Remark = "";
            if (!string.IsNullOrEmpty(Reason))
            {
                Remark = " ,BillExtra7=CONCAT(IF(BillExtra7 is NULL,'',CONCAT(BillExtra7,'@')),'Approved By : " + EmpName + "$Reason : " + Reason.Trim().ToUpper() + "')";
            }
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_BILL_BASE_REMARK.Replace("@Condition",Remark), param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateBillStatus...");
                throw;
            }
        }
        public async Task<int> ApprovalAuthWithSeqNo(string EmpCode, string EmpAddCode, string EmpName, string Designation, string TransId, string SeqNo)
        {
            try
            {
                //update approvals_authority set `Status`='Approved', DoneOn=now(), `Comment`='Approved & Proceed',EmployeeID=@EmpCode,EmployeeDetails=@EmpName,Col2=@Designation where TransactionID=@TransId And SequenceNo=@SeqNo And Type='Bills Approval -> Cheque Approval' And (EmployeeID=@EmpCode or EmployeeID=@EmpAddCode)
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpName", EmpName));
                param.Add(new SQLParameters("@Designation", Designation));
                param.Add(new SQLParameters("@TransId", TransId));
                param.Add(new SQLParameters("@SeqNo", SeqNo));
                param.Add(new SQLParameters("@EmpAddCode", EmpAddCode));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_AUTHORITY_WITH_SEQ, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During ApprovalAuthWithSeqNo...");
                throw;
            }
        }
        public async Task<DataTable> GetCequePendingAuthority(string TransId, string SeqNo)
        {
            //select * from approvals_authority where TransactionID=@TransId And SequenceNo=@SeqNo  And Type='Bills Approval -> Cheque Approval' And `Status` in ('Pending')
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@TransId",TransId),
                    new SQLParameters("@SeqNo",SeqNo)
                };
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_CHEQUE_STATUS, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetCequePendingAuthority...");
                throw;
            }
        }
        public async Task<DataTable> GetSmsDetails(string TransId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@TransId",TransId)
                };
                return await _dbOperations.SelectAsync(AdvanceSql.GET_SMS_CAT, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetSmsDetails...");
                throw;
            }
        }
        public async Task<int> UpdateTransactionIssued(string Sms, string TransId, string SeqNo, string Remark, string EmpName)
        {
            try
            {
                //update bill_transaction_issue set SignedBy=@Sms,SignedOn=now(),BillUpto=if(BillUpto is NULL,DATE_ADD(now(),INTERVAL (Select Distinct `Limit` from mylimits where LimitFor='Bill Upto') DAY),BillUpto) @Condition where TransactionID=@TransId And SequenceID=@SeqId"
                string Cond = "";
                if (!string.IsNullOrEmpty(Remark))
                {
                    Cond = ",ExtraCol6=CONCAT(IF(ExtraCol6 is NULL,'',CONCAT(ExtraCol6,'@')),'Approved By : " + EmpName + "$Reason : " + Remark.Trim().ToUpper() + "')";
                }

                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@Sms", Sms));
                param.Add(new SQLParameters("@TransId", TransId));
                param.Add(new SQLParameters("@SeqId", SeqNo));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_BILL_TRANSACTION_ISSUE_CHEQUE_DETAILS.Replace("@Condition", Cond), param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateTransactionIssued...");
                throw;
            }
        }
        public async Task<int> UpdateScheduled(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.GET_SCHEDULED_BILLS, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateScheduled...");
                throw;
            }
        }
        public async Task<int> UpdateBillTransactionIssuedTblAllAuthApproved(string EmpName, string Reason, string TransId, string SeqId)
        {
            try
            {
                string str = "update bill_transaction_issue set ExtraCol6=CONCAT(IF(ExtraCol6 is NULL,'',CONCAT(ExtraCol6,'@')),'Approved By : " + EmpName + "$Reason : " + Reason.Trim().ToUpper() + "') where TransactionID=@TransId And SequenceID=@SeqId";

                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                param.Add(new SQLParameters("@SeqId", SeqId));
                return await _dbOperations.DeleteInsertUpdateAsync(str, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateBillTransactionIssuedTblAllAuthApproved...");
                throw;
            }
        }
        public async Task<DataTable> GetPendingRejectedRecord(string TransId)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@TransId", TransId),
                };
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PENDING_REJECTED_RECORD, parameters, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetBillApprovalAdvanceBillTransactionIssueChequeAllAuthApproved...");
                throw;
            }

        }
        public async Task<int> UpdateREadyToIssueAmount(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_READY_TO_ISSUE_AMOUNT, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateREadyToIssueAmount...");
                throw;
            }
        }
        public async Task<int> ApproveApplication(string EmpCode, string TransId)
        {
            try
            {
                string Query = AdvanceSql.GET_BILL_APPLICATION_REPORT;
                List<SQLParameters> parameters = new List<SQLParameters>
                {
                    new SQLParameters("@BillId", TransId),
                    new SQLParameters("@EmpCode", EmpCode),
                };
                DataTable Main = await _dbOperations.SelectAsync(Query, parameters, DBConnections.Advance);
                if (Main.Rows.Count > 0)
                {
                    string qry = "";
                    for (int i = 0; i < Main.Rows.Count; i++)
                    {
                        if (Main.Rows[i]["VerifiedByID"].ToString() == EmpCode && Main.Rows[i]["VerifiedStatus"].ToString() == "Pending")
                        {
                            qry = qry + "update advances.bill_applications_new set VerifiedStatus='Approved',VerifiedOn=now(),VerifiedFrom='" + _general.GetIpAddress() + "' where Id=" + Main.Rows[i]["Id"].ToString() + ";";
                        }
                        if (Main.Rows[i]["ApprovedByID"].ToString() == EmpCode && Main.Rows[i]["ApprovedStatus"].ToString() == "Pending")
                        {
                            qry = qry + "update advances.bill_applications_new set ApprovedStatus='Approved',ApprovedOn=now(),ApprovedFrom='" + _general.GetIpAddress() + "' where Id=" + Main.Rows[i]["Id"].ToString() + ";";
                        }
                    }
                    qry = qry + "update advances.bill_applications_new set `Status`='Approved' where BillId=" + TransId + " And `Status`='Pending' And VerifiedStatus='Approved' And ApprovedStatus='Approved'";
                    //MyConnections.DeleteInsertUpdate(qry, "Advance");
                    int ins = await _dbOperations.DeleteInsertUpdateAsync(qry, parameters, DBConnections.Advance);
                }
                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During ApproveApplication...");
                throw;
            }
        }

        public async Task<int> UpdateRejectRecord(string EmpCode, string EmpName, string EmpAddCode, string Designation, string TransId)
        {

            List<SQLParameters> param = new List<SQLParameters>();
            param.Add(new SQLParameters("@EmpCode", EmpCode));
            param.Add(new SQLParameters("@EmpAddCode", EmpAddCode));
            param.Add(new SQLParameters("@EmpName", EmpName));
            param.Add(new SQLParameters("@TransId", TransId));
            param.Add(new SQLParameters("@Designation", Designation));
            try
            {
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_APPROVAL_AUTHORITY_REJECT, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateRejectRecord...");
                throw;
            }

        }
        public async Task<int> UpdateBillBaseReject(string TransId, string Reason, string EmpName)
        {
            try
            {
                string Req = "update bill_base set `Status`='Bill Rejected',BillExtra7='Rejected By : " + EmpName + " ( on " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss tt") + ")$Reason : " + Reason.Trim().ToUpper() + "' where TransactionID=@TransId";
                List<SQLParameters> param = new List<SQLParameters>
                {
                    new SQLParameters("@TransId",TransId)
                };
                return await _dbOperations.DeleteInsertUpdateAsync(Req, param, DBConnections.Advance);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateBillBaseReject...");
                throw;
            }
        }
        public async Task<int> UpdateAppAuthWithSeq(string EmpCode, string EmpAddCode, string Designation, string EmpName, string TransId, string SeqNo)
        {
            //update approvals_authority set `Status`='Rejected', DoneOn=now(), `Comment`='Rejected & Cant Proceed',EmployeeID=@EmpCode,EmployeeDetails=@EmpName,Col2=@Designation where TransactionID=@TransId  And SequenceNo=@SeqNo And Type='Bills Approval -> Cheque Approval' And  (EmployeeID=@EmpCode or EmployeeID=@EmpAddCode)
            try
            {
                List<SQLParameters> param = new List<SQLParameters>()
                {
                    new SQLParameters("@EmpCode",EmpCode),
                    new SQLParameters("@EmpAddCode",EmpAddCode),
                    new SQLParameters("@TransId",TransId),
                    new SQLParameters("@Designation",Designation),
                    new SQLParameters("@SeqNo",SeqNo)
                };
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_APPROVAL_AUTHORITY_REJECT_SEQ_NO, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateAppAuthWithSeq...");
                throw;
            }
        }
        public async Task<int> UpdateBillTransactionIssueReject(string EmpCode, string TransId, string SeqNo, string Reason, string EmpName)
        {
            try
            {
                //update bill_transaction_issue set Col3='Rejected',ExtraCol6='Rejected By : @EmpName ( on " + DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss tt") + ")$Reason : @Reason' where TransactionID=@TransId  And SequenceID=@SeqId
                List<SQLParameters> param = new List<SQLParameters>
                {
                    new SQLParameters("@SeqId",SeqNo),
                    new SQLParameters("@TransId",TransId),
                };
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.REJECT_BILL_TRANSACTION_ISSUE.Replace("@On", DateTime.Now.ToString("dd.MM.yyyy hh: mm:ss tt")).Replace("@Reason", Reason).Replace("@EmpName", EmpName), param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateBillTransactionIssueReject...");
                throw;
            }
        }

        public async Task<int> UpdateBillRejectStatus(string TransId)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                DataTable dt = await _dbOperations.SelectAsync(AdvanceSql.GET_REJECT_AUTH, param, DBConnections.Advance);
                if (dt.Rows.Count <= 0)
                {
                    int Update = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_READY_TO_ISSUE_AMOUNT, param, DBConnections.Advance);
                }
                return 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateBillRejectStatus...");
                throw;
            }
        }
        public async Task<int> BillAmountUpdate(string TransId, string SeqNo, string EmpName)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>
                {
                    new SQLParameters("@TransId",TransId),
                    new SQLParameters("@SeqId",SeqNo),
                    new SQLParameters("@EmpName",EmpName)
                };
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_BILL_BASE_AMOUNT_REJECTION_CASE, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During BillAmountUpdate...");
                throw;
            }
        }

        public async Task<int> InsertInBackUpDeleteCheque(string EmpName, string TransId, string SeqNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpName", EmpName));
                param.Add(new SQLParameters("@TransId", TransId));
                param.Add(new SQLParameters("@SeqNo", SeqNo));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.TRANSACTION_BILL_TRANSACTION_BACKUP, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During InsertInBackUpDeleteCheque...");
                throw;
            }
        }
        public async Task<int> DeleteChequeTransaction(string TransId, string SeqNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@TransId", TransId));
                param.Add(new SQLParameters("@SeqNo", SeqNo));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.DELETE_FROM_BILL_TRANSACTION_ISSUE, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During DeleteChequeTransaction...");
                throw;
            }

        }
        public async Task<int> UpdateAmount(string EmpName, string TransId, string SeqNo)
        {
            try
            {
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpName", EmpName));
                param.Add(new SQLParameters("@TransNo", TransId));
                param.Add(new SQLParameters("@Seqno", SeqNo));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.UPDATE_BILL_BASE_Before_CHEQUE_DELETE, param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During UpdateAmount...");
                throw;
            }
        }

        public async Task<DataTable> Purchasetime(string RefNo)
        {
            List<SQLParameters> parameters = new List<SQLParameters>
            {
                new SQLParameters("@RefNo", RefNo)
            };
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PURCHASE_TIMELINE, parameters, DBConnections.Advance);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error During Purchasetime...");
                throw;
            }
        }
        public async Task<DataTable> AdvanceSummary(string RefNo, string Type)
        {
            List<SQLParameters> parameters = new List<SQLParameters>
            {
                new SQLParameters("@RefNo", RefNo)
            };
            try
            {
                string str= "";
                if (Type != "Advance")
                {
                    str = "SELECT ReferenceNo,MyType,BillId,App1DoneOn,App2DoneOn,App3DoneOn,App4DoneOn,Amount,Status,App1ID,App2ID,App3ID,App4ID FROM otherapprovalsummary WHERE SUBSTRING_INDEX(PExtra4,'#',1)=@RefNo and `Status`!='Rejected' AND `Status`!='Deleted'";
                }
                else
                    str = "SELECT ReferenceNo,MyType,BillId,App1DoneOn,App2DoneOn,App3DoneOn,App4DoneOn,Amount,Status,App1ID,App2ID,App3ID,App4ID FROM otherapprovalsummary WHERE ReferenceNo=@RefNo and `Status`!='Rejected' AND `Status`!='Deleted'";
                
                return await _dbOperations.SelectAsync(str, parameters, DBConnections.Advance);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error During Purchasetime...");
                throw;
            }
        }
        public async Task<DataTable> BillSummary(string Ids)
        {
          
            try
            {
                string str = "SELECT (AmountRequired) 'Amount',(TransactionID) 'TransId',(AmountPaid) 'IssuedAmount',Status FROM bill_base WHERE FIND_IN_SET(TransactionID,'" + Ids + "') AND `Status`!='Bill Rejected';\r\n";
                return await _dbOperations.SelectAsync(str,  DBConnections.Advance);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error During Purchasetime...");
                throw;
            }
        }
        public async Task<DataTable> ChequeDetails(string Ids)
        {
            try
            {
                string query = "SELECT GROUP_CONCAT(TransactionID) 'TransId',SUM(PaidAmount) 'Amount',COUNT(TransactionID) 'Count' FROM bill_transaction_issue WHERE FIND_IN_SET(TransactionID,'" + Ids + "');\r\n";
                return await _dbOperations.SelectAsync(query, DBConnections.Advance);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error During ChequeDetails...");
                throw;
            }
        }
        public async Task<DataTable> BillDetaild(string Ids)
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BILL_DETAILS.Replace("@Ids", Ids), DBConnections.Advance);
                
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error During BillDetaild...");
                throw;
            }
        }
        public async Task<DataTable> GetAuthority(string strId)
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_APPROVAL_AUTHORITY_BILL.Replace("@TransId", strId), DBConnections.Advance);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error During GetAuthority...");
                throw;
            }
        }
        public async Task<DataTable> GetCheque(string TandId)
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_CHEQUE_DETAILS.Replace("@Ids", TandId), DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error During GetCheque...");
                throw;
            }
        }
        public async Task<DataTable> GetChequeAuthority(string strId)
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_APPROVAL_AUTHORITY_CHEQUE_BILL.Replace("@TransId", strId), DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error During GetChequeAuthority...");
                throw;
            }
        }
    }

}
