namespace AdvanceAPI.SQLConstants.Budget
{
    public class BudgetSql
    {
        public const string CHECK_ALREADY_MAAD = "SELECT * FROM maad_budget_mapping WHERE `Session`=@Session AND Maad=@Maad and CampusCode=@CampusCode";
        public const string INS__QUERY = "INSERT INTO `maad_budget_mapping` (`Session`,CampusCode,Maad,IsBudgetRequired,AddedBy,AddedOn,AddedFrom) VALUES (@Session,@CampusCode,@Maad,@IsBudgetRequired,@AddedBy,NOW(),@AddedFrom)";
        public const string UPDATE_MAAD_BUDGET_MAPPING = "UPDATE maad_budget_mapping SET `Status`=@Status,IsBudgetRequired=@IsBudgetRequired,UpdatedBy=@UpdatedBy,UpdatedFrom=@UpdatedFrom,UpdatedOn=now(),UpdateRemark=@UpdateRemark WHERE Id=@Id AND `Session`=@Session AND Maad=@Maap and CampusCode=@CampusCode";

        public const string GET_BUDGET_MAAD = "SELECT `Session`,CampusCode,Maad,IsBudgetRequired,Id,Status FROM maad_budget_mapping WHERE `Status`=1 ORDER BY Maad,CampusCode LIMIT @Limit OFFSET @OffSet";
    }
}
