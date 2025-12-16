namespace AdvanceAPI.DTO.FirmPaidDetails
{
    public class PaidDetailsResponse
    {
        public List<AdvancePayment>? AdvancePayments { get; set; }
        public HashSet<PersonWiseSummary>? PersonWiseSummaries { get; set; }
        public HashSet<YearWiseReport>? YearWiseReports { get; set; }
        public HashSet<BillModel>? DetailedStatement { get; set; }
        public HashSet<HeadWise>? HeadWises { get; set; }
        public HashSet<DepartmentWiseresponse>? DepartmentWise { get; set; }
        public HashSet<SessionWiseReport>? sessionWiseReports { get; set; }
        public HashSet<SessionWiseReport>? VendorWiseReports { get; set; }

    }
}
