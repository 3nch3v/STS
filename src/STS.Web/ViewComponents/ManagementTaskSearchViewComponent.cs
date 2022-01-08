using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using STS.Services.Contracts;
using STS.Web.ViewModels.User;
using STS.Web.ViewModels.Tasks;

namespace STS.Web.ViewComponents
{
    public class ManagementTaskSearchViewComponent : ViewComponent
    {
        private readonly IMapper mapper;
        private readonly ICommonService commonService;

        public ManagementTaskSearchViewComponent(IMapper mapper, ICommonService commonService)
        {
            this.mapper = mapper;
            this.commonService = commonService;
        }

        public IViewComponentResult Invoke(string keyword, string managerId)
        {
            var departmentId = commonService.GetDepartmentId(managerId);

            var employees = mapper.Map<IEnumerable<BaseUserViewModel>>(
                               commonService.GetEmployeesBase(departmentId));

            var searchModel = new ManagerTaskSearchViewModel
            {
                Keyword = keyword,
                Employees = employees,
            };

            return View(searchModel);        
        }
    }
}
