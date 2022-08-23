using HRMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HRMS.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("_Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("_Register");
        }
    }
}