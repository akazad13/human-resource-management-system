using HRMS.Domain.Entities;
using HRMS.Domain.Models.Employee;

namespace HRMS.Application.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeData>> All();
        Task<EmployeeDetails> GetDetails(long userid);
        Task<EmployeeDetails> GetDetailsByUserid(long userid);
        Task<Employee> Get(long empid);
        Task<bool> Commit();
        Task Create(Employee entity);
        void Delete(Employee entity);
        void Update(Employee entity);
    }
}
