using System.Data;

namespace AdvanceAPI.DTO.Budget
{
    public class DepartmentDetailsResponse
    {
        public int Id { get;set; }
        public string? ReferenceNo { get;set; }
        public string? BudgetType { get;set; }
        public string? BudgetHead { get;set; }
        public string? BudgetMaad { get;set; }
        public string? BudgetAmount { get;set; }
        public int AllowOverBudget { get;set; }

        public DepartmentDetailsResponse(DataRow dr)
        {
            Id = Convert.ToInt32(dr["Id"].ToString());
            ReferenceNo = dr["ReferenceNo"].ToString() ?? string.Empty;
            BudgetType = dr["BudgetType"].ToString() ?? string.Empty;
            BudgetHead = dr["BudgetHead"].ToString() ?? string.Empty;
            BudgetMaad = dr["BudgetMaad"].ToString()?? string.Empty;
            BudgetAmount = dr["BudgetAmount"].ToString()??string.Empty;
            AllowOverBudget = Convert.ToInt32(dr["AllowOverBudgetApproval"].ToString());
        }
    }
}
