using HRMS.Application.RepositoryInterfaces;
using HRMS.Domain.Entities;
using HRMS.Domain.Models.Employee;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Persistence.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly ApplicationDbContext _appDbContext;

        public EmployeeRepository(ILogger<EmployeeRepository> logger, ApplicationDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        public async Task<bool> Commit()
        {
            try
            {
                return await _appDbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
        public async Task<List<EmployeeData>?> All()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        public async Task<EmployeeDetails?> GetDetails(long empid)
        {
            try
            {
                var query = _appDbContext.Employees.Where(emp => emp.Id == empid);
                return await GetDetailsByFilter(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<EmployeeDetails?> GetDetailsByUserid(long userid)
        {
            try
            {
                var query = _appDbContext.Employees.Where(emp => emp.UserId == userid);
                return await GetDetailsByFilter(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<Employee?> Get(long empid)
        {
            try
            {
                return await _appDbContext.Employees.Where(emp => emp.Id == empid).Include(emp => emp.User).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task<List<Employee>?> GetManagers()
        {
            try
            {
                return await _appDbContext.Employees.Where(emp => emp.IsManger == true).Include(emp => emp.User).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task Create(Employee entity)
        {
            try
            {
                await _appDbContext.Employees.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }


        public void Delete(Employee entity)
        {
            try
            {
                _appDbContext.Employees.Remove(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
        public void Update(Employee entity)
        {
            try
            {
                _appDbContext.Entry<Employee>(entity).State = EntityState.Modified;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }


        #region
        private async Task<EmployeeDetails?> GetDetailsByFilter(IQueryable<Employee> query)
        {
            return await query.Include(emp => emp.User).Include(emp => emp.EmployeeWorkHistories).Select(emp => new EmployeeDetails
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
                WorkHistoryDetails = emp.EmployeeWorkHistories == null ? null : emp.EmployeeWorkHistories.OrderByDescending(wh => wh.EffectiveDate).Select(wh => new WorkHistoryDetails()
                {
                    Id = wh.Id,
                    EmployeeId = wh.EmployeeId,
                    EffectiveDate = wh.EffectiveDate,
                    Designation = wh.Designation,
                    ManagerId = wh.ManagerId,
                    ManagerName = wh.Manager == null ? "" : $"{ wh.Manager.User.FirstName } {wh.Manager.User.LastName}",
                    ChangedType = wh.ChangedType,
                    Reason = wh.Reason,
                    ModifiedBy = wh.ModifiedBy,
                    ModifiedOn = wh.ModifiedOn
                }).ToList()
            }).AsNoTracking().AsSplitQuery().FirstOrDefaultAsync();
        }
        #endregion
    }
}
