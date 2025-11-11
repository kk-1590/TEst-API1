namespace AdvanceAPI.SQLConstants.Budget
{
    public class BudgetSql
    {
        public const string CHECK_ALREADY_MAAD = "SELECT * FROM maad_budget_mapping WHERE `Session`=@Session AND Maad=@Maad and CampusCode=@CampusCode";
        public const string INS__QUERY = "INSERT INTO `maad_budget_mapping` (`Session`,CampusCode,Maad,IsBudgetRequired,AddedBy,AddedOn,AddedFrom) VALUES (@Session,@CampusCode,@Maad,@IsBudgetRequired,@AddedBy,NOW(),@AddedFrom)";
        public const string UPDATE_MAAD_BUDGET_MAPPING = "UPDATE maad_budget_mapping SET `Status`=@Status,IsBudgetRequired=@IsBudgetRequired,UpdatedBy=@UpdatedBy,UpdatedFrom=@UpdatedFrom,UpdatedOn=now(),UpdateRemark=@UpdateRemark WHERE Id=@Id AND `Session`=@Session AND Maad=@Maap and CampusCode=@CampusCode";

        public const string GET_BUDGET_MAAD = "SELECT `Session`,CampusCode,Maad,IsBudgetRequired,Id,Status FROM maad_budget_mapping WHERE `Status`=1 and Session=@Session and CampusCode=@CampusCode @Condition ORDER BY Maad,CampusCode LIMIT @Limit OFFSET @OffSet";
        public const string GET_REQUIRED_BUDGET_MAAD = "SELECT DISTINCT Maad from maad_budget_mapping WHERE IsBudgetRequired=1 and `Status`=1 AND CampusCode=@CampusCode AND `Session`=@Session;";
        public const string GET_NON_REQUIRED_BUDGET_MAAD = "SELECT DISTINCT Maad from maad_budget_mapping WHERE IsBudgetRequired=0 and `Status`=1 AND CampusCode=@CampusCode AND `Session`=@Session;";
        public const string GET_ADDED_MAAD = "SELECT Maad,BudgetAmount,Id,ReferenceNo FROM `department_budget_details` WHERE ReferenceNo=@RefNo;";

        public const string UPDATE_BUDGET_DETAILS = "UPDATE department_budget_details SET BudgetAmount=@BudgetAmount,UpdatedOn=NOW(),UpdatedFrom=@IpAddress,UpdatedBy=@EmpCode WHERE Id=@Id AND ReferenceNo=@RefNo";

        public const string DELETE_BUDGET_DETAILS = "DELETE FROM department_budget_details WHERE Id=@Id AND ReferenceNo=@RefNo";



        public const string CHECK_ALREADY_ADDED_ITEM = "SELECT * FROM department_budget_details WHERE ReferenceNo=@RefNo AND Maad in (@Maad) AND `Status`='Active';";


        public const string CHECK_ALL_DEPARTMENT_BUDGET_ALLOWED = "SELECT 1 FROM advances.`othervalues` where Type='All Department Budget Allowed' AND FIND_IN_SET(@EmployeeId,`Value`);";

        public const string GET_ALL_BUGDET_DEPARTMENT = "select DISTINCT `name` from salary_management.emp_department WHERE FIND_IN_SET(@CampusCode,CampusCodes) ORDER BY `name`;";

        public const string GET_IS_VALID_BUGDET_DEPARTMENT = "select DISTINCT `name` from salary_management.emp_department WHERE FIND_IN_SET(@CampusCode,CampusCodes)  AND name=@Department;";

        public const string GET_ALLOWED_BUGDET_DEPARTMENT = "SELECT DISTINCT A.`name` from salary_management.emp_department A, salary_management.departmenthod B WHERE A.`name`=B.Department AND B.EmployeeCode=@EmployeeId AND FIND_IN_SET(@CampusCode,A.CampusCodes) ORDER BY A.`name`;";

        public const string CHECK_IS_DEPARTMENT_BUDGET_SUMMARY_EXISTS = "SELECT ReferenceNo FROM department_budget_summary where `Session`=@Session AND CampusCode=@CampusCode AND Department=@Department AND `Status`!='Deleted';;";
        public const string GET_NEW_DEPARTMENT_BUDGET_SUMMARY_REFERENCE_NO = "SELECT CAST(IFNULL(MAX(ReferenceNo),  CONCAT(DATE_FORMAT(NOW(),'%y%m%d'),'0001')) AS UNSIGNED ) 'ReferenceNo' FROM `department_budget_summary` WHERE ReferenceNo LIKE CONCAT(DATE_FORMAT(NOW(),'%y%m%d'),'%');";

        public const string CREATE_NEW_BUDGET_SUMMARY = "INSERT INTO `department_budget_summary` (ReferenceNo,`Session`,CampusCode,Department,BudgetName,BudgetAmount,InitiatedBy,InitiatedOn,InitiatedFrom) VALUES (@ReferenceNo,@Session,@CampusCode,@Department,@BudgetName,@BudgetAmount,@InitiatedBy,NOW(),@InitiatedFrom);;";

        public const string GET_DEPARTMENT_BUDGET_SUMMARY_RESTRICTED = "SELECT A.ReferenceNo,A.`Session`,A.CampusCode,A.Department,A.BudgetName,A.BudgetAmount,A.BudgetStatus,DATE_FORMAT(A.InitiatedOn,'%d.%m.%Y %r')'InitiatedOn'  FROM `department_budget_summary` A, salary_management.departmenthod B  WHERE A.Department=B.Department AND A.`Session`=@Session AND A.CampusCode=@CampusCode   AND A.`Status`!='Deleted' @AdditionalCondition  AND (A.InitiatedBy=@EmployeeId OR (B.EmployeeCode=@EmployeeId AND B.`Status`='Activated')) LIMIT @LimitItems OFFSET @OffSetItems;";

        public const string GET_DEPARTMENT_BUDGET_SUMMARY_ALL = "SELECT A.ReferenceNo,A.`Session`,A.CampusCode,A.Department,A.BudgetName,A.BudgetAmount,A.BudgetStatus,DATE_FORMAT(A.InitiatedOn,'%d.%m.%Y %r')'InitiatedOn'  FROM `department_budget_summary` A  WHERE A.`Session`=@Session AND A.CampusCode=@CampusCode  AND A.`Status`!='Deleted' @AdditionalCondition LIMIT @LimitItems OFFSET @OffSetItems ;";
       
      
    }
}
