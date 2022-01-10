using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using STS.Services.Contracts;
using STS.Web.ViewModels.User;

namespace STS.Web.ViewComponents
{
    public class ProfileViewComponent : ViewComponent
    {
        private readonly IMapper mapper;
        private readonly IAdminService adminService;

        public ProfileViewComponent(IMapper mapper, IAdminService adminService)
        {
            this.mapper = mapper;
            this.adminService = adminService;
        }

        public IViewComponentResult Invoke(string userId)
        {
            var user = adminService.GetUserById(userId);
            var userDto = mapper.Map<UserViewModel>(user);
       
            return View(userDto);
        }
    }
}
