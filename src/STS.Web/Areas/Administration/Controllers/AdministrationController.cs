using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Admin;
using STS.Web.ViewModels.Common;
using STS.Web.ViewModels.User;

using static STS.Common.GlobalConstants;

namespace STS.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : Controller
    {
        private readonly IAdminService adminService;
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(
            IAdminService adminService,
            ICommonService commonService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.adminService = adminService;
            this.commonService = commonService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            var (departments, roles) = GetDepartmentsAndRoles();

            var userInput = new UserInputModel
            {
                Departments = departments,
                SystemRoles = roles,
            };

            return View(userInput);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserInputModel userInput)
        {
            if (!ModelState.IsValid)
            {
                var (departments, roles) = GetDepartmentsAndRoles();
                userInput.Departments = departments;
                userInput.SystemRoles = roles;

                return View(userInput);
            }

            await adminService.RegisterUserAsync(userInput);

            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var (departments, roles) = GetDepartmentsAndRoles();
            var userData = mapper.Map<UserEditModel>(adminService.GetUserById(id));
            userData.Departments = departments;
            userData.SystemRoles = roles;
            userData.Role = string.Join("", await adminService.GetUserRolesAsync(id));

            return View(userData);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditModel userInput)
        {
            await ValidateEmailAndUserName(userInput);

            if (!ModelState.IsValid)
            {
                var (departments, roles) = GetDepartmentsAndRoles();
                userInput.Departments = departments;
                userInput.SystemRoles = roles;

                return View(userInput);
            }

            await adminService.EditUserAsync(userInput);

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment(string departmentName)
        {
            if (departmentName.Length < DepartmentNameMinLength 
                || departmentName.Length > DepartmentNameMaxLength)
            {
                return BadRequest();
            }

            await adminService.CreateDepartmentAsync(departmentName);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (roleName.Length < RoleNameMinLength 
                || roleName.Length > RoleNameMaxLength)
            {
                return BadRequest();
            }

            await adminService.CreateRoleAsync(roleName);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> LockoutUser(string id)
        {
            var user = adminService.GetUserById(id);

            if (user == null)
            {
                return BadRequest();
            }

            await adminService.LockoutUserAsync(id);

            return RedirectToAction(nameof(Users));
        }

        public async Task<IActionResult> UnlockUser(string id)
        {
            var user = adminService.GetUserById(id);

            if (user == null)
            {
                return BadRequest();
            }

            await adminService.UnlockUserAsync(id);

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

        public async Task<IActionResult> Users(int? departmentId, string keyword, int page = DefaultPageNumber)
        {
            var users = await adminService.GetUsersAsync(page, keyword, departmentId);

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

        private (IEnumerable<DepartmentViewModel>, IEnumerable<RoleViewModel>) GetDepartmentsAndRoles()
        {
            var departmentsDtos = commonService.GetDepartmentsBase();
            var departments = mapper.Map<IEnumerable<DepartmentViewModel>>(departmentsDtos);
            var rolesDtos = commonService.GetRoles();
            var roles = mapper.Map<IEnumerable<RoleViewModel>>(rolesDtos);

            return (departments, roles);
        }

        private async Task ValidateEmailAndUserName(UserEditModel userInput)
        {
            var existingEmail = await userManager.FindByEmailAsync(userInput.Email);
            var existingUserName = await userManager.FindByNameAsync(userInput.UserName);

            if (existingEmail != null
                && existingEmail.Id != userInput.Id)
            {
                ModelState.AddModelError("Email", EmailErrorMsg); ;
            }

            if (existingUserName != null
                && existingUserName.Id != userInput.Id)
            {
                ModelState.AddModelError("UserName", UsernameErrorMsg);
            }
        }
    }
}
