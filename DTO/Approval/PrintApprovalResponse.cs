using AdvanceAPI.IServices;
using System.Data;

namespace AdvanceAPI.DTO.Approval
{
    public class PrintApprovalResponse
    {
        public string? AdditionalName { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public string? RelativePersonID { get; set; } = string.Empty;
        public string? RelativePersonName { get; set; } = string.Empty;
        public string? RelativeDepartment { get; set; } = string.Empty;
        public string? RelativeDesignation { get; set; } = string.Empty;
        public string? ForDepartment { get; set; } = string.Empty;
        public string? FirmAddress { get; set; } = string.Empty;
        public string? MyOrderDate { get; set; } = string.Empty;
        public string? MyType { get; set; } = string.Empty;
        public string? Note { get; set; } = string.Empty;
        public string? Purpose { get; set; } = string.Empty;
        public string? ExpDate { get; set; } = string.Empty;
        public string? TotalAmount { get; set; } = string.Empty;
        public string? Amount { get; set; } = string.Empty;
        public string? DiscountOverAll { get; set; } = string.Empty;
        public string? Other { get; set; } = string.Empty;
        public string? FirmName { get; set; } = string.Empty;
        public string? FirmContactNo { get; set; } = string.Empty;
        public string? IniName { get; set; } = string.Empty;
        public string? IniId { get; set; } = string.Empty;
        public string? OrderDate { get; set; } = string.Empty;
        public string? CreateDate { get; set; } = string.Empty;
        public string? ExtendedBillDate { get; set; } = string.Empty;
        public string? App1Name { get; set; } = string.Empty;
        public string? App2Name { get; set; } = string.Empty;
        public string? App3Name { get; set; } = string.Empty;
        public string? App4Name { get; set; } = string.Empty;
        public string? App4Designation { get; set; } = string.Empty;
        public string? App3Designation { get; set; } = string.Empty;
        public string? App2Designation { get; set; } = string.Empty;
        public string? App1Designation { get; set; } = string.Empty;
        public string? App1Status { get; set; } = string.Empty;
        public string? App2Status { get; set; } = string.Empty;
        public string? App3Status { get; set; } = string.Empty;
        public string? App4Status { get; set; } = string.Empty;

        public string? App1On { get; set; } = string.Empty;
        public bool? ShowApp1 { get; set; } = true;
        public bool? CancelledByApp1 { get; set; } = false;

        public string? App2On { get; set; } = string.Empty;
        public bool? ShowApp2 { get; set; } = true;
        public bool? CancelledByApp2 { get; set; } = false;

        public string? App3On { get; set; } = string.Empty;
        public bool? ShowApp3 { get; set; } = true;
        public bool? CancelledByApp3 { get; set; } = false;

        public string? App4On { get; set; } = string.Empty;
        public bool? ShowApp4 { get; set; } = true;
        public bool? CancelledByApp4 { get; set; } = false;

        public string? ByPass { get; set; } = string.Empty;
        public string? CancelBy { get; set; } = string.Empty;
        public string? CancelOn { get; set; } = string.Empty;
        public string? ReferenceBillStatus { get; set; } = string.Empty;
        public string? CancelledReason { get; set; } = string.Empty;
        public string? BillVariationRemark { get; set; } = string.Empty;
        public string? RejectionReason { get; set; } = string.Empty;
        public string? VatType { get; set; } = string.Empty;
        public string? BudgetRequired { get; set; } = string.Empty;
        public string? BudgetAmount { get; set; } = string.Empty;
        public string? PreviousTaken { get; set; } = string.Empty;
        public string? CurStatus { get; set; } = string.Empty;
        public string? BudgetStatus { get; set; } = string.Empty;
        public string? BudgetReferenceNo { get; set; } = string.Empty;
        public string? ReferenceNo { get; set; } = string.Empty;
        public string? AppDateDB { get; set; } = string.Empty;
        public string? CampusName { get; set; } = string.Empty;
        public string? DiscountAmount { get; set; } = string.Empty;
        public string? RelativePersonDesignation { get; set; } = string.Empty;
        public string? RelativePersonContact { get; set; } = string.Empty;

        public string? SumamryActualAmount { get; set; } = string.Empty;
        public string? SumamryDiscountAmount { get; set; } = string.Empty;
        public string? SumamryAmountAfterDiscount { get; set; } = string.Empty;
        public string? SumamryTaxAmount { get; set; } = string.Empty;
        public string? SumamryAmountToPay { get; set; } = string.Empty;
        public string? SumamryCashDiscountPercentage { get; set; } = string.Empty;
        public string? SumamryCashDiscount { get; set; } = string.Empty;
        public string? SumamryOtherDiscount { get; set; } = string.Empty;
        public string? SummaryFinalPayableAmount { get; set; } = string.Empty;

        public string? AmountInWords { get; set; } = string.Empty;

        public List<ApprovalBillExpenditureDetails>? BillExpenditureDetails { get; set; } = new List<ApprovalBillExpenditureDetails>();
        public ApprovalPaymentDetails? PaymentDetails { get; set; } = new ApprovalPaymentDetails();
        public List<ApprovalWarrantyDetails>? WarrentyDetails { get; set; } = new List<ApprovalWarrantyDetails>();
        public List<ApprovalRepairMaintinanceDetails>? RepairMaintinaceDetails { get; set; } = new List<ApprovalRepairMaintinanceDetails>();
        public List<ApprovalItemDetails>? Items { get; set; } = new List<ApprovalItemDetails>();


        public bool? CanViewComparisonChart { get; set; } = false;
        public bool? CanEditNote { get; set; } = false;
        public bool? CanShowApp4Name { get; set; } = false;
        public bool? ViewCurrentStock { get; set; } = false;
        public bool? ViewPreviousRate { get; set; } = false;

        public PrintApprovalResponse()
        {

        }

        public PrintApprovalResponse(DataRow dr)
        {
            AdditionalName = dr["AdditionalName"]?.ToString() ?? String.Empty;
            Status = dr["Status"]?.ToString() ?? String.Empty;
            RelativePersonID = dr["RelativePersonID"]?.ToString() ?? String.Empty;
            RelativePersonName = dr["RelativePersonName"]?.ToString() ?? String.Empty;
            RelativeDepartment = dr["RelativeDepartment"]?.ToString() ?? String.Empty;
            RelativeDesignation = dr["RelativeDesignation"]?.ToString() ?? String.Empty;
            ForDepartment = dr["ForDepartment"]?.ToString() ?? String.Empty;
            FirmAddress = dr["FirmAddress"]?.ToString() ?? String.Empty;
            MyOrderDate = dr["MyOrderDate"]?.ToString() ?? String.Empty;
            MyType = dr["MyType"]?.ToString() ?? String.Empty;
            Note = dr["Note"]?.ToString() ?? String.Empty;
            Purpose = dr["Purpose"]?.ToString() ?? String.Empty;
            ExpDate = dr["ExpDate"]?.ToString() ?? String.Empty;
            TotalAmount = dr["TotalAmount"]?.ToString() ?? String.Empty;
            Amount = dr["Amount"]?.ToString() ?? String.Empty;
            DiscountOverAll = dr["DiscountOverAll"]?.ToString() ?? String.Empty;
            Other = dr["Other"]?.ToString() ?? String.Empty;
            FirmName = dr["FirmName"]?.ToString() ?? String.Empty;
            FirmContactNo = dr["FirmContactNo"]?.ToString() ?? String.Empty;
            IniName = dr["IniName"]?.ToString() ?? String.Empty;
            IniId = dr["IniId"]?.ToString() ?? String.Empty;
            OrderDate = dr["OrderDate"]?.ToString() ?? String.Empty;
            CreateDate = dr["CreateDate"]?.ToString() ?? String.Empty;
            ExtendedBillDate = dr["ExtendedBillDate"]?.ToString() ?? String.Empty;
            App1Name = dr["App1Name"]?.ToString() ?? String.Empty;
            App2Name = dr["App2Name"]?.ToString() ?? String.Empty;
            App3Name = dr["App3Name"]?.ToString() ?? String.Empty;
            App4Name = dr["App4Name"]?.ToString() ?? String.Empty;
            App4Designation = dr["App4Designation"]?.ToString() ?? String.Empty;
            App3Designation = dr["App3Designation"]?.ToString() ?? String.Empty;
            App2Designation = dr["App2Designation"]?.ToString() ?? String.Empty;
            App1Designation = dr["App1Designation"]?.ToString() ?? String.Empty;
            App1Status = dr["App1Status"]?.ToString() ?? String.Empty;
            App2Status = dr["App2Status"]?.ToString() ?? String.Empty;
            App3Status = dr["App3Status"]?.ToString() ?? String.Empty;
            App4Status = dr["App4Status"]?.ToString() ?? String.Empty;
            App1On = dr["App1On"]?.ToString() ?? String.Empty;
            App2On = dr["App2On"]?.ToString() ?? String.Empty;
            App3On = dr["App3On"]?.ToString() ?? String.Empty;
            App4On = dr["App4On"]?.ToString() ?? String.Empty;
            ByPass = dr["ByPass"]?.ToString() ?? String.Empty;
            CancelBy = dr["CancelBy"]?.ToString() ?? String.Empty;
            CancelOn = dr["CancelOn"]?.ToString() ?? String.Empty;
            ReferenceBillStatus = dr["ReferenceBillStatus"]?.ToString() ?? String.Empty;
            CancelledReason = dr["CancelledReason"]?.ToString() ?? String.Empty;
            BillVariationRemark = dr["BillVariationRemark"]?.ToString() ?? String.Empty;
            RejectionReason = dr["RejectionReason"]?.ToString() ?? String.Empty;
            VatType = dr["VatType"]?.ToString() ?? String.Empty;
            BudgetRequired = dr["BudgetRequired"]?.ToString() ?? String.Empty;
            BudgetAmount = dr["BudgetAmount"]?.ToString() ?? String.Empty;
            PreviousTaken = dr["PreviousTaken"]?.ToString() ?? String.Empty;
            CurStatus = dr["CurStatus"]?.ToString() ?? String.Empty;
            BudgetStatus = dr["BudgetStatus"]?.ToString() ?? String.Empty;
            BudgetReferenceNo = dr["BudgetReferenceNo"]?.ToString() ?? String.Empty;
            ReferenceNo = dr["ReferenceNo"]?.ToString() ?? String.Empty;
            AppDateDB = dr["AppDateDB"]?.ToString() ?? String.Empty;
            CampusName = dr["CampusName"]?.ToString() ?? String.Empty;




            if (!string.IsNullOrWhiteSpace(this.App1On))
            {
                if (this.ByPass.Contains("Member1,"))
                {
                    this.App1On = "By Passed";
                    this.ShowApp1 = false;
                }

                if (this.Status == "Cancelled" && this.CancelBy == "1")
                {
                    this.App1On = this.CancelOn;
                    this.CancelledByApp1 = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(this.App2On))
            {
                if (this.ByPass.Contains("Member2,"))
                {
                    this.App2On = "By Passed";
                    this.ShowApp2 = false;
                }

                if (this.Status == "Cancelled" && this.CancelBy == "2")
                {
                    this.App2On = this.CancelOn;
                    this.CancelledByApp2 = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(this.App3On))
            {
                if (this.ByPass.Contains("Member3,"))
                {
                    this.App3On = "By Passed";
                    this.ShowApp3 = false;
                }
                if (this.Status == "Cancelled" && this.CancelBy == "3")
                {
                    this.App3On = this.CancelOn;
                    this.CancelledByApp3 = true;
                }
            }

            if (!string.IsNullOrWhiteSpace(this.App4On))
            {
                if (this.ByPass.Contains("Member4,"))
                {
                    this.App4On = "By Passed";
                    this.ShowApp4 = false;
                }
                if (this.Status == "Cancelled" && this.CancelBy == "4")
                {
                    this.App4On = this.CancelOn;
                    this.CancelledByApp4 = true;
                }


            }

            if (string.IsNullOrWhiteSpace(this.App1Name))
            {
                this.App1On = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(this.App2Name))
            {
                this.App2On = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(this.App3Name))
            {
                this.App3On = string.Empty;
            }
            if (string.IsNullOrWhiteSpace(this.App4Designation))
            {
                this.App4On = string.Empty;
            }

            this.CanShowApp4Name = string.IsNullOrWhiteSpace(this.App1Name) && string.IsNullOrWhiteSpace(this.App2Name) && string.IsNullOrWhiteSpace(this.App3Name);
        }
    }
}
