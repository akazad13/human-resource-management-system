using AutoMapper;
using HRMS.Application.Services.Employee;
using HRMS.Domain.Entities;
using HRMS.Domain.Models.Employee;
using HRMS.Models.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMS.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IWorkHistoryService _workHistoryService;
        private readonly IMapper _mapper;
        public EmployeeController(
            ILogger<EmployeeController> logger,
            IEmployeeService employeeService,
            IWorkHistoryService workHistoryService,
            IMapper mapper
        )
        {
            _logger = logger;
            _employeeService = employeeService;
            _mapper = mapper;
            _workHistoryService = workHistoryService;
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

        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> AddEditWorkHistoryModal(long employeeId, long workHxId)
        {
            var managers = await _employeeService.GetManagers();
            var model = new WorkHistoryDetailsModel
            {
                ManagerList = managers?.Select(m => new SelectListItem()
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.User?.FirstName} {m.User?.LastName}"
                }),
                EmployeeId = employeeId,
                Id = workHxId
            };
            return PartialView("_AddEditWorkHistoryModal", model);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditWorkHistoryModal(WorkHistoryDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var id = await _workHistoryService.Save(_mapper.Map<WorkHistoryDetailsModel, WorkHistory>(model));
                    if (id != 0)
                    {
                        return Json(new
                        {
                            status = "success",
                            message = "Successfully Saved.",
                            id = id
                        }, new Newtonsoft.Json.JsonSerializerSettings());
                    }
                    else
                    {
                        return Json(new
                        {
                            status = "failed",
                            message = "Failed to Save.",
                            id = 0
                        }, new Newtonsoft.Json.JsonSerializerSettings());
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    return Json(new
                    {
                        status = "failed",
                        message = "Internal problem, please try again!",
                        id = 0
                    }, new Newtonsoft.Json.JsonSerializerSettings());
                }
            }
            else
            {
                return Json(new
                {
                    status = "failed",
                    message = "Invalid data.",
                    id = 0
                }, new Newtonsoft.Json.JsonSerializerSettings());
            }
        }


        #region Private Methods
        private async Task<IActionResult> Save(EmployeeDetailsModel model)
        {
            ViewBag.activeMenu = "employee";

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
        #endregion
    }
}
