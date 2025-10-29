using System.Data;
using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Approval;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices.Approval;

namespace AdvanceAPI.Services.Approval
{
    public class ApprovalService : IApprovalService
    {
        private readonly IApprovalRepository _approvalRepository;

        public ApprovalService(IApprovalRepository approvalRepository)
        {
            _approvalRepository = approvalRepository;
        }
        public async Task<ApiResponse> AddItemDraft(AddStockItemRequest AddStockItem,string EmpCode)
        {
             string RefNo=string.Empty;
             DataTable refNoDataTable=await _approvalRepository.GetDraftItemRefNo(EmpCode,AddStockItem.ApprovalType);
             if (refNoDataTable.Rows.Count > 0)
             {
                 RefNo=refNoDataTable.Rows[0][0].ToString();
             }
             else
             {
                 refNoDataTable = await _approvalRepository.GetAutoDraftItemRefNo();
                 if (refNoDataTable.Rows.Count > 0)
                 {
                     RefNo = refNoDataTable.Rows[0][0].ToString();
                 }
             }
             int AddItem=await _approvalRepository.AddDraftItem(RefNo,AddStockItem,EmpCode);
             if (AddItem > 0)
             {
                 return new ApiResponse(StatusCodes.Status200OK,"Success","Item Add Successfully");
             }
             else
             {
                 return new ApiResponse(StatusCodes.Status500InternalServerError,"Error","Add Failed");
             }
             
        }
    }
}
