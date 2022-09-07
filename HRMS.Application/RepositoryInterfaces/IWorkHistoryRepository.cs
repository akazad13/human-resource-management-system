using HRMS.Domain.Entities;

namespace HRMS.Application.RepositoryInterfaces
{
    public interface IWorkHistoryRepository
    {
        Task<bool> Commit();
        Task Create(WorkHistory entity);
        void Update(WorkHistory entity);
        void Delete(WorkHistory entity);
    }
}