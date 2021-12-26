using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Tickets;

namespace STS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentController(ICommentService commentService, UserManager<ApplicationUser> userManager)
        {
            this.commentService = commentService;
            this.userManager = userManager;
        }


        [HttpPost]
        public async Task<ActionResult<CommentViewModel>> Create([FromBody] CommentInputModel comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = userManager.GetUserId(User);
            var username = userManager.GetUserName(User);
            var result = await commentService.Create(comment, userId);

            var commentDto = new CommentViewModel
            {
                Id = result.Id,
                Content = result.Content,
                CreatedOn = result.CreatedOn.ToString(),
                UserUserName = username,
            };

            return commentDto;
        }
    }
}
