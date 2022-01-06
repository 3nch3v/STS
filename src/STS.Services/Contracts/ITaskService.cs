using System.Collections.Generic;
using System.Threading.Tasks;

using STS.Data.Dtos.Task;

namespace STS.Services.Contracts
{
    public interface ITaskService
    {
        int GetCount();

        TaskDto GetById(int id);

        Task<TaskDto> EditAsync<T>(T task);

        Task CreateAsync<T>(string userId, T task);

        Task DeleteAsync(int id);

        IEnumerable<BaseTaskDto> GetSideBarTasks(string userId, bool isManager, int tasksCount);

        IEnumerable<TaskListingDto> GetAll(
            string userId, 
            bool isManager, 
            bool isTaskActive, 
            int page, 
            string keyword, 
            string employeeId);
    }
}
