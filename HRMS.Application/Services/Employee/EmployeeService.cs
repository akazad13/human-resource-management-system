using HRMS.Application.RepositoryInterfaces;
using HRMS.Domain.Models.Employee;

namespace HRMS.Application.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepositroy;
        public EmployeeService(IEmployeeRepository employeeRepositroy)
        {
            _employeeRepositroy = employeeRepositroy;
        }

        public async Task<IEnumerable<EmployeeData>> GetAll()
        {
            return await _employeeRepositroy.All();
        }

        public async Task<EmployeeDetails> Get(long empid)
        {
            var emp = await _employeeRepositroy.Get(empid);
            return emp;
        }
    }
}
