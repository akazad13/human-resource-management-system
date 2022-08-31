using HRMS.Application.RepositoryInterfaces;
using HRMS.Application.Services.Auth;
using HRMS.Domain.Models.Employee;

namespace HRMS.Application.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepositroy;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        public EmployeeService(IEmployeeRepository employeeRepositroy, IUserRepository userRepository, IAuthService authService)
        {
            _employeeRepositroy = employeeRepositroy;
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<IEnumerable<EmployeeData>> GetAll()
        {
            return await _employeeRepositroy.All();
        }

        public async Task<EmployeeDetails> Get(long empid)
        {
            var emp =  await _employeeRepositroy.GetDetails(empid);
            emp.DateOfBirthStr = emp.DateOfBirth?.ToString("yyyy-MM-dd");
            return emp;
        }
        public async Task<EmployeeDetails> GetByUserid(long userid)
        {
            var emp = await _employeeRepositroy.GetDetailsByUserid(userid);

            var latestWork = emp.WorkHistoryDetails?.FirstOrDefault();
            emp.ReportsToString = latestWork?.ManagerName;
            emp.Designation = latestWork?.Designation;
            emp.DateOfBirthStr = emp.DateOfBirth?.ToString("yyyy-MM-dd");
            var effectiveDateOfCurrentPostion = latestWork?.EffectiveDate;

            emp.StartDate = emp.WorkHistoryDetails?.LastOrDefault()?.EffectiveDate.ToShortDateString();


            return emp;
        }
        public async Task<EmployeeDetails> Save(EmployeeDetails data)
        {
            try
            {

                Domain.Entities.Employee model;

                if (data.EmployeeId == 0)
                {
                    model = new Domain.Entities.Employee();
                    var user = await _authService.Register(data.FirstName, data.LastName, data.Email, data.PhoneNumber, "Password123", new[] { "User" });
                    model.UserId = user.Id;
                    model.User = await _userRepository.Get(data.EmployeeId);
                }
                else
                {
                    model = await _employeeRepositroy.Get(data.EmployeeId);
                    model.User.FirstName = data.FirstName;
                    model.User.LastName = data.LastName;
                    model.User.Email = data.Email;
                    model.User.NormalizedEmail = data.Email?.ToUpper();
                    model.User.PhoneNumber = data.PhoneNumber;
                }

                if (model == null)
                    return null;

                model.BloodGroup = data.BloodGroup;
                model.DateofBirth = data.DateOfBirthStr == null? null: DateTime.Parse(data.DateOfBirthStr);
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

                if (data.EmployeeId == null || data.EmployeeId == 0)
                {
                    await _employeeRepositroy.Create(model);
                }
                else
                {
                    _employeeRepositroy.Update(model);
                }

                var res = await _employeeRepositroy.Commit();
                return await Get(model.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
