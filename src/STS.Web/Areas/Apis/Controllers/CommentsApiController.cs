using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using STS.Data.Models;
using STS.Services.Contracts;
using STS.Web.ViewModels.Comment;

namespace STS.Web.Areas.Api.Controllers
{
    [Authorize]
    [Route("api/Comments")]
    [ApiController]
    public class CommentsApiController : ControllerBase
    {
        private readonly ICommentService commentService;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsApiController(
            ICommentService commentService,
            UserManager<ApplicationUser> userManager)
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

            var user = await userManager.GetUserAsync(User);
            var result = await commentService.CreateAsync(comment, user.Id);

            if (comment.sendEmail)
            {
                await commentService.SendEmailAsync(comment.TicketId, comment.Content, user);
            }

            var commentDto = new CommentViewModel
            {
                Id = result.Id,
                Content = result.Content,
                CreatedOn = result.CreatedOn.ToString(),
                UserUserName = user.UserName,
            };

            return commentDto;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var comment = commentService.GetById(id);

            if (comment == null)
            {
                return NotFound();
            }

            var result = await commentService.DeleteAsync(id);

            return Ok(result);
        }
    }
}
