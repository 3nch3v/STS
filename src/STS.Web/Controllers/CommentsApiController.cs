using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using STS.Data.Models;
using STS.Messaging;
using STS.Services.Contracts;
using STS.Web.ViewModels.Comment;

namespace STS.Web.Controllers
{
    [Route("api/Comments")]
    [ApiController]
    public class CommentsApiController : ControllerBase
    {
        private readonly ICommentService commentService;
        private readonly ITicketService ticketService;
        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsApiController(
            ICommentService commentService,
            ITicketService ticketService,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager)
        {
            this.commentService = commentService;
            this.ticketService = ticketService;
            this.emailSender = emailSender;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CommentViewModel>> Create([FromBody] CommentInputModel comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var user = await userManager.GetUserAsync(User);
                var result = await commentService.CreateAsync(comment, user.Id);

                if (comment.sendEmail)
                {
                    await SendEmail(comment, user);
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
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Comment could not be created.");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var comment = commentService.GetById(id);

            if (comment == null)
            {
                return NotFound("Comment with the given id was not found");
            }

            try
            {
                var result = await commentService.DeleteAsync(id);
                return Ok(result);
            }
            catch(Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError, 
                    "Comment could not be deleted.");
            }
        }
        private async Task SendEmail(CommentInputModel comment, ApplicationUser user)
        {
            var ticket = ticketService.GetById(comment.TicketId);

            ApplicationUser assignedToUser = null;

            if (ticket.AssignedToId != null)
            {
                assignedToUser = await userManager.FindByIdAsync(ticket.AssignedToId);
            }

            var receiver = user.Id != ticket.EmployeeId 
                            ? ticket.Employee.Email 
                            : assignedToUser?.Email;

            if (receiver != null)
            {
                string senderNames = $"{user.FirstName} {user.LastName}";
                string subject = $"Ticket: #{ticket.Id} Priority: {ticket.Priority.Name} *{ticket.Title}*.";
                string htmlContent = $"<p>{comment.Content}</p><br><a href='/Tickets/Ticket/{ticket.Id}'>Click here</a>";
                await emailSender.SendEmailAsync(user.Email, senderNames, receiver, subject, htmlContent);
            }
        }
    }
}
