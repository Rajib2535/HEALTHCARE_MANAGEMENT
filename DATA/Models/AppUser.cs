
using DATA.Interface;
using Microsoft.AspNetCore.Identity;

namespace DATA.Models
{
    public class AppUser : IdentityUser<long>, ITrackable, ISoftDeletable
    {
        public string FullName { get; set; }
        public string TransactionPin { get; set; }
        public string ProfileImage { get; set; }
        public string SecondaryMobile { get; set; }
        public string SecondaryEmail { get; set; }
        public string Occupation { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceModel { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }
        public string Status { get; set; }
        public string UnapprovedCause { get; set; }
        public DateTimeOffset LastPasswordChangedDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
      

      
       
        public long TrFeePackageId { set; get; }
        public long TrLimitPackageId { set; get; }
        public string DeviceToken { get; set; }
        public long? BillerCategoryId { set; get; }
        public string PromoCode { get; set; }
        public long? PromotPackageId { get; set; }
        public long? ReferralAppUserId { get; set; }
        public string ReferralCode { get; set; }
        public string UserPin { get; set; }
        public int? UserPinCount { get; set; }
        public DateTimeOffset? UserPinLockDateTime { get; set; }
        public DateTimeOffset? AgreedDateTime { get; set; }
        public bool? SentMoneyToNonRegWallet { get; set; }
        public long ParentUserId { get; set; }
        public bool IsWalletUser { get; set; }
        public virtual ICollection<IdentityUserClaim<long>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<long>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<long>> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public bool IsActive { set; get; }
        public string InActiveCause { set; get; }
        public bool? IsIdtpUser { set; get; }
        public int? BiometricCount { get; set; }
        public DateTimeOffset? BiometricLockDateTime { get; set; }
        public bool? BiometricEnabled { get; set; }
        public bool? IsSystemWallet { get; set; }
    }
}
