using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

using AutoMapper;

using STS.Data;
using STS.Data.Models;
using STS.Services.Contracts;
using STS.Data.Dtos.User;

namespace STS.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly ApplicationDbContext dbContext;

        public UserService(
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

        public int GetDepartmentId(string userId)
        {
            var user = dbContext.Users
                .Where(x => x.Id == userId)
                .FirstOrDefault();

            return user.DepartmentId;
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            var users = dbContext.Users
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
                .ToList();

            foreach (var user in users)
            {
                var currUser = await userManager.FindByIdAsync(user.Id);
                user.Roles = await userManager.GetRolesAsync(currUser);
            }

            return users;
        }

        public async Task RegisterUser<T>(T userInput)
        {
            var userDto = mapper.Map<UserInputDto>(userInput);

            var user = new ApplicationUser
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Position = userDto.Position,
                UserName = userDto.Email,
                Email = userDto.Email,
                DepartmentId = userDto.DepartmentId,
                PhoneNumber = userDto.PhoneNumber,
            };

            var userResult = await userManager.CreateAsync(user, userDto.Password);

            if (userResult.Succeeded)
            {
                var role = await roleManager.FindByNameAsync(userDto.Role);

                IdentityResult roleResult = null;

                if (role == null)
                {
                    roleResult = await roleManager.CreateAsync(new ApplicationRole(userDto.Role));
                }

                if (role != null || roleResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, userDto.Role);
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
