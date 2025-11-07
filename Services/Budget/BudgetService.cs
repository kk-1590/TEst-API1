using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Budget;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.Budget;
using System.Data;

namespace AdvanceAPI.Services.Budget
{
    public class BudgetService : IBudget
    {

        private readonly IBudgetRepository _budgetRepository;
        private readonly IGeneral _general; 
        public BudgetService(IBudgetRepository budgetRepository,IGeneral general)
        {
            _budgetRepository = budgetRepository;
            _general = general;
        }

        public async Task<ApiResponse> AddItemWithSession(string EmpCode, MapNewMaad mapNewMaad)
        {
            using (DataTable d = await _budgetRepository.CheckAlreadyAdded(mapNewMaad)) 
            {
                if (d.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", $"`{mapNewMaad.Maad}` Already Added With Current Session");
                }
                else
                {
                    int ins = await _budgetRepository.AddDetails(EmpCode, mapNewMaad);
                    if (ins > 0) 
                    {
                        return new ApiResponse(StatusCodes.Status200OK, "Success", $"`{mapNewMaad.Maad}` Added Successfully");
                    }
                    else
                    {
                        return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", $"Sorry Some Error Occured During This Operation");
                    }
                }
            }

           return new ApiResponse(StatusCodes.Status200OK, null);
        }
        public async Task<ApiResponse> UpdateBudgetMaad(string EmpCode, UpdateMaadBudegtRequest updateMaadBudegtRequest)
        {
            //Task<int> UpdateMaadForBudget(string EmpCode, UpdateMaadBudegtRequest updateMaadBudegtRequest)
            int ins=await _budgetRepository.UpdateMaadForBudget(EmpCode, updateMaadBudegtRequest);
            if (ins > 0)
            {
                return new ApiResponse(StatusCodes.Status200OK,"Success",$"`{ins}` Record Update Successfully");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Record Not Update/Invelid Access");
            }
        }
        public async Task<ApiResponse> GetRecord(int Limit, int Offset,string CampusCode,string Session,int BudgetRequired)
        {
            using (DataTable dt = await _budgetRepository.GetMaadBudgetDetails(Limit, Offset,CampusCode,Session,BudgetRequired))
            {
                List<MapDetailsResponse> lst = new List<MapDetailsResponse>();
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new MapDetailsResponse
                    {
                        Session = dr["Session"].ToString(),
                        CampusCode = dr["CampusCode"].ToString(),
                        CampusName = _general.CampusNameByCode(dr["CampusCode"].ToString()!),
                        Maad = dr["Maad"].ToString(),
                        IsBudgetRequired = Convert.ToInt16(dr["IsBudgetRequired"].ToString()),
                        Status = Convert.ToInt16(dr["Status"].ToString()),
                        id = Convert.ToInt32(dr["Id"].ToString()),

                    });
                }
                return new ApiResponse(StatusCodes.Status200OK, "Success", lst);
            }
        }

    }
}
