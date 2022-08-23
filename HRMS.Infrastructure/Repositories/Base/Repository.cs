using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Repositories.Base;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IApplicationDbContext _dbContext;
        public Repository(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> AddAsync(T entity)
        {
            throw new NotImplementedException();
            //await _dbContext.Set<T>().AddAsync(entity);
            //await _dbContext.SaveChangesAsync();
            //return entity;
        }
        public async Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
            //_dbContext.Set<T>().Remove(entity);
            //await _dbContext.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            throw new NotImplementedException();
            //return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
            //return await _dbContext.Set<T>().FindAsync(id);
        }
        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
