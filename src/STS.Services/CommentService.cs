using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using STS.Data;
using STS.Data.Models;
using STS.Services.Contracts;

namespace STS.Services
{
    public class CommentService : ICommentService
    {
        private const string status = "Open";

        private readonly IMapper mapper;
        private readonly ITicketService ticketService;
        private readonly ICommonService commonService;
        private readonly ApplicationDbContext dbContext;

        public CommentService(
            IMapper mapper,
            ITicketService ticketService,
            ICommonService commonService,
            ApplicationDbContext dbContext)
        {
            this.mapper = mapper;
            this.ticketService = ticketService;
            this.commonService = commonService;
            this.dbContext = dbContext;
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
            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public Comment GetById(int id)
        {
            return dbContext.Comments.FirstOrDefault(x => x.Id == id);    
        }
    }
}
