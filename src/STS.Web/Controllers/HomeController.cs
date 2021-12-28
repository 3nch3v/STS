using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using STS.Web.ViewModels;

using static STS.Common.GlobalConstants;

namespace STS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
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
