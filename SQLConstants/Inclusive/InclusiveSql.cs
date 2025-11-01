namespace AdvanceAPI.SQLConstants.Inclusive
{
    public static class InclusiveSql
    {
        public const string GET_ALLOWED_CAMPUS = "SELECT A.CampusCode,B.CampusName from gla_student_management.employee_campus_master A, gla_student_management.campus_master B WHERE A.CampusCode=B.CampusCode AND A.EmployeeCode=@EmployeeCode AND A.ApplicationType='FINANCE' AND B.IsActive=1 ORDER BY A.CampusCode;";

        public const string GET_APPROVAL_TYPE = "Select `Value` from othervalues where Type='Approval' ORDER BY Id";

        public const string Get_Purchase_Department = "SELECT DISTINCT DepartmentCode FROM departmentregister ORDER BY DepartmentCode";

        public const string Get_ITEM_NAME = "SELECT ItemCode,ItemName,Size,Make,Unit,DepartmentCode FROM `itemregister` WHERE true @Condition";

        public const string Get_Base_Url_Table = "SELECT Tag,Site,Type from gla_student_management.siteurls where true ";

        public const string GET_ITEM_BY_CODE = "SELECT ItemName,Size,Unit FROM purchase.itemregister WHERE ItemCode=@itemcode LIMIT 1";

        public const string GET_ITEM_MAKE_CODE = "Select DISTINCT Make,ItemCode from advances.purchaseapprovaldetail where ItemName=@itemname AND Size=@size AND Unit=@unit And `Status` ='Approved'  ORDER BY  ItemName,Make"; 
      
        public const string GET_ITEM_DETAILS = "Select ItemCode,ItemName,Make,Size,Unit,CurRate,DATE_FORMAT(IniOn,'%d.%m.%Y') 'LastPur' from advances.purchaseapprovaldetail where ItemCode=@itemcode AND Make=@make And `Status` ='Approved'  ORDER BY  IniOn DESC LIMIT 1";


        public const string GET_ALLOWED_CAMPUS_CODES = "SELECT DISTINCT A.CampusCode  from gla_student_management.employee_campus_master A, gla_student_management.campus_master B WHERE A.CampusCode=B.CampusCode AND A.EmployeeCode=@EmployeeCode AND A.ApplicationType='FINANCE' AND B.IsActive=1 ORDER BY A.CampusCode;";

        public const string GET_ALL_MAAD = "Select `Value` as 'Text',UPPER(`Value`) 'Value' from othervalues where Type = 'Additional Name' ORDER BY Value";

        public const string GET_ALL_DEPARTMENTS = "select DISTINCT `name` from salary_management.emp_department WHERE  @ChangeCondition ORDER BY `name` ";

        public const string GET_ALL_VENDORS = "select CONCAT(VendorName,' [ ',DepartmentName,' ] ',Address) 'Text',VendorID 'Value' from purchase.vendorregister A, purchase.departmentregister B where A.DepartmentCode=B.DepartmentCode AND VendorName LIKE CONCAT('%',@VendorName,'%')  ORDER BY VendorName,DepartmentName";

        public const string GET_VENDOR_SUBFIRMS = "select Offices from vendoroffices where VendorID=@VendorID order by Offices";

        public const string GET_ALL_EMPLOYEES = "select CONCAT(first_name,' - ',deisgnation,' [',santioneddeptt,']') 'Text',CONCAT(employee_code,'#',first_name,'#',deisgnation,'#',santioneddeptt) 'Value',employee_code from salary_management.emp_master where `status`!='INACTIVE' AND staff_type!='IV CLASS' AND (first_name LIKE CONCAT('%',@EmployeeName,'%') OR employee_code LIKE CONCAT('%',@EmployeeCode,'%') ) ORDER BY first_name,deisgnation desc,santioneddeptt";

        public const string GET_VENDOR_BUDGET_ENABLE = "Select ForType from othervalues where Type='Budget Approval' And TRIM(REPLACE(SUBSTRING_INDEX(SUBSTRING_INDEX(ForType,'#',2),'#',-1),'in','')) = CONCAT('(',@VendorId,')') ;";
        public const string CHECK_BUGET_EXISTENSE = "Select Amount,`Status`,DATE_FORMAT(BudFrom,'%Y-%m-%d') 'Fr',DATE_FORMAT(BudTill,'%Y-%m-%d') 'To',ReferenceNo from budgetapprovalsummary where VendorID =@VendorId And PExtra3=@SubFirm And `Status` in ('Approved','Pending') And BudFrom<=DATE(now()) And BudTill>=DATE(now()) ;";

        public const string CHECK_IS_VENDOR_BUGET_PENDING_OTHER_APPOROVALS = "Select IniName,Amount,DATE_FORMAT(AppDate,'%d %b,%Y') 'Date',ReferenceNo from otherapprovalsummary where VendorID =@VendorId And PExtra3=@SubFirm And `Status` in ('Pending') And @FromDate<=AppDate And @ToDate>=AppDate; ;";

        public const string CHECK_IS_VENDOR_BUGET_PENDING_PURCHASE_APPOROVALS = "Select IniName,Amount,DATE_FORMAT(AppDate,'%d %b,%Y') 'Date',ReferenceNo from purchaseapprovalsummary where VendorID =@VendorId  And PExtra3= @SubFirm And `Status` in ('Pending') And @FromDate<=AppDate And @ToDate>=AppDate;";

        public const string GET_VENDOR_BUDGET_DETAILS = "Select IFNULL(SUM(A.Amount),0) 'Amount' from ((Select Amount from otherapprovalsummary where VendorID =@VendorId And PExtra3=@SubFirm And PExtra4 like 'No%' And `Status` in ('Approved') And @FromDate<=AppDate And @ToDate>=AppDate) UNION (Select TotalAmount from purchaseapprovalsummary where VendorID =@VendorId And PExtra3=@SubFirm And `Status` in ('Approved') And @FromDate<=AppDate And @ToDate>=AppDate)) A ; ";


        public const string CHECK_USER_ROLE = "Select * from userroles where employee_code=@EmployeeCode And @ColumnName is not null And ((CAST(@ColumnName as CHAR)='1') or (CAST(@ColumnName as CHAR) ='Yes'))";

        public const string GET_FILE_ENC_KEY = "SELECT edkey from gla_student_management.barriers";

    }
}
