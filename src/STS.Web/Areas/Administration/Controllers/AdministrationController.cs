using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using STS.Services.Contracts;
using STS.Web.ViewModels.Admin;
using STS.Web.ViewModels.Common;
using STS.Web.ViewModels.User;

using static STS.Common.GlobalConstants;
using Microsoft.AspNetCore.Identity;
using STS.Data.Models;

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

        [Route("/[controller]")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/[controller]/CreateUser")]
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

        [Route("/[controller]/CreateUser")]
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

        [Route("/[controller]/EditUser/{id}")]
        public async Task<IActionResult> EditUser(string id)
        {
            var (departments, roles) = GetDepartmentsAndRoles();
            var userData = mapper.Map<UserEditModel>(
                adminService.GetUserById(id));
            userData.Departments = departments;
            userData.SystemRoles = roles;
            userData.Role = string.Join("", await adminService.GetUserRoles(id));

            return View(userData);
        }

        [Route("/[controller]/EditUser")]
        [HttpPost]
        public async Task<IActionResult> EditUser(UserEditModel userInput)
        {
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
            if (departmentName.Length < DepartmentNameMinLength || departmentName.Length > DepartmentNameMaxLength)
            {
                return BadRequest();
            }

            await adminService.CreateDepartmentAsync(departmentName);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (roleName.Length < RoleNameMinLength || roleName.Length > RoleNameMaxLength)
            {
                return BadRequest();
            }

            await adminService.CreateRoleAsync(roleName);

            return RedirectToAction(nameof(Index));
        }

        [Route("/[controller]/Users")]
        public async Task<IActionResult> Users()
        {
            //all users
            //user per department
            // search for user with email, username or id

            var users = await adminService.GetUsers();

            var usersDto = new UsersViewModel
            {
                Users = mapper.Map<IEnumerable<UserViewModel>>(users),
            };

            return View(usersDto);
        }

        private (IEnumerable<DepartmentViewModel>, IEnumerable<RoleViewModel>) GetDepartmentsAndRoles()
        {
            var departmentsDtos = commonService.GetDepartmentsBase();
            var departments = mapper.Map<IEnumerable<DepartmentViewModel>>(departmentsDtos);
            var rolesDtos = commonService.GetRoles();
            var roles = mapper.Map<IEnumerable<RoleViewModel>>(rolesDtos);

            return (departments, roles);
        }
    }
}
