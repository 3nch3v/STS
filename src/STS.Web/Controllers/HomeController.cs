using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels;
using STS.Web.ViewModels.Identity;

using static STS.Common.GlobalConstants;

namespace STS.Web.Controllers
{
    public class HomeController : Controller
    {
        private const string invalidLogin = "Invalid login attempt.";
        private const string errKey = "LoginAttempt";
        private const string logInfoMsg = "User logged in.";

        private readonly IAdminService adminService;
        private readonly ILogger<HomeController> logger;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(
            IAdminService adminService,
            ILogger<HomeController> logger,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.adminService = adminService;
            this.logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Tickets", "Tickets");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginInputModel userData)
        {
            if (this.ModelState.IsValid)
            {
                var userByEmail = await userManager.FindByEmailAsync(userData.Email);
                var userByUsername = await userManager.FindByNameAsync(userData.Email);

                if (userByEmail == null && userByUsername == null)
                {
                    this.ModelState.AddModelError(errKey, invalidLogin);
                    return View();
                }
                
                var result = await this.signInManager
                    .PasswordSignInAsync(userByEmail ?? userByUsername, 
                                         userData.Password, 
                                         true, 
                                         lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    this.logger.LogInformation(logInfoMsg);
                    return RedirectToAction("Tickets", "Tickets");
                }

                this.ModelState.AddModelError(errKey, invalidLogin);
                return View();
            }

            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(ChangePassInputModel passInput)
        {
            if (passInput.NewPass != passInput.RepeatPass) 
            {
                ModelState.AddModelError("RepeatPass", "Password doesn't match."); 
            }

            if (!ModelState.IsValid) 
            {
                return View(passInput);
            }

            string userId = userManager.GetUserId(User);
            var user = await userManager.FindByIdAsync(userId);

            var passChangeResult = await userManager.ChangePasswordAsync(user, passInput.OldPass, passInput.NewPass);

            if (!passChangeResult.Succeeded)
            {
                await signInManager.SignOutAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Tickets", "Tickets");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int code)
        {
            string msg = this.GetMessage(code);

            var error = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = msg,
            };

            return View(error);
        }

        private string GetMessage(int code)
        {
            return code switch
            {
                400 => ErrorMessage.BadRequest,
                401 => ErrorMessage.Unauthorised,
                403 => ErrorMessage.Forbidden,
                404 => ErrorMessage.NotFound,
                500 => ErrorMessage.ServerError,
                502 => ErrorMessage.BadGateway,
                505 => ErrorMessage.HttpNotSupported,
                _ => this.ViewBag.ErrorMessage = ErrorMessage.NotFound,
            };
        }
    }
}
