using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tasks;

namespace STS.Web.Controllers
{
    [Route("api/replay")]
    [ApiController]
    public class ReplayTaskApiController : ControllerBase
    {
        private readonly ITaskService taskServie;
        private readonly IReplayTaskService replayTaskService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ReplayTaskApiController(ITaskService taskServie,
            IReplayTaskService replayTaskService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.taskServie = taskServie;
            this.replayTaskService = replayTaskService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Replay([FromBody] ReplayTaskInputModel replay)
        {
            var task = taskServie.GetById(replay.EmployeeTaskId);

            if (task == null)
            {
                return NotFound();
            }

            try
            {
                var userId = userManager.GetUserId(User);
                var result = await replayTaskService.ReplayTaskAsync(userId, replay);
                var response = mapper.Map<ReplayTaskViewModel>(result);

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
