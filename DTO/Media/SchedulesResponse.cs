using System.Data;

namespace AdvanceAPI.DTO.Media
{
    public class SchedulesResponse
    {
        public string? ScheduleBillUpto { get; set; } = string.Empty;
        public string? ScheduleDate { get; set; } = string.Empty;
        public string? Id { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public string? MediaTitle { get; set; } = string.Empty;
        public string? Schedule { get; set; } = string.Empty;
        public string? AdvertisementType { get; set; } = string.Empty;
        public string? Edition { get; set; } = string.Empty;
        public string? SizeW { get; set; } = string.Empty;
        public string? SizeH { get; set; } = string.Empty;
        public string? Amount { get; set; } = string.Empty;
        public string? Discount { get; set; } = string.Empty;
        public string? Actual { get; set; } = string.Empty;
        public string? ExecutedOn { get; set; } = string.Empty;
        public string? Billinitiated { get; set; } = string.Empty;
        public string? Tax { get; set; } = string.Empty;
        public string? PageNo { get; set; } = string.Empty;
        public string? Rate { get; set; } = string.Empty;
        public string? MMBillTill { get; set; } = string.Empty;

        public bool? CanEdit { get; set; } = true;
        public bool? AdvertisementFileExists { get; set; } = false;
        public bool? CanDelete { get; set; } = true;
        public bool? CanGenerateReleaseOrder { get; set; } = true;
        public bool? IsReleaseOrderGenerated { get; set; } = false;
        public string? LockIt { get; set; } = "No";
        public System.Drawing.Color? BackColor { get; set; } = System.Drawing.Color.Empty;
        public string? StoredBillRecieptLink { get; set; } = string.Empty;
        public string? ReleaseOrderPrintLink { get; set; } = string.Empty;
        public string? ReleaseOrderDownloadLink { get; set; } = string.Empty;


        public SchedulesResponse()
        {

        }

        public SchedulesResponse(DataRow dr)
        {
            ScheduleBillUpto = dr["MyBill"]?.ToString() ?? string.Empty;
            ScheduleDate = dr["MyCheck"]?.ToString() ?? string.Empty;
            Id = dr["Id"]?.ToString() ?? string.Empty;
            Type = dr["Type"]?.ToString() ?? string.Empty;
            MediaTitle = dr["MediaTitle"]?.ToString() ?? string.Empty;
            Schedule = dr["Schedule"]?.ToString() ?? string.Empty;
            AdvertisementType = dr["AdvertisementType"]?.ToString() ?? string.Empty;
            Edition = dr["Edition"]?.ToString() ?? string.Empty;
            SizeW = dr["SizeW"]?.ToString() ?? string.Empty;
            SizeH = dr["SizeH"]?.ToString() ?? string.Empty;
            Amount = dr["Amount"]?.ToString() ?? string.Empty;
            Discount = dr["Discount"]?.ToString() ?? string.Empty;
            Actual = dr["Actual"]?.ToString() ?? string.Empty;
            ExecutedOn = dr["ExecutedOn"]?.ToString() ?? string.Empty;
            Billinitiated = dr["Billinitiated"]?.ToString() ?? string.Empty;
            Tax = dr["Tax"]?.ToString() ?? string.Empty;
            PageNo = dr["PageNo"]?.ToString() ?? string.Empty;
            Rate = dr["Rate"]?.ToString() ?? string.Empty;
            MMBillTill = dr["MMBillTill"]?.ToString() ?? string.Empty;

        }
    }
}
