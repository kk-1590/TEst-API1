namespace AdvanceAPI.SQLConstants.Approval;

public class ApprovalSql
{
    public const string GET_ITEM_REFERENCE_NO = "SELECT DISTINCT ReferenceNo FROM `purchaseapprovaldetail_draft` WHERE IniId =@EmpCode AND AppType=@AppType;";

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
    
    
}