using HRMS.Domain.Entities;

namespace HRMS.Application.Common.Utilities
{
    public interface IHelper
    {
        Task<string> GenerateJwtToken(User user);
    }
}
