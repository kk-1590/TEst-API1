namespace AdvanceAPI.SQLConstants.Budget
{
    public static class BudgetV2Sql
    {
        public const string GET_BUDGET_SESSIONS_FOR_FILTER = "SELECT DISTINCT A.`Session` FROM `budget_session_amount_summary` A, gla_student_management.campus_master B WHERE A.CampusCode=B.CampusCode AND A.CampusCode=@CampusCode AND B.IsActive=1 ORDER By A.`Session` DESC;";

        public const string GET_BUDGET_SESSIONS_AMOUNT_SUMMARY = "SELECT A.`Session`,B.CampusName,A.CampusCode,A.BudgetAmount,A.UsedAmount,A.RemainingAmount FROM `budget_session_amount_summary` A, gla_student_management.campus_master B WHERE A.CampusCode=B.CampusCode AND B.IsActive=1 AND A.`Status`='Active' @ChangeCondition ORDER BY `Session` DESC,CampusCode LIMIT @LimitItems OFFSET @OffSetItems ;;";

    }
}
