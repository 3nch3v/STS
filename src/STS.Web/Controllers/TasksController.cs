using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tasks;
using STS.Web.ViewModels.Common;
using STS.Web.ViewModels.User;

using static STS.Common.GlobalConstants;

namespace STS.Web.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly IMapper mapper;
        private readonly ITaskService taskService;
        private readonly ICommonService commonService;
        private readonly UserManager<ApplicationUser> userManager;

        public TasksController(
            IMapper mapper,
            ITaskService taskService,
            ICommonService commonService,
            UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            this.taskService = taskService;
            this.commonService = commonService;
            this.userManager = userManager;
        }

        public IActionResult Task(int id)
        {
            var currUser = userManager.GetUserId(User);
            var taskDto = taskService.GetById(id);

            if (taskDto == null ||
                (taskDto.EmployeeId != currUser 
                 && !User.IsInRole(ManagerRoleName) 
                 && !User.IsInRole(AdministratorRoleName)))
            {
                return BadRequest();
            }

            var task = mapper.Map<TaskViewModel>(taskDto);
            task.Statuses = mapper.Map<List<StatusViewModel>>(commonService.GetStatuses());

            if (User.IsInRole(ManagerRoleName) 
               || User.IsInRole(AdministratorRoleName)) 
            {
                var departmentId = commonService.GetDepartmentId(currUser);
                task.Employees = mapper.Map<List<BaseUserViewModel>>(commonService.GetEmployeesBase(departmentId));
            }

            return View(task);
        }

        public IActionResult Index(string keyword, bool isActive, int page = DefaultPageNumber)
        {
            var tasks = PrepareViewModel(false, keyword, isActive, null, page);

            return View(tasks);
        }

        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Management(
            string keyword, 
            bool isActive, 
            string employeeId, 
            int page = DefaultPageNumber)
        {
            var tasks = PrepareViewModel(true, keyword, isActive, employeeId, page);

            return View(tasks);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Create(TaskInputModel taskInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = userManager.GetUserId(User);
            await taskService.CreateAsync(userId, taskInput);

            return RedirectToAction(nameof(Management));
        }

        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = taskService.GetById(id);
            var userId = userManager.GetUserId(User);

            if (task == null 
                || task.ManagerId != userId)
            {
                return BadRequest();
            }

            await taskService.DeleteAsync(id);

            return RedirectToAction(nameof(Management));
        }

        private TasksViewModel PrepareViewModel(
            bool isManager,
            string keyword,
            bool isActive,
            string employeeId,
            int page)
        {
            var currUserId = userManager.GetUserId(User);
            var tasksDtos = taskService.GetAll(currUserId, isManager, isActive, page, keyword, employeeId);

            var tasksDto = new TasksViewModel
            {
                Keyword = keyword,
                OnlyActive = isActive,
                Page = page,
                TasksCount = taskService.GetCount(),
                Tasks = mapper.Map<List<TaskLinstingViewModel>>(tasksDtos),
            };

            if (isManager)
            {
                tasksDto.EmployeeId = employeeId;
                tasksDto.ManagerId = currUserId;
            }

            return tasksDto;
        }
    }
}
