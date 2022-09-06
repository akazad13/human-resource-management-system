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
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        public EmployeeController(
            ILogger<EmployeeController> logger,
            IEmployeeService employeeService,
            IMapper mapper
        )
        {
            _logger = logger;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = _mapper.Map<List<EmployeeData>?, List<EmployeeDataModel>?>(await _employeeService.GetAll());
                ViewBag.activeMenu = "employee";
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return View(new List<EmployeeDataModel>());
            }
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult Add()
        {
            ViewBag.activeMenu = "employee";
            return View("_AddEditEmployee", new EmployeeDetailsModel());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            ViewBag.activeMenu = "employee";
            try
            {
                var model = _mapper.Map<EmployeeDetails?, EmployeeDetailsModel?>(await _employeeService.Get(id));
                if (model == null)
                {
                    ViewBag.Success = "false";
                    ViewBag.msg = $"Can not fetch data to be edited.";
                    return View("_AddEditEmployee", new EmployeeDetailsModel());
                }
                return View("_AddEditEmployee", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                ViewBag.Success = "false";
                ViewBag.msg = $"Can not fetch data to be edited.";
                return View("_AddEditEmployee", new EmployeeDetailsModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Add(EmployeeDetailsModel model)
        {
            return await Save(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeDetailsModel model)
        {
            return await Save(model);
        }

        //public IActionResult AddEditWorkHistoryModal(long employeeId, long workHxId)
        //{
        //    if (workHxId == 0)
        //    {
        //        return PartialView("_AddEditWorkHistoryModal", new WorkHistoryDetailsModel() { EmployeeId = employeeId });
        //    }

        //    return PartialView("_AddEditWorkHistoryModal", new WorkHistoryDetailsModel() { EmployeeId = employeeId });
        //}

        #region Private Methods
        private async Task<IActionResult> Save(EmployeeDetailsModel model)
        {
            ViewBag.activeMenu = "employee";
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var ret = await _employeeService.Save(_mapper.Map<EmployeeDetailsModel, EmployeeDetails>(model));
                        if (ret == 0)
                        {
                            ViewBag.Success = "false";
                            ViewBag.msg = "Failed to save data.";
                        }
                        else
                        {
                            ViewBag.Success = "true";
                            ViewBag.msg = $"Successfully {(model.EmployeeId == 0 ? "Added" : "Updated")}.";
                            model.EmployeeId = ret;
                        }
                        return View("_AddEditEmployee", model);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                        ViewBag.msg = $"{(model.EmployeeId == 0 ? "Add" : "Update")} fails: ";
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                ViewBag.msg = $"{(model.EmployeeId == 0 ? "Add" : "Update")} fails: ";
                ViewBag.Success = "false";
                return View("_AddEditEmployee", model);
            }
        }
        #endregion
    }
}
