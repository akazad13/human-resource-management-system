using HRMS.Domain.Entities;

namespace HRMS.Domain.Models.Employee
{
    public class EmployeeDetails
    {
        public long EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Designation { get; set; }
        public bool IsManger { get; set; }
        public string? ReportsToString { get; set; }
        public double? TimeInCurrentPosition { get; set; }
        public string? StartDate { get; set; }
        public double? AccumulatedTenure { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? DateOfBirthStr { get; set; }
        public string? Nationality { get; set; }
        public string? BloodGroup { get; set; }
        public char? Gender { get; set; }
        public int? MaritalStatus { get; set; }
        public string? MaritalStatusDesc { get; set; }
        public string? Religion { get; set; }
        public string? NID { get; set; }
        public string? PassportNo { get; set; }
        public string? TIN { get; set; }
        public string? TaxCircel { get; set; }
        public string? TaxZone { get; set; }
        public string? PersonalEmail { get; set; }
        public string? PersonalPhone { get; set; }
        public string? PersonalMobile { get; set; }
        public IEnumerable<WorkHistoryDetails>? WorkHistoryDetails { get; set; }
    }
}
