using AdvanceAPI.DTO.Advance;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.SQLConstants.Advance;
using AdvanceAPI.SQLConstants.Approval;
using Mysqlx.Crud;
using MySqlX.XDevAPI;
using NuGet.Protocol.Plugins;
using Org.BouncyCastle.Ocsp;
using System.Data;
using System.Reflection.Emit;
using System.Security.Cryptography.Xml;

namespace AdvanceAPI.Repository
{
    public class AdvanceRepository: IAdvanceRepository
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
                sqlParameters.Add(new SQLParameters("@RefNo",RefNo));
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
        public async Task<DataTable> getVendorDetails(string VendorId,string Office)
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

        public async Task<DataTable> GetBackValue()
        {
            try
            {
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PURCHASE_APPROVAL_BACK_VALUE, DBConnections.Advance);
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
                sqlParameters.Add(new SQLParameters("RefNo",RefNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PURCHASE_APPROVAL_DETAILS_PREVIOUS_VALUE, sqlParameters, DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error During GetPurchaseApprovalPValues...");
                throw;
            }
        }
        public async Task<int> SaveAdvance(GenerateAdvancerequest req, string EmpCode, string EmpName, string RefNo,string FirmName,string FirmPerson,string FirmEmail,string FirmPanNo,string FirmAddress,string FirmContactNo,string FirmAlternateContactNo,string BackValue,string PurchaseApprovalDetails)
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

            List<SQLParameters> param=new List<SQLParameters>();
            param.Add(new SQLParameters("@ReferenceNo",RefNo));
            param.Add(new SQLParameters("@ForDepartment", req.ForDepartment??string.Empty));
            param.Add(new SQLParameters("@Session", req.Session ?? string.Empty));
            param.Add(new SQLParameters("@MyType", req.ApprovalType??string.Empty));
            param.Add(new SQLParameters("@Note", req.Note??string.Empty));
            param.Add(new SQLParameters("@Purpose", req.Purpose??string.Empty));
            param.Add(new SQLParameters("@BillUptoValue", req.BillUptoValue??string.Empty));
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
            param.Add(new SQLParameters("@TotalAmount", req.TotalAmount??string.Empty));
            param.Add(new SQLParameters("@Status", "Pending"));
            param.Add(new SQLParameters("@App1ID", req.App1ID??string.Empty));
            param.Add(new SQLParameters("@App1Name", req.App1Name??string.Empty));
            param.Add(new SQLParameters("@App1Designation", req.App1Designation??string.Empty));
            param.Add(new SQLParameters("@App1Status", "Pending"));
            param.Add(new SQLParameters("@App1DoneOn", null));
            param.Add(new SQLParameters("@App2ID", req.App2ID??string.Empty));
            param.Add(new SQLParameters("@App2Name", req.App2Name??string.Empty));
            param.Add(new SQLParameters("@App2Designation", req.App2Designation??string.Empty));
            param.Add(new SQLParameters("@App2Status", "Pending"));
            param.Add(new SQLParameters("@App2DoneOn", null));
            param.Add(new SQLParameters("@App3ID", req.App3ID??string.Empty));
            param.Add(new SQLParameters("@App3Name", req.App3Name??string.Empty));
            param.Add(new SQLParameters("@App3Designation", req.App3Designation??string.Empty));
            param.Add(new SQLParameters("@App3Status", "Pending"));
            param.Add(new SQLParameters("@App3DoneOn", null));
            param.Add(new SQLParameters("@App4ID", req.App4ID??string.Empty));
            param.Add(new SQLParameters("@App4Name", req.App4Name??string.Empty));
            param.Add(new SQLParameters("@App4Designation", req.App4Designation??string.Empty));
            param.Add(new SQLParameters("@App4Status", "Pending"));
            param.Add(new SQLParameters("@App4DoneOn", null));
            param.Add(new SQLParameters("@AppCat", req.Category??string.Empty));
            param.Add(new SQLParameters("@MessageSend", "Pending"));
            param.Add(new SQLParameters("@VendorID", req.VendorID??string.Empty));
            param.Add(new SQLParameters("@RelativePersonName", req.RelativePersonName??string.Empty));
            param.Add(new SQLParameters("@RelativePersonID", req.RelativePersonID??string.Empty));
            param.Add(new SQLParameters("@RelativeDesignation", req.RelativeDesignation??string.Empty));
            param.Add(new SQLParameters("@RelativeDepartment", req.RelativeDepartment ?? string.Empty));
            param.Add(new SQLParameters("@AppDate", req.AppDate ?? string.Empty));
            param.Add(new SQLParameters("@ReferenceBillStatus", "Open"));
            param.Add(new SQLParameters("@BillTill", req.BillTill??string.Empty));
            param.Add(new SQLParameters("@ExtendedBillDate", req.ExtendedBillDate??string.Empty));
            param.Add(new SQLParameters("@R_Pending", req.Amount??string.Empty));
            param.Add(new SQLParameters("@R_Status", "Pending"));
            param.Add(new SQLParameters("@PExtra3", req.Office??string.Empty));
            param.Add(new SQLParameters("@PExtra2", BackValue.Split("#")[0]));
            param.Add(new SQLParameters("@PExtra4", PurchaseApprovalDetails));
            param.Add(new SQLParameters("@BudgetRequired", req.BudgetRequired??string.Empty));
            param.Add(new SQLParameters("@BudgetAmount", req.BudgetAmount??string.Empty));
            param.Add(new SQLParameters("@PreviousTaken", req.PreviousTaken??string.Empty));
            param.Add(new SQLParameters("@CurStatus", req.CurStatus??string.Empty));
            param.Add(new SQLParameters("@BudgetStatus", req.BudgetStatus??string.Empty));
            param.Add(new SQLParameters("@BudgetReferenceNo", req.BudgetReferenceNo??string.Empty));
            param.Add(new SQLParameters("@AdvFrom", req.DistFrom??string.Empty));
            param.Add(new SQLParameters("@AdvTo", req.DistTo??string.Empty));
            param.Add(new SQLParameters("@CampusCode", req.CampusCode??string.Empty));
            param.Add(new SQLParameters("@CampusName", _general.CampusNameByCode(req.CampusCode!)));
            try
            {
                int ins = await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.INSERT_ADVANCE_DETAILS, param, DBConnections.Advance);
                //GET_UPDATE_ADVANCETOKEN
                param.Clear();
                param.Add(new SQLParameters("@EmpCode",EmpCode));
                param.Add(new SQLParameters("@VendorId",req.VendorID??string.Empty));
                param.Add(new SQLParameters("@IniBy",req.App1ID??string.Empty));
                param.Add(new SQLParameters("@VendorIdOffice",req.VendorID+"#"+req.Office??string.Empty));
                int iin=await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.GET_UPDATE_ADVANCETOKEN, param, DBConnections.Advance);
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
            if(CampusCode=="101")
            {
                string Query = AdvanceSql.GET_FINAL_APPROVAL_AUTH + " and CampusCode=@CampusCode ORDER BY SubType,MyOrder";
                List<SQLParameters> param = new List<SQLParameters>();
                param.Add(new SQLParameters("@CampusCode", CampusCode));
                return await _dbOperations.SelectAsync(Query,param,DBConnections.Advance);
            }
            else
            {
                string Query = AdvanceSql.GET_FINAL_APPROVAL_AUTH + " AND Employee_Code=(SELECT `Value` FROM `othervalues` WHERE Type='Online And Noida Campus Member 4 Authority') GROUP BY Employee_Code  ORDER BY SubType,MyOrder";
                return await _dbOperations.SelectAsync(Query,DBConnections.Advance);
            }
        }

        public async Task<DataTable> GetMyAdvanceDetails(string Status,string Session,string CampusCode,string RefNo,string Type,string EmpCode,string Department,string itemsFrom,string noOfItems)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                


                string Con = "";
                if(!string.IsNullOrEmpty(RefNo))
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
               
                if (Type.ToUpper()== "STORE")
                {
                    Con += "  And (IniId = '" + EmpCode + "' or RelativePersonID='" + EmpCode + "')";
                }
                
                return await _dbOperations.SelectAsync(AdvanceSql.GET_MY_ADVANCE_DETAILS.Replace("@Condition", Con).Replace("@limit",noOfItems).Replace("@offset",itemsFrom), parameters, DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error During GetMyAdvanceDetails...");
                throw;
            }
           // return new DataTable();
        }

        public async Task<int> DeleteAdvance(string RefNo,string EmpCode)
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

        public async Task<DataTable> GetPassApprovalDetails(string EmpCode,string EmpCodeAdd,string Type, GetMyAdvanceRequest req)
        {
            try
            {
                List<SQLParameters> param= new List<SQLParameters>();
                string Cond = "  And `Session`=@Session And ( App1ID=@EmpCode || App2ID=@EmpCode  || App4ID=@EmpCode  || App1ID=@EmpCodeAdd || App2ID=@EmpCodeAdd  || App4ID=@EmpCodeAdd   || (App3ID is not NULL && (App3ID=@EmpCodeAdd  || App3ID=@EmpCodeAdd)) )";
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                param.Add(new SQLParameters("@EmpCodeAdd", EmpCodeAdd));
                param.Add(new SQLParameters("@Session", req.Session!));

                if (!string.IsNullOrEmpty(req.ReferenceNo))
                {
                    Cond += "  And ReferenceNo=@RefNo";
                    param.Add(new SQLParameters("@RefNo",req.ReferenceNo));
                }
                else
                {
                    //if(!string.IsNullOrEmpty(req.Session))
                    //{
                    //    Cond += " And `Session`=@Session";
                    //    param.Add(new SQLParameters("@Session", req.Session));
                    //}
                    if(!string.IsNullOrEmpty(req.CampusCode))
                    {
                        Cond += " AND CampusCode=@CampusCode";
                        param.Add(new SQLParameters("@CampusCode", req.CampusCode));
                    }
                    if(!string.IsNullOrEmpty(req.Department))
                    {
                        Cond += " And ForDepartment=@Dept";
                        param.Add(new SQLParameters("@Dept", req.Department));
                    }
                    switch(req.Status)
                    {
                        case "Pending My":
                            Cond += "  And `Status`='Pending' And ( (App1ID=@EmpCode And App1Status='Pending') || (App2ID=@EmpCode And App2Status='Pending')  || (App3ID=@EmpCode And App3Status='Pending')  || (App4ID=@EmpCode And App4Status='Pending') || (App1ID=@EmpCodeAdd And App1Status='Pending') || (App2ID=@EmpCodeAdd And App2Status='Pending')  || (App3ID=@EmpCodeAdd And App3Status='Pending')  || (App4ID=@EmpCodeAdd And App4Status='Pending') )";
                           
                            break;
                        case "Pending All":
                            Cond += "  And `Status`='Pending'";
                            
                            break;
                        default:
                            Cond += " And `Status`=@Status";
                            param.Add(new SQLParameters("@Status", req.Status??string.Empty));
                            break;
                    }
                }

                return await _dbOperations.SelectAsync(AdvanceSql.GET_ADVANCE_APPROVAL_DETAILS.Replace("@Condition",Cond).Replace("@limit",req.NoOfItems.ToString()).Replace("@offset",req.ItemsFrom.ToString()),param,DBConnections.Advance);

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
        public async Task<int> InsertBillAuthority(string BillTransId,string RefNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@BillTransectionNo", BillTransId));
                parameters.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.INSERT_AUTHORITY,parameters,DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error During InsertBillAuthority...");
                throw;
            }
        }
        public async Task<int> UpdateBillStatus(string BillTransId,string RefNo)
        {
            try
            {
                List<SQLParameters> parameters = new List<SQLParameters>();
                parameters.Add(new SQLParameters("@BillTransId", BillTransId));
                parameters.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.DeleteInsertUpdateAsync(AdvanceSql.CLOSE_BILL_OTHER_APPROVAL, parameters,DBConnections.Advance);
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
                List<SQLParameters> param= new List<SQLParameters>();
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
                List<SQLParameters> param= new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo", RefNo));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PURCHASE_DETAILS_FOR_GENERATE_BILL,param,DBConnections.Advance);
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
                List<SQLParameters> param=new List<SQLParameters>();
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
            DataTable dtn = await _dbOperations.SelectAsync(AdvanceSql.CHECK_WAR_IN_QUERY,param,DBConnections.Advance);
            if (dtn.Rows.Count <= 0)
            {
                return "";
            }
            //else
            {
                dtn = await _dbOperations.SelectAsync(AdvanceSql.CHECK_WAR_STATUS, param, DBConnections.Advance);
                if(dtn.Rows.Count <= 0)
                {
                    return "";
                }
                else
                {
                    if(dtn.Rows[0][0].ToString() == "0")
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

        public async Task <DataTable> CanGenerateBill(string Type,string EmpCode,bool IsThousand,string RefNo="")
        {
            try
            {
                string Cond = "";
                List<SQLParameters> param= new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                if(Type.ToUpper()=="STORE")
                {
                    Cond += " And (IniId = @EmpCode Or RelativePersonID=@EmpCode)";
                }
                if(IsThousand)
                {
                    Cond += "  And BillRequired='No' And ReferenceBillStatus ='Closed' And BillId is null";
                }
                else
                {
                    Cond += " And ReferenceBillStatus ='Open'";
                }

                DataTable LockMenuDetails = await LockMenu(EmpCode);
                string ids = "";
                if(RefNo!="")
                {
                    ids= ids == "" ? RefNo : (ids + "," + RefNo);
                }
                if (ids.Length > 0)
                {
                    Cond += " And ReferenceNo in (" + ids + ")";
                }

                return await _dbOperations.SelectAsync(AdvanceSql.GET_DETAILS_FOR_CAN_BILL_UPLOAD.Replace("@Condition",Cond), param, DBConnections.Advance);

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
                return await _dbOperations.SelectAsync(AdvanceSql.GETLOCKMENU, parameters,DBConnections.Advance);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Error During LockMenu...",ex);
                throw;
            }

        }

        public async Task<DataTable> LoadApprovalDetails(string Type,string EmpCode)
        {
            try
            {
                string Cond = "";
                List<SQLParameters> param= new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode",EmpCode));
                if(Type.ToUpper()=="STORE")
                {
                    Cond += "  And (IniId = @EmpCode Or RelativePersonID=@EmpCode";
                }
                return await _dbOperations.SelectAsync(AdvanceSql.GET_APPROVAL_REFNO.Replace("@EmpCode",EmpCode),param,DBConnections.Advance);
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
                List<SQLParameters> param= new List<SQLParameters>();
                param.Add(new SQLParameters("@RefNo",Id));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_BASIC_DETAILS_ADVANCE,param,DBConnections.Advance);
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
                List<SQLParameters> param= new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode",EmpCode));
                return await _dbOperations.SelectAsync(AdvanceSql.GET_CONTACT_NO,param,DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetContactNo...", ex);
                throw;
            }
        }

        public async Task<DataTable> GetVisitBasicDetails(string Type,string EmpCode,string Prefix)
        {
            try
            {
                string str = "";
                List<SQLParameters> param= new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                if(Type.ToUpper()=="STORE")
                {
                    str += "  And (IniId = @EmpCode Or RelativePersonID=@EmpCode)";
                }
                return await _dbOperations.SelectAsync(AdvanceSql.LOAD_VISIT.Replace("@Condition", str).Replace("@Prefix","'"+ Prefix+"'"),param,DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetVisitBasicDetails...", ex);
                throw;
            }
        }
        public async Task<DataTable> GetPartnerVisit(string Type,string EmpCode,string Prefix)
        {
            try
            {
                string str = "";
                List<SQLParameters> param= new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                if(Type.ToUpper()=="STORE")
                {
                    str += "  And (IniId = @EmpCode Or RelativePersonID=@EmpCode)";
                }
                return await _dbOperations.SelectAsync(AdvanceSql.GET_PARTNER_VISIT.Replace("@Condition", str).Replace("@Prefix","'"+ Prefix+"'"),param,DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetVisitBasicDetails...", ex);
                throw;
            }
        }
        public async Task<DataTable> GetSchoolVisit(string Type,string EmpCode,string Prefix)
        {
            try
            {
                string str = "";
                List<SQLParameters> param= new List<SQLParameters>();
                param.Add(new SQLParameters("@EmpCode", EmpCode));
                if(Type.ToUpper()=="STORE")
                {
                    str += "  And (IniId = @EmpCode Or RelativePersonID=@EmpCode)";
                }
                return await _dbOperations.SelectAsync(AdvanceSql.GET_SCHOOL_VISIT.Replace("@Condition", str).Replace("@Prefix","'"+ Prefix+"'"),param,DBConnections.Advance);
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
                List<SQLParameters> param= new List<SQLParameters>();
                param.Add(new SQLParameters("@VendorId", VendorId));
                
                return await _dbOperations.SelectAsync(AdvanceSql.GET_FIRM_DETAILS,param,DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetVisitBasicDetails...", ex);
                throw;
            }
        }
        public async Task<DataTable> LoadOffices(string RefNo,string VendorId)
        {
            try
            {
                
                string Cond = "";
                List<SQLParameters> param= new List<SQLParameters>();
               
                param.Add(new SQLParameters("@VendorId", VendorId));
                if(RefNo.Contains("#"))
                {
                    string[] splt = RefNo.Split('#');
                    param.Add(new SQLParameters("@RefNo", splt[0]));
                    DataTable dt = await _dbOperations.SelectAsync(AdvanceSql.GET_PURCHASE_OFFICE, param, DBConnections.Advance);
                    if (dt.Rows.Count > 0)
                    {
                        Cond = " And Offices='" + dt.Rows[0]["PExtra3"].ToString() + "'";
                    }
                }
                
                return await _dbOperations.SelectAsync(AdvanceSql.LOAD_VENDOR_OFFICE.Replace("@Condition",Cond), param, DBConnections.Advance);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error During GetVisitBasicDetails...", ex);
                throw;
            }
        }

        public async Task<DataTable> GetDepartDetails(string Values,string Type)
        {
            try
            {
                string[] splt = Values.Split("#");
                if (Type.Equals("Against Purchase Approval"))
                {
                    List<SQLParameters> param = new List<SQLParameters>();
                    param.Add(new SQLParameters("@RefNo", splt[0]));
                    return await _dbOperations.SelectAsync(AdvanceSql.AGAINST_PURCHASE_APPROVAL,param,DBConnections.Advance);
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

    }
}
