namespace HRMS.Domain.Entities
{
    public class EmployeeLeavePolicy
    {
        public long EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int LeavePolicyId { get; set; }
        public LeavePolicy? LeavePolicy { get; set; }
    }
}
