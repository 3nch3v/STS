using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tasks;

namespace STS.Web.Areas.Api.Controllers
{
    [Route("api/reply")]
    [ApiController]
    [Authorize]
    public class ReplyTaskController : ControllerBase
    {
        private readonly ITaskService taskServie;
        private readonly IReplyTaskService replyTaskService;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ReplyTaskController(
            ITaskService taskServie,
            IReplyTaskService replyTaskService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.taskServie = taskServie;
            this.replyTaskService = replyTaskService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Reply([FromBody] ReplyTaskInputModel reply)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var task = taskServie.GetById(reply.EmployeeTaskId);

            if (task == null)
            {
                return NotFound();
            }

            var userId = userManager.GetUserId(User);
            var replyResult = await replyTaskService.ReplyTaskAsync(userId, reply);
            var response = mapper.Map<ReplyTaskViewModel>(replyResult);

            return Ok(response);
        }
    }
}
