using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using STS.Data;
using STS.Data.Dtos.Task;
using STS.Data.Models;
using STS.Services.Contracts;

namespace STS.Services
{
    public class ReplayTaskService : IReplayTaskService
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public ReplayTaskService(IMapper mapper, ApplicationDbContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task<ReplayTaskDto> ReplayTaskAsync<T>(string userId, T replay)
        {
            var task = mapper.Map<ReplyTask>(replay);
            task.User = dbContext.Users.FirstOrDefault(x => x.Id == userId);

            await dbContext.RepliesTasks.AddAsync(task);
            await dbContext.SaveChangesAsync();

            var result = mapper.Map<ReplayTaskDto>(task);
            return result;
        }
    }
}
