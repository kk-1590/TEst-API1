using Microsoft.AspNetCore.Mvc.Formatters;

namespace AdvanceAPI.SQLConstants.Media
{
    public static class MediaSql
    {
        public const string GET_MEDIA_SCHEDULE_TYPES = "select Types from schedulestype order by Types;";
        public const string GET_MEDIA_TYPES = "select DISTINCT Media as 'Type' from mediatypes where Main=@ScheduleType ORDER BY Type;";
        public const string GET_MEDIA_TYPE_HEADS = "select distinct Id,Media,Title from mediatypes where Main=@ScheduleType @AdditionalCondition ORDER BY Media,Title  LIMIT @LimitItems OFFSET @OffSetItems;";
        public const string CHECK_MEDIA_TYPE_HEAD_EXISTS = "select 1 from mediatypes where Main=@ScheduleType And Media=@MediaType And UPPER(Title)=@Head;";
        public const string CHECK_MEDIA_TYPE_HEAD_EXISTS_BY_ID = "SELECT 1 FROM `mediatypes` WHERE Id=@Id ;";
        public const string ADD_MEDIA_TYPE_HEAD = "insert into mediatypes (Main,Media,Title) values (@ScheduleType,@MediaType,@Head);";
        public const string DELETE_MEDIA_TYPE_HEAD = "delete from mediatypes where Id=@Id;";


        public const string GET_TYPE_EDITION_ADVERTISEMENT = "select DISTINCT Type from othervalues where ForType=@ScheduleType ORDER BY Type";
        public const string GET_EDITION_ADVERTISEMENT_HEADS = "select Id,Type,`Value` from othervalues where ForType=@ScheduleType @AdditionalCondition ORDER BY Type,`Value`  LIMIT @LimitItems OFFSET @OffSetItems;";
        public const string CHECK_EDITION_ADVERTISEMENT_HEAD_EXISTS = "select 1 from othervalues where ForType=@ScheduleType And Type=@Type And UPPER(Value)=@Head;";
        public const string ADD_EDITION_ADVERTISEMENT_HEAD = "insert into othervalues (ForType,Type,Value) values (@ScheduleType,@Type,@Head);";
        public const string CHECK_EDITION_ADVERTISEMENT_HEAD_EXISTS_BY_ID = "select 1 from othervalues where Id=@Id;";
        public const string DELETE_EDITION_ADVERTISEMENT_HEAD_BY_ID = "DELETE from othervalues where Id=@Id;";


        public const string GET_MEDIA_BUDGET_DETAILS = "SELECT Id,`Session`,Main AS 'ScheduleType',Media AS 'MediaType',Title ,Amount,TypeId  AS 'MediaTypeId'  from mediabudgets WHERE CampusCode=@CampusCode AND `Session`=@Session AND Main=@ScheduleType @AdditionalCondition  ORDER BY Session,Media,Title LIMIT @LimitItems OFFSET @OffSetItems;";
        public const string CHECK_MEDIA_BUDGET_DETAILS_EXISTS = "SELECT Id FROM `mediabudgets` WHERE CampusCode=@CampusCode AND TypeId=@TypeId AND `Session`=@Session;";
        public const string CHECK_MEDIA_TYPE_DETAILS_ISVALID = "SELECT 1 FROM mediatypes WHERE Id=@Id AND Main=@ScheduleType AND Media=@MediaType AND Title=@Head;";
        public const string ADD_MEDIA_BUDGET_DETAILS = "INSERT INTO `mediabudgets` (CampusCode,TypeId,Main,Media,Title,Amount,ExecutedAmount,`Session`,UpdateOn,UpdatedFrom,UpdatedBy) VALUES (@CampusCode,@TypeId,@ScheduleType,@MediaType,@Head,@Amount,0,@Session,NOW(),@UpdatedFrom,@UpdatedBy);";
        public const string UPDATE_MEDIA_BUDGET_DOCUMENT_DETAILS = "UPDATE `mediabudgets` SET Col1=CONCAT(Id,'_',TypeId) WHERE Id=@Id;";
        public const string CHECK_MEDIA_BUDGET_DETAILS_EXISTS_BY_ID = "SELECT Col1 FROM `mediabudgets` where Id=@Id;";
        public const string DELETE_MEDIA_BUDGET_DETAILS_BY_ID = "delete from mediabudgets where Id=@Id;";


        public const string GET_MEDIA_SCHEDULES_SESSION = "select Distinct Session from schedules order by Session desc;";
        public const string GET_MEDIA_SCHEDULES_MEDIA_TYPES = "select Distinct Type from schedules order by Type;";
        public const string GET_MEDIA_SCHEDULES_MEDIA_TITLES = "select Distinct MediaTitle from schedules @AdditionalCondition  order by MediaTitle  LIMIT @LimitItems OFFSET @OffSetItems;";
        public const string GET_MEDIA_SCHEDULES_RO_AUTHORITY = "select Distinct Employee_Code,`Name`,Designation from authorities where Type='RELEASE ORDER' And SubType=@AuthorityType order by MyOrder;";
        public const string GET_ADVERTISEMNT_VENDORS = "Select VendorName,VendorID from vendorregister where DealsIn like '%Advertisement%' @AdditionalCondition ORDER BY VendorName LIMIT @LimitItems OFFSET @OffSetItems;";
        public const string GET_MEDIA_SCHEDULES_NEW_CREATE_MEDIA_TYPES = "select Id,Media,Title from mediatypes WHERE Main='Advertisement' @AdditionalCondition  order by Main,Media,Title LIMIT @LimitItems OFFSET @OffSetItems;";
        public const string CHECK_MEDIA_BUDGET_DETAILS_EXISTS_WITH_AMOUNT = "Select * from mediabudgets where CampusCode=@CampusCode AND `Session`=@Session And Media=@Media And Title=@Title And Amount>0;";
        public const string GET_PENDING_RELEASE_ORDERS_SCHEDULE_IDS = "Select ScheduleIds from releaseorders where `Status`='Pending';";
        public const string GET_PENDING_MEDIA_SCHEDULES = "Select * from schedules where Id in (@ScheduleIds)";
        public const string GET_MEDIA_BUDGET_AMOUNT = "Select 'Budget' as 'Type',IFNULL(Amount,0) 'Amount' from mediabudgets where CampusCode=@CampusCode AND `Session`=@Session and Media=@MediaType And Title=@Title;";
        public const string GET_SCHEDULED_MEDIA_BUDGET_AMOUNT = "Select 'Scheduled' as 'Type',SUM(Actual) 'Amount' from schedules where CampusCode=@CampusCode AND `Session`=@Session and Type=@MediaType And MediaTitle=@Title;";
        public const string GET_APPROVED_RELEASE_ORDERS_BY_SESSION = "Select `Session`,ScheduleIds,TotalAmount from releaseorders  where Status='Approved' And `Session`=@Session";
        public const string GET_USED_SCHEDULED_AMOUNT = "Select IFNULL(ROUND(SUM(Actual),0),0) from schedules where Id in (@ScheduleIds) And CampusCode=@CampusCode AND `Session`=@Session and Type=@MediaType And MediaTitle=@Title";
        public const string GET_PENDING_MEDIA_SCHEDULES_RO_GENERATED = "Select * from schedules where Id in (@ScheduleIds) And CampusCode=@CampusCode AND `Session`=@Session and Type=@MediaType And MediaTitle=@Title";
        public const string GET_MEDIA_SCHEDULES = "Select if(MyBillUpto is null,DATE_FORMAT(now(),'%Y-%m-%d'),DATE_FORMAT(MyBillUpto,'%Y-%m-%d')) 'MyBill',if(ScheduleTime is null,DATE_FORMAT(Schedule,'%Y-%m-%d'),DATE_FORMAT(ScheduleTime,'%Y-%m-%d')) 'MyCheck',Id,Type, MediaTitle,if(ScheduleTime is null,DATE_FORMAT(Schedule,'%d %b,%y'),DATE_FORMAT(ScheduleTime,'%d %b,%y')) 'Schedule',AdvertisementType,Edition,SizeW,SizeH,Amount,Discount,Actual,if(ExecutedOn is NULL,'----',DATE_FORMAT(ExecutedOn,'%d %b,%y')) 'ExecutedOn',CAST(if(TransactionID is NULL,'N/A',TransactionID) as CHAR) 'Billinitiated',Tax, PageNo,Rate,IFNULL(CONCAT(' ( ',CAST(DATE_FORMAT(MyBillUpto,'%d %b, %y') as CHAR),' ) '),'') as 'MMBillTill' from schedules where CampusCode=@CampusCode AND Session=@Session And Main=@ScheduleType @AdditionalCondition  @OrderByCondition LIMIT @LimitItems OFFSET @OffSetItems;";
        public const string GET_MEDIA_SCHEDULES_BY_ID = "Select if(MyBillUpto is null,DATE_FORMAT(now(),'%Y-%m-%d'),DATE_FORMAT(MyBillUpto,'%Y-%m-%d')) 'MyBill',if(ScheduleTime is null,DATE_FORMAT(Schedule,'%Y-%m-%d'),DATE_FORMAT(ScheduleTime,'%Y-%m-%d')) 'MyCheck',Id,Type, MediaTitle,if(ScheduleTime is null,DATE_FORMAT(Schedule,'%d %b,%y'),DATE_FORMAT(ScheduleTime,'%d %b,%y')) 'Schedule',AdvertisementType,Edition,SizeW,SizeH,Amount,Discount,Actual,if(ExecutedOn is NULL,'----',DATE_FORMAT(ExecutedOn,'%d %b,%y')) 'ExecutedOn',CAST(if(TransactionID is NULL,'N/A',TransactionID) as CHAR) 'Billinitiated',Tax, PageNo,Rate,IFNULL(CONCAT(' ( ',CAST(DATE_FORMAT(MyBillUpto,'%d %b, %y') as CHAR),' ) '),'') as 'MMBillTill' from schedules where Id=@Id;";
        public const string GET_EXTERNAL_RELEASE_ORDER_DETAILS_BY_SCHEDULE_ID = "select * from releaseorders where ROCategory='External' And ScheduleIds like  CONCAT('%',@ScheduleId,'%')   And (`Status`='Pending' or `Status`='Approved' or (`Status`='Pending' And (App1Status='Approved' OR App2Status='Approved' OR App3Status='Approved' OR App4Status='Approved') ));";
        public const string GET_INTERNAL_RELEASE_ORDER_DETAILS_BY_SCHEDULE_ID = "select * from releaseorders where (`Status`='Pending' or Status='Approved') And ROCategory='Internal' And ScheduleIds like CONCAT('%',@ScheduleId,'%');";
        public const string GET_EXTERNAL_RELEASE_ORDER_PENDING_OR_APPROVED_DETAILS_BY_SCHEDULE_ID = "select * from releaseorders where ROCategory='External' And ScheduleIds like CONCAT('%',@ScheduleId,'%')  And `Status` in ('Approved','Pending')";
        public const string GET_NEW_SCHEDULE_ID = "SELECT CAST(IFNULL((MAX(Id)+1),1) AS CHAR) AS 'NewScheduleId' FROM `schedules`;";
        public const string ADD_NEW_SCHEDULE = "insert into schedules (Id,CampusCode,Main,Type,HasChild,Schedule,MediaTitle,AdvertisementType,Edition,SizeW,SizeH,Amount,Discount,Actual,Session,AddedOn,AddedBy,Tax, PageNo,Rate,Col1,MyBillUpto) VALUES (@Id,@CampusCode,@ScheduleType,@MediaType,-1,@ScheduleDate,@MediaTitle,@AdvertisementTypes,@Editions,@SizeW,@SizeH,@Amount,@Discount,@FinalAmount,@Session,NOW(),@AddedBy,@Tax,@PageNo,@Rate,@AddingSession,@MyBillUpto);";
        public const string UPDATE_NEW_SCHEDULE_DOCUMENT_ID = "update schedules set Col4=Id where Id=@Id";
        public const string CHECK_IS_MEDIA_SCHEDULE_EXISTS_BY_ID = "SELECT Id,DATE_FORMAT(`Schedule`,'%Y-%m-%d')'ScheduleOn',DATE_FORMAT(`MyBillUpto`,'%Y-%m-%d')'BillUpto',Type, MediaTitle,AdvertisementType,Edition,SizeW,SizeH,Rate,Amount,Discount, Tax,Actual,PageNo FROM `schedules` where Id=@Id;";
        public const string DELETE_MEDIA_SCHEDULE_BY_ID = "DELETE FROM `schedules` where Id=@Id;";
        public const string UPDATE_MEDIA_SCHEDULE_BY_ID = "update schedules  @UpdateColumns where Id=@Id;";
        public const string LOG_UPDATE_MEDIA_SCHEDULE_BY_ID = "insert into scheduleupdates (Id,Updates,TotalUpdates,Reason,UpdatedOn,UpdatedBy) values (@ScheduleId,@Updations,@UpdationCount,@Reason,now(),@EmployeeId)";
        public const string GET_EMPLOYEE_ONLY_MEDIA_PERMISSION_DETAILS = "Select onlymedia from userroles where employee_code=@EmployeeId;";

    }
}
