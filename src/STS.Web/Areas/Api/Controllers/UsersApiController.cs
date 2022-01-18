using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using STS.Services.Contracts;
using STS.Web.ViewModels.User;
using System.Threading.Tasks;

namespace STS.Web.Areas.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersApiController : ControllerBase
    {
        private readonly ICommonService commonService;
        private readonly IMapper mapper;

        public UsersApiController(ICommonService commonService, IMapper mapper)
        {
            this.commonService = commonService;
            this.mapper = mapper;
        }

        [HttpGet("{departmentId:int}")]
        public async Task<ActionResult<BaseUserViewModel>> GetEmployees(int departmentId)
        {
            bool isExisting = await commonService.IsDepartmentExisting(departmentId);
            if (!isExisting)
            {
                return BadRequest();
            }

            var employees = mapper.Map<IEnumerable<BaseUserViewModel>>(commonService.GetEmployeesBase(departmentId));

            return Ok(employees);
        }
    }
}
