using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AdvanceAPI.DTO.Media
{
    public class EditMediaScheduleOldDetails
    {
        public string? Id { get; set; }
        public string? OldScheduleOn { get; set; }
        public string? OldBillOn { get; set; }
        public string? OldMediaType { get; set; }
        public string? OldMediaTitle { get; set; }
        public string[]? OldAdvertisements { get; set; }
        public string[]? OldEditions { get; set; }
        public string? OldWidth { get; set; }
        public string? OldHeight { get; set; }
        public string? OldRate { get; set; }
        public string? OldAmount { get; set; }
        public string? OldDiscount { get; set; }
        public string? OldTax { get; set; }
        public string? OldFinalAmount { get; set; }
        public string? OldPageNo { get; set; }


        public EditMediaScheduleOldDetails()
        {

        }

        public EditMediaScheduleOldDetails(DataRow dr)
        {
            Id = dr["Id"].ToString();
            OldScheduleOn = dr["ScheduleOn"].ToString();
            OldBillOn = dr["BillUpto"].ToString();
            OldMediaType = dr["Type"].ToString();
            OldMediaTitle = dr["MediaTitle"].ToString();
            OldWidth = dr["SizeW"].ToString();
            OldHeight = dr["SizeH"].ToString();
            OldRate = dr["Rate"].ToString();
            OldAmount = dr["Amount"].ToString();
            OldDiscount = dr["Discount"].ToString();
            OldTax = dr["Tax"].ToString();
            OldFinalAmount = dr["Actual"].ToString();
            OldPageNo = dr["PageNo"].ToString();
            OldAdvertisements = dr["AdvertisementType"].ToString()!.Replace(", ", "?").Split('?').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            OldEditions = dr["Edition"].ToString()!.Replace(", ", "?").Split('?').Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }
    }
}
