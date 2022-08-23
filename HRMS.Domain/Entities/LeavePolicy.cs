using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class LeavePolicy
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Leave policy name is required")]
        [StringLength(50, ErrorMessage = "Leave policy name should be maximum of 50 characters")]
        [DataType(DataType.Text)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Total number of leave is required")]
        [Range(0, 366, ErrorMessage = "The {0} must be between {1} and {2}.")]
        public int TotalNoOfLeave { get; set; }
        public ICollection<EmployeeLeavePolicy>? EmployeeLeavePolicies { get; set; }

    }
}
