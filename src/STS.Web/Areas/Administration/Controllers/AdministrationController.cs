using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using STS.Services.Contracts;
using STS.Web.ViewModels.User;

using static STS.Common.GlobalConstants;

namespace STS.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AdministrationController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [Route("/[controller]")]
        public IActionResult Index() 
        {
            return View();
        }

        [Route("/[controller]/Users")]
        [HttpGet]
        public async Task<IActionResult> Users(UserInputModel userInput)
        {
            //all users
            //user per department
            // search for user with email, username or id

            var users = await userService.GetUsers();

            var usersDto = new UsersViewModel
            { 
                Users = mapper.Map<IEnumerable<UserViewModel>>(users),
            };

            return View(usersDto);
        }

        [Route("/[controller]/CreateUser")]
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [Route("/[controller]/CreateUser")]
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserInputModel userInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await userService.RegisterUser(userInput);

            return RedirectToAction(nameof(Users));
        }

        //create deparment
        //create status
        //create priority
        // add new role
    }
}
