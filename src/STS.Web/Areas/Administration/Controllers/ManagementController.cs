using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tasks;
using STS.Web.ViewModels.User;

using static STS.Common.GlobalConstants;

namespace STS.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator, Manager")]
    [Area("Administration")]
    public class ManagementController : Controller
    {
        private readonly ICommonService commonService;
        private readonly ITaskService taskService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ManagementController(
            ICommonService commonService,
            ITaskService taskService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.commonService = commonService;
            this.taskService = taskService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public IActionResult Index(string keyword, string employeeId, bool isActive, int page = DefaultPageNumber)
        {
            var userId = userManager.GetUserId(User);
            var departmentId = commonService.GetDepartmentId(userId);
            var tasksDtos = taskService.GetAll(userId, true, isActive, page, keyword, employeeId);

            var tasks = new TasksViewModel
            {
                Keyword = keyword,
                EmployeeId = employeeId,
                OnlyActive= isActive,
                Page = page,
                TasksCount = taskService.GetCount(),
                Tasks = mapper.Map<IEnumerable<TaskLinstingViewModel>>(tasksDtos),
                Employees = mapper.Map<IEnumerable<BaseUserViewModel>>(
                    commonService.GetEmployeesBase(departmentId)),
            };

            return View(tasks);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = taskService.GetById(id);
            var userId = userManager.GetUserId(User);

            if (task == null || task.ManagerId != userId)
            {
                return BadRequest();
            }

            await taskService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskInputModel taskInput)
        {
            var userId = userManager.GetUserId(User);

            if (!ModelState.IsValid)
            {
                return StatusCode(
                  StatusCodes.Status422UnprocessableEntity,
                  "Invalid input data.");
            }

            await taskService.CreateAsync(userId, taskInput);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Task(int id)
        {
            return View();
        }
    }
}
