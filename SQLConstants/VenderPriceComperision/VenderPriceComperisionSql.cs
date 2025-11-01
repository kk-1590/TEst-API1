namespace AdvanceAPI.SQLConstants.VenderPriceComperision;

public class VenderPriceComperisionSql
{
    public const string GET_BASIC_DETAILS="SELECT A.TotalAmount,A.ReferenceNo,MyType,Note,Purpose,FirmName,VendorID,Unit,Quantity,ItemCode,ItemName,CurRate FROM `purchaseapprovalsummary` A,purchaseapprovaldetail B WHERE A.ReferenceNo=B.ReferenceNo AND A.ReferenceNo=@ReferenceNo;";

    public const string CHECK_PURCHASE_LOCK =
        "SELECT DISTINCT `Lock`,Doc FROM price_comparison_chart WHERE ReferenceNo=@ReferenceNo AND VendorNo=1";

    public const string GET_VENDER =
        "SELECT `Lock`,ItemCode,ItemName,ItemPrice,VendorName,VendorID,VendorNo FROM price_comparison_chart WHERE ReferenceNo=@ReferenceNo AND ItemCode=@ItemCode ";

    public const string GET_COMPARISON_CHART =
        "select * from price_comparison_chart where VendorNo=1 and ReferenceNo=@ReferenceNo";

    public const string INSERT_COMPARISON_CHART =
        "INSERT into price_comparison_chart(ReferenceNo,ItemName,ItemCode,VendorName,VendorID,ItemPrice,VendorNo,IsUsed,SubmittedBy,SubmittedOn) SELECT A.ReferenceNo,ItemName,ItemCode,FirmName,VendorID,CurRate,1,1,'Admin',NOW() FROM `purchaseapprovalsummary` A,purchaseapprovaldetail B WHERE A.ReferenceNo=B.ReferenceNo AND A.ReferenceNo=@ReferenceNo";

    public const string GET_COMPARISON_CHART_LOCK = "select * from price_comparison_chart where `Lock`='Locked' and ReferenceNo=@ReferenceNo";

    public const string GET_All_VENDER_DETAILS =
        "SELECT Distinct CAST(CONCAT(VendorID,'#',VendorName) AS CHAR) 'Value',CAST(CONCAT(VendorName) AS CHAR) 'Text' FROM purchase.vendorregister WHERE VendorName!=''  ORDER BY VendorName";

    public const string CHECK_VENDOR_ALREADY_ = "select * from price_comparison_chart where VendorNo=@VendorNo and ItemCode=@ItemCode and ReferenceNo=@ReferenceNo";

    public const string INSERT_VENDOR_PRICE_COMPARISON =
        "INSERT into price_comparison_chart(ReferenceNo,ItemName,ItemCode,VendorName,VendorID,ItemPrice,VendorNo,SubmittedBy,SubmittedOn,SubmittedFrom,Remark) \nVALUES\n(@ReferenceNo,@ItemName,@ItemCode,@VendorName,@VendorID,@ItemPrice,@VendorNo,@SubmittedBy,NOW(),@SubmittedFrom,@Remark)";
    public const string LOCK_DETAILS="UPDATE price_comparison_chart SET `Lock`='Locked',LockedBy=@EmpCode,LockedOn=NOW(),LockedFrom=@IpAddress,Doc=@EncDoCId WHERE ReferenceNo=@ReferenceNo";
}