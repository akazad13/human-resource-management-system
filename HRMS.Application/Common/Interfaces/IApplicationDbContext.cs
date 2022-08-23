using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }

}