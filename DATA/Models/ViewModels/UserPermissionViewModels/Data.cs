using DATA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels.UserPermissionViewModels
{
    public class Data
    {
        public Menu menu { get; set; }
        public string parentMenu { get; set; }
        public Data()
        {
            menu = null;
            parentMenu = "";
        }

    }
    public class SelectedData
    {
        public string selectedID { get; set; }
    }
    public class ExcelData
    {

        public string SerialOfDay { get; set; }
        public string GatewayCardNumber { get; set; }
        public string refID { get; set; }
        public string clientID { get; set; }
        public string GatewayRespondTime { get; set; }
        public string PaidFor { get; set; }
        public string PaidForDetail { get; set; }
        public string PolicyNumber { get; set; }
        public string ReceiptHeading { get; set; }
        public decimal GrandAmount { get; set; }
        public string GatewayStatus { get; set; }
        public string POName { get; set; }
        public string Agent { get; set; }
        public string PayerEmail { get; set; }
        public string GatewayCardBrand { get; set; }
        public string GatewayTransactionID { get; set; }
        public string GatewayName { get; set; }
    }
    public class ExcelDataSubmission
    {
        public string PolicyNumber { get; set; }
        public string YearOfBirth { get; set; }
        public string POName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BenefitType { get; set; }
        public string CreationTime { get; set; }

    }
    public class ExcelDatForEcollection
    {
        public string Orderid { get; set; }
        public string beneficiaryName { get; set; }
        public string bankAccountNumber { get; set; }
        public string payingBank { get; set; }
        public decimal Amount { get; set; }
        public string bankName { get; set; }
        public string branch { get; set; }
    }


    public class DummySubmissionSearch
    {
        [StringLength(40), Display(Name = "Policy Number")]
        public string PolicyNumber { get; set; }

        [StringLength(40), Display(Name = "YearOfBirth")]
        public string YearOfBirth { get; set; }

        [StringLength(40), Display(Name = "Policy Owner Name")]
        public string POName { get; set; }

        [StringLength(40), Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(20), Display(Name = "Phone")]
        public string Phone { get; set; }

        [StringLength(20), Display(Name = "Benefit Type")]
        public string BenefitType { get; set; }

        [Display(Name = "From Date")]
        public string startDate { get; set; }

        [Display(Name = "To Date")]
        public string endDate { get; set; }


        [Display(Name = "Download Status")]
        public int downloadStatus { get; set; }

    }
    public partial class UnclaimedSubmission
    {
        public int ID { get; set; }
        public byte[] PolicyNumber { get; set; }
        public byte[] YearOfBirth { get; set; }
        public byte[] POName { get; set; }
        public byte[] Email { get; set; }
        public byte[] Phone { get; set; }
        public string BenefitType { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? UpdationTime { get; set; }
        public byte? Downloaded { get; set; }
    }
    public class DummySearch
    {
        [StringLength(40), Display(Name = "Customer Name")]
        public string customerName { get; set; }

        [StringLength(40), Display(Name = "Payer Email")]
        public string payerEmail { get; set; }

        [StringLength(40), Display(Name = "Policy Owner Email")]
        public string poEmail { get; set; }

        [StringLength(40), Display(Name = "Transaction ID")]
        public string TransactionID { get; set; }

        [StringLength(20), Display(Name = "Payo ID")]
        public string payoid { get; set; }

        [StringLength(20), Display(Name = "Policy Number")]
        public string policyNumber { get; set; }

        [StringLength(20), Display(Name = "Type")]
        public string type { get; set; }

        [StringLength(20), Display(Name = "Gateway")]
        public string gateways { get; set; }

        [Display(Name = "Download Status")]
        public int downloadStatus { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Payment From Date")]
        public string startDate { get; set; }

        [Display(Name = "Payment To Date")]
        public string endDate { get; set; }

        public IEnumerable<SelectListItem> DropDownListForPaymentType { get; set; }
        public IEnumerable<SelectListItem> DropDownListForGateways { get; set; }
    }

}
