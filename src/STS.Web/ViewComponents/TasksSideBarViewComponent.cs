using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tasks;

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

        public IViewComponentResult Invoke(bool isManager)
        {
            var user = Request.HttpContext.User;
            var userId = userManager.GetUserId(user);

            var tasksDtos = taskService.GetSideBarTasks(userId, isManager);

            var tasks = new TasksSideBarViewModel
            {
                Tasks = mapper.Map<IEnumerable<BaseTaskViewModel>>(tasksDtos),
            };

            return View(tasks);
        }
    }
}
