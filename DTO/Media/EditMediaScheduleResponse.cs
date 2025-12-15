using System.Data;
using System.Text.Json.Serialization;

namespace AdvanceAPI.DTO.Media
{
    public class EditMediaScheduleResponse : SchedulesResponse
    {

        public string[] Editions { get; set; }
        public string[] Advertisements { get; set; }
        public bool CanEditScheduleOn { get; set; } = false;
        public bool CanEditMediaType { get; set; } = false;
        public bool CanEditAmount { get; set; } = false;
        public bool CanEditWidth { get; set; } = false;
        public bool CanEditHeight { get; set; } = false;
        public bool CanEditRate { get; set; } = false;
        public bool CanEditDiscount { get; set; } = false;
        public bool CanEditTax { get; set; } = false;
        public bool CanEditActualPaid { get; set; } = false;
        public bool CanEditPageNo { get; set; } = false;


        public EditMediaScheduleResponse()
        {

        }

        public EditMediaScheduleResponse(DataRow dr) : base(dr)
        {

        }
    }
}
