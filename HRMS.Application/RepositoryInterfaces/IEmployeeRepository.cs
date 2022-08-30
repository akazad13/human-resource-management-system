using HRMS.Domain.Entities;
using HRMS.Domain.Models.Employee;

namespace HRMS.Application.RepositoryInterfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeData>> All();
        Task<EmployeeDetails> Get(long empid);
        Task Commit();
        Task Create(Employee entity);
        void Delete(Employee entity);
        void Update(Employee entity);
    }
}
