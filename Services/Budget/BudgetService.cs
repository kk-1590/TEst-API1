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
        public async Task<ApiResponse> GetMaadBudgetRequired(LimitRequest limitRequest)
        {
            List<string> list = new List<string>();
            using (DataTable dt = await _budgetRepository.GetRequiredBudget(limitRequest.CampusCode!,limitRequest.Session!))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString() ?? string.Empty);
                }
                return new ApiResponse(StatusCodes.Status200OK,"Success",list);
            }
        }
        public async Task<ApiResponse> GetMaadNonBudgetRequired(LimitRequest limitRequest)
        {
            List<string> list = new List<string>();
            using (DataTable dt = await _budgetRepository.GetRequiredBudget(limitRequest.CampusCode!,limitRequest.Session!))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(dr[0].ToString() ?? string.Empty);
                }
                return new ApiResponse(StatusCodes.Status200OK,"Success",list);
            }
        }
        public async Task<ApiResponse> GetAddedMaad(string RefNo)
        {
            using(DataTable dt=await _budgetRepository.GetMaadAddedDetails(RefNo))
            {
                List< AddedMaadResponse > lst= new List< AddedMaadResponse >();
                foreach(DataRow dr in dt.Rows)
                {
                    lst.Add(new AddedMaadResponse(dr));
                }
                return new ApiResponse(StatusCodes.Status200OK,"Success",lst);
            }
        }
        public async Task<ApiResponse> UpdateaadDetails(string EmpCode, UpdateBudgetDetails updateBudgetDetails)
        {
            // Task<int> UpdateBudgetDetails(string EmpCode,UpdateBudgetDetails updateBudgetDetails)
            int ins=await _budgetRepository.UpdateBudgetDetails(EmpCode,updateBudgetDetails);
            if(ins>0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", $"`{ins}` Record Updated Successfully");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", $"Sorry Some Error Occured During This Operation");

            }
        }
        public async Task<ApiResponse> DeleteaadDetails( UpdateBudgetDetails updateBudgetDetails)
        {
            int ins =await _budgetRepository.DeleteDetails(updateBudgetDetails);
            if(ins>0)
            {
                return new ApiResponse(StatusCodes.Status200OK, "Success", $"`{ins}` Record Delete Successfully");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", $"Sorry Some Error Occured During This Operation");

            }
        }
        public async Task<ApiResponse> CheckAlreadyAddedMaadDetails(List<AddBudgetDetailsRequest> Adddetails)
        {
            //  Task<DataTable> CheckAllreadyAddedDetails(List<AddBudgetDetailsRequest> add)
           
            using(DataTable dt=await _budgetRepository.CheckAllreadyAddedDetails(Adddetails))
            {
                string str = string.Empty;
                if (dt.Rows.Count > 0) 
                {
                    foreach (DataRow row in dt.Rows) 
                    {

                        str += row["Maad"].ToString()+" , ";
                    }
                }
                if(str.Length > 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", str+="\n Maad Already Added");
                }
                else
                {
                    return new ApiResponse(StatusCodes.Status200OK, "Success", "");
                }
                

            }
        }
        public async Task<ApiResponse> GetCreateBudgetSummaryDepartments(string? employeeId, string? campusCode)
        {
            if (!_general.IsValidCampusCode(campusCode))
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Campus Found...");
            }

            bool alldepartments = (await _budgetRepository.CheckAllDepartmentBudgetAllowed(employeeId)).Rows.Count > 0;

            using (DataTable dt = await _budgetRepository.GetBudgetDepartments(employeeId, campusCode, alldepartments))
            {
                List<string> departments = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    departments.Add(dr["name"]?.ToString() ?? string.Empty);
                }

                return new ApiResponse(StatusCodes.Status200OK, "Success", departments);
            }



        }

        public async Task<ApiResponse> CreateBudgetSummary(string? employeeId, CreateDepartmentBudgetSummaryRequest? budgetRequest)
        {

            if (budgetRequest == null)
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Budget Details Found...");
            }

            if (budgetRequest?.BudgetAmount <= 0)
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Budget Amount Must Be Greater Than Zero...");
            }

            using (DataTable dtDepartmentExists = await _budgetRepository.CheckIsValidDepartmentBudgetDepartment(budgetRequest))
            {
                if (dtDepartmentExists.Rows.Count == 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", "Sorry!! Invalid Department Found For Budget Summary...");
                }
            }

            using (DataTable dtExists = await _budgetRepository.CheckIsDepartmentBudgetSummaryExists(budgetRequest))
            {

                if (dtExists.Rows.Count > 0)
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", $"Sorry!! Budget Details Already Exists.");
                }

                string newBudgetReferenceNo = string.Empty;

                using (DataTable dtNewBudgetNo = await _budgetRepository.GetNewDepartmentBudgetSummaryReferenceNo())
                {
                    if (dtNewBudgetNo.Rows.Count > 0)
                    {
                        newBudgetReferenceNo = dtNewBudgetNo.Rows[0]["ReferenceNo"]?.ToString() ?? string.Empty;
                    }
                }

                if (string.IsNullOrWhiteSpace(newBudgetReferenceNo))
                {
                    return new ApiResponse(StatusCodes.Status422UnprocessableEntity, "Error", $"Sorry!! Unable to generate Budget Reference No at this moment. Please try again.");
                }

                await _budgetRepository.CreateNewBudgetSummary(employeeId, newBudgetReferenceNo, _general.GetIpAddress(), budgetRequest);

                return new ApiResponse(StatusCodes.Status200OK, "Success", newBudgetReferenceNo);

            }
        }

    }
}
