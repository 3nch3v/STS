using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using STS.Data;
using STS.Data.Models;
using STS.Data.Dtos.Department;
using STS.Data.Dtos.User;
using STS.Data.Dtos.Role;
using STS.Data.Dtos.Ticket;
using STS.Services.Contracts;

namespace STS.Services
{
    public class CommonService : ICommonService
    {
        private const string statusClosed = "Closed";
        private const string statusSolved = "Solved";

        private readonly StsDbContext dbContext;

        public CommonService(StsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int GetStatusId(string status)
        {
            var requestedStatus = dbContext.Statuses
                .Where(x => x.Name.ToLower() == status.ToLower())
                .FirstOrDefault();

            return requestedStatus.Id;
        }

        public Task<bool> IsDepartmentExisting(int id)
        {
            if (dbContext.Departments.Any(x => x.Id == id))
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public int GetDepartmentId(string userId)
        {
            var user = dbContext.Users
                .Where(x => x.Id == userId)
                .FirstOrDefault();

            return user.DepartmentId;
        }

        public Dictionary<string, Dictionary<string, int>> GetTicketsStatistic()
        {
            var ticketStatistic = dbContext.Tickets
              .GroupBy(x => new
              {
                  StatusName = x.Status.Name,
                  DepartmentName = x.Department.Name
              })
              .Select(g => new TicketStatisticDto
              {
                  Status = g.Key.StatusName,
                  Department = g.Key.DepartmentName,
                  Count = g.Count(),
              })
              .ToList();

            Dictionary<string, Dictionary<string, int>> byStatus = new Dictionary<string, Dictionary<string, int>>();

            foreach (var currStat in ticketStatistic)
            {
                if (!byStatus.ContainsKey(currStat.Status))
                {
                    byStatus.Add(currStat.Status, new Dictionary<string, int>());
                }

                if (!byStatus[currStat.Status].ContainsKey(currStat.Department))
                {
                    byStatus[currStat.Status].Add(currStat.Department, 0);
                }

                byStatus[currStat.Status][currStat.Department] += currStat.Count;
            }

            return byStatus;
        }

        public IEnumerable<DepartmentBaseDto> GetDepartmentsBase()
        {
            return dbContext.Departments
                .Select(department => new DepartmentBaseDto
                {
                    Id = department.Id,
                    Name = department.Name,
                })
                .ToList();
        }

        public IEnumerable<DepartmentStatisticDto> GetDepartmentsWithStatistic()
        {
            var departments = dbContext.Departments
                .Select(department => new DepartmentStatisticDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    EmployeesCount = department.Employees.Count(),
                    ActiveTicketsCount = department.Tickets
                        .Where(ticket => ticket.Status.Name != statusClosed
                                 && ticket.Status.Name != statusSolved)
                        .Count(),
                    ActiveTasksCount = department.Employees
                        .SelectMany(employee => employee.Tasks
                            .Where(task => task.Status.Name != statusClosed
                                     && task.Status.Name != statusSolved))
                        .Count(),
                })
                .ToList();

            return departments;
        }

        public IEnumerable<BaseUserDto> GetEmployeesBase(int departmentId)
        {
            return dbContext.Users
                .Where(x => x.DepartmentId == departmentId)
                .Select(x => new BaseUserDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                })
                .ToList();
        }


        public IEnumerable<Priority> GetPriorities()
        {
            return dbContext.Priorities.ToList();
        }

        public IEnumerable<Status> GetStatuses()
        {
            return dbContext.Statuses.ToList();
        }

        public IEnumerable<RoleDto> GetRoles()
        {
            return dbContext.Roles
                .Select(role => new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                })
                .ToList();
        }
    }
}
