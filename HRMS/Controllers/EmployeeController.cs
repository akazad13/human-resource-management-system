using AutoMapper;
using HRMS.Application.Services.Employee;
using HRMS.Domain.Models.Employee;
using HRMS.Models.Employee;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Index()
        {
            var model = _mapper.Map<IEnumerable<EmployeeData>, IEnumerable<EmployeeDataModel>>(await _employeeService.GetAll());
            ViewBag.activeMenu = "employee";
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult Add()
        {
            ViewBag.activeMenu = "employee";
            return View("_AddEditEmployee", new EmployeeDetailsModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id = 0)
        {
            ViewBag.activeMenu = "employee";
            if (id != 0)
            {
                var model = _mapper.Map<EmployeeDetails, EmployeeDetailsModel>(await _employeeService.Get(id));
                return View("_AddEditEmployee", model);
            }
            return View("_AddEditEmployee", new EmployeeDetailsModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Add(EmployeeDetailsModel model)
        {
            ViewBag.activeMenu = "employee";
            if (ModelState.IsValid)
            {
                try
                {
                    var p = await _employeeService.Save(_mapper.Map<EmployeeDetailsModel, EmployeeDetails>(model));
                    ViewBag.Success = "true";
                    ViewBag.SuccessMsg = "Successfully Added.";
                    return View("_AddEditEmployee", model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Add fails: ", ex.Message);
                    ViewBag.Success = "false";
                    return View("_AddEditEmployee", model);
                }
            }
            else
            {
                ViewBag.Success = "false";
                return View("_AddEditEmployee", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeDetailsModel model)
        {
            ViewBag.activeMenu = "employee";
            if (ModelState.IsValid)
            {
                try
                {
                    var p = await _employeeService.Save(_mapper.Map<EmployeeDetailsModel, EmployeeDetails>(model));
                    ViewBag.Success = "true";
                    ViewBag.SuccessMsg = "Successfully Updated.";
                    return View("_AddEditEmployee", model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Update fails: ", ex.Message);
                    ViewBag.Success = "false";
                    return View("_AddEditEmployee", model);
                }
            }
            else
            {
                ViewBag.Success = "false";
                return View("_AddEditEmployee", model);
            }
        }

        public IActionResult AddEditWorkHistoryModal(long employeeId, long workHxId)
        {
            if (workHxId == 0)
            {
                return PartialView("_AddEditWorkHistoryModal", new WorkHistoryDetailsModel() { EmployeeId = employeeId });
            }

            return PartialView("_AddEditWorkHistoryModal", new WorkHistoryDetailsModel() { EmployeeId = employeeId });
        }
    }
}
