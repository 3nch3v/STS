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
        private const int statusErrorCode500 = 500;
        private const string invalidLogin = "Invalid login attempt.";
        private const string loginErrKey = "LoginAttempt";
        private const string passErrKey = "RepeatPass";
        private const string logInfoMsg = "User logged in.";
        private const string passErrMsg = "Password doesn't match.";

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
                    this.ModelState.AddModelError(loginErrKey, invalidLogin);
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

                this.ModelState.AddModelError(loginErrKey, invalidLogin);
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
                ModelState.AddModelError(passErrKey, passErrMsg); 
            }

            if (!ModelState.IsValid) 
            {
                return View(passInput);
            }

            var user = await userManager.GetUserAsync(User);
            var passChangeResult = await userManager.ChangePasswordAsync(user, passInput.OldPass, passInput.NewPass);

            if (!passChangeResult.Succeeded)
            {
                await signInManager.SignOutAsync();
                return RedirectToAction(nameof(Error), new { statusCode = statusErrorCode500 });
            }

            return RedirectToAction("Tickets", "Tickets");
        }

        public IActionResult Error(int statusCode)
        {
            string msg = this.GetMessage(statusCode);
            var error = new ErrorViewModel(msg);

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
