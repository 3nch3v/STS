using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using AutoMapper;

using STS.Services.Contracts;
using STS.Web.ViewModels.Tasks;

namespace STS.Web.Controllers
{
    [Route("api/tasks")]
    [ApiController]
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
        [Authorize]
        public async Task<ActionResult<TaskViewModel>> Edit([FromBody] TaskEditModel taskDto)
        {
            var task = taskServie.GetById(taskDto.Id);

            if (task == null)
            {
                return NotFound();
            }

            try
            {
                var result = await taskServie.EditAsync(taskDto);
                var response = mapper.Map<TaskViewModel>(result);

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(
                   StatusCodes.Status500InternalServerError,
                   "Comment could not be updated.");
            }
        }
    }
}
