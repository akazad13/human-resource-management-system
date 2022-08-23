using HRMS.Application.Common.Interfaces;

namespace HRMS.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
