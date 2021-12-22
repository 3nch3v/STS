using System.Collections.Generic;
using System.Linq;

using STS.Data;
using STS.Data.Models;
using STS.Services.Contracts;

namespace STS.Services
{
    public class CommonService : ICommonService
    {
        private readonly ApplicationDbContext dbContext;

        public CommonService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<Department> GetDepartments()
        {
            return dbContext.Departments.ToList();
        }

        public IEnumerable<Priority> GetPriorities()
        {
            return dbContext.Priorities.ToList();
        }

        public IEnumerable<Status> GetStatuses()
        {
            return dbContext.Statuses.ToList();
        }

        public int GetStatusId(string status)
        {
            var requestedStatus =  dbContext.Statuses
                .Where(x => x.Name.ToLower() == status.ToLower())
                .FirstOrDefault();

            return requestedStatus.Id;
        }
    }
}
