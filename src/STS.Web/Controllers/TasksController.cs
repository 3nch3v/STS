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

        public IActionResult Index(bool isActive, string keyword, int page = DefaultPageNumber)
        {
            var userId = userManager.GetUserId(User);
            var tasks = taskService.GetAll(userId, false, isActive, page, keyword, null);

            var tasksDto = new TasksViewModel
            {
                Keyword = keyword,
                OnlyActive = isActive,
                Page = page,
                TasksCount = taskService.GetCount(),
                Tasks = mapper.Map<List<TaskLinstingViewModel>>(tasks),
            };

            return View(tasksDto);
        }

        public IActionResult Task(int id)
        {
            var currUser = userManager.GetUserId(User);
            var taskDto = taskService.GetById(id);

            if (taskDto.EmployeeId != currUser)
            {
                return BadRequest();
            }

            var task = mapper.Map<TaskViewModel>(taskDto);

            return View(task);
        }
    }
}
