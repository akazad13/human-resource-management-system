using HRMS.Domain.Models.Employee;

namespace HRMS.Application.Services.Employee
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeData>> GetAll();
        Task<EmployeeDetails> Get(long empid);
        Task<EmployeeDetails> GetByUserid(long userid);
        Task<EmployeeDetails> Save(EmployeeDetails data);
    }
}