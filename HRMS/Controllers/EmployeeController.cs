using AutoMapper;
using HRMS.Application.Services.Employee;
using HRMS.Domain.Models.Employee;
using HRMS.Models.Employee;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var model = _mapper.Map<IEnumerable<EmployeeData>, IEnumerable<EmployeeDataModel>>(await _employeeService.GetAll());
            ViewBag.activeMenu = "employee";
            return View(model);
        }
        public IActionResult Add()
        {
            ViewBag.activeMenu = "employee";
            return View("_AddEditEmployee");
        }
        public IActionResult Edit(long id)
        {
            ViewBag.activeMenu = "employee";
            return View("_AddEditEmployee");
        }
    }
}
