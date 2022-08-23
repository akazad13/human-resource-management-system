using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class LeaveHistory
    {
        public long Id { get; set; }
        public DateTimeOffset ApplicationDate { get; set; }
        public string? Comment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Range(0, 366, ErrorMessage = "The {0} must be between {1} and {2}.")]
        public int NoOfLeaves { get; set; }

        public LeaveStatus? LeaveStatus { get; set; }
        public int LeaveStatusId { get; set; }
        public long EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
