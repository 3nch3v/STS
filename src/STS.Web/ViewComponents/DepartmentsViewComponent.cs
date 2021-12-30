using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using STS.Services.Contracts;
using STS.Web.ViewModels.Admin;

namespace STS.Web.ViewComponents
{
    public class DepartmentsViewComponent : ViewComponent
    {
        private readonly ICommonService commonService;
        private readonly IMapper mapper;

        public DepartmentsViewComponent(ICommonService commonService, IMapper mapper)
        {
            this.commonService = commonService;
            this.mapper = mapper;
        }

        public IViewComponentResult Invoke()
        {
            var departmentsDto = commonService.GetDepartmentsWithStatistic();
            var departments = mapper.Map<IEnumerable<DepartmentStatisticViewModel>>(departmentsDto);

            return View(departments);
        }
    }
}
