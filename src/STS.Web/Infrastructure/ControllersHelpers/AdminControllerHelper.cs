using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Admin;
using STS.Web.ViewModels.Common;
using STS.Web.ViewModels.User;
using STS.Web.Infrastructure.ControllersHelpers.Contracts;

using static STS.Common.GlobalConstants;

namespace STS.Web.Infrastructure.ControllersHelpers
{
    public class AdminControllerHelper : IAdminControllerHelper
    {
        private readonly ICommonService commonService;
        private readonly IMapper mapper;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminControllerHelper(
          ICommonService commonService,
          IMapper mapper,
          RoleManager<ApplicationRole> roleManager,
          UserManager<ApplicationUser> userManager)
        {
            this.commonService = commonService;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public (IEnumerable<DepartmentViewModel>, IEnumerable<RoleViewModel>) GetDepartmentsAndRoles()
        {
            var departmentsDtos = commonService.GetDepartmentsBase();
            var departments = mapper.Map<IEnumerable<DepartmentViewModel>>(departmentsDtos);
            var rolesDtos = commonService.GetRoles();
            var roles = mapper.Map<IEnumerable<RoleViewModel>>(rolesDtos);

            return (departments, roles);
        }

        public async Task<Dictionary<string, string>> ValidateEmailAndUserName(UserEditModel userInput)
        {
            var existingEmail = await userManager.FindByEmailAsync(userInput.Email);
            var existingUserName = await userManager.FindByNameAsync(userInput.UserName);

            var errors = new Dictionary<string, string>();  

            if (existingEmail != null
                && existingEmail.Id != userInput.Id)
            {
                errors.Add("Email", EmailErrorMsg);
            }

            if (existingUserName != null
                && existingUserName.Id != userInput.Id)
            {
                errors.Add("UserName", UsernameErrorMsg);
            }

            return errors;
        }

        public async Task SignInUser(UserInputModel userInput)
        {
            var user = new ApplicationUser
            {
                FirstName = userInput.FirstName,
                LastName = userInput.LastName,
                Position = userInput.Position,
                UserName = userInput.UserName,
                Email = userInput.Email,
                DepartmentId = userInput.DepartmentId,
                PhoneNumber = userInput.PhoneNumber,
            };

            var userResult = await userManager.CreateAsync(user, userInput.Password);

            if (userResult.Succeeded)
            {
                var role = await roleManager.FindByNameAsync(userInput.Role);

                if (role != null)
                {
                    await userManager.AddToRoleAsync(user, userInput.Role);
                }
            }
        }

        public async Task EditUserRole(string roleName, ApplicationUser user)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (!user.Roles.Any(r => r.RoleId == role.Id))
            {
                foreach (var (roleId, userId) in user.Roles.Select(x => (x.RoleId, x.UserId)))
                {
                    var currRole = await roleManager.FindByIdAsync(roleId);
                    await userManager.RemoveFromRoleAsync(user, currRole.Name);
                }

                await userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}
