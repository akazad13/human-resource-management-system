using HRMS.Application.Common.Interfaces;
using HRMS.Application.RepositoryInterfaces;
using HRMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Services.Employee
{
    public class WorkHistoryService : IWorkHistoryService
    {
        private readonly IWorkHistoryRepository _workHistoryRepository;
        private readonly ILogger<WorkHistoryService> _logger;
        private readonly ICurrentUserService _currentUserService;
        public WorkHistoryService(
            ILogger<WorkHistoryService> logger,
            IWorkHistoryRepository workHistoryRepository,
            ICurrentUserService currentUserService
        )
        {
            _logger = logger;
            _workHistoryRepository = workHistoryRepository;
            _currentUserService = currentUserService;
        }

        public async Task<long> Save(WorkHistory data)
        {
            try
            {
                data.ModifiedBy = _currentUserService.UserId;
                data.ModifiedOn = DateTime.Now;
                if (data.Id == 0)
                {
                    await _workHistoryRepository.Create(data);
                }
                else
                {
                    _workHistoryRepository.Update(data);
                }

                var ret = await _workHistoryRepository.Commit();
                if (ret)
                {
                    return data.Id;
                }
                _logger.LogTrace("Failed to save", data);
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return 0;
            }
        }
    }
}
