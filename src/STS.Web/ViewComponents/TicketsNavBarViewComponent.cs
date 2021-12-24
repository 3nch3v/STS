using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using STS.Data.Models;
using STS.Services.Contracts;

namespace STS.Web.ViewComponents
{
    public class TicketsNavBarViewComponent : ViewComponent
    {
        private readonly ITicketService ticketService;
        private readonly UserManager<ApplicationUser> userManager;

        public TicketsNavBarViewComponent(
            ITicketService ticketService,
            UserManager<ApplicationUser> userManager)
        {
            this.ticketService = ticketService;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var user = Request.HttpContext.User;
            var userId = userManager.GetUserId(user);
            var navStatistic = ticketService.GetCategoriesTicketsCount(userId);

            return View(navStatistic);
        }
    }
}
