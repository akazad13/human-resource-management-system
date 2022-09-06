using HRMS.Application.RepositoryInterfaces;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _appDbContext;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ILogger<UserRepository> logger, ApplicationDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public async Task<bool> Commit()
        {
            try
            {
                return await _appDbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public async Task<User?> Get(long userid)
        {
            try
            {
                return await _appDbContext.Users.Where(u => u.Id == userid).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
