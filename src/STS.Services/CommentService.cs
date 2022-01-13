using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using AutoMapper;

using STS.Data;
using STS.Data.Models;
using STS.Messaging;
using STS.Services.Contracts;

namespace STS.Services
{
    public class CommentService : ICommentService
    {
        private const string status = "Open";

        private readonly IMapper mapper;
        private readonly ITicketService ticketService;
        private readonly ICommonService commonService;
        private readonly IEmailSender emailSender;
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentService(
            IMapper mapper,
            ITicketService ticketService,
            ICommonService commonService,
            IEmailSender emailSender,
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            this.ticketService = ticketService;
            this.commonService = commonService;
            this.emailSender = emailSender;
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public Comment GetById(int id)
        {
            return dbContext.Comments.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Comment> CreateAsync<T>(T commentDto, string userId)
        {
            var comment = mapper.Map<Comment>(commentDto);
            comment.UserId = userId;

            var ticket = ticketService.GetById(comment.TicketId);
            ticket.StatusId = commonService.GetStatusId(status);

            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            return comment;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var comment = GetById(id);

            try
            {
                dbContext.Comments.Remove(comment);
                await dbContext.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task SendEmailAsync(int ticketId, string content, ApplicationUser sender)
        {
            var ticket = ticketService.GetById(ticketId);

            ApplicationUser assignedToUser = null;

            if (ticket.AssignedToId != null)
            {
                assignedToUser = await userManager.FindByIdAsync(ticket.AssignedToId);
            }

            var receiver = sender.Id != ticket.EmployeeId
                            ? ticket.Employee.Email
                            : assignedToUser?.Email;

            if (receiver != null)
            {
                string senderNames = $"{sender.FirstName} {sender.LastName}";
                string subject = $"Ticket: #{ticket.Id} Priority: {ticket.Priority.Name} *{ticket.Title}*.";
                string htmlContent = $"<p>{content}</p><br><a href='/Tickets/Ticket/{ticket.Id}'>Click here</a>";
                await emailSender.SendEmailAsync(sender.Email, senderNames, receiver, subject, htmlContent);
            }
        }
    }
}
