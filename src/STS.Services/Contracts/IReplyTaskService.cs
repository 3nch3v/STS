using System.Threading.Tasks;

using STS.Data.Dtos.Task;

namespace STS.Services.Contracts
{
    public interface IReplyTaskService
    {
        Task<ReplyTaskDto> ReplyTaskAsync<T>(string userId, T reply);
    }
}
