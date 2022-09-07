using HRMS.Domain.Entities;

namespace HRMS.Application.Services.Employee
{
    public interface IWorkHistoryService
    {
        Task<long> Save(WorkHistory data);
    }
}