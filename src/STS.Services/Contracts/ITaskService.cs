using STS.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STS.Services.Contracts
{
    public interface ITaskService
    {
        EmployeeTask GetById(int id);

        IEnumerable<EmployeeTask> GetAll(string userId);

        Task CreateAsync<T>(string userId, T task);

        Task DeleteAsync(int id);
    }
}
