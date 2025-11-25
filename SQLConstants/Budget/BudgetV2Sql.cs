namespace AdvanceAPI.SQLConstants.Budget
{
    public static class BudgetV2Sql
    {
        public const string GET_BUDGET_SESSIONS_FOR_FILTER = "SELECT DISTINCT A.`Session` FROM `budget_session_amount_summary` A, gla_student_management.campus_master B WHERE A.CampusCode=B.CampusCode AND A.CampusCode=@CampusCode AND B.IsActive=1 AND A.`Status`='Active' ORDER By A.`Session` DESC;";

        public const string GET_BUDGET_SESSIONS_AMOUNT_SUMMARY = "SELECT A.Id AS 'BudgetId',A.`Session`,B.CampusName,A.CampusCode,A.BudgetAmount,A.UsedAmount,A.RemainingAmount,A.LockStatus FROM `budget_session_amount_summary` A, gla_student_management.campus_master B WHERE A.CampusCode=B.CampusCode AND B.IsActive=1 AND A.`Status`='Active' @ChangeCondition ORDER BY `Session` DESC,CampusCode LIMIT @LimitItems OFFSET @OffSetItems ;;";

        public const string GET_BUDGET_SESSION_EDITABLE_SUMMARY_EXISTS = "SELECT * from `budget_session_amount_summary` WHERE Id=@BudgetId  AND `Status`='Active' AND LockStatus='Pending'";

        public const string GET_BUDGET_SESSION_SUMMARY_EXISTS = "SELECT 1 FROM `budget_session_amount_summary` where `Session`=@Session AND CampusCode=@CampusCode AND `Status`='Active'; ";

        public const string UPDATE_BUDGET_SESSION_SUMMARY_AMOUNT = "UPDATE `budget_session_amount_summary` SET BudgetAmount = @BudgetAmount,UpdatedBy=@UpdatedBy,UpdatedFrom=@UpdatedFrom,UpdatedOn=NOW(),UpdateRemark=IF(UpdateRemark IS NULL,@UpdateRemark,CONCAT(UpdateRemark,' $$$ ',@UpdateRemark)) WHERE Id=@BudgetId  AND `Status`='Active' AND LockStatus='Pending'";

        public const string UPDATE_BUDGET_SESSION_SUMMARY_AMOUNT_CALCULATIONS = "UPDATE  `budget_session_amount_summary` SET RemainingAmount=(BudgetAmount-UsedAmount) WHERE Id=@BudgetId  AND `Status`='Active';";

        public const string ADD_BUDGET_SESSION_SUMMARY_AMOUNT = "INSERT INTO  `budget_session_amount_summary` (`Session`,CampusCode,BudgetAmount,AddedBy,AddedOn,AddedFrom) VALUES (@Session,@CampusCode,@BudgetAmount,@AddedBy,NOW(),@AddedFrom)";

        public const string DELETE_BUDGET_SESSION_SUMMARY_AMOUNT = "UPDATE `budget_session_amount_summary` SET `Status`='Deleted',DeletedBy=@DeletedBy,DeletedOn=NOW(),DeletedFrom=@DeletedFrom,UpdateRemark=IF(UpdateRemark IS NULL,@UpdateRemark,CONCAT(UpdateRemark,' $$$ ',@UpdateRemark)) WHERE Id=@BudgetId AND `Status`='Active' AND LockStatus='Pending';";

        public const string LOCK_BUDGET_SESSION_SUMMARY_AMOUNT = "UPDATE `budget_session_amount_summary` SET `LockStatus`='Locked',UpdatedBy=@UpdatedBy,UpdatedFrom=@UpdatedFrom,UpdatedOn=NOW(),UpdateRemark=IF(UpdateRemark IS NULL,@UpdateRemark,CONCAT(UpdateRemark,' $$$ ',@UpdateRemark)) WHERE Id=@BudgetId AND `Status`='Active' AND LockStatus='Pending';";

        public const string GET_BUDGET_MAAD_FILTER = "SELECT BudgetMaad from budget_maad_list WHERE `Status`='Active' AND BudgetMaad LIKE '%@Maad%' ORDER BY BudgetMaad LIMIT 15;";

        public const string CHECK_ALREADY_ADDED_DEPARTMENT_DETAILS = "SELECT * FROM department_budget_details WHERE ReferenceNo=@RefNo AND BudgetHead=@BudgetHead AND BudgetMaad=@BudgetMaad AND BudgetType=@BudgetType;";
        public const string GET_ALREADY_ADDED_MAAD = "SELECT * from budget_maad_list WHERE BudgetMaad=@BudgetMaad and `Status`='Active';";
        public const string ADD_MAAD_IN_LIST = "INSERT INTO budget_maad_list(BudgetMaad,AddedOn,AddedFrom,AddedBy) VALUES (@Maad,NOW(),@IpAddress,@AddBy);";

        public const string ADD_DEPARTMENT_BUDGET_DETAILS = "INSERT INTO department_budget_details (ReferenceNo,BudgetType,BudgetHead,BudgetMaad,BudgetAmount,AllowOverBudgetApproval,AddedOn,AddedFrom,AddedBy) VALUES \r\n(@ReferenceNo,@BudgetType,@BudgetHead,@BudgetMaad,@BudgetAmount,@AllowOverBudgetApproval,NOW(),@AddedFrom,@AddedBy)";

        public const string GET_ALL_BUDGET_SESSION_SUMMARY_AMOUNT_SESSIONS = "SELECT DISTINCT `Session` FROM `budget_session_amount_summary` WHERE `Status`='Active' ORDER BY `Session` DESC;";

        public const string GET_DEPARMENT_BUDGET_DETAILS = "SELECT Id,ReferenceNo,BudgetType,BudgetHead,BudgetMaad,BudgetAmount,AllowOverBudgetApproval FROM department_budget_details WHERE ReferenceNo=@RefNo and Status='Active'";

        public const string UPDATE_DEPARTMENT_BUDGET_DETAILS = "UPDATE department_budget_details SET BudgetType=@BudgetType,BudgetHead=@BudgetHead,BudgetMaad=@BudgetMaad,BudgetAmount=@BudgetAmount,AllowOverBudgetApproval=@AllowOverBudgetApproval,UpdatedBy=@UpdateBy,UpdatedOn=NOW(),UpdatedFrom=@IP WHERE Id=@Id;";


        public const string GET_BUDGET_TYPE_HEAD_MAPPING = "SELECT A.Id,A.`Session`,A.CampusCode,B.CampusName,A.BudgetType,A.BudgetHead FROM `budget_type_head_mapping` A, gla_student_management.campus_master B WHERE A.CampusCode=B.CampusCode AND B.IsActive=1 AND A.`Status`='Active' AND A.`Session`=@Session AND A.CampusCode=@CampusCode @ExtraCondition ORDER BY A.Id LIMIT @LimitItems OFFSET @OffSetItems ;";

        public const string CHECK_BUDGET_TYPE_HEAD_MAPPING_ALREADY_EXISTS = "SELECT * from budget_type_head_mapping where `Session`=@Session AND CampusCode=@CampusCode AND BudgetType=@BudgetType AND BudgetHead=@BudgetHead";

        public const string ADD_BUDGET_TYPE_HEAD_MAPPING = "INSERT INTO `budget_type_head_mapping` ( `Session`,CampusCode,BudgetType,BudgetHead,AddedBy,AddedOn,AddedFrom) VALUES ( @Session,@CampusCode,@BudgetType,@BudgetHead,@AddedBy,NOW(),@AddedFrom)";

        public const string CHECK_BUDGET_TYPE_HEAD_MAPPING_EXISTS_BY_ID = "SELECT * FROM `budget_type_head_mapping` WHERE Id=@HeadMappingId and `Status`='Active'";

        public const string CHECK_BUDGET_TYPE_HEAD_MAPPING_USED_OR_NOT = "SELECT 1 FROM `department_budget_summary` A, department_budget_details B , budget_type_head_mapping C WHERE A.ReferenceNo=B.ReferenceNo AND A.`Status`='Active' AND A.`Session`=C.`Session` AND B.BudgetType=C.BudgetType AND B.BudgetHead=C.BudgetHead AND A.CampusCode=C.CampusCode AND C.Id=@HeadMappingId ";

        public const string DELETE_BUDGET_TYPE_HEAD_MAPPING = "UPDATE `budget_type_head_mapping` SET `Status`='Deleted',DeletedBy=@DeletedBy,DeletedOn=NOW(),DeletedFrom=@DeletedFrom,UpdateRemark=IF(UpdateRemark IS NULL,@UpdateRemark,CONCAT(UpdateRemark,' $$$ ',@UpdateRemark)) WHERE Id=@HeadMapId ";

        public const string CHECK_ALL_DEPARTMENT_BUDGET_ALLOWED = "SELECT 1 FROM advances.`othervalues` where Type='All Department Budget Allowed' AND FIND_IN_SET(@EmployeeId,`Value`);";

        public const string GET_ALL_BUGDET_DEPARTMENT = "select DISTINCT `name` from salary_management.emp_department WHERE FIND_IN_SET(@CampusCode,CampusCodes) ORDER BY `name`;";

        public const string GET_ALLOWED_BUGDET_DEPARTMENT = "SELECT DISTINCT A.`name` from salary_management.emp_department A, salary_management.departmenthod B WHERE A.`name`=B.Department AND B.EmployeeCode=@EmployeeId AND FIND_IN_SET(@CampusCode,A.CampusCodes) ORDER BY A.`name`;";


        public const string CHECK_IS_DEPARTMENT_BUDGET_SUMMARY_EXISTS = "SELECT ReferenceNo FROM department_budget_summary where `Session`=@Session AND CampusCode=@CampusCode AND Department=@Department AND `Status`!='Deleted';;";

        public const string GET_NEW_DEPARTMENT_BUDGET_SUMMARY_REFERENCE_NO = "SELECT CAST(IFNULL(MAX(ReferenceNo),  CONCAT(DATE_FORMAT(NOW(),'%y%m%d'),'0001')) AS UNSIGNED ) 'ReferenceNo' FROM `department_budget_summary` WHERE ReferenceNo LIKE CONCAT(DATE_FORMAT(NOW(),'%y%m%d'),'%');";

        public const string CREATE_NEW_BUDGET_SUMMARY = "INSERT INTO `department_budget_summary` (ReferenceNo,`Session`,CampusCode,Department,BudgetName,InitiatedBy,InitiatedOn,InitiatedFrom) VALUES (@ReferenceNo,@Session,@CampusCode,@Department,@BudgetName,@InitiatedBy,NOW(),@InitiatedFrom);;";

        public const string GET_IS_VALID_BUGDET_DEPARTMENT = "select DISTINCT `name` from salary_management.emp_department WHERE FIND_IN_SET(@CampusCode,CampusCodes)  AND name=@Department;";

        public const string GET_DEPARTMENT_BUDGET_SUMMARY = "SELECT A.ReferenceNo,A.`Session`,B.CampusName,A.CampusCode,A.Department,A.BudgetName,A.BudgetAmount,A.RecurringBudgetAmount,A.NonRecurringBudgetAmount,A.BudgetAmountUsed,A.RecurringBudgetAmountUsed,A.NonRecurringBudgetAmountUsed,A.BudgetAmountRemaining,A.RecurringBudgetAmountRemaining,A.NonRecurringBudgetAmountRemaining,A.`Status`,A.BudgetStatus FROM `department_budget_summary` A, gla_student_management.campus_master B WHERE A.CampusCode=B.CampusCode AND A.`Status`='Active' AND B.IsActive=1 AND A.`Session`=@Session  AND A.CampusCode=@CampusCode  @ChangeCondition ORDER BY ReferenceNo DESC  LIMIT @LimitItems OFFSET @OffSetItems ;";

        public const string DELETE_DEPARMENT_BUDGETDETAILS = "UPDATE department_budget_details SET `Status`='Deleted',DeletedBy=@DeletedBy,DeletedFrom=@Ip,DeletedOn=NOW() WHERE Id=@Id and Status='Active'\r\n";
        public const string ISVALIDDEPARMENTFORDELETE = "select * department_budget_details WHERE Id=@Id and ReferenceNo=@refNo and Status='Active'";
        public const string IsValidDetails = "select * department_budget_details WHERE  ReferenceNo=@refNo and Status='Active'";

        public const string GET_BUDGETDETAILS = "SELECT BudgetType,BudgetHead,BudgetMaad,BudgetAmount FROM department_budget_details WHERE `Status`='Active' AND ReferenceNo=@ReferenceNo;\r\n";

        public const string UPDATE_BUDGET_SUMMARY = "UPDATE department_budget_summary SET BudgetAmount=@BudgetAmount,RecurringBudgetAmount=@RecurringBudgetAmount,NonRecurringBudgetAmount=@NonRecurringBudgetAmount,BudgetAmountRemaining=@BudgetAmountRemaining,RecurringBudgetAmountRemaining=@RecurringBudgetAmountRemaining,NonRecurringBudgetAmountRemaining=@NonRecurringBudgetAmountRemaining,BudgetStatus='Created' WHERE ReferenceNo=@ReferenceNo AND BudgetStatus='Initiated' AND `Status`='Active'\r\n";
        public const string UPDATE_BUDGET_DETAILS = "UPDATE department_budget_details SET Status='Lock',LockBy=@EmpCode,LockOn=NOW(),LockFrom=@Ip WHERE  `Status`='Active' AND ReferenceNo=@ReferenceNo";


        public const string GETBUDGETHEADFILTER = "SELECT BudgetHead FROM `budget_type_head_mapping` WHERE BudgetType=@Type AND `Status`='Active';";
    }
}
