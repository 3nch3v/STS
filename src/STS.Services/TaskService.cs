using System;
using System.Linq;
using System.Collections.Generic;
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
        private readonly StsDbContext dbContext;

        public TaskService(
            IMapper mapper,
            ICommonService commonService,
            StsDbContext dbContext)
        {
            this.mapper = mapper;
            this.commonService = commonService;
            this.dbContext = dbContext;
        }

        public int GetCount() => tasksCount;

        public TaskDto GetById(int id)
        {
            var task = dbContext.EmployeesTasks
               .Where(task => task.Id == id)
               .Select(task => new TaskDto
               { 
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Deadline = task.Deadline,
                    StatusName = task.Status.Name,
                    PriorityName = task.Priority.Name,
                    ManagerId = task.ManagerId,
                    ManagerUserName = task.Manager.UserName,
                    EmployeeId = task.EmployeeId,
                    EmployeeUserName = task.Employee.UserName,
                    Comments = task.Comments
                        .Select(reply => new ReplyTaskDto
                        {
                            Id = reply.Id,
                            Content = reply.Content,
                            UserUserName = reply.User.UserName,
                        })
                        .ToList(),
               })
               .FirstOrDefault();

            return task;
        }

        public async Task CreateAsync<T>(string userId, T taskDto)
        {
            var task = mapper.Map<EmployeeTask>(taskDto);
            task.ManagerId = userId;
            task.StatusId = commonService.GetStatusId(openStatus);

            await dbContext.EmployeesTasks.AddAsync(task);
            await dbContext.SaveChangesAsync();
        }

        public async Task<TaskDto> EditAsync<T>(T taskInput)
        {
            var taskDto = mapper.Map<TaskEditDto>(taskInput);
            var task = dbContext.EmployeesTasks.FirstOrDefault(x => x.Id == taskDto.Id);

            if (taskDto.Title != null 
                && task.Title != taskDto.Title)
            {
                task.Title = taskDto.Title;
            }

            if (taskDto.Description != null
                && task.Description != taskDto.Description)
            {
                task.Description = taskDto.Description;
            }

            if (taskDto.PriorityId != null
                && task.PriorityId != taskDto.PriorityId)
            {
                task.PriorityId = (int)taskDto.PriorityId;
            }

            if (taskDto.StatusId != null
              && task.StatusId != taskDto.StatusId)
            {
                task.StatusId = (int)taskDto.StatusId;
            }

            if (taskDto.EmployeeId != null
              && task.EmployeeId != taskDto.EmployeeId)
            {
                task.EmployeeId = taskDto.EmployeeId;
            }

            if (taskDto.Deadline != null
                && (DateTime.Compare((DateTime)taskDto.Deadline, task.Deadline) != 0))
            {
                task.Deadline = (DateTime)taskDto.Deadline;
            }

            await dbContext.SaveChangesAsync();

            var result = GetById(task.Id);
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            var task = dbContext.EmployeesTasks
               .Where(x => x.Id == id)
               .FirstOrDefault();

            task.IsDeleted = true;

            var repliesTasks = dbContext.RepliesTasks
                .Where(x => x.EmployeeTaskId == id)
                .ToList();

            repliesTasks.ForEach(x => x.IsDeleted = true);

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
                    .Where(task => task.EmployeeId == userId
                            && (task.Status.Name.ToLower() != "closed"
                                && task.Status.Name.ToLower() != "solved"))
                    .AsQueryable();
            }

            if (isTaskActive)
            {
                if (isManager)
                {
                    tasksQuery = tasksQuery
                        .Where(task => task.Status.Name.ToLower() != "closed")
                        .AsQueryable();
                }
                else 
                {
                    tasksQuery = tasksQuery
                        .Where(task => task.Status.Name.ToLower() != "closed"
                                    && task.Status.Name.ToLower() != "solved")
                        .AsQueryable();
                }       
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