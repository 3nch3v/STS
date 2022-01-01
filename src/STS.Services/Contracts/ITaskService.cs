using System.Collections.Generic;
using System.Threading.Tasks;

using STS.Data.Dtos.Task;
using STS.Data.Models;

namespace STS.Services.Contracts
{
    public interface ITaskService
    {
        int GetCount();

        EmployeeTask GetById(int id);

        Task CreateAsync<T>(string userId, T task);

        Task DeleteAsync(int id);

        IEnumerable<BaseTaskDto> GetAll(
            string userId, 
            bool isManager, 
            bool isTaskActive, 
            int page, 
            string keyword, 
            string employeeId);
    }
}
