using System.Collections.Generic;
using System.Threading.Tasks;

using STS.Data.Dtos.User;
using STS.Data.Models;

namespace STS.Services.Contracts
{
    public interface IAdminService
    {
        int GetUsersCount();

        ApplicationUser GetUserById(string id);

        IEnumerable<UserDto> GetUsersAsync(int page, string searchTerm, int? departmentId);

        Task CreateDepartmentAsync(string departmentName);

        Task DeleteUserAsync(string userId);

        Task<ApplicationUser> EditUserAsync<T>(T userInput);
    }
}
