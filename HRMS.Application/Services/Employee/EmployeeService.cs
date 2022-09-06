using HRMS.Application.RepositoryInterfaces;
using HRMS.Application.Services.Auth;
using HRMS.Domain.Models.Employee;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepositroy;
        private readonly IAuthService _authService;
        private readonly ILogger<EmployeeService> _logger;
        public EmployeeService(
            ILogger<EmployeeService> logger,
            IEmployeeRepository employeeRepositroy,
            IAuthService authService
        )
        {
            _logger = logger;
            _employeeRepositroy = employeeRepositroy;
            _authService = authService;
        }

        public async Task<List<EmployeeData>?> GetAll()
        {
            return await _employeeRepositroy.All();
        }

        public async Task<EmployeeDetails?> Get(long empid)
        {
            try
            {
                var emp = await _employeeRepositroy.GetDetails(empid);

                if (emp == null) return null;

                emp.DateOfBirthStr = emp.DateOfBirth?.ToString("yyyy-MM-dd");
                return emp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
        public async Task<EmployeeDetails?> GetByUserid(long userid)
        {
            try
            {
                var emp = await _employeeRepositroy.GetDetailsByUserid(userid);

                if (emp == null) return null;

                emp.DateOfBirthStr = emp.DateOfBirth?.ToShortDateString();
                emp.MaritalStatusDesc = emp.MaritalStatus == 0 ? "Unmarried" : "Married";

                var latestWork = emp.WorkHistoryDetails?.FirstOrDefault();
                if (latestWork != null)
                {
                    emp.ReportsToString = latestWork.ManagerName;
                    emp.Designation = latestWork.Designation;
                    emp.TimeInCurrentPosition = DateTime.Now.Subtract(latestWork.EffectiveDate).TotalDays / 365;
                }

                var startDate = emp.WorkHistoryDetails?.LastOrDefault()?.EffectiveDate;
                emp.StartDate = startDate?.ToShortDateString();
                emp.AccumulatedTenure = startDate == null ? null : DateTime.Now.Subtract(startDate.Value).TotalDays / 365;

                return emp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return null;
            }
        }
        public async Task<long> Save(EmployeeDetails data)
        {
            try
            {
                Domain.Entities.Employee model;

                if (data.EmployeeId == 0)
                {
                    model = new Domain.Entities.Employee();
                    var user = await _authService.Register(data.FirstName, data.LastName, data.Email, data.PhoneNumber, "Password123", new[] { "User" });
                    model.UserId = user.Id;
                    model.User = user;
                }
                else
                {
                    var res = await _employeeRepositroy.Get(data.EmployeeId);
                    if (res == null)
                        return 0;
                    model = res;
                    model.User.FirstName = data.FirstName;
                    model.User.LastName = data.LastName;
                    model.User.Email = data.Email;
                    model.User.NormalizedEmail = data.Email?.ToUpper();
                    model.User.PhoneNumber = data.PhoneNumber;
                }

                model.BloodGroup = data.BloodGroup;
                model.DateofBirth = data.DateOfBirthStr == null ? null : DateTime.Parse(data.DateOfBirthStr);
                model.Gender = data.Gender;
                model.IsManger = data.IsManger;
                model.MaritalStatus = data.MaritalStatus;
                model.Nationality = data.Nationality;
                model.NID = data.NID;
                model.PassportNo = data.PassportNo;
                model.PersonalEmail = data.PersonalEmail;
                model.PersonalMobile = data.PersonalMobile;
                model.PersonalPhone = data.PersonalPhone;
                model.Religion = data.Religion;
                model.TaxCircle = data.TaxCircel;
                model.TaxZone = data.TaxZone;
                model.TIN = data.TIN;

                if (data.EmployeeId == 0)
                {
                    await _employeeRepositroy.Create(model);
                }
                else
                {
                    _employeeRepositroy.Update(model);
                }

                var ret = await _employeeRepositroy.Commit();
                if (ret)
                {
                    return model.Id;
                }
                _logger.LogTrace("Failed to save", model);
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return 0;
            }
        }
    }
}
