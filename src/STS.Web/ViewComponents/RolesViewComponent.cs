using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using STS.Services.Contracts;
using STS.Web.ViewModels.Admin;

namespace STS.Web.ViewComponents
{
    public class RolesViewComponent : ViewComponent
    {
        private readonly ICommonService commonService;
        private readonly IMapper mapper;

        public RolesViewComponent(ICommonService commonService, IMapper mapper)
        {
            this.commonService = commonService;
            this.mapper = mapper;
        }

        public IViewComponentResult Invoke()
        {
            var rolesDto = commonService.GetRoles();
            var roles = mapper.Map<IEnumerable<RoleViewModel>>(rolesDto);

            return View(roles);
        }
    }
}
