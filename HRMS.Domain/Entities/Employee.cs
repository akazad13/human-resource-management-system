using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class Employee
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public DateTime? DateofBirth { get; set; }
        [StringLength(20)]
        public string? Nationality { get; set; }
        [StringLength(5)]
        public string? BloodGroup { get; set; }
        [StringLength(15)]
        public string? NID { get; set; }
        [StringLength(15)]
        public string? PassportNo { get; set; }
        [StringLength(15)]
        public string? TIN { get; set; }
        [StringLength(50)]
        public string? TaxCircle { get; set; }
        [StringLength(50)]
        public string? Taxzone { get; set; }
        public int? MaritalStatus { get; set; }
        public char? Gender { get; set; }
        [StringLength(20)]
        public string? Religion { get; set; }
        [StringLength(50)]
        public string? PersonalEmail { get; set; }
        [StringLength(15)]
        public string? PersonalPhone { get; set; }
        [StringLength(15)]
        public string? PersonalMobile { get; set; }
        public bool? IsManger { get; set; }

        public ICollection<EmployeeLeavePolicy>? EmployeeLeavePolicies { get; set; }
        public ICollection<LeaveHistory>? LeaveHistories { get; set; }
        public ICollection<WorkHistory>? EmployeeWorkHistories { get; set; }
        public ICollection<WorkHistory>? EmployeeManagerHistories { get; set; }
    }
}
