using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CORPORATE_DISBURSEMENT_UTILITY
{
    /// <summary>
    /// Generic class to keep commonly used enumerations in project
    /// </summary>
    public static class CommonEnum
    {
        public enum API_Response_Codes
        {
            [Description("Operation was successful")]
            Success = 200,
            [Description("Unauthozied")]
            Unauthorized = 401,
            [Description("Forbidden access")]
            Forbidden = 403,
            [Description("Something went wrong, please try again later.")]
            UnprocessableEntity = 422,
            [Description("Internal server error")]
            InternalServerError = 500,
            [Description("transaction_id cannot be empty")]
            TransactionIdEmpty = 6001,
            [Description("Transaction cannot be found!")]
            TransactionNotFound = 6002,
            [Description("Could not create/update refund request. Please try again later.")]
            RefundRequestCreateOrUpdateFailure = 6003
        }
        public enum CommonAPIResponseCodes
        {
            payment_cancelled = 113,
        }
        public enum ExternalAPIRequestType
        {
            GET = 1,
            POST = 2,
            POSTHttpFormURLEncoded = 3,
            POST_FORMDATA = 4,
            POSTXML = 5
        }
        public enum APIScope
        {
            CashBaba = 1,
            CommonAPIRefundApproval = 2,
            CB_API = 3,
            CB_APITE = 4,
            CB_SSM = 5,
            CB_ADMIN = 6,
            CB_EC = 7
        }
        public enum Refund_Request_Status
        {
            [Display(Name = "Approved", Description = "Approval")]
            APPROVED = 1,
            [Display(Name = "Rejected", Description = "Rejection")]
            REJECTED = 2,
            [Display(Name = "Pending", Description = "Pending")]
            PENDING = 3
        }
        public enum Settlement_Status
        {
            SETTLED = 1,
            PROCESSING = 2,
            UNSETTLED = 3
        }
        public enum Common_API_Status
        {
            [Display(Name = "Successful", Description = "Successful")]
            SUCCESSFUL = 11,
            [Display(Name = "Refunded", Description = "Refunded")]
            REFUNDED = 17,
            [Display(Name = "Initiated", Description = "Initiated")]
            INITIATED = 5,
            [Display(Name = "Failed_1", Description = "Failed_1")]
            FAILED_1 = 7,
            [Display(Name = "Failed_2", Description = "Failed_2")]
            FAILED_2 = 9,
            [Display(Name = "Pending", Description = "Pending")]
            PENDING = 48
        }
        public enum Audit_Trail_Purpose
        {
            AcceptRefundRequest,
            RejectRefundRequest,
            SubmitRefundRequest,
            SettleTransactionRequests
        }
        public enum LogFolderNames
        {
            SerLogs, ErrorLogs
        }
        public static T[] GetEnumValues<T>() where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("GetValues<T> can only be called for types derived from System.Enum", "T");
            }
            return (T[])Enum.GetValues(typeof(T));
        }
        public static List<string> GetEnumNames(Enum enm)
        {
            var type = enm.GetType();
            var displaynames = new List<string>();
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {

                displaynames.Add(name);

            }
            return displaynames;
        }
        public static T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            return Enum.TryParse(str, true, out T val) ? val : default;
        }

        public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }

            return (T)Enum.ToObject(enumType, intValue);
        }
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()?
                            .GetName() ?? string.Empty;
        }
        public static string? GetDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                       .GetMember(enumValue.ToString())
                       .First()
                       .GetCustomAttribute<DescriptionAttribute>()?
                       .Description ?? string.Empty;
        }

        public enum Reconcile_Config_Status
        {
            INACTIVE = 0,
            ACTIVE = 1
        }

        public enum Reconcile_Log_Status
        {
            INITIATED = 1,
            SUCCESS = 2,
            FAILED = 3,
        }

        public enum Reconcile_Log_File_Status
        {
            FAILED = 0,
            SUCCESS = 1,
        }

        public enum Reconcile_Log_Email_Status
        {
            FAILED = 0,
            SUCCESS = 1,
        }
        public enum StakeholderCredential
        {
            grant_type = 1,
            client_id = 2,
            client_secret = 3,
            cb_base_url = 4,
            cb_token_url_endpoint = 5,
            cb_ssl_corporateDisbursement_url_endpoint = 6
        }
        public enum DEV_CERT_HTTP_CLIENT
        {
            ExternalApiClient = 1
        }
        public enum Client
        {
            CashBaba = 1
        }
    }
}
