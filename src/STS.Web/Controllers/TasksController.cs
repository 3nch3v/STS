using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tasks;

using static STS.Common.GlobalConstants;

namespace STS.Web.Controllers
{
    public class TasksController : Controller
    {
        private readonly IMapper mapper;
        private readonly ITaskService taskService;
        private readonly UserManager<ApplicationUser> userManager;

        public TasksController(
            IMapper mapper, 
            ITaskService taskService, 
            UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            this.taskService = taskService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("/[controller]")]
        public IActionResult Index()
        {
            var userId = userManager.GetUserId(User);

            var tasks = taskService.GetAll(userId, false, true, DefaultPageNumber, null, null);

            var tasksDto = new TasksViewModel
            {
                Tasks = mapper.Map<List<TaskLinstingViewModel>>(tasks),
            };

            return View(tasksDto);
        }
    }
}
