using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using AutoMapper;

using STS.Data;
using STS.Data.Models;
using STS.Services.Contracts;
using STS.Data.Dtos.User;

using static STS.Common.GlobalConstants;

namespace STS.Services
{
    public class AdminService : IAdminService
    {
        private int usersCount = 0;

        private readonly IMapper mapper;
        private readonly StsDbContext dbContext;

        public AdminService(
            IMapper mapper,
            StsDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public int GetUsersCount() => usersCount;

        public ApplicationUser GetUserById(string id)
        {
            return dbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<UserDto> GetUsersAsync(int page, string searchTerm, int? departmentId)
        {
            var usersQuery = dbContext.Users
                .AsQueryable();

            if (departmentId != null)
            {
                usersQuery = usersQuery
                    .Where(user => user.DepartmentId == departmentId)
                    .AsQueryable(); ;
            }

            if (searchTerm != null)
            {
                usersQuery = usersQuery.Where(user => user.Email.ToLower() == searchTerm.ToLower()
                                           || user.UserName == searchTerm.ToLower()
                                           || user.Department.Name.ToLower() == searchTerm.ToLower()
                                           || user.FirstName.ToLower() == searchTerm.ToLower()
                                           || user.LastName.ToLower() == searchTerm.ToLower()
                                           || user.FirstName.ToLower() + ' ' + user.LastName.ToLower() == searchTerm.ToLower()
                                           || user.Position.ToLower() == searchTerm.ToLower());
            }

            usersCount = usersQuery.Count();

            var users = usersQuery
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    UserName = u.UserName,
                    Position = u.Position,
                    PhoneNumber = u.PhoneNumber,
                    DepartmentId = u.DepartmentId,
                    DepartmentName = u.Department.Name,
                })
                .Skip((page - 1) * UsersPerPage)
                .Take(UsersPerPage)
                .ToList();

            return users;
        }

        public async Task<ApplicationUser> EditUserAsync<T>(T userInput)
        {
            var userDto = mapper.Map<UserEditDto>(userInput);
            var user = GetUserById(userDto.Id);

            if (user.Email != userDto.Email)
            {
                user.Email = userDto.Email;
                user.NormalizedEmail = userDto.Email.ToUpper();
            }

            if (user.UserName != userDto.UserName)
            {
                user.UserName = userDto.UserName;
                user.NormalizedUserName = userDto.UserName.ToUpper();
            }

            if (user.FirstName != userDto.FirstName)
            {
                user.FirstName = userDto.FirstName;
            }

            if (user.LastName != userDto.LastName)
            {
                user.LastName = userDto.LastName;
            }

            if (user.Position != userDto.Position)
            {
                user.Position = userDto.Position;
            }

            if (user.PhoneNumber != userDto.PhoneNumber)
            {
                user.PhoneNumber = userDto.PhoneNumber;
            }

            if (user.DepartmentId != userDto.DepartmentId)
            {
                user.DepartmentId = userDto.DepartmentId;
            }

            await dbContext.SaveChangesAsync();

            return user;
        }

        public async Task CreateDepartmentAsync(string departmentName)
        {
            var dbDepartment = dbContext.Departments
                .FirstOrDefault(d => d.Name.ToLower() == departmentName.ToLower());

            if (dbDepartment == null)
            {
                var department = new Department()
                {
                    Name = departmentName,
                };

                await dbContext.Departments.AddAsync(department);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = GetUserById(userId);

            user.IsDeleted = true;

            var tasks = dbContext.EmployeesTasks
                .Where(task => task.EmployeeId == userId)
                .ToList();

            tasks.ForEach(task => task.IsDeleted = true);

            var replaiesTasks = dbContext.RepliesTasks
               .Where(task => task.UserId == userId)
               .ToList();

            replaiesTasks.ForEach(task => task.IsDeleted = true);

            await dbContext.SaveChangesAsync();
        }
    }
}
