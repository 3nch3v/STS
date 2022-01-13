using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using STS.Services.Contracts;
using STS.Web.ViewModels.Common;
using STS.Web.ViewModels.Tickets;

namespace STS.Web.ViewComponents
{
    public class CreateTicketViewComponent : ViewComponent
    {
        private readonly ICommonService commonService;
        private readonly IMapper mapper;

        public CreateTicketViewComponent(ICommonService commonService, IMapper mapper)
        {
            this.commonService = commonService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public IViewComponentResult Invoke()
        {
            var inputDto = new TicketInputModel
            {
                Priorities = mapper.Map<List<PriorityViewModel>>(commonService.GetPriorities()),
                Departments = mapper.Map<List<DepartmentViewModel>>(commonService.GetDepartmentsBase()),
            };

            return View(inputDto);
        }
    }
}
