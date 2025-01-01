namespace CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.Helper
{
    public class Helper
    {
        public enum UserStatus
        {
            New,
            Approved,
            Banned,
            Dormant,
            Unapproved
        }

        public enum UserRole
        {
            MonitoringUser,
            NRB
        }
        public enum CorporateDisbursementSummaryStatus
        {
            Authorized, Initiate, Fail, Complete, Processing, Rejected
        }
        public enum CorporateDisbursementsStatus
        {
            Initiate, Fail, Success, Pending, Processing, Rejected
        }
    }
}
