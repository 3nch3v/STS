using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using STS.Data;
using STS.Data.Models;
using STS.Services.Contracts;

using static STS.Common.GlobalConstants;

namespace STS.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public TaskService(
            IUserService userService,
            IMapper mapper,
            ApplicationDbContext dbContext)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task CreateAsync<T>(string userId, T taskDto)
        {
            var task = mapper.Map<EmployeeTask>(taskDto);
            task.ManagerId = userId;

            await dbContext.EmployeesTasks.AddAsync(task);

            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = dbContext.EmployeesTasks
               .Where(x => x.Id == id)
               .FirstOrDefault();

            dbContext.EmployeesTasks.Remove(task);

            await dbContext.SaveChangesAsync();
        }

        public IEnumerable<EmployeeTask> GetAll(string userId)   //DTO
        {
            var tasks = dbContext.EmployeesTasks
               .Where(x => x.EmployeeId == userId)
               .ToList();

            return tasks;
        }

        public EmployeeTask GetById(int id)
        {
            return dbContext.EmployeesTasks
               .Where(x => x.Id == id)
               .FirstOrDefault();
        }

        public IEnumerable<EmployeeTask> GetOpenTasks(string userId)   //DTO
        {
            return dbContext.EmployeesTasks
               .Where(x => x.EmployeeId == userId)
               .OrderBy(x => x.Deadline)
               .ThenByDescending(x => x.PriorityId)
               .Take(TasksSideBarCount)
               .ToList();
        }
    }
}
