using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tasks;

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
        public IActionResult Tasks()
        {
            var userId = userManager.GetUserId(User);

            var tasks = taskService.GetAll(userId);

            var tasksDto = new TasksViewModel
            {
                Tasks = mapper.Map<List<TaskViewModel>>(tasks),
            };

            return View(tasksDto);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskInputModel task)
        {
            if (!ModelState.IsValid)
            {
                return View(task);
            }

            var userId = userManager.GetUserId(User);

            await taskService.CreateAsync(userId, task);

            return RedirectToAction(nameof(Tasks));   // Redirect to manager Panel! or just SPA princip
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var task = taskService.GetById(id);
            var userId = userManager.GetUserId(User);

            if (task.EmployeeId != userId || task == null)
            {
                return BadRequest();
            }

            await taskService.DeleteAsync(id);

            return RedirectToAction(nameof(Tasks)); // Redirect to manager Panel! or just SPA princip
        }
    }
}
