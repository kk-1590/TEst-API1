namespace AdvanceAPI.DTO.Advance
{
    public class DateLockResponse
    {
        public DateTime StoreDate { get; set; }
        public DateTime DepartmentDate { get; set; }
        public DateTime GateDate { get; set; }
        public DateTime BillOnGate { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime InitiateOn { get; set; }
        public DateTime TestingOnDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
