using HRMS.Application.Services.Auth;
using HRMS.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return PartialView("_Login", new AuthRequest());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthRequest authRequest)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var p = await _authService.Login(authRequest.Email, authRequest.Password);
                    if (p == true)
                    {
                        return RedirectToAction("index", "home", new { });
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("loginFailed", ex.Message);
                    ViewBag.Success = "false";
                    return PartialView("_Login", authRequest);
                }
            }
            else
            {
                ViewBag.Success = "false";
                return PartialView("_Login", authRequest);
            }
        }

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View("_Register");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return RedirectToAction("login", "auth");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

    }
}