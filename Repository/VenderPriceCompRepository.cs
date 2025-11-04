using System.Data;
using AdvanceAPI.DTO.DB;
using AdvanceAPI.DTO.VenderPriceComp;
using AdvanceAPI.ENUMS.DB;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.DB;
using AdvanceAPI.IServices.Inclusive;
using AdvanceAPI.Services.DB;
using AdvanceAPI.Services.VenderPriceCompServices;
using AdvanceAPI.SQLConstants.VenderPriceComperision;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace AdvanceAPI.Repository;

public class VenderPriceCompRepository : IVenderPriceCompRepository
{
    
    private readonly ILogger<VenderPriceCompRepository> _logger;
    private readonly IDBOperations _dbOperations;
    private readonly IGeneral _general;
    private readonly IInclusiveService _inclusiveService;
    public VenderPriceCompRepository( ILogger<VenderPriceCompRepository> logger, IDBOperations dbOperations, IGeneral general, IInclusiveService incusiveRepository)
    {
        
        _logger = logger;
        _dbOperations = dbOperations;
        _general = general;
        _inclusiveService = incusiveRepository;
    }

    public async Task<DataTable> GetBasicPurchaseApprovalDetails(string referenceNo)
    {
        try
        {
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
            sqlParameters.Add(new SQLParameters("@ReferenceNo",referenceNo));
            return await _dbOperations.SelectAsync(VenderPriceComperisionSql.GET_BASIC_DETAILS, sqlParameters,DBConnections.Advance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Error during GetBasicPurchaseApprovalDetails.");
            throw;
        }
    }
    public async Task<DataTable> GetBasiclockDetails(string referenceNo)
    {
        try
        {
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
            sqlParameters.Add(new SQLParameters("@ReferenceNo",referenceNo));
            return await _dbOperations.SelectAsync(VenderPriceComperisionSql.CHECK_PURCHASE_LOCK, sqlParameters,DBConnections.Advance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Error during GetBasiclockDetails.");
            throw;
        }
    }
    public async Task<DataTable> GetVenderDetails(string referenceNo,string ItemCode)
    {
        try
        {
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
            sqlParameters.Add(new SQLParameters("@ReferenceNo",referenceNo));
            sqlParameters.Add(new SQLParameters("@ItemCode",ItemCode));
            return await _dbOperations.SelectAsync(VenderPriceComperisionSql.GET_VENDER, sqlParameters,DBConnections.Advance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Error during GetVenderDetails.");
            throw;
        }
    }

    public async Task<DataTable> GetVendorDetails(string referenceNo)
    {
        try
        {
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
            sqlParameters.Add(new SQLParameters("@ReferenceNo",referenceNo));
           
            return await _dbOperations.SelectAsync(VenderPriceComperisionSql.GET_COMPARISON_CHART, sqlParameters,DBConnections.Advance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Error during GetVendorDetails.");
            throw;
        }
    }
    public async Task<int> InsertVenderInChart(string referenceNo)
    {
        try
        {
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
            sqlParameters.Add(new SQLParameters("@ReferenceNo",referenceNo));
           
            return await _dbOperations.DeleteInsertUpdateAsync(VenderPriceComperisionSql.INSERT_COMPARISON_CHART, sqlParameters,DBConnections.Advance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Error during InsertVenderInChart.");
            throw;
        }
    }
    public async Task<DataTable> CheckLockComparisionChart(string referenceNo)
    {
        try
        {
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
            sqlParameters.Add(new SQLParameters("@ReferenceNo",referenceNo));
           
            return await _dbOperations.SelectAsync(VenderPriceComperisionSql.GET_COMPARISON_CHART_LOCK, sqlParameters,DBConnections.Advance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Error during CheckLockComparisionChart.");
            throw;
        }
    }
    public async Task<DataTable> GetAllVendor(string referenceNo)
    {
        try
        {
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
            sqlParameters.Add(new SQLParameters("@ReferenceNo",referenceNo));
           
            return await _dbOperations.SelectAsync(VenderPriceComperisionSql.GET_All_VENDER_DETAILS, sqlParameters,DBConnections.Advance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Error during GetAllVendor.");
            throw;
        }
    }
    public async Task<DataTable> Checkvendorinchart(string ItemCode,string VendorNo,string RefNo)
    {
        try
        {
            //ItemCode,VendorNo
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
            sqlParameters.Add(new SQLParameters("@ItemCode",ItemCode));
            sqlParameters.Add(new SQLParameters("@VendorNo",VendorNo));
            sqlParameters.Add(new SQLParameters("@ReferenceNo",RefNo));
            return await _dbOperations.SelectAsync(VenderPriceComperisionSql.CHECK_VENDOR_ALREADY_, sqlParameters,DBConnections.Advance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Error during Checkvendorinchart.");
            throw;
        }
    }
    public async Task<int> InsertVendoorPrice(string EmpCode,string RefNo,InsertDetails Details)
    {
        try
        {
            //@ReferenceNo,@ItemName,@ItemCode,@VendorName,@VendorID,@ItemPrice,@VendorNo,@SubmittedBy,NOW(),@SubmittedFrom,@Remark
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
            sqlParameters.Add(new SQLParameters("@ItemName",Details.ItemName));
            sqlParameters.Add(new SQLParameters("@VendorNo",Details.VendorNo));
            sqlParameters.Add(new SQLParameters("@ReferenceNo",RefNo));
            sqlParameters.Add(new SQLParameters("@ItemPrice",Details.Price));
            sqlParameters.Add(new SQLParameters("@ItemCode",Details.ItemCode));
            sqlParameters.Add(new SQLParameters("@VendorName",Details.VendorName));
            sqlParameters.Add(new SQLParameters("@VendorID",Details.VendorId));
            sqlParameters.Add(new SQLParameters("@SubmittedBy",EmpCode));
            sqlParameters.Add(new SQLParameters("@Remark",Details.Remark));
            sqlParameters.Add(new SQLParameters("@SubmittedFrom",_general.GetIpAddress()));
            return await _dbOperations.DeleteInsertUpdateAsync(VenderPriceComperisionSql.INSERT_VENDOR_PRICE_COMPARISON, sqlParameters,DBConnections.Advance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Error during Checkvendorinchart.");
            throw;
        }
    }
    public async Task<int> LockDetails(string EmpCode,string RefNo)
    {
        try
        {
            //LockedBy=@EmpCode,LockedOn=NOW(),LockedFrom=@IpAddress,Doc=@EncDoCId WHERE ReferenceNo=@ReferenceNo
            List<SQLParameters> sqlParameters = new List<SQLParameters>();
            sqlParameters.Add(new SQLParameters("@ReferenceNo",RefNo));
            sqlParameters.Add(new SQLParameters("@EmpCode",EmpCode));
            sqlParameters.Add(new SQLParameters("@EncDoCId",_general.EncryptWithKey(RefNo,await _inclusiveService.GetEnCryptedKey())));
            sqlParameters.Add(new SQLParameters("@IpAddress",_general.GetIpAddress()));
            return await _dbOperations.DeleteInsertUpdateAsync(VenderPriceComperisionSql.LOCK_DETAILS, sqlParameters,DBConnections.Advance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _logger.LogError(e, "Error during LockDetails.");
            throw;
        }
    }
    
    
    
}