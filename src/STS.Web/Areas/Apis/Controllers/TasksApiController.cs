using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using STS.Services.Contracts;
using STS.Web.ViewModels.Tasks;

namespace STS.Web.Areas.Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    [Authorize]
    public class TasksApiController : ControllerBase
    {
        private readonly ITaskService taskServie;
        private readonly IMapper mapper;

        public TasksApiController(
            ITaskService taskServie,
            IMapper mapper)
        {
            this.taskServie = taskServie;
            this.mapper = mapper;
        }

        [HttpPut]
        public async Task<ActionResult<TaskViewModel>> Edit([FromBody] TaskEditModel taskDto)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest();
            }

            var task = taskServie.GetById(taskDto.Id);

            if (task == null)
            {
                return NotFound();
            }

            var result = await taskServie.EditAsync(taskDto);
            var response = mapper.Map<TaskViewModel>(result);

            return Ok(response);
        }
    }
}
