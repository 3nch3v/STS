using AutoMapper;
using STS.Data;
using STS.Data.Models;
using STS.Services.Contracts;
using System.Threading.Tasks;

namespace STS.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public CommentService(IMapper mapper, ApplicationDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task<Comment> Create<T>(T commentDto, string userId)
        {
            var comment = mapper.Map<Comment>(commentDto);
            comment.UserId = userId;

            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();

            return comment;
        }
    }
}
