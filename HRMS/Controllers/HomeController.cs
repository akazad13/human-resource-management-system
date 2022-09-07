using AutoMapper;
using HRMS.Application.Services.Employee;
using HRMS.Domain.Models.Employee;
using HRMS.Models;
using HRMS.Models.Employee;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HRMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IEmployeeService employeeService, IMapper mapper)
        {
            _logger = logger;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            long.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out long userid);
            var model = _mapper.Map<EmployeeDetails?, EmployeeDetailsModel?>(await _employeeService.GetByUserid(userid));
            ViewBag.activeMenu = "home";
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}