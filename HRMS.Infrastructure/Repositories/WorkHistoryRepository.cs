using HRMS.Application.RepositoryInterfaces;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Persistence.Repositories
{
    public class WorkHistoryRepository : IWorkHistoryRepository
    {
        private readonly ILogger<WorkHistoryRepository> _logger;
        private readonly ApplicationDbContext _appDbContext;

        public WorkHistoryRepository(ILogger<WorkHistoryRepository> logger, ApplicationDbContext appDbContext)
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
        public async Task Create(WorkHistory entity)
        {
            try
            {
                await _appDbContext.WorkHistories.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
        public void Update(WorkHistory entity)
        {
            try
            {
                _appDbContext.Entry<WorkHistory>(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        public void Delete(WorkHistory entity)
        {
            try
            {
                _appDbContext.WorkHistories.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}
