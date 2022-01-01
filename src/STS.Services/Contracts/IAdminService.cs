using System.Collections.Generic;
using System.Threading.Tasks;

using STS.Data.Dtos.User;
using STS.Data.Models;

namespace STS.Services.Contracts
{
    public interface IAdminService
    {
        ApplicationUser GetUserById(string id);

        Task DeleteUserAsync(string userId);

        Task RegisterUserAsync<T>(T userInput);

        Task EditUserAsync<T>(T userInput);

        Task CreateDepartmentAsync(string departmentName);

        Task CreateRoleAsync(string roleName);

        Task<IEnumerable<string>> GetUserRolesAsync(string id);

        Task<IEnumerable<UserDto>> GetUsersAsync(int page, int usersPerPage, string searchTerm);
    }
}
