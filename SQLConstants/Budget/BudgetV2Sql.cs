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

    }
}
