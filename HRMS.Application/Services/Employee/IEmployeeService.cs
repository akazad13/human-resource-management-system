using HRMS.Domain.Models.Employee;

namespace HRMS.Application.Services.Employee
{
    public interface IEmployeeService
    {
        Task<List<EmployeeData>?> GetAll();
        Task<EmployeeDetails?> Get(long empid);
        Task<EmployeeDetails?> GetByUserid(long userid);
        Task<long> Save(EmployeeDetails data);
    }
}