using System.Data;
using AdvanceAPI.DTO;
using AdvanceAPI.DTO.Inclusive;
using AdvanceAPI.DTO.VenderPriceComp;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices.VenderPriceComp;

namespace AdvanceAPI.Services.VenderPriceCompServices;

public class VenderPriceCompServices : IVenderPriceComparisionServices
{
    private readonly IVenderPriceCompRepository _repository;
   
    
    public VenderPriceCompServices(IVenderPriceCompRepository repository)
    {
        _repository = repository;
       
    }
    public async Task<ApiResponse> GetApprovalBasicDetails(string RefNo)
    {
        using (DataTable Details=await _repository.GetBasicPurchaseApprovalDetails(RefNo))
        {
            if (Details.Rows.Count > 0)
            {
                
                BasicDetailsResponse basicDetailsResponse = new BasicDetailsResponse();
                basicDetailsResponse.ReferenceNo = Details.Rows[0]["ReferenceNo"].ToString()??string.Empty;
                basicDetailsResponse.Type = Details.Rows[0]["MyType"].ToString()??string.Empty;
                basicDetailsResponse.Purpose = Details.Rows[0]["Purpose"].ToString()??string.Empty;
                basicDetailsResponse.Note = Details.Rows[0]["Note"].ToString()??string.Empty;
                basicDetailsResponse.TotalBalance = Details.Rows[0]["TotalAmount"].ToString()??string.Empty;
                using (DataTable chkDetails = await _repository.GetBasiclockDetails(RefNo)) 
                {
                    if (chkDetails.Rows.Count > 0)
                    {
                       // myLock = chkDetails.Rows[0][0].ToString()??string.Empty;
                       basicDetailsResponse.Url = chkDetails.Rows[0][1].ToString()??string.Empty;
                    }
                }
                // basicDetailsResponse.Url = Details.Rows[0]["Url"].ToString();
                return new ApiResponse(StatusCodes.Status200OK,"Success", basicDetailsResponse);
            }
            else
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity,"Error","Record not found");
            }
        }
    }

    public async Task<ApiResponse> GetItemDetails(string RefNo)
    {
        using (DataTable details=await _repository.GetBasicPurchaseApprovalDetails(RefNo))
        {
            DataTable LocakDetails=await _repository.GetBasiclockDetails(RefNo);
            string Lock=string.Empty;
            if (LocakDetails.Rows.Count > 0)
            {
                Lock=LocakDetails.Rows[0][0].ToString()??string.Empty;
            }
            
            List<PurchaseItemList> purchaseItemLists = new List<PurchaseItemList>();
            List<PurchaseItemList> Vender2lst = new List<PurchaseItemList>();
            List<PurchaseItemList> Vender3lst = new List<PurchaseItemList>();
            
            foreach (DataRow dr in details.Rows)
            {
                purchaseItemLists.Add(new PurchaseItemList
                {
                    VenderName = dr["FirmName"].ToString(),
                    VendorId = dr["VendorID"].ToString(),
                    Unit = dr["Unit"].ToString(),
                    ItemCode = dr["ItemCode"].ToString(),
                    ItemName = dr["ItemName"].ToString(),
                    Quantity = dr["Quantity"].ToString(),
                    Price = dr["CurRate"].ToString(),
                    Lock = Lock
                });
                DataTable getVenderDetails = await _repository.GetVenderDetails(RefNo,dr["ItemCode"].ToString());
                DataRow[] Vender2Details = getVenderDetails.Select("VendorNo=2");
                DataRow[] Vender3Details = getVenderDetails.Select("VendorNo=3");
                if (Vender2Details != null && Vender2Details.Length > 0)
                {
                    Vender2lst.Add(new PurchaseItemList
                    {
                        VenderName = Vender2Details[0]["VendorName"].ToString()??string.Empty,
                        VendorId = Vender2Details[0]["VendorID"].ToString()??string.Empty,
                        Unit = "",
                        ItemCode = Vender2Details[0]["ItemCode"].ToString()??string.Empty,
                        ItemName = Vender2Details[0]["ItemName"].ToString()??string.Empty,
                        Quantity = "",
                        Price = Vender2Details[0]["ItemPrice"].ToString()??string.Empty,
                        Lock = Vender2Details[0]["Lock"].ToString()??string.Empty
                    });
                }
                else
                {
                    Vender2lst.Add(new PurchaseItemList
                    {
                        VenderName = String.Empty,
                        VendorId = string.Empty,
                        Unit = "",
                        ItemCode = string.Empty,
                        ItemName = string.Empty,
                        Quantity = "",
                        Price = string.Empty,
                        Lock = Lock
                    });
                }
                if (Vender3Details != null && Vender3Details.Length > 0)
                {
                    Vender3lst.Add(new PurchaseItemList
                    {
                        VenderName = Vender3Details[0]["VendorName"].ToString()??string.Empty,
                        VendorId = Vender3Details[0]["VendorID"].ToString()??string.Empty,
                        Unit = "",
                        ItemCode = Vender3Details[0]["ItemCode"].ToString()??string.Empty,
                        ItemName = Vender3Details[0]["ItemName"].ToString()??string.Empty,
                        Quantity = "",
                        Price = Vender3Details[0]["ItemPrice"].ToString()??string.Empty,
                        Lock = Vender3Details[0]["Lock"].ToString()??string.Empty
                    });
                }
                else
                {
                    Vender3lst.Add(new PurchaseItemList
                    {
                        VenderName = String.Empty,
                        VendorId = string.Empty,
                        Unit = "",
                        ItemCode = string.Empty,
                        ItemName = string.Empty,
                        Quantity = "",
                        Price = string.Empty,
                        Lock = Lock
                    });
                }
            }
            return new ApiResponse(StatusCodes.Status200OK,"Success", new {Vender1=purchaseItemLists,Vender2=Vender2lst,Vender3=Vender3lst});
        }
    }

    public async Task<ApiResponse> GetVendorDetails(string RefNo)
    {
        using (DataTable dt=await _repository.GetVendorDetails(RefNo))
        {
            if (dt.Rows.Count == 0)
            {
                int ins = await _repository.InsertVenderInChart(RefNo);
            }
            List<TextValue> lst = new List<TextValue>();
            DataTable VendorLock = await _repository.CheckLockComparisionChart(RefNo);
            if (VendorLock.Rows.Count == 0)
            {
                // DataTable vendorDetails = await _repository.GetAllVendor(RefNo);
                // foreach (DataRow dr in vendorDetails.Rows)
                // {
                //     lst.Add(new TextValue
                //     {
                //         Value = dr["Value"].ToString(),
                //         Text = dr["Text"].ToString(),
                //     });
                // }
                return new ApiResponse(StatusCodes.Status200OK, "Success","Open");
            }
            return new ApiResponse(StatusCodes.Status200OK,"Error","Lock");
        }
    }

    public async Task<ApiResponse> SubmitVendorDetails(string RefNo,string empCode,InsertDetails Details)
    {
        //Task<int> InsertVendoorPrice(string EmpCode,string RefNo,InsertDetails Details)
        using (DataTable dt = await _repository.Checkvendorinchart (Details.ItemCode,Details.VendorNo,RefNo))
        {
            if (dt.Rows.Count == 0)
            {
                int ins=await _repository.InsertVendoorPrice(empCode,RefNo,Details);
                return new ApiResponse(StatusCodes.Status200OK,"Success",$"`{ins}` Record Add Successfully");
            }
            else
            {
                return new ApiResponse(StatusCodes.Status422UnprocessableEntity,"Error","Already Added");
            }
        }
    }
    public async Task<ApiResponse> LockDetails(string RefNo,string empCode)
    {
        //Task<int> InsertVendoorPrice(string EmpCode,string RefNo,InsertDetails Details)
        DataTable VendorLock = await _repository.CheckLockComparisionChart(RefNo);
        if (VendorLock.Rows.Count > 0)
        {
            return new ApiResponse(StatusCodes.Status422UnprocessableEntity,"Error","Details Already Locked");
        }
       int ins=await _repository.LockDetails(empCode,RefNo);
       return new ApiResponse(StatusCodes.Status200OK,"Success",$"`{ins}` Details Lock Successfully");
    }
}