namespace HRMS.Domain.Models.Employee
{
    public class WorkHistoryDetails
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? Designation { get; set; }
        public long? ManagerId { get; set; }
        public string? ChangedType { get; set; }
        public string? Reason { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
