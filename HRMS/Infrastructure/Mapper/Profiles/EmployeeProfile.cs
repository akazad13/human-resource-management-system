using AutoMapper;
using HRMS.Domain.Entities;
using HRMS.Domain.Models.Employee;
using HRMS.Models.Employee;

namespace HRMS.Infrastructure.Mapper.Profiles
{
    public class EmployeeProfile : Profile, IOrderedMapperProfile
    {
        #region Ctor
        public EmployeeProfile()
        {
            this.CreateMap<EmployeeData, EmployeeDataModel>();
            this.CreateMap<WorkHistory, WorkHistoryModel>();
            this.CreateMap<EmployeeDetails, EmployeeDetailsModel>();
        }
        #endregion

        public int Order => 1;
    }
}
