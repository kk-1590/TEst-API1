namespace AdvanceAPI.SQLConstants.Approval;

public class ApprovalSql
{
    public const string GET_ITEM_REFERENCE_NO = "SELECT DISTINCT ReferenceNo FROM `purchaseapprovaldetail_draft` WHERE IniId =@EmpCode AND AppType=@AppType and ReferenceNo=@ReferenceNo;";
    public const string CHECK_ALREADY_ITEM = "SELECT ItemCode FROM `purchaseapprovaldetail_draft` WHERE IniId =@EmpCode AND AppType=@AppType and ItemCode=@ItemCode and ReferenceNo=@ReferenceNo;";

    public const string GET_AUTO_ITEM_REFNO =
        "Select IFNULL(CAST(MAX(ReferenceNo)+1 as CHAR),CONCAT(CAST(DATE_FORMAT(now(),'%y%m%d') as CHAR),'0001')) 'ReferenceNo' from (Select ReferenceNo from purchaseapprovaldetail_draft) A  where ReferenceNo like CONCAT(CAST(DATE_FORMAT(now(),'%y%m%d') as CHAR),'%');";

    public const string INERT_DRAFT =
        "insert into  purchaseapprovaldetail_draft (ReferenceNo,AppType,ItemCode,ItemName,Make,Size,Unit,Balance,Quantity,PrevRate,CurRate,ChangeReason,TotalAmount,IniOn,IniId,Status,WarIn,WarType,ActualAmount,VatPer,R_Total,R_Pending,R_Status,SerialNo,DisPer,CampusCode,DraftName)\n values (@ReferenceNo,@AppType,@ItemCode,@ItemName,@Make,@Size,@Unit,@Balance,@Quantity,@PrevRate,@CurRate,@ChangeReason,@TotalAmount,NOW(),@IniId,'Pending',@WarIn,@WarType,@ActualAmount,@VatPer,@R_Total,@R_Pending,'Pending',@SerialNo,@DisPer,@CampusCode,@DraftName);";

    public const string GET_DRAFTED_ITEM =
        "select id,ReferenceNo,AppType,ItemCode,ItemName,Make,Size,Unit,Balance,Quantity,PrevRate,CurRate,ChangeReason,TotalAmount,DATE_FORMAT(IniOn,'%d.%m.%Y') 'IniOn',IniId,Status,WarIn,WarType,ActualAmount,VatPer,R_Total,R_Pending,R_Status,SerialNo,DisPer,DATE_FORMAT(IniOn,'%d.%m.%Y')'InitOn',CampusCode from purchaseapprovaldetail_draft where IniId=@EmpCode AND CampusCode=@CampusCode AND AppType=@ApprovalType and ReferenceNo=@ReferenceNo;";

    public const string Get_Draft_Summary =
        "SELECT ReferenceNo,CampusCode,AppType,COUNT(*)'Total',SUM(TotalAmount)'Amt' FROM `purchaseapprovaldetail_draft` WHERE IniId=@EmpCode AND CampusCode=@CampusCode and AppType=@AppType and ReferenceNo=@ReferenceNo ;";
    public const string Generate_RefNo_Purchaseapprovalsummary = "Select IFNULL(CAST(MAX(ReferenceNo)+1 as CHAR),CONCAT(CAST(DATE_FORMAT(now(),'%y%m%d') as CHAR),'0001')) 'ReferenceNo' from (Select ReferenceNo from purchaseapprovalsummary UNION Select ReferenceNo from otherapprovalsummary) A  where ReferenceNo like CONCAT(CAST(DATE_FORMAT(now(),'%y%m%d') as CHAR),'%')";

    public const string GET_APPROVALS_SESSION = "select Distinct Session from receive_base order by Session desc";

    public const string GET_APPROVAL_FINAL_AUTHORITIES_MATHURA = "Select `Name`,Employee_Code,Designation,SubType AS 'ApprovalCategory' ,MyFrom AS 'LimitFrom',MyTo AS 'LimitTo' from authorities WHERE Type='PO'  and CampusCode='101' ORDER BY SubType,MyOrder;";

    public const string GET_APPROVAL_FINAL_AUTHORITIES_NON_MATHURA = "Select  `Name`,Employee_Code,Designation,SubType AS 'ApprovalCategory' ,MyFrom AS 'LimitFrom',MyTo AS 'LimitTo' from authorities WHERE Type='PO'  AND Employee_Code = (SELECT `Value` FROM `othervalues` WHERE Type = 'Online And Noida Campus Member 4 Authority') GROUP BY Employee_Code";

    public const string GET_APPROVAL_NUMBER_3_NON_MATHURA_AUTHORITIES_DEFINED = "SELECT `Value` FROM `othervalues` WHERE Type='Online And Noida Campus Member 3 Authority'";


    public const string GET_APPROVAL_NUMBER_3_MATHURA_AUTHORITIES = "select CONCAT(first_name,' - ',deisgnation,' [',santioneddeptt,']') 'Text',CONCAT(employee_code,'#',first_name,'#',deisgnation,'#',santioneddeptt) 'Value',employee_code from salary_management.emp_master where `status`!='INACTIVE' AND staff_type!='IV CLASS' AND (first_name LIKE CONCAT('%',@EmployeeName,'%') OR employee_code LIKE CONCAT('%',@EmployeeCode,'%') ) ORDER BY first_name,deisgnation desc,santioneddeptt";

    public const string GET_APPROVAL_NUMBER_3_NON_MATHURA_AUTHORITIES = "select CONCAT(first_name,' - ',deisgnation,' [',santioneddeptt,']') 'Text',CONCAT(employee_code,'#',first_name,'#',deisgnation,'#',santioneddeptt) 'Value',employee_code from salary_management.emp_master where `status`!='INACTIVE' AND staff_type!='IV CLASS' AND FIND_IN_SET(employee_code,(SELECT `Value` FROM `othervalues` WHERE Type='Online And Noida Campus Member 3 Authority')) AND (first_name LIKE CONCAT('%',@EmployeeName,'%') OR employee_code LIKE CONCAT('%',@EmployeeCode,'%') ) ORDER BY first_name,deisgnation desc,santioneddeptt";

    public const string CHEKC_IS_DRAFT_APPROVAL_ITEMS_EXISTS = "SELECT 1 FROM `purchaseapprovaldetail_draft` WHERE IniId=@IniId AND AppType=@AppType AND CampusCode=@CampusCode AND ReferenceNo=@ReferenceNo;";

    public const string DELETE_DRAFT_APPROVAL_ITEMS = "DELETE FROM `purchaseapprovaldetail_draft` WHERE IniId=@IniId AND AppType=@AppType AND CampusCode=@CampusCode AND ReferenceNo=@ReferenceNo;";

    public const string GET_VENDER_REGISTER = "select * from vendorregister where true  and VendorID = @VenderID";

    public const string INSERT_FINAL_PURCHASE_APPROVAL_BELOW_1000 = "insert into purchaseapprovalsummary (ReferenceNo,Session,MyType,Note,Purpose,BillUptoValue,BillUptoType,IniId,IniName,IniOn,IniFrom,ForDepartment,FirmName,FirmPerson,FirmEmail,FirmPanNo,FirmAddress,\nFirmContactNo,FirmAlternateContactNo,TotalItem,Amount,VatPer,TotalAmount,Status,CashPer,Other,App4ID,App4Name,App4Designation,App4Status,App4DoneOn,AppCat,MessageSend,VendorID,RelativePersonID,\nRelativePersonName,RelativeDesignation,RelativeDepartment,AppDate,ReferenceBillStatus,BillTill,ExtendedBillDate,R_Total,R_Pending,R_Status,BillRequired,AdditionalName,PExtra3,BudgetRequired,BudgetAmount,\nPreviousTaken,CurStatus,BudgetStatus,BudgetReferenceNo,CampusCode,CampusName) values (@RefNo,@Session,@MyType,@Note,@Purpose,@BillUptoValue,@BillUptoType,@UploadById,@UploadByName,NOW(),@IpAddress,@ForDepartment,\n@FirmName,@FirmPerson,@FirmEmail,@FirmPanNo,@FirmAddress,\n@FirmContactNo,@FirmAlternateContactNo,@TotalItem,@Amount,@VatPer,@TotalAmount,'Pending',@CashPer,@Other,@FinalApprovalId,@FinalApprovalName,@FinalApprovalDesignation,'Pending',NULL,@ApprovalCategory,'Pending',@VendorID,@IniID,\n@IniName,@IniDesignation,@IniDepartment,@AppDate,@ReferenceBillStatus,@BillTill,@ExtendedBillDate,'0.00',@ApprovalItemCount,'Pending',@BillRequired,@Maad,@BudgetBalanceAmount,@BudgetRequired,@BudgetAmount,\n@PreviousTaken,@CurStatus,@BudgetStatus,@BudgetReferenceNo,@CampusCode,@CampusName)";
    public const string INSERT_FINAL_PURCHASE_APPROVAL_ABOVE_1000 = "insert into purchaseapprovalsummary (ReferenceNo,Session,MyType,Note,Purpose,BillUptoValue,BillUptoType,IniId,IniName,IniOn,IniFrom,ForDepartment,FirmName,FirmPerson,FirmEmail,FirmPanNo,FirmAddress,\nFirmContactNo,FirmAlternateContactNo,TotalItem,Amount,VatPer,TotalAmount,Status,CashPer,Other,App1ID,App1Name,App1Designation,App1Status,App1DoneOn,App2ID,App2Name,App2Designation,App2Status,App2DoneOn,App3ID,App3Name,App3Designation,App3Status,App3DoneOn,App4ID,App4Name,App4Designation,App4Status,App4DoneOn,AppCat,MessageSend,VendorID,RelativePersonID,\nRelativePersonName,RelativeDesignation,RelativeDepartment,AppDate,ReferenceBillStatus,BillTill,ExtendedBillDate,R_Total,R_Pending,R_Status,BillRequired,AdditionalName,PExtra3,BudgetRequired,BudgetAmount,\nPreviousTaken,CurStatus,BudgetStatus,BudgetReferenceNo,CampusCode,CampusName) values (@RefNo,@Session,@MyType,@Note,@Purpose,@BillUptoValue,@BillUptoType,@UploadById,@UploadByName,NOW(),@IpAddress,@ForDepartment,\n@FirmName,@FirmPerson,@FirmEmail,@FirmPanNo,@FirmAddress,\n@FirmContactNo,@FirmAlternateContactNo,@TotalItem,@Amount,@VatPer,@TotalAmount,'Pending',@CashPer,@Other,@App1ID,@App1Name,@App1Designation,'Pending',NULL,@App2ID,@App2Name,@App2Designation,'Pending',NULL,@App3ID,@App3Name,@App3Designation,'Pending',NULL,@FinalApprovalId,@FinalApprovalName,@FinalApprovalDesignation,'Pending',NULL,@ApprovalCategory,'Pending',@VendorID,@IniID,\n@IniName,@IniDesignation,@IniDepartment,@AppDate,@ReferenceBillStatus,@BillTill,@ExtendedBillDate,'0.00',@ApprovalItemCount,'Pending',@BillRequired,@Maad,@BudgetBalanceAmount,@BudgetRequired,@BudgetAmount,\n@PreviousTaken,@CurStatus,@BudgetStatus,@BudgetReferenceNo,@CampusCode,@CampusName)";

    public const string INSER_APPROVAL_DETAILS =
        "insert into  purchaseapprovaldetail (ReferenceNo,ItemCode,ItemName,Make,Size,Unit,Balance,Quantity,PrevRate,CurRate,ChangeReason,TotalAmount,IniOn,IniId,Status,WarIn,WarType,ActualAmount,VatPer,R_Total,R_Pending,R_Status,SerialNo,DisPer) SELECT @RefNo AS 'ReferenceNo', ItemCode,ItemName,Make,Size,Unit,Balance,Quantity,PrevRate,CurRate,ChangeReason,TotalAmount,IniOn,IniId,Status,WarIn,WarType,ActualAmount,VatPer,R_Total,R_Pending,R_Status,SerialNo,DisPer from purchaseapprovaldetail_draft WHERE AppType=@AppType AND CampusCode=@CampusCode AND IniId=@EmpCode and ReferenceNo=@ReferenceNo;";

    public const string DELETE_APPROVAL_DRAFT_DETAILS = "DELETE FROM purchaseapprovaldetail_draft WHERE AppType=@AppType AND CampusCode=@CampusCode AND IniId=@EmpCode AND ReferenceNo=@ReferenceNo";

    public const string DELETE_PURCHASE_SUMMARY = "DELETE FROM purchaseapprovalsummary WHERE ReferenceNo=@ReferenceNo";

    public const string DELETE_PURCHASE_DETAILS = "DELETE FROM purchaseapprovaldetail WHERE ReferenceNo=@ReferenceNo";

    public const string DELETE_DRAFTED_ITEM = "DELETE from purchaseapprovaldetail_draft where id=@Id";

    public const string GETDRAFTEDITEM = "select * from purchaseapprovaldetail_draft where id=@Id";

    public const string GET_MY_APPROVALS = "Select PreviousCancelRemark,ReGenerated,IF(`Status`='Rejected',IF(App1Status='Rejected',App1Name,IF(App2Status='Rejected',App2Name,IF(App3Status='Rejected',App3Name,App4Name))),'') 'RejectBy',IF(`Status`='Rejected', IFNULL(RejectionReason,'N/A'),'') AS 'RejectReason' ,RelativePersonID,RelativePersonName,ReferenceNo,`Session`,CampusName,Purpose,TotalItem,DATE_FORMAT(ExtendedBillDate,'%d %b, %y') 'ExeOn',TotalAmount,`Status`,IniName,DATE_FORMAT(AppDate,'%d %b, %y') 'AppDate',DATE_FORMAT(IniOn,'%d %b, %y') 'IniOn',App1Name,App2Name,App3Name,App4Name,App1Status,App2Status,App3Status,App4Status,IFNULL(DATE_FORMAT(App1DoneOn,'%d %b, %y'),'NA') 'App1On',IFNULL(DATE_FORMAT(App2DoneOn,'%d %b, %y'),'NA') 'App2On',IFNULL(DATE_FORMAT(App3DoneOn,'%d %b, %y'),'NA') 'App3On',IFNULL(DATE_FORMAT(App4DoneOn,'%d %b, %y'),'NA') 'App4On',CancelledReason,DATE_FORMAT(CancelledOn,'%d %b, %y') 'CancelledOn',CancelledBy,CloseReason,DATE_FORMAT(CloseOn,'%d %b, %y') 'CloseOn',CloseBy,IF(`Status`='Pending' And HOUR(TIMEDIFF(now(),IniOn))>=48,'Y','N') 'FinalStat',BudgetRequired,BudgetAmount,PreviousTaken,CurStatus,BudgetStatus,BudgetReferenceNo,BillId,BillRequired,VendorID,ForDepartment,Note,MyType,ExtendedBillDate,AdditionalName,AppCat,CONCAT(IFNULL(CONCAT(App1Name,', '),''),IFNULL(CONCAT(App2Name,', '),''),IFNULL(CONCAT(App3Name,', '),''),IFNULL(CONCAT(App4Name,', '),'')) 'Auth',DATE_FORMAT(AppDate,'%m/%d/%Y') 'AD',DATE_FORMAT(ExtendedBillDate,'%m/%d/%Y') 'EBD',FirmName,FirmPerson,FirmContactNo,ReferenceBillStatus,R_Status as 'RecievingStatus',DATE_FORMAT(R_LastRcvOn,'%d.%m.%Y %r')'LastRecieveOn',CampusCode,AdditionalName AS 'Maad'  from purchaseapprovalsummary where true @Condition  ORDER BY IniOn,`Status`  LIMIT @LimitItems OFFSET @OffSetItems";

    public const string GET_MY_APPROVAL_COUNT = "select count(*) from purchaseapprovalsummary where true @Condition  ORDER BY IniOn,`Status` ";


    public const string CHECK_IS_APPROVAL_COMPARISON_DEFINED = "SELECT DISTINCT ReferenceNo from price_comparison_chart WHERE ReferenceNo=@ReferenceNo";

    public const string CHECK_APPROVAL_EXISTS = "SELECT * from purchaseapprovalsummary WHERE ReferenceNo=@ReferenceNo AND `Status`='Pending' ";

    public const string DELETE_APPROVAL = "update purchaseapprovalsummary set `Status`='Deleted',DeletedOn=now(),DeletedBy=@EmployeeId,DeletedFrom=@DeleteFrom where  ReferenceNo=@ReferenceNo AND App1Status='Pending'  AND App2Status='Pending'  AND App3Status='Pending'  AND App4Status='Pending' ;";
    public const string GET_DRAFT = "SELECT AppType,DraftName,CampusCode,COUNT(ReferenceNo) 'ItemCount',SUM(ActualAmount) 'TotalBalance',ReferenceNo from purchaseapprovaldetail_draft WHERE IniId=@EmpCode GROUP BY ReferenceNo ORDER BY IniOn DESC;";


    public const string GET_PO_APPROVAL_DETAILS = "Select AdditionalName,`Status`,RelativePersonID,RelativePersonName,RelativeDepartment,RelativeDesignation,ForDepartment,FirmAddress,DATE_FORMAT(now(),'%d.%m.%Y') as 'MyOrderDate',MyType,Note,Purpose,DATE_FORMAT(if(BillUptoType='Day',ADDDATE(AppDate,INTERVAL BillUptoValue Day),if(BillUptoType='Month',ADDDATE(AppDate,INTERVAL BillUptoValue MONTH),if(BillUptoType='Year',ADDDATE(AppDate,INTERVAL BillUptoValue Year),NULL))),'%d %M, %Y') 'ExpDate',TotalAmount,Amount,CashPer AS 'DiscountOverAll',Other,FirmName,FirmContactNo,IniName,IniId,DATE_FORMAT(AppDate,'%d.%m.%Y') 'OrderDate',DATE_FORMAT(IniOn,'%d.%m.%Y  %h:%i %p') 'CreateDate',IF(BillTill!=ExtendedBillDate,DATE_FORMAT(ExtendedBillDate,'%d %M, %Y'),NULL) 'ExtendedBillDate',App1Name,App2Name,App3Name,App4Name,App4Designation,App3Designation,App2Designation,App1Designation,App1Status,App2Status,App3Status,App4Status,DATE_FORMAT(App1DoneOn,'%d %b, %Y %h:%i %p') 'App1On',DATE_FORMAT(App2DoneOn,'%d %b, %Y %h:%i %p') 'App2On',DATE_FORMAT(App3DoneOn,'%d %b, %Y %h:%i %p') 'App3On',DATE_FORMAT(App4DoneOn,'%d %b, %Y %h:%i %p') 'App4On',ByPass,if(`Status`='Cancelled',if(CancelledBy=App1Name,'1',if(CancelledBy=App2Name,'2',if(CancelledBy=App3Name,'3',if(CancelledBy=App4Name,'4','NA')))),'') 'CancelBy',DATE_FORMAT(CancelledOn,'%d %b, %Y %h:%i %p') 'CancelOn',ReferenceBillStatus,CancelledReason,BillVariationRemark,RejectionReason,VatType,BudgetRequired,BudgetAmount,PreviousTaken,CurStatus,BudgetStatus,BudgetReferenceNo,ReferenceNo,DATE_FORMAT(AppDate,'%Y-%m-%d') as 'AppDateDB',CampusName from purchaseapprovalsummary where ReferenceNo=@ReferenceNo AND `Status`!='Deleted' ";

    public const string CHECK_IS_APPPROVAL_COMPARISON_LOCKED = "SELECT * from price_comparison_chart WHERE ReferenceNo = @ReferenceNo AND `Lock`='Locked'";

    public const string GET_APPROVAL_BILL_EXPENDITURE_DETAILS = "Select Title,SUM(Amount) 'Amount' from billdistribution where Id in (Select TransactionID from bill_base where BillExtra3=@ReferenceNo And `Status`!='Bill Rejected' And ForType!='Advance Bill') GROUP BY Title ORDER BY Title";

    public const string GET_APPROVAL_PAYMENT_DETAILS = "Select PaidAmount,TaxAmount,IssuedAmount,TRIM(REPLACE(TransactionNo,',',', ')) 'TransactionNo',DATE_FORMAT(IssuedOn,'%d.%m.%y') 'IssuedOn',SUBSTRING_INDEX(IssuedByName,' ',1) 'IssuedByName', IFNULL(DATE_FORMAT(SignedOn,'%d.%m.%y'),'---') 'SignedOn',IFNULL(DATE_FORMAT(ReceivedOn,'%d.%m.%y'),'---') 'ReceivedOn',IFNULL(DATE_FORMAT(Col4,'%d.%m.%y'),'---') 'Bank',IF(Col4 is not null,'Clr.',IF(ReceivedOn is not null,'Rcv.',IF(SignedOn is not null,'Sgn.','Prp.')))  'Status' from bill_transaction_issue where (TransactionID in (Select TransactionID from bill_base where BillExtra3=@ReferenceNo And `Status`!='Bill Rejected' And ForType!='Advance Bill')  or TransactionID in (Select DISTINCT TransactionID from otherapprovalsummary A, bill_base B where SUBSTRING_INDEX(PExtra4,'#',1)=@ReferenceNo  And A.ReferenceNo=B.BillExtra3 And B.ForType='Advance Bill'))  And Col3 is NULL ORDER BY IssuedOn,SequenceID";

    public const string GET_APPROVAL_WARRENTY_DETAILS = "Select SerialNo from purchaseapprovaldetail where IFNULL(SerialNo,'')!='' And ReferenceNo=@ReferenceNo";

    public const string GET_APPROVAL_REPAIR_MAINTINANCE_DETAILS = "Select A.SerialNo,DATE_FORMAT(AppDate,'%d.%m.%Y') 'App',A.TotalAmount 'Amount',B.`Status` from  advances.purchaseapprovaldetail A, purchaseapprovalsummary B,(Select DISTINCT ItemCode,SUBSTRING_INDEX(SerialNo,'|',1) as 'SerialNo' from purchaseapprovaldetail where IFNULL(SerialNo,'')!='' And ReferenceNo=@ReferenceNo) C where A.ReferenceNo=B.ReferenceNo And B.`Status` in ('Pending','Approved') And A.SerialNo is not null And  SUBSTRING_INDEX(A.SerialNo,'|',1)=C.SerialNo And A.ItemCode=C.ItemCode ORDER BY A.ItemCode,A.SerialNo,AppDate DESC";

    public const string GET_APPROVAL_ITEMS = "Select *,(Quantity+Balance) 'Tot',if(WarIn>0,CONCAT(CAST(WarIn as CHAR),' ',LEFT(WarType,1)),'---') 'Warrenty' from purchaseapprovaldetail where ReferenceNo=@ReferenceNo And `Status`!='Deleted'";
    public const string GET_APPROVAL_ITEMS_PREVIOUS_PURCHASE_DETAILS = "Select ReferenceNo,Quantity,DATE_FORMAT(IniOn,'%d %b, %Y') 'IniOn',IFNULL(CAST(DATE_FORMAT(R_LastRcvOn,'%d %b, %Y') as CHAR),'N/F') 'RecieveOn' from purchaseapprovaldetail where ItemCode=@ItemCode And ReferenceNo<@ReferenceNo And `Status`='Approved' ORDER BY ReferenceNo desc LIMIT 1";

    public const string GET_APPROVAL_EMPLOYEES_DESIGNATION_DEPARTMENT = "Select deisgnation,contactno from salary_management.emp_master where employee_code=@EmployeeId";

    public const string UPDATE_APPROVAL_NOTE = "update purchaseapprovalsummary SET Note=@Note,CommentEditedBy=IF(CommentEditedBy is null,CONCAT(@EmployeeId,' on ',CAST(DATE_FORMAT(now(),'%d %b, %Y %h:%i %p') as CHAR)),CONCAT(CommentEditedBy,'$',@EmployeeId,' on ',CAST(DATE_FORMAT(now(),'%d %b, %Y %h:%i %p') as CHAR))) where ReferenceNo=@ReferenceNo AND `Status`!='Deleted'";

    public const string CHECK_IS_APPROVAL_EXISTS_TO_UPDATE = "Select 1 from purchaseapprovalsummary where ReferenceNo=@ReferenceNo AND `Status`!='Deleted'";

    public const string GET_EDIT_APPROVAL_DETAILS = "SELECT ReferenceNo,MyType,CampusName,TotalAmount,AppCat AS 'ApprovalCategory',CONCAT(IFNULL(CONCAT(App1Name,', '),''),IFNULL(CONCAT(App2Name,', '),''),IFNULL(CONCAT(App3Name,', '),''),IFNULL(CONCAT(App4Name,', '),'')) AS 'Authorities',AdditionalName AS 'Maad',ForDepartment as 'Department',VendorID,FirmName,FirmAddress,Note,Purpose,DATE_FORMAT(AppDate,'%d.%m.%Y')'AppDateShow',DATE_FORMAT(BillTill,'%d.%m.%Y')'BillTillShow',DATE_FORMAT(AppDate,'%Y-%m-%d')'AppDateDB',DATE_FORMAT(BillTill,'%Y-%m-%d')'BillTillDB' from purchaseapprovalsummary WHERE ReferenceNo=@ReferenceNo AND `Status`!='Deleted';";

    public const string UPDATE_APPROVAL_DETAILS =
        "UPDATE purchaseapprovalsummary SET AdditionalName=@Maad,ForDepartment=@DepartMent,VendorID=@Vendorid,FirmName=@FirmName,FirmAddress=@Address,FirmContactNo=@FirmContactNo,FirmPerson=@FirmPerson,FirmEmail=@FirmEmail,\nFirmAlternateContactNo=@AltContactNo,Note=@Note,Purpose=@Purpose,AppDate=@AppDate,BillTill=@BillDate,ExtendedBillDate=@ExtendedBillDate,MyType=@MyType WHERE ReferenceNo=@RefNo";

    public const string UPDATE_LOG =
        "insert into changeslog (Type,ChangeUniqueNo,ChangeIn,FromData,ToData,Operation,DoneOn,DoneBy,DoneFrom) values (@Type,@ChangeUniqueNo,@ChangeIn,@FromData,@ToData,@Operation,now(),@DoneBy,@IpAddress)";
    public const string GET_BASE_URL = "SELECT Tag,Site,Type from gla_student_management.siteurls where true ";
    #region PassApproval

    public const string GET_APPROVAL_DETAILS = "Select PreviousCancelRemark,CAST(CONCAT(AdditionalName,'$',ForDepartment,'$',CAST(VendorID as CHAR),'$',FirmName,'$',RelativePersonID,'$',RelativePersonName)  as CHAR) as 'Test',IF(`Status`='Rejected',CONCAT('Rejected By : ',IF(App1Status='Rejected',App1Name,IF(App2Status='Rejected',App2Name,IF(App3Status='Rejected',App3Name,App4Name))),'<br/>Reason : ',IFNULL(RejectionReason,'N/A')),'') 'RejectedReason',RelativePersonID,RelativePersonName,ReferenceNo,`Session`,MyType,Purpose,TotalItem,TotalAmount,`Status`,DATE_FORMAT(ExtendedBillDate,'%d %b, %y') 'ExeOn',IniName,DATE_FORMAT(AppDate,'%d %b, %y') 'AppDate',DATE_FORMAT(IniOn,'%d %b, %y') 'IniOn',App1Name,CampusName, App2Name,App3Name,App4Name,App1Status,App2Status,App3Status,App4Status,IFNULL(DATE_FORMAT(App1DoneOn,'%d %b, %y'),'NA') 'App1On',IFNULL(DATE_FORMAT(App2DoneOn,'%d %b, %y'),'NA') 'App2On',IFNULL(DATE_FORMAT(App3DoneOn,'%d %b, %y'),'NA') 'App3On',IFNULL(DATE_FORMAT(App4DoneOn,'%d %b, %y'),'NA') 'App4On',App1ID,App2ID,App3ID,App4ID,ByPass,BillId,CancelledReason,DATE_FORMAT(CancelledOn,'%d %b, %y') 'CancelledOn',CancelledBy,CloseReason,DATE_FORMAT(CloseOn,'%d %b, %y') 'CloseOn',CloseBy,IF(`Status`='Pending' And HOUR(TIMEDIFF(now(),IniOn))>=0,'Y','N') 'FinalStat',BudgetRequired,BudgetAmount,PreviousTaken,CurStatus,BudgetStatus,BudgetReferenceNo,FirmName,FirmPerson,FirmContactNo,ReferenceBillStatus,R_Status as 'RecievingStatus',DATE_FORMAT(R_LastRcvOn,'%d.%m.%Y %r')'LastRecieveOn',Note,ReferenceBillStatus,AdditionalName AS 'Maad'  from purchaseapprovalsummary where true @AdditinalQuery ORDER BY IniOn,`Status` limit @Limit offset @Offset";

    public const string CHECK_STATUS_ACTION_APPROVAL_VALID_EXISTS = "Select 1 from advances.purchaseapprovalsummary where ReferenceNo=@ReferenceNo AND `Status`='Pending' AND @AppNumberCondition";

    public const string APPROVE_REJECT_APPROVAL = "update purchaseapprovalsummary set @PassQueryPart where  ReferenceNo=@ReferenceNo";

    public const string CHECK_FINAL_STATUS_TO_PASS_APPROVAL = "Select if(App1ID is not NULL And App1Status='Pending','Pending',if(App2ID is not NULL And App2Status='Pending','Pending',if(App3ID is not NULL And App3Status='Pending','Pending',if(App4ID is not NULL And App4Status='Pending','Pending','Approved')))) As 'FinalStatus' from purchaseapprovalsummary where ReferenceNo=@ReferenceNo";

    public const string UPDATE_APPROVAL_SUMMARY_FINAL_STATUS_APPROVED = "update purchaseapprovalsummary set `Status`='Approved'  where `Status`='Pending' And ReferenceNo=@ReferenceNo";

    public const string UPDATE_APPROVAL_DETAIL_FINAL_STATUS_APPROVED = "update purchaseapprovalsummary set `Status`='Approved'  where `Status`='Pending' And ReferenceNo=@ReferenceNo";

    public const string CHECK_IS_VIVEK_SIR_APPROVED_APPROVAL = "SELECT 1 from advances.purchaseapprovalsummary where ReferenceNo=@ReferenceNo AND App4ID='GLAVIVEK' AND App4Status='Approved'";

    public const string CHECK_CAN_CANCEL_APPROVAL = "SELECT 1 FROM `purchaseapprovalsummary` WHERE ReferenceNo=@ReferenceNo AND `Status`='Approved' @AppNumberCondition AND (BillId IS NULL OR BillId='')  AND ( ByPass IS NULL OR  ByPass NOT LIKE CONCAT('%Member',@MemberNumber,'%') ) ";

    public const string CANCEL_APPROVAL = "update purchaseapprovalsummary set Status='Cancelled',CancelledReason=@CancelledReason,CancelledOn=now(),CancelledBy=@EmployeeName,CancelledFrom=@IpAddress where  ReferenceNo=@ReferenceNo";



    public const string GET_APPROVAL_SUMMARY_AMOUNT_DETAILS = "SELECT TotalItem,Amount,TotalAmount,CashPer AS 'Discount',Other AS 'OtherCharges' FROM `purchaseapprovalsummary` where ReferenceNo=@ReferenceNo;";

    public const string GET_APPROVAL_ITEMS_SUMMARY_AMOUNT = "SELECT COUNT(*) AS 'TotalItems',SUM(TotalAmount) AS 'TotalAmount' FROM `purchaseapprovaldetail` where ReferenceNo=@ReferenceNo;";

    public const string UPDATE_APPROVAL_ITEMS_SUMMARY_AMOUNT = "UPDATE purchaseapprovalsummary SET TotalItem=@TotalItem,Amount=@ItemsAmount,TotalAmount=@PayableAmount WHERE ReferenceNo=@ReferenceNo ;";


    public const string CHECK_IS_APPROVAL_COMPLETE_PENDING = "SELECT 1 FROM `purchaseapprovalsummary` where `Status`='Pending' AND App1Status='Pending' AND App2Status='Pending' AND App3Status='Pending' AND App4Status='Pending' AND ReferenceNo=@ReferenceNo; ;";

    public const string CHECK_APPROVAL_HAS_ITEM_CODE = "SELECT 1 FROM `purchaseapprovaldetail` where ReferenceNo=@ReferenceNo AND ItemCode=@ItemCode AND `Status`!='Deleted';";

    public const string CHECK_APPROVAL_HAS_OTHER_ITEM_CODE = "SELECT 1 FROM `purchaseapprovaldetail` where ReferenceNo=@ReferenceNo AND ItemCode!=@ItemCode AND `Status`!='Deleted';";

    public const string DELETE_ITEM_FROM_APPROVAL = "UPDATE `purchaseapprovaldetail` SET `Status`='Deleted',ChangeReason=IF(ChangeReason IS NULL,CONCAT('Deleted By: `',@EmployeeId,'` On: `',NOW(),'` From: `',@IpAddress,'` Reason: `',@Reason,'`'),CONCAT(ChangeReason,' $$$ Deleted By: `',@EmployeeId,'` On: `',NOW(),'` From: `',@IpAddress,'` Reason: `',@Reason,'`')) where ReferenceNo=@ReferenceNo AND ItemCode=@ItemCode AND `Status`!='Deleted';";
    public const string ADD_ITEM_IN_CREATED_APPROVAL = "INSERT INTO `purchaseapprovaldetail` ( ReferenceNo,ItemCode,ItemName,Make,Size,Unit,Balance,Quantity,PrevRate,CurRate,ChangeReason,WarIn,WarType,ActualAmount,DisPer,VatPer,TotalAmount,SerialNo,IniOn,IniId,`Status`,R_Total,R_Pending,R_Status) VALUES (@ReferenceNo,@ItemCode,@ItemName,@Make,@Size,@Unit,@Balance,@Quantity,@PrevRate,@CurRate,@ChangeReason,@WarIn,@WarType,@ActualAmount,@DisPer,@VatPer,@TotalAmount,@SerialNo,NOW(),@EmployeeId,'Pending','0.00',@Quantity,'Pending')";

    public const string UPDATE_ITEM_IN_CREATED_APPROVAL = "UPDATE  purchaseapprovaldetail  SET Balance=@Balance,Quantity=@Quantity,R_Pending=@Quantity,PrevRate=@PrevRate,CurRate=@CurRate,ChangeReason=@ChangeReason,WarIn=@WarIn,WarType=@WarType,ActualAmount=@ActualAmount,DisPer=@DisPer,VatPer=@VatPer,TotalAmount=@TotalAmount,SerialNo=@SerialNo,IniOn=NOW(),IniId=@EmployeeId  where ReferenceNo=@ReferenceNo AND ItemCode=@ItemCode AND `Status`!='Deleted';";


    #endregion


}