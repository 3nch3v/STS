using System.Collections.Generic;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Common;
using STS.Web.ViewModels.Tasks;
using STS.Web.ViewModels.User;

namespace STS.Web.ViewComponents
{
    public class CreateTaskViewComponent : ViewComponent
    {
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public CreateTaskViewComponent(
            ICommonService commonService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.commonService = commonService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public IViewComponentResult Invoke()
        {
            var user = Request.HttpContext.User;
            var userId = userManager.GetUserId(user);
            var departmentId = commonService.GetDepartmentId(userId);

            var inputModel = new TaskInputModel
            {
                Priorities = mapper.Map<List<PriorityViewModel>>(commonService.GetPriorities()),
                Employees = mapper.Map<IEnumerable<BaseUserViewModel>>(commonService.GetEmployeesBase(departmentId))
            };

            return View(inputModel);
        }
    }
}
