namespace AdvanceAPI.SQLConstants.Budget
{
    public class BudgetSql
    {
        public const string INS__QUERY = "INSERT INTO `maad_budget_mapping` (`Session`,CampusCode,Maad,IsBudgetRequired,AddedBy,AddedOn) VALUES (@Session,@CampusCode,@Maad,@IsBudgetRequired,@AddedBy,NOW())";
    }
}
