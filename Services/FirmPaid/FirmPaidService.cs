using AdvanceAPI.DTO;
using AdvanceAPI.DTO.FirmPaidDetails;
using AdvanceAPI.DTO.FirmPaidDetails.ApplicationReport;
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
            PaidDetailsResponse paidDetailsResponse = new PaidDetailsResponse();
            string cond = "";
            string VendorId = "", Offices = "";

            if(req.IsAdvance != null && req.IsAdvance.Length > 0 && req.IsAdvance=="Y")
            {
                if(req.CallBy != null && req.CallBy.Length > 0)
                {
                    cond += " And ForType='Advance Bill' And RelativePersonID='" + _general.Decrypt(req.CallBy) + "'";
                    string pcond = "";
                    {
                        pcond = " And RelativePersonID='" + _general.Decrypt(req.CallBy) + "'";
                    }
                    cond = cond + pcond;
                }
                else
                {
                    return new ApiResponse(StatusCodes.Status403Forbidden, "Error", "Invalid Access");
                }
            }


            if (req.Id != null && req.Id.Length > 0)
            {
                using (DataTable dt = await _firmPaidRepository.GetVendorIdOffices(req.Id ?? ""))
                {
                    if (dt.Rows.Count > 0)
                    {
                        VendorId = dt.Rows[0]["VendorID"].ToString() ?? "";
                        Offices = dt.Rows[0]["Col3"].ToString() ?? "";
                        if (dt.Rows[0][1].ToString() != "" && dt.Rows[0][1].ToString().Contains("---") == false)
                        {
                            cond = cond + $" And (( A.VendorID={VendorId} And A.Col3='{Offices}') or ( B.CVId={VendorId} And B.CVSubFirm='{Offices}'))";
                        }
                        else
                        {
                            cond = cond + $" And (( A.VendorID={VendorId}) or ( B.CVId={VendorId}))";
                        }
                    }

                }
            }
            else
            {
                if (req.VId != null  && req.SubId != null )
                {
                    string DecVId = _general.Decrypt(req.VId);
                    if (req.SubId == "D" || req.SubId == "D" && req.SubId == "M" || req.SubId == "S" || req.SubId == "Y"|| req.SubId == "P"|| req.SubId == "A"|| req.SubId == "E")
                    {
                        if(req.SubId == "D")
                        cond += "  And A.Col1='" + DecVId + "'";

                        if (req.SubId == "M")
                            cond += "  And (UPPER(IFNULL(A.Col5,''))='" + DecVId + "' Or UPPER(IFNULL(B.CVAddName,''))='" + DecVId + "')";

                        if(req.SubId == "Y")
                            cond += " And IFNULL(CAST(YEAR(B.SignedOn) as CHAR),'Not Yet')='" + DecVId + "'";

                        if (req.SubId == "S")
                            cond += " And A.`Session`='" + DecVId + "'";

                        if (req.SubId == "P")
                            cond += "  And A.RelativePersonID='" + DecVId.Split('@')[1] + "'";

                        if (req.SubId == "E")
                            cond += "  And B.`ExtraCol8`='" + DecVId + "'";

                        if (req.SubId == "A")
                            cond += "  And B.`ExtraCol9`='" + DecVId.Split('@')[1] + "'";
                    }
                    else
                    {
                        string cond1 = "";
                        string vid = req.VId, off = "";
                        if (req.SubId.ToString() != "" && req.SubId.Contains("---") == false)
                        {
                            cond1 = cond1 + " And (( A.VendorID='" + req.VId + "' And A.Col3='" + req.SubId + "') or ( B.CVId='" + req.VId + "' And B.CVSubFirm='" + req.SubId + "'))";
                            off = " (" + req.SubId + ")";
                        }
                        else
                        {
                            cond1 = cond1 + " And (( A.VendorID='" + req.VId + "') or ( B.CVId='" + req.VId + "'))";
                        }
                        string Pcond = "";
                        if(req.CallBy!=null && req.CallBy.Length>0)
                        {
                            Pcond += "  And RelativePersonID='" + _general.Decrypt(req.CallBy) + "'";
                        }
                        cond += cond1 + Pcond;
                    }
                }
                else
                {
                    return new ApiResponse(StatusCodes.Status403Forbidden, "Error", "Invalid Access");
                }
            }

            DataTable VendorDetails = await _firmPaidRepository.GetVendorRegDetails(VendorId);

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
                HashSet<YearWiseReport> yearWiseReport = new HashSet<YearWiseReport>();
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
                        headWise.Add(new HeadWise(dr));
                    }
                }

                //Department Wise
                HashSet<DepartmentWiseresponse> DepartmentWise = new HashSet<DepartmentWiseresponse>();
                using(DataTable dt=await _firmPaidRepository.DepartmentWiseDetails(cond))
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        DepartmentWise.Add(new DepartmentWiseresponse(dr));
                    }
                }
                //Session Wise
                HashSet<SessionWiseReport> SessionWise = new HashSet<SessionWiseReport>();
                using(DataTable dt=await _firmPaidRepository.sessionWiseReports(cond))
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        SessionWise.Add(new SessionWiseReport(dr));
                    }
                }

                //Vendor/Firm Wise
                HashSet<SessionWiseReport> VendorWise = new HashSet<SessionWiseReport>();
                using(DataTable dt=await _firmPaidRepository.VenderWiseDetails(cond))
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        VendorWise.Add(new SessionWiseReport(dr));
                    }
                }


                paidDetailsResponse.VendorWiseReports = VendorWise;
                paidDetailsResponse.sessionWiseReports = SessionWise;
                paidDetailsResponse.DepartmentWise = DepartmentWise;
                paidDetailsResponse.AdvancePayments = anvancePayment;
                paidDetailsResponse.PersonWiseSummaries = personWise;
                paidDetailsResponse.YearWiseReports = yearWiseReport;
                paidDetailsResponse.HeadWises = headWise;
                paidDetailsResponse.DetailedStatement = BaseDetails;  //BaseDetails
            }
            else
            {
                
            }
            
            return new ApiResponse(StatusCodes.Status200OK,"Success", paidDetailsResponse);
        }


        public async Task<ApiResponse> ApplicationReport(string EmpCode, ApplicationReportRequest req)
        {
            if (req.Id != null)
            {
                if (req.Id.Length > 0)
                {
                    return new ApiResponse(StatusCodes.Status200OK, "Success", await LoadApplicationReport(req.Id,""));
                }
                else
                {
                    return new ApiResponse(StatusCodes.Status403Forbidden, "Error", "Invalid Access");
                }
            }
            else
            {
                if (req.BId != null)
                {
                    if (req.BId.Length > 0)
                    {
                        string RelativeId = await _firmPaidRepository.GetRelativePerson(req.BId);
                        return new ApiResponse(StatusCodes.Status200OK, "Success", await LoadApplicationReport( req.BId, RelativeId));
                    }
                    else
                    {
                        return new ApiResponse(StatusCodes.Status403Forbidden, "Error", "Invalid Access");
                    }
                }
                else
                {
                    return new ApiResponse(StatusCodes.Status403Forbidden, "Error", "Invalid Access");
                }
            }
        }

        public async Task<ApplicationReportResponse> LoadApplicationReport(string BillId, string MyId)
        {
            //Application Summarized
            ApplicationReportResponse res = new ApplicationReportResponse();
            using (DataTable dt = await _firmPaidRepository.BillApplicationNewDetails((MyId)))
            {
                HashSet<ApplicationDetaild> detailds = new HashSet<ApplicationDetaild>();
                string ParentPath = Directory.GetCurrentDirectory();
                foreach (DataRow dr in dt.Rows)
                {
                    ApplicationDetaild stmt = new ApplicationDetaild(dr);
                    if (File.Exists(Path.Combine(ParentPath, "Applications", stmt.Id + "_" + stmt.BillId + ".pdf")))
                    {
                        stmt.PdfFile = Path.Combine(ParentPath, "Applications", stmt.Id + "_" + stmt.BillId + ".pdf");
                    }
                    if (stmt.Status == "Pending")
                    {
                        stmt.StatusColor = Color.LightPink;
                    }
                    else
                    {
                        stmt.StatusColor = Color.LightGreen;
                    }
                    if (BillId != "" && BillId == stmt.BillId)
                    {
                        stmt.RowBackColor = Color.LightBlue;
                    }
                    detailds.Add(stmt);
                }
                res.DetailedStatement = detailds;
                //Application Summarized Statement
                using (DataTable summm = await _firmPaidRepository.SummarizedData(MyId))
                {
                    HashSet<SummarizedStatement> summarizedStatements = new HashSet<SummarizedStatement>();
                    int ini = 0;
                    int totini = 0;

                    string lastyr = summm.Rows[0]["MyYear"].ToString() ?? string.Empty;
                    int myfrm = 0, myto = 0;
                    int sno = 1;
                    foreach (DataRow dr in summm.Rows)
                    {
                        summarizedStatements.Add(new SummarizedStatement(dr));
                    }
                    res.Summarizedstmt= summarizedStatements;
                }



            }

            return res;
        }

       

    }
}
