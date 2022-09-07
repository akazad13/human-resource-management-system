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
            this.CreateMap<WorkHistoryDetails, WorkHistoryDetailsModel>();
            this.CreateMap<EmployeeDetails, EmployeeDetailsModel>();
            this.CreateMap<EmployeeDetails, EmployeeDetailsModel>().ReverseMap();
            this.CreateMap<WorkHistory, WorkHistoryDetailsModel>().ReverseMap();
        }
        #endregion

        public int Order => 1;
    }
}
