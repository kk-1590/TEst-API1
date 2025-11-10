using System.Data;

namespace AdvanceAPI.DTO.Budget
{
    public class AddedMaadResponse
    {
        public string? RefNo { get; set; }
        public int Id { get; set; }
        public string? BudgetAmount { get; set; }
        public string? Maad { get;set; }
        public AddedMaadResponse(DataRow dr) 
        {
            RefNo = dr["ReferenceNo"].ToString();
            Id =Convert.ToInt32( dr["Id"].ToString());
            BudgetAmount =  dr["BudgetAmount"].ToString();
            Maad =  dr["Maad"].ToString();
        }
    }
}
