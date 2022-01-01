using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tasks;

using static STS.Common.GlobalConstants;

namespace STS.Web.ViewComponents
{
    public class TasksSideBarViewComponent : ViewComponent
    {
        private readonly ITaskService taskService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public TasksSideBarViewComponent(
            ITaskService taskService,
            IMapper mapper, 
            UserManager<ApplicationUser> userManager)
        {
            this.taskService = taskService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var user = Request.HttpContext.User;
            var userId = userManager.GetUserId(user);

            var tasks = new TasksSideBarViewModel
            {
                Tasks = mapper.Map<IEnumerable<BaseTaskViewModel>>(taskService.GetAll(userId, false, true, DefaultPageNumber, null, null)),
            };

            return View(tasks);
        }
    }
}
