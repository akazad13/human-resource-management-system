using HRMS.Application.RepositoryInterfaces;
using HRMS.Domain.Entities;
using HRMS.Domain.Models.Employee;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Persistence.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public EmployeeRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> Commit()
        {
            return await _appDbContext.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<EmployeeData>> All()
        {
            return await _appDbContext.Employees.Include(emp => emp.User).Include(emp => emp.EmployeeWorkHistories.OrderByDescending(wh => wh.EffectiveDate).Take(1)).Select(emp => new EmployeeData
            {
                EmployeeId = emp.Id,
                FirstName = emp.User.FirstName,
                LastName = emp.User.LastName,
                Email = emp.User.Email,
                Designation = emp.EmployeeWorkHistories == null ? null : emp.EmployeeWorkHistories.Select(wh => wh.Designation).FirstOrDefault()

            }).AsNoTracking().ToListAsync();
        }
        public async Task<EmployeeDetails> GetDetails(long empid)
        {
            return await _appDbContext.Employees.Where(emp => emp.Id == empid).Include(emp => emp.User).Include(emp => emp.EmployeeWorkHistories).Select(emp => new EmployeeDetails
            {
                EmployeeId = emp.Id,
                FirstName = emp.User.FirstName,
                LastName = emp.User.LastName,
                Email = emp.User.Email,
                IsManger = emp.IsManger?? false,
                Gender = emp.Gender,
                PhoneNumber = emp.User.PhoneNumber,
                DateOfBirth = emp.DateofBirth,
                Nationality = emp.Nationality,
                BloodGroup = emp.BloodGroup,
                MaritalStatus = emp.MaritalStatus,
                Religion = emp.Religion,
                NID = emp.NID,
                PassportNo = emp.PassportNo,
                TIN = emp.TIN,
                TaxCircel = emp.TaxCircle,
                TaxZone = emp.TaxZone,
                PersonalEmail = emp.PersonalEmail,
                PersonalPhone = emp.PersonalPhone,
                PersonalMobile = emp.PersonalMobile,
                WorkHistoryDetails = emp.EmployeeWorkHistories.OrderByDescending(wh => wh.EffectiveDate).Select(wh => new WorkHistoryDetails()
                {
                    Id = wh.Id,
                    EmployeeId = wh.EmployeeId,
                    EffectiveDate = wh.EffectiveDate,
                    Designation = wh.Designation,
                    ManagerId = wh.ManagerId,
                    ManagerName = wh.Manager.User.FirstName + " " + wh.Manager.User.LastName,
                    ChangedType = wh.ChangedType,
                    Reason = wh.Reason,
                    ModifiedBy = wh.ModifiedBy,
                    ModifiedOn = wh.ModifiedOn
                }).ToList()
            }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<EmployeeDetails> GetDetailsByUserid(long userid)
        {
            return await _appDbContext.Employees.Where(emp => emp.UserId == userid).Include(emp => emp.User).Include(emp => emp.EmployeeWorkHistories).Select(emp => new EmployeeDetails
            {
                EmployeeId = emp.Id,
                FirstName = emp.User.FirstName,
                LastName = emp.User.LastName,
                Email = emp.User.Email,
                IsManger = emp.IsManger ?? false,
                Gender = emp.Gender,
                PhoneNumber = emp.User.PhoneNumber,
                DateOfBirth = emp.DateofBirth,
                Nationality = emp.Nationality,
                BloodGroup = emp.BloodGroup,
                MaritalStatus = emp.MaritalStatus,
                Religion = emp.Religion,
                NID = emp.NID,
                PassportNo = emp.PassportNo,
                TIN = emp.TIN,
                TaxCircel = emp.TaxCircle,
                TaxZone = emp.TaxZone,
                PersonalEmail = emp.PersonalEmail,
                PersonalPhone = emp.PersonalPhone,
                PersonalMobile = emp.PersonalMobile,
                WorkHistoryDetails = emp.EmployeeWorkHistories.OrderByDescending(wh => wh.EffectiveDate).Select(wh => new WorkHistoryDetails()
                {
                    Id = wh.Id,
                    EmployeeId = wh.EmployeeId,
                    EffectiveDate = wh.EffectiveDate,
                    Designation = wh.Designation,
                    ManagerId = wh.ManagerId,
                    ManagerName = wh.Manager.User.FirstName + " " + wh.Manager.User.LastName,
                    ChangedType = wh.ChangedType,
                    Reason = wh.Reason,
                    ModifiedBy = wh.ModifiedBy,
                    ModifiedOn = wh.ModifiedOn
                }).ToList()
            }).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Employee> Get(long empid)
        {
            return await _appDbContext.Employees.Where(emp => emp.Id == empid).Include(emp => emp.User).FirstOrDefaultAsync();
        }

        public async Task Create(Employee entity)
        {
            await _appDbContext.Employees.AddAsync(entity);
        }


        public void Delete(Employee entity)
        {
            _appDbContext.Employees.Remove(entity);
        }
        public void Update(Employee entity)
        {
            _appDbContext.Entry<Employee>(entity).State = EntityState.Modified;
        }
    }
}
