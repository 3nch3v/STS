using System.Collections.Generic;
using System.Threading.Tasks;

using STS.Data.Dtos.User;
using STS.Data.Models;

namespace STS.Services.Contracts
{
    public interface IAdminService
    {
        int GetDepartmentId(string userId);

        ApplicationUser GetUserById(string id);

        Task<IEnumerable<UserDto>> GetUsers();

        Task RegisterUserAsync<T>(T userInput);

        Task EditUserAsync<T>(T userInput);

        Task CreateDepartmentAsync(string departmentName);

        Task CreateRoleAsync(string roleName);

        Task<IEnumerable<string>> GetUserRoles(string id);
    }
}
