namespace AdvanceAPI.SQLConstants.Approval;

public class ApprovalSql
{
    public const string GET_ITEM_REFERENCE_NO = "SELECT DISTINCT ReferenceNo FROM `purchaseapprovaldetail_draft` WHERE IniId =@EmpCode AND AppType=@AppType;";

    public const string GET_AUTO_ITEM_REFNO =
        "Select IFNULL(CAST(MAX(ReferenceNo)+1 as CHAR),CONCAT(CAST(DATE_FORMAT(now(),'%y%m%d') as CHAR),'0001')) 'ReferenceNo' from (Select ReferenceNo from purchaseapprovaldetail_draft) A  where ReferenceNo like CONCAT(CAST(DATE_FORMAT(now(),'%y%m%d') as CHAR),'%');";

    public const string INERT_DRAFT =
        "insert into  purchaseapprovaldetail_draft (ReferenceNo,AppType,ItemCode,ItemName,Make,Size,Unit,Balance,Quantity,PrevRate,CurRate,ChangeReason,TotalAmount,IniOn,IniId,Status,WarIn,WarType,ActualAmount,VatPer,R_Total,R_Pending,R_Status,SerialNo,DisPer,CampusCode)\n values (@ReferenceNo,@AppType,@ItemCode,@ItemName,@Make,@Size,@Unit,@Balance,@Quantity,@PrevRate,@CurRate,@ChangeReason,@TotalAmount,NOW(),@IniId,'Pending',@WarIn,@WarType,@ActualAmount,@VatPer,@R_Total,@R_Pending,'Pending',@SerialNo,@DisPer,@CampusCode);";
}