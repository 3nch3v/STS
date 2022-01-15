using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Common;
using STS.Web.ViewModels.User;
using STS.Web.Infrastructure.ControllersHelpers.Contracts;

using static STS.Common.GlobalConstants;

namespace STS.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : Controller
    {
        private readonly DateTime lockoutEnd = DateTime.Now.AddYears(100);

        private readonly IAdminService adminService;
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        private readonly IAdminControllerHelper adminHelper;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(
            IMapper mapper,
            IAdminService adminService,
            ICommonService commonService,
            IAdminControllerHelper adminControllerHelper,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            this.adminService = adminService;
            this.commonService = commonService;
            this.mapper = mapper;
            this.adminHelper = adminControllerHelper;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Users(int? departmentId, string keyword, int page = DefaultPageNumber)
        {
            var users = adminService.GetUsersAsync(page, keyword, departmentId);
            foreach (var user in users)
            {
                var currUser = await userManager.FindByIdAsync(user.Id);
                user.Roles = await userManager.GetRolesAsync(currUser);
                user.IsLockedOut = await userManager.IsLockedOutAsync(currUser);
            }

            var usersDtos = new UsersViewModel
            {
                Page = page,
                Keyword = keyword,
                DepartmentId = departmentId,
                UsersCount = adminService.GetUsersCount(),
                Users = mapper.Map<IEnumerable<UserViewModel>>(users),
                Departments = mapper.Map<List<DepartmentViewModel>>(commonService.GetDepartmentsBase()),
            };

            return View(usersDtos);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            var (departments, roles) = adminHelper.GetDepartmentsAndRoles();
            var user = new UserInputModel
            {
                Departments = departments,
                SystemRoles = roles,
            };

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserInputModel user)
        {
            if (!ModelState.IsValid)
            {
                var (departments, roles) = adminHelper.GetDepartmentsAndRoles();
                user.Departments = departments;
                user.SystemRoles = roles;

                return View(user);
            }

            await adminHelper.SignInUser(user);

            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var (departments, roles) = adminHelper.GetDepartmentsAndRoles();
            var user = adminService.GetUserById(id);
            var userRoles = await userManager.GetRolesAsync(user);

            var userDto = mapper.Map<UserEditModel>(user);
            userDto.Departments = departments;
            userDto.SystemRoles = roles;
            userDto.Role = string.Join("", userRoles);

            return View(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditModel userInput)
        {
            var erros = await adminHelper.ValidateEmailAndUserName(userInput);

            if (erros.Any())
            {
                foreach (var (key, errorMessage) in erros)
                {
                    ModelState.AddModelError(key, errorMessage);
                }
            }

            if (!ModelState.IsValid)
            {
                var (departments, roles) = adminHelper.GetDepartmentsAndRoles();
                userInput.Departments = departments;
                userInput.SystemRoles = roles;

                return View(userInput);
            }

            var user = await adminService.EditUserAsync(userInput);

            if (userInput.Role != null)
            {
                await adminHelper.EditUserRole(userInput.Role, user);
            }

            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> LockoutUser(string id)
        {
            var user = adminService.GetUserById(id);

            if (user == null)
            {
                return BadRequest();
            }

            await userManager.SetLockoutEnabledAsync(user, true);
            await userManager.SetLockoutEndDateAsync(user, lockoutEnd);
            await userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> UnlockUser(string id)
        {
            var user = adminService.GetUserById(id);

            if (user == null)
            {
                return BadRequest();
            }

            await userManager.SetLockoutEndDateAsync(user, DateTime.Now);
            await userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = adminService.GetUserById(id);

            if (user == null)
            {
                return BadRequest();
            }

            await adminService.DeleteUserAsync(id);

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([StringLength(DepartmentNameMaxLength, MinimumLength = DepartmentNameMinLength)]  string departmentName)
        {
            if (ModelState.IsValid)
            {
                await adminService.CreateDepartmentAsync(departmentName);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([StringLength(RoleNameMaxLength, MinimumLength = RoleNameMinLength)] string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (!ModelState.IsValid || role != null)
            {
                return RedirectToAction(nameof(Index));
            }

            await roleManager.CreateAsync(new ApplicationRole(roleName));

            return RedirectToAction(nameof(Index));
        }
    }
}
