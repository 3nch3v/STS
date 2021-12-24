using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tickets;
using System.Collections.Generic;

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
        public IViewComponentResult Invoke()
        {
            var inputDto = new TicketInputModel
            {
                Priorities = mapper.Map<List<PriorityViewModel>>(commonService.GetPriorities()),
                Departments = mapper.Map<List<DepartmentViewModel>>(commonService.GetDepartments()),
            };

            return View(inputDto);
        }
    }
}
