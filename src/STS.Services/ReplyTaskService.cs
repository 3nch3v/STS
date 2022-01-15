using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using STS.Data;
using STS.Data.Dtos.Task;
using STS.Data.Models;
using STS.Services.Contracts;

namespace STS.Services
{
    public class ReplyTaskService : IReplyTaskService
    {
        private readonly IMapper mapper;
        private readonly StsDbContext dbContext;

        public ReplyTaskService(IMapper mapper, StsDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task<ReplyTaskDto> ReplyTaskAsync<T>(string userId, T reply)
        {
            var task = mapper.Map<ReplyTask>(reply);
            task.User = dbContext.Users.FirstOrDefault(x => x.Id == userId);

            await dbContext.RepliesTasks.AddAsync(task);
            await dbContext.SaveChangesAsync();

            var result = mapper.Map<ReplyTaskDto>(task);

            return result;
        }
    }
}
