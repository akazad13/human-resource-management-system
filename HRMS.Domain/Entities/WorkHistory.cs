using System.ComponentModel.DataAnnotations;

namespace HRMS.Domain.Entities
{
    public class WorkHistory
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime EffectiveDate { get; set; }
        [StringLength(50)]
        public string? Designation { get; set; }
        public long? ManagerId { get; set; }
        public Employee? Manager { get; set; }
        public string? ChangedType { get; set; }
        public string? Reason { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
