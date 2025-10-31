namespace AdvanceAPI.SQLConstants.Approval;

public class ApprovalSql
{
    public const string GET_ITEM_REFERENCE_NO = "SELECT DISTINCT ReferenceNo FROM `purchaseapprovaldetail_draft` WHERE IniId =@EmpCode AND AppType=@AppType;";
    public const string CHECK_ALREADY_ITEM = "SELECT ItemCode FROM `purchaseapprovaldetail_draft` WHERE IniId =@EmpCode AND AppType=@AppType and ItemCode=@ItemCode;";

    public const string GET_AUTO_ITEM_REFNO =
        "Select IFNULL(CAST(MAX(ReferenceNo)+1 as CHAR),CONCAT(CAST(DATE_FORMAT(now(),'%y%m%d') as CHAR),'0001')) 'ReferenceNo' from (Select ReferenceNo from purchaseapprovaldetail_draft) A  where ReferenceNo like CONCAT(CAST(DATE_FORMAT(now(),'%y%m%d') as CHAR),'%');";

    public const string INERT_DRAFT =
        "insert into  purchaseapprovaldetail_draft (ReferenceNo,AppType,ItemCode,ItemName,Make,Size,Unit,Balance,Quantity,PrevRate,CurRate,ChangeReason,TotalAmount,IniOn,IniId,Status,WarIn,WarType,ActualAmount,VatPer,R_Total,R_Pending,R_Status,SerialNo,DisPer,CampusCode)\n values (@ReferenceNo,@AppType,@ItemCode,@ItemName,@Make,@Size,@Unit,@Balance,@Quantity,@PrevRate,@CurRate,@ChangeReason,@TotalAmount,NOW(),@IniId,'Pending',@WarIn,@WarType,@ActualAmount,@VatPer,@R_Total,@R_Pending,'Pending',@SerialNo,@DisPer,@CampusCode);";

    public const string GET_DRAFTED_ITEM =
        "select id,ReferenceNo,AppType,ItemCode,ItemName,Make,Size,Unit,Balance,Quantity,PrevRate,CurRate,ChangeReason,TotalAmount,DATE_FORMAT(IniOn,'%d.%m.%Y') 'IniOn',IniId,Status,WarIn,WarType,ActualAmount,VatPer,R_Total,R_Pending,R_Status,SerialNo,DisPer,DATE_FORMAT(IniOn,'%d.%m.%Y')'InitOn',CampusCode from purchaseapprovaldetail_draft where IniId=@EmpCode AND CampusCode=@CampusCode AND AppType=@ApprovalType";

    public const string Get_Draft_Summary =
        "SELECT ReferenceNo,CampusCode,AppType,COUNT(*)'Total',SUM(TotalAmount)'Amt' FROM `purchaseapprovaldetail_draft` WHERE IniId=@EmpCode AND CampusCode=@CampusCode and AppType=@AppType GROUP BY AppType;";
    public const string Generate_RefNo_Purchaseapprovalsummary = "Select IFNULL(CAST(MAX(ReferenceNo)+1 as CHAR),CONCAT(CAST(DATE_FORMAT(now(),'%y%m%d') as CHAR),'0001')) 'ReferenceNo' from (Select ReferenceNo from purchaseapprovalsummary UNION Select ReferenceNo from otherapprovalsummary) A  where ReferenceNo like CONCAT(CAST(DATE_FORMAT(now(),'%y%m%d') as CHAR),'%')";

    public const string GET_APPROVALS_SESSION = "select Distinct Session from receive_base order by Session desc";

    public const string GET_APPROVAL_FINAL_AUTHORITIES_MATHURA = "Select `Name`,Employee_Code,Designation,SubType AS 'ApprovalCategory' ,MyFrom AS 'LimitFrom',MyTo AS 'LimitTo' from authorities WHERE Type='PO'  and CampusCode='101' ORDER BY SubType,MyOrder;";

    public const string GET_APPROVAL_FINAL_AUTHORITIES_NON_MATHURA = "Select  `Name`,Employee_Code,Designation,SubType AS 'ApprovalCategory' ,MyFrom AS 'LimitFrom',MyTo AS 'LimitTo' from authorities WHERE Type='PO'  AND Employee_Code = (SELECT `Value` FROM `othervalues` WHERE Type = 'Online And Noida Campus Member 4 Authority') GROUP BY Employee_Code";

    public const string GET_APPROVAL_NUMBER_3_NON_MATHURA_AUTHORITIES_DEFINED = "SELECT `Value` FROM `othervalues` WHERE Type='Online And Noida Campus Member 3 Authority'";


    public const string GET_APPROVAL_NUMBER_3_MATHURA_AUTHORITIES = "select CONCAT(first_name,' - ',deisgnation,' [',santioneddeptt,']') 'Text',CONCAT(employee_code,'#',first_name,'#',deisgnation,'#',santioneddeptt) 'Value',employee_code from salary_management.emp_master where `status`!='INACTIVE' AND staff_type!='IV CLASS' AND (first_name LIKE CONCAT('%',@EmployeeName,'%') OR employee_code LIKE CONCAT('%',@EmployeeCode,'%') ) ORDER BY first_name,deisgnation desc,santioneddeptt";

    public const string GET_APPROVAL_NUMBER_3_NON_MATHURA_AUTHORITIES = "select CONCAT(first_name,' - ',deisgnation,' [',santioneddeptt,']') 'Text',CONCAT(employee_code,'#',first_name,'#',deisgnation,'#',santioneddeptt) 'Value',employee_code from salary_management.emp_master where `status`!='INACTIVE' AND staff_type!='IV CLASS' AND FIND_IN_SET(employee_code,(SELECT `Value` FROM `othervalues` WHERE Type='Online And Noida Campus Member 3 Authority')) AND (first_name LIKE CONCAT('%',@EmployeeName,'%') OR employee_code LIKE CONCAT('%',@EmployeeCode,'%') ) ORDER BY first_name,deisgnation desc,santioneddeptt";

    public const string CHEKC_IS_DRAFT_APPROVAL_ITEMS_EXISTS = "SELECT 1 FROM `purchaseapprovaldetail_draft` WHERE IniId=@IniId AND AppType=@AppType AND CampusCode=@CampusCode;";

    public const string DELETE_DRAFT_APPROVAL_ITEMS = "DELETE FROM `purchaseapprovaldetail_draft` WHERE IniId=@IniId AND AppType=@AppType AND CampusCode=@CampusCode;";

    public const string GET_VENDER_REGISTER = "select * from vendorregister where true  and VendorID = @VenderID";

    public const string INSERT_FINAL_PURCHASE_APPROVAL_BELOW_1000="insert into purchaseapprovalsummary (ReferenceNo,Session,MyType,Note,Purpose,BillUptoValue,BillUptoType,IniId,IniName,IniOn,IniFrom,ForDepartment,FirmName,FirmPerson,FirmEmail,FirmPanNo,FirmAddress,\nFirmContactNo,FirmAlternateContactNo,TotalItem,Amount,VatPer,TotalAmount,Status,CashPer,Other,App4ID,App4Name,App4Designation,App4Status,App4DoneOn,AppCat,MessageSend,VendorID,RelativePersonID,\nRelativePersonName,RelativeDesignation,RelativeDepartment,AppDate,ReferenceBillStatus,BillTill,ExtendedBillDate,R_Total,R_Pending,R_Status,BillRequired,AdditionalName,PExtra3,BudgetRequired,BudgetAmount,\nPreviousTaken,CurStatus,BudgetStatus,BudgetReferenceNo,CampusCode,CampusName) values (@RefNo,@Session,@MyType,@Note,@Purpose,@BillUptoValue,@BillUptoType,@UploadById,@UploadByName,NOW(),@IpAddress,@ForDepartment,\n@FirmName,@FirmPerson,@FirmEmail,@FirmPanNo,@FirmAddress,\n@FirmContactNo,@FirmAlternateContactNo,@TotalItem,@Amount,@VatPer,@TotalAmount,'Pending',@CashPer,@Other,@FinalApprovalId,@FinalApprovalName,@FinalApprovalDesignation,'Pending',NULL,@ApprovalCategory,'Pending',@VendorID,@IniID,\n@IniName,@IniDesignation,@IniDepartment,@AppDate,@ReferenceBillStatus,@BillTill,@ExtendedBillDate,'0.00',@ApprovalItemCount,'Pending',@BillRequired,@Maad,@BudgetBalanceAmount,@BudgetRequired,@BudgetAmount,\n@PreviousTaken,@CurStatus,@BudgetStatus,@BudgetReferenceNo,@CampusCode,@CampusName)";
    public const string INSERT_FINAL_PURCHASE_APPROVAL_ABOVE_1000="insert into purchaseapprovalsummary (ReferenceNo,Session,MyType,Note,Purpose,BillUptoValue,BillUptoType,IniId,IniName,IniOn,IniFrom,ForDepartment,FirmName,FirmPerson,FirmEmail,FirmPanNo,FirmAddress,\nFirmContactNo,FirmAlternateContactNo,TotalItem,Amount,VatPer,TotalAmount,Status,CashPer,Other,App1ID,App1Name,App1Designation,App1Status,App1DoneOn,App2ID,App2Name,App2Designation,App2Status,App2DoneOn,App3ID,App3Name,App3Designation,App3Status,App3DoneOn,App4ID,App4Name,App4Designation,App4Status,App4DoneOn,AppCat,MessageSend,VendorID,RelativePersonID,\nRelativePersonName,RelativeDesignation,RelativeDepartment,AppDate,ReferenceBillStatus,BillTill,ExtendedBillDate,R_Total,R_Pending,R_Status,BillRequired,AdditionalName,PExtra3,BudgetRequired,BudgetAmount,\nPreviousTaken,CurStatus,BudgetStatus,BudgetReferenceNo,CampusCode,CampusName) values (@RefNo,@Session,@MyType,@Note,@Purpose,@BillUptoValue,@BillUptoType,@UploadById,@UploadByName,NOW(),@IpAddress,@ForDepartment,\n@FirmName,@FirmPerson,@FirmEmail,@FirmPanNo,@FirmAddress,\n@FirmContactNo,@FirmAlternateContactNo,@TotalItem,@Amount,@VatPer,@TotalAmount,'Pending',@CashPer,@Other,@App1ID,@App1Name,@App1Designation,'Pending',NULL,@App2ID,@App2Name,@App2Designation,'Pending',NULL,@App3ID,@App3Name,@App3Designation,'Pending',NULL,@FinalApprovalId,@FinalApprovalName,@FinalApprovalDesignation,'Pending',NULL,@ApprovalCategory,'Pending',@VendorID,@IniID,\n@IniName,@IniDesignation,@IniDepartment,@AppDate,@ReferenceBillStatus,@BillTill,@ExtendedBillDate,'0.00',@ApprovalItemCount,'Pending',@BillRequired,@Maad,@BudgetBalanceAmount,@BudgetRequired,@BudgetAmount,\n@PreviousTaken,@CurStatus,@BudgetStatus,@BudgetReferenceNo,@CampusCode,@CampusName)";

    public const string INSER_APPROVAL_DETAILS =
        "insert into  purchaseapprovaldetail (ReferenceNo,ItemCode,ItemName,Make,Size,Unit,Balance,Quantity,PrevRate,CurRate,ChangeReason,TotalAmount,IniOn,IniId,Status,WarIn,WarType,ActualAmount,VatPer,R_Total,R_Pending,R_Status,SerialNo,DisPer) SELECT @RefNo AS 'ReferenceNo', ItemCode,ItemName,Make,Size,Unit,Balance,Quantity,PrevRate,CurRate,ChangeReason,TotalAmount,IniOn,IniId,Status,WarIn,WarType,ActualAmount,VatPer,R_Total,R_Pending,R_Status,SerialNo,DisPer from purchaseapprovaldetail_draft WHERE AppType=@AppType AND CampusCode=@CampusCode AND IniId=@EmpCode;";

    public const string DELETE_APPROVAL_DRAFT_DETAILS = "DELETE FROM purchaseapprovaldetail_draft WHERE AppType=@AppType AND CampusCode=@CampusCode AND IniId=@EmpCode";

    public const string DELETE_PURCHASE_SUMMARY = "DELETE FROM purchaseapprovalsummary WHERE ReferenceNo=@ReferenceNo";

    public const string DELETE_PURCHASE_DETAILS = "DELETE FROM purchaseapprovaldetail WHERE ReferenceNo=@ReferenceNo";

    public const string DELETE_DRAFTED_ITEM = "DELETE from purchaseapprovaldetail_draft where id=@Id";
    
    public const string GETDRAFTEDITEM="select * from purchaseapprovaldetail_draft where id=@Id";

    public const string GET_MY_APPROVALS = "Select PreviousCancelRemark,ReGenerated,IF(`Status`='Rejected',IF(App1Status='Rejected',App1Name,IF(App2Status='Rejected',App2Name,IF(App3Status='Rejected',App3Name,App4Name))),'') 'RejectBy',IF(`Status`='Rejected', IFNULL(RejectionReason,'N/A'),'') AS 'RejectReason' ,RelativePersonID,RelativePersonName,ReferenceNo,`Session`,CampusName,Purpose,TotalItem,DATE_FORMAT(ExtendedBillDate,'%d %b, %y') 'ExeOn',TotalAmount,`Status`,IniName,DATE_FORMAT(AppDate,'%d %b, %y') 'AppDate',DATE_FORMAT(IniOn,'%d %b, %y') 'IniOn',App1Name,App2Name,App3Name,App4Name,App1Status,App2Status,App3Status,App4Status,IFNULL(DATE_FORMAT(App1DoneOn,'%d %b, %y'),'NA') 'App1On',IFNULL(DATE_FORMAT(App2DoneOn,'%d %b, %y'),'NA') 'App2On',IFNULL(DATE_FORMAT(App3DoneOn,'%d %b, %y'),'NA') 'App3On',IFNULL(DATE_FORMAT(App4DoneOn,'%d %b, %y'),'NA') 'App4On',CancelledReason,DATE_FORMAT(CancelledOn,'%d %b, %y') 'CancelledOn',CancelledBy,CloseReason,DATE_FORMAT(CloseOn,'%d %b, %y') 'CloseOn',CloseBy,IF(`Status`='Pending' And HOUR(TIMEDIFF(now(),IniOn))>=48,'Y','N') 'FinalStat',BudgetRequired,BudgetAmount,PreviousTaken,CurStatus,BudgetStatus,BudgetReferenceNo,BillId,BillRequired,VendorID,ForDepartment,Note,MyType,ExtendedBillDate,AdditionalName,AppCat,CONCAT(IFNULL(CONCAT(App1Name,', '),''),IFNULL(CONCAT(App2Name,', '),''),IFNULL(CONCAT(App3Name,', '),''),IFNULL(CONCAT(App4Name,', '),'')) 'Auth',DATE_FORMAT(AppDate,'%m/%d/%Y') 'AD',DATE_FORMAT(ExtendedBillDate,'%m/%d/%Y') 'EBD' from purchaseapprovalsummary where true @Condition  ORDER BY IniOn,`Status`";

    public const string CHECK_IS_APPROVAL_COMPARISON_DEFINED = "SELECT DISTINCT ReferenceNo from price_comparison_chart WHERE ReferenceNo=@ReferenceNo";

    public const string CHECK_APPROVAL_EXISTS = "SELECT * from purchaseapprovalsummary WHERE ReferenceNo=@ReferenceNo AND `Status`='Pending' ";

    public const string DELETE_APPROVAL = "update purchaseapprovalsummary set `Status`='Deleted',DeletedOn=now(),DeletedBy=@EmployeeId,DeletedFrom=@DeleteFrom where  ReferenceNo=@ReferenceNo AND App1Status='Pending'  AND App2Status='Pending'  AND App3Status='Pending'  AND App4Status='Pending' ;";
}