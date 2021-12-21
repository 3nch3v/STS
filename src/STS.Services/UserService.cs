using System;
using System.Linq;
using STS.Data;
using STS.Services.Contracts;

namespace STS.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int GetDepartmentId(string userId)
        {
            var user = dbContext.Users
                .Where(x => x.Id == userId)
                .FirstOrDefault();

            return user.DepartmentId;
        }
    }
}
