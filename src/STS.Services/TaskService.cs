using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using STS.Data;
using STS.Data.Dtos.Task;
using STS.Data.Models;
using STS.Services.Contracts;

using static STS.Common.GlobalConstants;

namespace STS.Services
{
    public class TaskService : ITaskService
    {
        private int tasksCount = 0;
        private readonly string openStatus = "open";

        private readonly IMapper mapper;
        private readonly ICommonService commonService;
        private readonly ApplicationDbContext dbContext;

        public TaskService(
            IMapper mapper,
            ICommonService commonService,
            ApplicationDbContext dbContext)
        {
            this.mapper = mapper;
            this.commonService = commonService;
            this.dbContext = dbContext;
        }

        public int GetCount() => tasksCount;

        public TaskDto GetById(int id)
        {
            return dbContext.EmployeesTasks
               .Where(x => x.Id == id)
               .Select(x => new TaskDto
               { 
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Deadline = x.Deadline,
                    StatusName = x.Status.Name,
                    PriorityName = x.Priority.Name,
                    ManagerUserName = x.Manager.UserName,
                    EmployeeId = x.EmployeeId,
                    Comments = x.Comments
                        .Select(c => new ReplayTaskDto
                        {
                            Id = c.Id,
                            Content = c.Content,
                            UserUserName = c.User.UserName,
                        })
                        .ToList(),
               })
               .FirstOrDefault();
        }

        public async Task CreateAsync<T>(string userId, T taskDto)
        {
            var task = mapper.Map<EmployeeTask>(taskDto);
            task.ManagerId = userId;
            task.StatusId = commonService.GetStatusId(openStatus);

            await dbContext.EmployeesTasks.AddAsync(task);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = dbContext.EmployeesTasks
               .Where(x => x.Id == id)
               .FirstOrDefault();

            task.IsDeleted = true;

            await dbContext.SaveChangesAsync();
        }

        public IEnumerable<TaskListingDto> GetAll(
            string userId,
            bool isManager,
            bool isTaskActive,
            int page,
            string keyword,
            string employeeId)
        {
            string searchTerm = keyword == null ? null : keyword.Trim().ToLower();
            var tasksQuery = dbContext.EmployeesTasks.AsQueryable();

            if (isManager)
            {
                tasksQuery = tasksQuery
                    .Where(task => task.ManagerId == userId)
                    .AsQueryable();

                if (employeeId != null)
                {
                    tasksQuery = tasksQuery
                        .Where(task => task.EmployeeId == employeeId)
                        .AsQueryable();
                }
            }
            else
            {
                tasksQuery = tasksQuery
                    .Where(task => task.EmployeeId == userId)
                    .AsQueryable();
            }

            if (isTaskActive)
            {
                tasksQuery = tasksQuery
                    .Where(task => task.Status.Name.ToLower() != "closed"
                                && task.Status.Name.ToLower() != "solved")
                    .AsQueryable();
            }

            if (keyword != null)
            {
                tasksQuery = tasksQuery
                    .Where(task => task.Title.ToLower().Contains(searchTerm)
                                || task.Description.ToLower().Contains(searchTerm)
                                || task.Employee.UserName.ToLower().Contains(searchTerm))
                    .AsQueryable();
            }

            tasksCount = tasksQuery.Count();

            var tasks = tasksQuery
               .Select(task => new TaskListingDto
               {
                   Id = task.Id,
                   Title = task.Title,
                   Deadline = task.Deadline,
                   EmployeeUserName = task.Employee.UserName,
                   ManagerUserName = task.Manager.UserName,
                   StatusName = task.Status.Name,
                   PriorityName = task.Priority.Name,
               })
               .OrderByDescending(x => x.Deadline)
               .Skip((page - 1) * TasksPerPage)
               .Take(TasksPerPage)
               .ToList();

            return tasks;
        }

        public IEnumerable<BaseTaskDto> GetSideBarTasks(string userId, bool isManager, int tasksCount)
        {
            var tasksQuery = dbContext.EmployeesTasks.AsQueryable();

            if (isManager)
            {
                tasksQuery = tasksQuery
                   .Where(task => task.ManagerId == userId)
                   .AsQueryable();
            }
            else 
            {
                tasksQuery = tasksQuery
                    .Where(task => task.EmployeeId == userId)
                    .AsQueryable();
            }

            var tasks = tasksQuery
                    .Where(task => task.Status.Name.ToLower() != "closed"
                                && task.Status.Name.ToLower() != "solved")
                     .Select(task => new BaseTaskDto
                     {
                         Id = task.Id,
                         Title = task.Title,
                         Deadline = task.Deadline,
                         PriorityName = task.Priority.Name,
                     })
                    .OrderBy(x => x.Deadline)
                    .Take(tasksCount)
                    .ToList();

            return tasks;
        }
    }
}