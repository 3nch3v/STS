using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using STS.Data;
using STS.Data.Models;
using STS.Messaging;
using STS.Services.Contracts;

namespace STS.Services
{
    public class CommentService : ICommentService
    {
        private const string status = "New reply";

        private readonly IMapper mapper;
        private readonly ITicketService ticketService;
        private readonly ICommonService commonService;
        private readonly IEmailSender emailSender;
        private readonly StsDbContext dbContext;

        public CommentService(
            IMapper mapper,
            ITicketService ticketService,
            ICommonService commonService,
            IEmailSender emailSender,
            StsDbContext dbContext)
        {
            this.mapper = mapper;
            this.ticketService = ticketService;
            this.commonService = commonService;
            this.emailSender = emailSender;
            this.dbContext = dbContext;
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

        public async Task DeleteAsync(int id)
        {
            var comment = GetById(id);
            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();
        }

        public async Task SendEmailAsync(int ticketId, string content, ApplicationUser sender)
        {
            var ticket = ticketService.GetById(ticketId);

            ApplicationUser assignedToUser = null;

            if (ticket.AssignedToId != null)
            {
                var user = dbContext.Users.FirstOrDefault(x => x.Id == ticket.AssignedToId);
                assignedToUser = user;
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
