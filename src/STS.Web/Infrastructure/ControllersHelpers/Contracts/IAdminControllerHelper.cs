using System.Collections.Generic;
using System.Threading.Tasks;

using STS.Data.Models;
using STS.Web.ViewModels.Admin;
using STS.Web.ViewModels.Common;
using STS.Web.ViewModels.User;

namespace STS.Web.Infrastructure.ControllersHelpers.Contracts
{
    public interface IAdminControllerHelper
    {
        Task SignInUser(UserInputModel userInput);

        Task EditUserRole(string roleName, ApplicationUser user);

        Task<Dictionary<string, string>> ValidateEmailAndUserName(UserEditModel userInput);

        (IEnumerable<DepartmentViewModel>, IEnumerable<RoleViewModel>) GetDepartmentsAndRoles();
    }
}
