using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Models.Employee
{
    public class WorkHistoryDetailsModel
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string? Designation { get; set; }
        public long? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public string? ChangedType { get; set; }
        public string? Reason { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public IEnumerable<SelectListItem>? ManagerList { get; set; }
    }
}
