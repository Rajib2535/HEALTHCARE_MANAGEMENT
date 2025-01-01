namespace CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ReportModels
{
    public class UserRegistrationSummary
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public DateTimeOffset RegistrationTime { get; set; }
    }
    public class SSLUserRegistrationSummary
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public long EmployeeId { get; set; }
        public string Department { get; set; }
        public DateTimeOffset RegistrationTime { get; set; }
    }

    public class StatusWiseUserCount
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class DepartmentWiseUserCount
    {
        public string Department { get; set; }
        public int Count { get; set; }
    }
    public class Departments
    {
        public string Department { get; set; }
    }
}
