using HRMS.Application.RepositoryInterfaces;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _appDbContext;

        public UserRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Commit()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
