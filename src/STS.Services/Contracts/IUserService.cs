using System.Collections.Generic;
using System.Threading.Tasks;

using STS.Data.Dtos.User;

namespace STS.Services.Contracts
{
    public interface IUserService
    {
        int GetDepartmentId(string userId);

        Task<IEnumerable<UserDto>> GetUsers();

        Task RegisterUser<T>(T userInput);
    }
}
