using HRMS.Application.RepositoryInterfaces;
using HRMS.Domain.Entities;
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

        public async Task<bool> Commit()
        {
            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task<User> Get(long userid)
        {
            return await _appDbContext.Users.Where(u=> u.Id == userid).FirstOrDefaultAsync();
        }
    }
}
