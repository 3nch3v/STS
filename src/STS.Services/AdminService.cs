using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ApplicationDbContext dbContext;

        public AdminService(
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext dbContext)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.dbContext = dbContext;
        }

        public int GetUsersCount() => usersCount;

        public async Task CreateDepartmentAsync(string departmentName)
        {
            var department = new Department()
            {
                Name = departmentName,
            };

            await dbContext.Departments.AddAsync(department);
            await dbContext.SaveChangesAsync();
        }

        public async Task CreateRoleAsync(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                await roleManager.CreateAsync(new ApplicationRole(roleName));
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task RegisterUserAsync<T>(T userInput)
        {
            var userDto = mapper.Map<UserInputDto>(userInput);

            var user = new ApplicationUser
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Position = userDto.Position,
                UserName = userDto.UserName,
                Email = userDto.Email,
                DepartmentId = userDto.DepartmentId,
                PhoneNumber = userDto.PhoneNumber,
            };

            var userResult = await userManager.CreateAsync(user, userDto.Password);

            if (userResult.Succeeded)
            {
                var role = await roleManager.FindByNameAsync(userDto.Role);

                if (role != null)
                {
                    await userManager.AddToRoleAsync(user, userDto.Role);
                }
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task EditUserAsync<T>(T userInput)
        {
            var userDto = mapper.Map<UserEditDto>(userInput);
            var user = await userManager.FindByIdAsync(userDto.Id);

            if (user.Email != userDto.Email)
            {
                user.Email = userDto.Email;
                user.NormalizedEmail = userDto.Email;
                await userManager.UpdateAsync(user);
            }

            if (user.UserName != userDto.UserName)
            {
                user.UserName = userDto.UserName;
                user.NormalizedUserName = userDto.UserName;
                await userManager.UpdateAsync(user);
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

            var role = await roleManager.FindByNameAsync(userDto.Role);

            if (!user.Roles.Any(r => r.RoleId == role.Id))
            {
                foreach (var (roleId, userId) in user.Roles.Select(x => (x.RoleId, x.UserId)))
                {
                    var currRole = await roleManager.FindByIdAsync(roleId);
                    await userManager.RemoveFromRoleAsync(user, currRole.Name);
                }

                await userManager.AddToRoleAsync(user, userDto.Role);
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(string userId, string currPassword, string newPassword) 
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.ChangePasswordAsync(user, currPassword, newPassword);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string id)
        {
            return await userManager.GetRolesAsync(GetUserById(id));
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = GetUserById(userId);
            user.IsDeleted = true;

            await dbContext.SaveChangesAsync();
        }

        public ApplicationUser GetUserById(string id)
        {
            return dbContext.Users.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IEnumerable<UserDto>> GetUsersAsync(int page, string searchTerm, int? departmentId)
        {
            var usersQuery = dbContext.Users.AsQueryable();

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

            foreach (var user in users)
            {
                var currUser = await userManager.FindByIdAsync(user.Id);
                user.Roles = await userManager.GetRolesAsync(currUser);
            }

            return users;
        }
    }
}
