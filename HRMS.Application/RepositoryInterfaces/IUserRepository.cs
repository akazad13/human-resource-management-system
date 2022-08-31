using HRMS.Domain.Entities;

namespace HRMS.Application.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<bool> Commit();
        Task<User> Get(long userid);
    }
}
