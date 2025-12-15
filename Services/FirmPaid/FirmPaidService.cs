using AdvanceAPI.DTO;
using AdvanceAPI.DTO.FirmPaidDetails;
using AdvanceAPI.IRepository;
using AdvanceAPI.IServices;
using AdvanceAPI.IServices.FirmPaideport;
using System.Data;
using System.Drawing;

namespace AdvanceAPI.Services.FirmPaid
{
    public class FirmPaidService: IFirmPaidServices
    {
        private readonly IFirmPaidRepository _firmPaidRepository;
        private readonly IGeneral _general;

        public FirmPaidService(IFirmPaidRepository firmPaidRepository,IGeneral general) 
        {
            _firmPaidRepository=firmPaidRepository;
            _general=general;
        }

        public async Task<ApiResponse> GetFirmPaidReportById(string EmpName,FirmPaidrequest req)
        {
            string cond = "";
            string VendorId = "", Offices = ""; 
            using(DataTable dt= await _firmPaidRepository.GetVendorIdOffices (req.Id??""))
            {
                if(dt.Rows.Count > 0)
                {
                    VendorId = dt.Rows[0]["VendorID"].ToString() ?? "";
                    Offices = dt.Rows[0]["Col3"].ToString() ?? "";
                    if (dt.Rows[0][1].ToString() != "" && dt.Rows[0][1].ToString().Contains("---") == false)
                    {
                        cond = cond + $" And (( A.VendorID={ VendorId } And A.Col3='{ Offices }') or ( B.CVId={ VendorId } And B.CVSubFirm='{ Offices }'))";
                    }
                    else
                    {
                        cond = cond + $" And (( A.VendorID={VendorId}) or ( B.CVId={VendorId}))";
                    }
                }
            }
            DataTable VendorDetails=await _firmPaidRepository.GetVendorRegDetails(VendorId);

            using DataTable LoadDetails = await _firmPaidRepository.DepartMentWise(cond);
            HashSet<BillModel> BaseDetails=new HashSet<BillModel>();
            if (LoadDetails.Rows.Count > 0)
            {
                //Detailed Statement
                foreach (DataRow row in LoadDetails.Rows)
               {
                    BaseDetails.Add(new BillModel(row));
               }
               DataTable LoadWithOutAdvance=await _firmPaidRepository.DepartMentWiseWithOUTAdvance(cond);
                if(LoadWithOutAdvance.Rows.Count>0)
                {
                    BaseDetails = new HashSet<BillModel>();
                }
               foreach(DataRow row in LoadWithOutAdvance.Rows)
               {
                    BaseDetails.Add(new BillModel(row));
               }

                //Advance Payment - First Payment  Bill Receiving
                using DataTable AdvanceBill= await _firmPaidRepository.VenderAdvanceBill(cond);
                List<AdvancePayment> anvancePayment=new List<AdvancePayment>();
                if (AdvanceBill.Rows.Count > 0)
                {
                    decimal sub = 0;
                    decimal bal = 0;
                    foreach (DataRow row in AdvanceBill.Rows)
                    {
                        AdvancePayment details = new AdvancePayment(row);

                        DataTable UpdateBalance=await _firmPaidRepository.AgainstBillBaseDetails(details.TransactionID.ToString(), details.SequenceID.ToString());
                        sub = sub + details.IssuedAmount;
                        if(UpdateBalance.Rows.Count > 0)
                        {
                            details.ReceivedAmount=Convert.ToInt32( UpdateBalance.Rows[0]["Amt"].ToString());
                            int diff = details.IssuedAmount - Convert.ToInt32(UpdateBalance.Rows[0]["Amt"].ToString());
                            if (diff > 0)
                            {
                                details.Balance=diff;
                                details.BackColor = Color.Red;
                                details.ForeColor = Color.White;
                            }
                            else
                            {
                                details.Balance = diff;
                                details.BackColor = Color.LightGreen;

                            }
                        }
                        else
                        {
                            details.ReceivedAmount = 0;
                            details.Balance = details.IssuedAmount;
                            details.BackColor = Color.Red;
                            details.ForeColor = Color.White;
                        }
                        using DataTable HighlightReceivedAmt= await _firmPaidRepository.BillDistributionAdvanceDetails(details.TransactionID.ToString(), details.SequenceID.ToString());
                        if(HighlightReceivedAmt.Rows.Count > 0)
                        {
                            details.HighlightReceivedAmount = "LoadModel("+_general.Encrypt(details.TransactionID.ToString()) +"#"+ details.SequenceID.ToString() + ")";
                        }
                        else
                        {
                            details.HighlightReceivedAmount = "";
                        }

                        anvancePayment.Add(new AdvancePayment(row));
                    }
                }


                // Year Wise Statement
                List<YearWiseReport> yearWiseReport = new List<YearWiseReport>();
                using(DataTable YearWise=await _firmPaidRepository.GetYearWiseReport(cond))
                {
                    foreach(DataRow dr in YearWise.Rows)
                    {
                        yearWiseReport.Add(new YearWiseReport(dr));
                    }
                }

                //Person Wise Statement
                HashSet<PersonWiseSummary> personWise = new HashSet<PersonWiseSummary>();
                using(DataTable PersonWiseData=await _firmPaidRepository.PersonWiseDetails(cond))
                {
                    foreach(DataRow dr in PersonWiseData.Rows)
                    {



                        personWise.Add(new PersonWiseSummary(dr));
                    }
                }

                //Head Wise Statement
                HashSet<HeadWise> headWise = new HashSet<HeadWise>();
                using(DataTable HeadWiseData=await _firmPaidRepository.HeadWise(cond))
                {
                    foreach(DataRow dr in HeadWiseData.Rows)
                    {
                        HeadWise head = new HeadWise();
                        head = new HeadWise(dr);
                    }
                    
                }


            }
            else
            {
                
            }
            return new ApiResponse(StatusCodes.Status200OK,"Success","");
        }
        

    }
}
